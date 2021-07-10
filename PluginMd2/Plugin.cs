using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public class Plugin
	{
		public class CMD2Model
		{
			// glcmds related
			public static int[] commands = new int[65536];
			public static int numcommands;
			public static int numglverts;
			public static int[] used = new int[CMD2Model.Constants.MaxTriangles];

			public static int[] strip_xyz = new int[128];
			public static int[] strip_st = new int[128];
			public static int[] strip_tris = new int[128];
			public static int stripcount;
			//

			public class Constants
			{
				public const int Header = (('2' << 24) + ('P' << 16) + ('D' << 8) + 'I'),
				Version = 8,

				MaxTriangles = 4096,
				MaxVerts = 2048,
				MaxFrames = 512,
				MaxSkins = 32,
				MaxSkinName = 64,
				MaxFrameName = 16;
			};

			public class Header
			{
				public int id = 0;		 // equals IDALIASHEADER
				public int version = 0;    // equals ALIAS_VERSION

				public int skinwidth = 0;
				public int skinheight = 0;
				public int framesize = 0; // byte size of each frame

				public int num_skins = 0;
				public int num_xyz = 0;
				public int num_st = 0; // greater than num_xyz for seams
				public int num_tris = 0;
				public int num_glcmds = 0; // dwords in strip/fan command list
				public int num_frames = 0;

				public int ofs_skins = 0; // each skin is a MAX_SKINNAME string
				public int ofs_st = 0; // byte offset from start for stverts
				public int ofs_tris = 0; // offset for dtriangles
				public int ofs_frames = 0; // offset for first frame
				public int ofs_glcmds = 0;
				public int ofs_end = 0; // end of file

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(id);
					writer.Write(version);
					writer.Write(skinwidth);
					writer.Write(skinheight);
					writer.Write(framesize);
					writer.Write(num_skins);
					writer.Write(num_xyz);
					writer.Write(num_st);
					writer.Write(num_tris);
					writer.Write(num_glcmds);
					writer.Write(num_frames);
					writer.Write(ofs_skins);
					writer.Write(ofs_st);
					writer.Write(ofs_tris);
					writer.Write(ofs_frames);
					writer.Write(ofs_glcmds);
					writer.Write(ofs_end);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					id = reader.ReadInt32();
					version = reader.ReadInt32();
					skinwidth = reader.ReadInt32();
					skinheight = reader.ReadInt32();
					framesize = reader.ReadInt32();
					num_skins = reader.ReadInt32();
					num_xyz = reader.ReadInt32();
					num_st = reader.ReadInt32();
					num_tris = reader.ReadInt32();
					num_glcmds = reader.ReadInt32();
					num_frames = reader.ReadInt32();
					ofs_skins = reader.ReadInt32();
					ofs_st = reader.ReadInt32();
					ofs_tris = reader.ReadInt32();
					ofs_frames = reader.ReadInt32();
					ofs_glcmds = reader.ReadInt32();
					ofs_end = reader.ReadInt32();
				}

				public const int StructureSize = 68;

				public void Clear()
				{
					id = version = skinwidth = skinheight = framesize = num_skins = num_xyz = num_st = num_tris = num_glcmds = num_frames = ofs_skins = ofs_st = ofs_tris = ofs_frames = ofs_glcmds = ofs_end = 0;
				}
			};

			public struct SkinVertex
			{
				public short s, t;

				public const int StructureSize = 4;

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(s);
					writer.Write(t);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					s = reader.ReadInt16();
					t = reader.ReadInt16();
				}
			};

			public struct Triangle
			{
				public short[] vertices;
				public short[] skinverts;

				public const int StructureSize = 12;

				public void Init()
				{
					vertices = new short[3];
					skinverts = new short[3];
				}

				public void Write(System.IO.BinaryWriter writer)
				{
					for (int i = 0; i < 3; ++i)
						writer.Write(vertices[i]);
					for (int i = 0; i < 3; ++i)
						writer.Write(skinverts[i]);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					Init();

					for (int i = 0; i < 3; ++i)
						vertices[i] = reader.ReadInt16();
					for (int i = 0; i < 3; ++i)
						skinverts[i] = reader.ReadInt16();
				}
			};

			public class AnimVertex
			{
				public byte[] v = new byte[3];    // X,Y,Z coordinate, packed on 0-255
				public byte lightnormalindex;     // index of the vertex normal

				public const int StructureSize = 4;

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(v, 0, 3);
					writer.Write(lightnormalindex);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					v = reader.ReadBytes(3);
					lightnormalindex = reader.ReadByte();
				}
			};

			public class FrameInfo
			{
				public Vector3 scale = Vector3.Empty;
				public Vector3 translate = Vector3.Empty;
				public string name;

				public const int StructureSize = 40;

				public void Write(System.IO.BinaryWriter writer)
				{
					scale.Write(writer);
					translate.Write(writer);

					CCString.Write(writer, name, Constants.MaxFrameName);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					scale.Read(reader);
					translate.Read(reader);

					name = CCString.Read(reader, Constants.MaxFrameName);
				}
			};

			// Quake2 md2 related functions
			static void BuildGlCmds(CPluginModel Model)
			{
				//
				// build tristrips
				//
				numcommands = 0;
				numglverts = 0;

				commands[numcommands++] = 0;		// end of list marker
			}

			class MD2TooManySkinsException : Exception
			{
				public MD2TooManySkinsException()
					: base("Model has too many skins to export to MD2, the maximum allowed is "+Constants.MaxSkins.ToString())
				{
				}
			}

			class MD2TooManyFramesException : Exception
			{
				public MD2TooManyFramesException()
					: base("Model has too many frames to export to MD2, the maximum allowed is "+Constants.MaxFrames.ToString())
				{
				}
			}

			class MD2NotFileException : Exception
			{
				public MD2NotFileException()
					: base("File is not a valid MD2 model")
				{
				}
			}

			class MD2TooManySkinsLoadException : Exception
			{
				public MD2TooManySkinsLoadException()
					: base("Model has too many skins to import from MD2, the maximum allowed is "+Constants.MaxSkins.ToString())
				{
				}
			}

			class MD2TooManyFramesLoadException : Exception
			{
				public MD2TooManyFramesLoadException()
					: base("Model has too many frames to import from MD2, the maximum allowed is "+Constants.MaxFrames.ToString())
				{
				}
			}

			public static void SaveToMD2(CPluginModel Model, string FileName)
			{
				// do gl commands here
				BuildGlCmds(Model);

				Header Head = new Header();
				Head.num_glcmds = numcommands;

				if (Model.GetSkinCount() > CMD2Model.Constants.MaxSkins)
					throw new MD2TooManySkinsException();
				if (Model.GetFrameCount() > CMD2Model.Constants.MaxFrames)
					throw new MD2TooManyFramesException();

				OpenFileDialog ChooseSkinDlg = new OpenFileDialog();

				ChooseSkinDlg.DefaultExt = "PCX";
				ChooseSkinDlg.Filter = "PCX Files (*.PCX)|*.PCX";

				//int BaseDirLen = Model.GetSettings(ESettingType.QuakeRootDirectory, null).Length;
				//int SkinNameLen;

				var baseDir = Model.GetSettings(ESettingType.QuakeRootDirectory, null);
				string[] SkinsList = new string[Model.GetSkinCount()];

				for (int i = 0; i < Model.GetSkinCount(); i++)
				{
					//int fp = 0;

					if (ChooseSkinDlg.ShowDialog() == DialogResult.Cancel)
						return;

					string SkinName = ChooseSkinDlg.FileName;
					/*SkinNameLen = SkinName.Length;

					if (BaseDirLen < SkinNameLen) // BaseDir should always be smaller
					{
						for (fp = 0; fp < BaseDirLen; fp++)
						{
							if (Char.ToLower(Model.GetSettings(ESettingType.QuakeRootDirectory, null)[fp]) != Char.ToLower(SkinName[fp]))
							{
								SkinsList[i] = SkinName;
								break;
							}
						}
					}

					if (fp < BaseDirLen)    // the loop stopped before comparison ended
						continue;*/

					if (SkinName.ToLower().Contains(baseDir.ToLower()))
						SkinsList[i] = SkinName.Substring(baseDir.Length);
					else
						throw new Exception("Skin dir doesn't contain base dir");

					//SkinsList[i] = SkinName.Substring(BaseDirLen);
				}

				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(file))
					{
						if (writer == null)
							throw new System.IO.FileLoadException();

						Head.num_frames = Model.GetFrameCount();
						Head.num_skins = Model.GetSkinCount();

						var mesh = Model.GetMeshAt(0);

						Head.num_st = mesh.GetSkinVerticeCount();
						Head.num_tris = mesh.GetTriangleCount();
						Head.num_xyz = mesh.GetVerticeCount();

						Head.framesize = CMD2Model.AnimVertex.StructureSize * Head.num_xyz + CMD2Model.FrameInfo.StructureSize;

						Head.skinwidth = Model.GetSkinAt(0).Width;
						Head.skinheight = Model.GetSkinAt(0).Height;

						Head.ofs_skins = CMD2Model.Header.StructureSize;
						Head.ofs_st = Head.ofs_skins + (Constants.MaxSkinName * Head.num_skins);
						Head.ofs_tris = Head.ofs_st + (Head.num_st * CMD2Model.SkinVertex.StructureSize);
						Head.ofs_frames = Head.ofs_tris + (Head.num_tris * CMD2Model.Triangle.StructureSize);
						Head.ofs_glcmds = Head.ofs_frames + (Head.framesize * Head.num_frames);
						Head.ofs_end = Head.ofs_glcmds + (numcommands * 4);

						Head.id = CMD2Model.Constants.Header;
						Head.version = CMD2Model.Constants.Version;

						for (int n = 0; n < Head.num_skins; n++)
							SkinsList[n] = SkinsList[n].ToLower().Replace('\\', '/');

						Head.Write(writer);

						for (int n = 0; n < Head.num_skins; n++)
							CCString.Write(writer, SkinsList[n], Constants.MaxSkinName);

						CMD2Model.SkinVertex[] tempskinverts = new CMD2Model.SkinVertex[Head.num_st];

						for (int i = 0; i < Head.num_st; i++)
						{
							if (mesh.GetSkinVerticeAt(i).s < 0)
								mesh.GetSkinVerticeAt(i).s = 0;
							if (mesh.GetSkinVerticeAt(i).s > 1)
								mesh.GetSkinVerticeAt(i).s = 1;
								
							if (mesh.GetSkinVerticeAt(i).t < 0)
								mesh.GetSkinVerticeAt(i).t = 0;
							if (mesh.GetSkinVerticeAt(i).t > 1)
								mesh.GetSkinVerticeAt(i).t = 1;

							int LargestWidth = 0, LargestHeight = 0;
							for (int x = 0; x < Model.GetSkinCount(); ++x)
							{
								if (LargestHeight < Model.GetSkinAt(x).Height)
									LargestHeight = Model.GetSkinAt(x).Height;
								if (LargestWidth < Model.GetSkinAt(x).Width)
									LargestWidth = Model.GetSkinAt(x).Width;
							}

							tempskinverts[i].s = (short)(mesh.GetSkinVerticeAt(i).s * LargestWidth);
							tempskinverts[i].t = (short)(mesh.GetSkinVerticeAt(i).t * LargestHeight);
						}

						for (int i = 0; i < Head.num_st; ++i)
							tempskinverts[i].Write(writer);

						CMD2Model.Triangle[] temptris = new CMD2Model.Triangle[Head.num_tris];

						for (int i = 0; i < Head.num_tris; i++)
						{
							temptris[i].Init();
							temptris[i].vertices[0] = (short)mesh.GetTriangleAt(i).Vertices[0];
							temptris[i].vertices[1] = (short)mesh.GetTriangleAt(i).Vertices[1];
							temptris[i].vertices[2] = (short)mesh.GetTriangleAt(i).Vertices[2];
							temptris[i].skinverts[0] = (short)mesh.GetTriangleAt(i).SkinVertices[0];
							temptris[i].skinverts[1] = (short)mesh.GetTriangleAt(i).SkinVertices[1];
							temptris[i].skinverts[2] = (short)mesh.GetTriangleAt(i).SkinVertices[2];
						}

						for (int i = 0; i < Head.num_tris; ++i)
							temptris[i].Write(writer);

						CMD2Model.AnimVertex[] tempFileFrame = new CMD2Model.AnimVertex[Head.num_xyz];
						CMD2Model.FrameInfo tempFileFrameInfo = new CMD2Model.FrameInfo();

						ProgressBar.ProgressBarBackgroundWorker worker = ProgressBar.OpenProgressBar(Head.num_frames * ((Head.num_xyz * 3) + Head.num_tris));

						for (int f = 0; f < Head.num_frames; f++)
						{
							tempFileFrameInfo.name = Model.GetFrameAt(f).FrameName;

							Vector3 vmin = new Vector3(100000, 100000, 100000), vmax = new Vector3(-100000, -100000, -100000);

							for (int v = 0; v < Head.num_xyz; v++)
							{
								tempFileFrame[v] = new AnimVertex();

								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.x < vmin.x)
									vmin.x = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.x;
								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.y < vmin.y)
									vmin.y = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.y;
								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.z < vmin.z)
									vmin.z = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.z;
								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.x > vmax.x)
									vmax.x = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.x;
								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.y > vmax.y)
									vmax.y = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.y;
								if (mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.z > vmax.z)
									vmax.z = mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector.z;

								worker.Tick();
							}

							Vector3[] trinormals = new Vector3[Head.num_tris];

							Vector3 va = Vector3.Empty, vb = Vector3.Empty, vc = Vector3.Empty, vd = Vector3.Empty, ve = Vector3.Empty, vn = Vector3.Empty;

							for (int n = 0; n < Head.num_tris; n++)
							{
								va.Set(mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[0]).GetFrameDataAt(f).Vector.x,
								   mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[0]).GetFrameDataAt(f).Vector.y,
								   mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[0]).GetFrameDataAt(f).Vector.z);
								vb.Set(mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[1]).GetFrameDataAt(f).Vector.x,
								   mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[1]).GetFrameDataAt(f).Vector.y,
								   mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[1]).GetFrameDataAt(f).Vector.z);
								vc.Set(mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[2]).GetFrameDataAt(f).Vector.x,
								 mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[2]).GetFrameDataAt(f).Vector.y,
								mesh.GetVerticeAt(mesh.GetTriangleAt(n).Vertices[2]).GetFrameDataAt(f).Vector.z);

								vd = vb-va;
								ve = vc-vb;
								vn = vd/ve;

								vn.Normalize();
								trinormals[n] = Vector3.Empty;
								trinormals[n][0] = vn[0];
								trinormals[n][1] = vn[1];
								trinormals[n][2] = vn[2];

								worker.Tick();
							}

							for (int n = 0; n < Head.num_xyz; n++)
							{
								Vector3 vn1 = Vector3.Empty;
								vn.Set(0, 0, 0);

								for (int i = 0; i < Head.num_tris; i++)
								{
									if (mesh.GetTriangleAt(i).Vertices[0] == n ||
										mesh.GetTriangleAt(i).Vertices[1] == n ||
										mesh.GetTriangleAt(i).Vertices[2] == n)
										vn += trinormals[i];
								}
								vn.Normalize();

								int best = 0;
								float bestf = 1, newf;
								for (int norm = 0; norm < 162; norm++)
								{
									vn1 = ANorms.Normals[norm];

									newf = vn.DotProduct (vn1);
									if (newf < bestf)
									{
										bestf = newf;
										best = norm;
									}
								}
								tempFileFrame[n].lightnormalindex = (byte)best;

								worker.Tick();
							}

							tempFileFrameInfo.scale = ((vmax - vmin) / 255.0f);

							for (int i = 0; i < 3; ++i)
							{
								if (tempFileFrameInfo.scale[i] == 0)
									tempFileFrameInfo.scale[i] = 1.0f;
							}

							tempFileFrameInfo.translate = vmin;

							for (int v = 0; v < Head.num_xyz; v++)
							{
								Vector3 mf = ((mesh.GetVerticeAt(v).GetFrameDataAt(f).Vector - vmin) / tempFileFrameInfo.scale) + 0.5f;

								for (int i = 0; i < 3; ++i)
								{
									if (mf[i] > 255.0)
										mf[i] = 255.0f;
									if (mf[i] < 0)
										mf[i] = 0;
								}

								for (int i = 0; i < 3; ++i)
									tempFileFrame[v].v[i] = (byte)mf[i];

								worker.Tick();
							}

							tempFileFrameInfo.Write(writer);
							for (int i = 0; i < Head.num_xyz; ++i)
								tempFileFrame[i].Write(writer);
						}

						worker.Done();

						for (int i = 0; i < numcommands; ++i)
							writer.Write(BitConverter.GetBytes(commands[i]), 0, 4);

						writer.Close();
					}
				}
			}

			public static void LoadFromMD2(CPluginModel Model, string FileName)
			{
				CMD2Model.Header tHead = new CMD2Model.Header();

				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Open))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
					{
						if (reader == null)
							throw new System.IO.FileLoadException();

						Model.Clear();
						tHead.Read(reader);

						if (tHead.id != CMD2Model.Constants.Header || tHead.version != CMD2Model.Constants.Version)
							throw new MD2NotFileException();
						if (tHead.num_skins > CMD2Model.Constants.MaxSkins)
							throw new MD2TooManySkinsLoadException();
						if (tHead.num_frames > Constants.MaxFrames)
							throw new MD2TooManyFramesLoadException();

						for (int n = 0; n < tHead.num_skins; n++)
						{
							CPluginSkin Skin = Model.CreateSkin();
							string Name = CCString.Read(reader, Constants.MaxSkinName);

							Skin.Path = Model.GetSettings(ESettingType.QuakeRootDirectory, null) + Name;

							if (!System.IO.File.Exists(Skin.Path))
							{
								try
								{
									string nam = FileName.Substring(0, FileName.LastIndexOfAny(new char[] { '\\', '/' })).Replace('\\', '/');
									string pn = nam + Name.Substring(Name.LastIndexOfAny(new char[] { '\\', '/' })).Replace('\\', '/');

									if (pn.EndsWith(Name.Replace('\\', '/')))
									{
										Skin.Path = pn;
										if (!System.IO.File.Exists(Skin.Path))
											Skin.Path = "";
									}
								}
								catch
								{
									Skin.Path = "";
								}
							}

							Skin.Width = tHead.skinwidth;
							Skin.Height = tHead.skinheight;
						}

						CMD2Model.SkinVertex[] tempskinverts = new CMD2Model.SkinVertex[tHead.num_st];

						CPluginMesh mesh = Model.CreateMesh();

						for (int i = 0; i < tHead.num_st; ++i)
							tempskinverts[i].Read(reader);

						for (int i = 0; i < tHead.num_st; i++)
						{
							CPluginSkinVertice rsk = mesh.CreateSkinVertice();
							rsk.s = (float)tempskinverts[i].s / (float)tHead.skinwidth;
							rsk.t = (float)tempskinverts[i].t / (float)tHead.skinheight;

							if (rsk.s < 0)
								rsk.s = 0;
							if (rsk.s > 1)
								rsk.s = 1;

							if (rsk.t < 0)
								rsk.t = 0;
							if (rsk.t > 1)
								rsk.t = 1;
						}

						CMD2Model.Triangle[] temptris = new CMD2Model.Triangle[tHead.num_tris];

						for (int i = 0; i < tHead.num_tris; ++i)
							temptris[i].Read(reader);

						for (int i = 0; i < tHead.num_tris; i++)
						{
							CPluginTriangle rt = mesh.CreateTriangle();

							rt.Vertices[0] = temptris[i].vertices[0];
							rt.Vertices[1] = temptris[i].vertices[1];
							rt.Vertices[2] = temptris[i].vertices[2];
							rt.SkinVertices[0] = temptris[i].skinverts[0];
							rt.SkinVertices[1] = temptris[i].skinverts[1];
							rt.SkinVertices[2] = temptris[i].skinverts[2];
						}

						CMD2Model.AnimVertex[] tempFileFrame = new CMD2Model.AnimVertex[tHead.num_xyz];
						CMD2Model.FrameInfo tempFileFrameInfo = new CMD2Model.FrameInfo();

						for (int n = 0; n < tHead.num_frames; n++)
							Model.CreateFrame();

						for (int v = 0; v < tHead.num_xyz; v++)
							mesh.CreateVertice();

						for (int n = 0; n < tHead.num_frames; n++)
						{
							CPluginFrame Frame = Model.GetFrameAt(n);

							tempFileFrameInfo.name = "";
							tempFileFrameInfo.Read(reader);
							Frame.FrameName = tempFileFrameInfo.name;

							for (int i = 0; i < tHead.num_xyz; ++i)
							{
								tempFileFrame[i] = new AnimVertex();
								tempFileFrame[i].Read(reader);
							}

							for (int v = 0; v < tHead.num_xyz; v++)
							{
								CPluginVertice rv = mesh.GetVerticeAt(v);
								rv.GetFrameDataAt(n).Vector = (new Vector3(tempFileFrame[v].v[0], tempFileFrame[v].v[1], tempFileFrame[v].v[2]) * tempFileFrameInfo.scale) + tempFileFrameInfo.translate;
							}
						}
					}
				}
			}
		}

		public class CMd2ExportPlugin : CPlugin
		{
			public CMd2ExportPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export MD2...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "MD2 Models (*.md2)|*.md2|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMD2Model.SaveToMD2(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CMd2ExportPlugin();
			}
		}


		public class CMd2ImportPlugin : CPlugin
		{
			public CMd2ImportPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import MD2...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "MD2 Models (*.md2)|*.md2|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMD2Model.LoadFromMD2(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CMd2ImportPlugin();
			}
		}
	}
}
