using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VCMDL.NET
{
	public class Plugin
	{
		public class CAQMModel
		{
			public enum EAQMType
			{
				AQM_VC,
				AQM_9B,
				AQM_911
			};

			public class Constants
			{
				public const string Header_vc = "AQN";
				public const string Header_9b = "AQM";
				public const string Header_911 = "AQ2";
				public static byte[] Header_vc_b = new byte[] { unchecked((byte)'A'), unchecked((byte)'Q'), unchecked((byte)'N') };
				public static byte[] Header_9b_b = new byte[] { unchecked((byte)'A'), unchecked((byte)'Q'), unchecked((byte)'M') };
				public static byte[] Header_911_b = new byte[] { unchecked((byte)'A'), unchecked((byte)'Q'), unchecked((byte)'2') };
				public const int HeaderSize = 3;
			}

			public class CModelConstants
			{
				public const int MaxFrames = 2048,
					MaxSkinName = 64,
					MaxFrameName = 16;
			};

			public class TRealSkinVertex
			{
				public float s = 0;						// horiz
				public float t = 0;						// vert
				public bool Selected = false;

				public void Read(System.IO.BinaryReader reader)
				{
					s = reader.ReadSingle();
					t = reader.ReadSingle();
					Selected = reader.ReadBoolean();
					reader.ReadBytes(3);
				}

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(s);
					writer.Write(t);
					writer.Write(Selected);

					for (int i = 0; i < 3; ++i)
						writer.Write((byte)0);
				}
			};

			public class TBaseTriangle
			{
				public short[] Vertices = new short[3];
				public short[] SkinVerts = new short[3];
				public Vector3 Centre = Vector3.Empty;
				public Vector3 Normal = Vector3.Empty;

				public virtual void Read(System.IO.BinaryReader reader)
				{
					for (int i = 0; i < 3; ++i)
						Vertices[i] = reader.ReadInt16();
					for (int i = 0; i < 3; ++i)
						SkinVerts[i] = reader.ReadInt16();

					Centre.Read(reader);
					Normal.Read(reader);
				}

				public virtual void Write(System.IO.BinaryWriter writer)
				{
					for (int i = 0; i < 3; ++i)
						writer.Write(Vertices[i]);
					for (int i = 0; i < 3; ++i)
						writer.Write(SkinVerts[i]);

					Centre.Write(writer);
					Normal.Write(writer);
				}
			};

			public class TRealTriangle : TBaseTriangle
			{
				public bool Selected = false,
							 Visible = true,
							 SkinSelected = false;

				public override void Read(System.IO.BinaryReader reader)
				{
					base.Read(reader);
					Selected = reader.ReadBoolean();
					Visible = reader.ReadBoolean();
					SkinSelected = reader.ReadBoolean();

					reader.ReadBytes(1);
				}

				public override void Write(System.IO.BinaryWriter writer)
				{
					base.Write(writer);
					writer.Write(Selected);
					writer.Write(Visible);
					writer.Write(SkinSelected);
					writer.Write((byte)0);
				}
			};

			public static TRealSkinVertex ReadSkinVert(System.IO.BinaryReader reader)
			{
				TRealSkinVertex v = new TRealSkinVertex();
				v.Read(reader);
				return v;
			}

			public static TRealTriangle ReadTriangle(System.IO.BinaryReader reader)
			{
				TRealTriangle v = new TRealTriangle();
				v.Read(reader);
				return v;
			}

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

			public class TVertex
			{
				public float x = 0, y = 0, z = 0;
				public bool Selected = false, Visible = true;

				public Vector3 Vector
				{
					get { return new Vector3(x, y, z); }
					set { x = value.x; y = value.y; z = value.z; }
				}

				public virtual void Read(System.IO.BinaryReader reader)
				{
					x = reader.ReadSingle();
					y = reader.ReadSingle();
					z = reader.ReadSingle();
					Selected = reader.ReadBoolean();
					Visible = reader.ReadBoolean();

					reader.ReadBytes(2);
				}

				public virtual void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(x);
					writer.Write(y);
					writer.Write(z);
					writer.Write(Selected);
					writer.Write(Visible);

					for (int i = 0; i < 2; ++i)
						writer.Write((byte)0);
				}
			};

			public class TRealVertex : TVertex
			{
				public Vector3 Normal = Vector3.Empty;

				public override void Read(System.IO.BinaryReader reader)
				{
					base.Read(reader);
					Normal.Read(reader);
				}

				public override void Write(BinaryWriter writer)
				{
					base.Write(writer);
					Normal.Write(writer);
				}
			};

			public class CFrame
			{
				public List<TRealVertex> Vertices = new List<TRealVertex>();
				public string FrameName = "";

				public void ReadFrameInfo(System.IO.BinaryReader reader)
				{
					FrameName = CCString.Read(reader, 16);
				}

				public TRealVertex ReadVertex(System.IO.BinaryReader reader)
				{
					TRealVertex v = new TRealVertex();
					v.Read(reader);
					return v;
				}

				public void ReadFrame(System.IO.BinaryReader reader, int NumVertices)
				{
					for (int i = 0; i < NumVertices; ++i)
						Vertices.Add(ReadVertex(reader));
				}

				public void ReadFrame(System.IO.BinaryReader reader, int NumVertices, CAQMModel.EAQMType Type)
				{
					for (int i = 0; i < NumVertices; ++i)
					{
						Vertices.Add(ReadVertex(reader));

						if (Type == CAQMModel.EAQMType.AQM_911)
							reader.ReadBytes(8);
					}
				}
			};

			static public void WriteVertex(System.IO.BinaryWriter writer, CPluginVerticeFrameData Vertice)
			{
				TRealVertex v = new TRealVertex();
				v.Vector = Vertice.Vector;
				v.Normal = Vertice.Normal;
				v.Write(writer);
			}

			static public void WriteFrame(int Frame, CPluginModel Model, System.IO.BinaryWriter writer, CAQMModel.EAQMType Type)
			{
				for (int i = 0; i < Model.GetVerticeCount(); ++i)
				{
					WriteVertex(writer, Model.GetVerticeAt(i).GetFrameDataAt(Frame));

					if (Type == CAQMModel.EAQMType.AQM_911)
						for (int z = 0; z < 8; ++z)
							writer.Write((byte)0);
				}
			}

			public static void SaveToAQM(CPluginModel Model, string FileName, CAQMModel.EAQMType Type)
			{
				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new FileNotFoundException();

					using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(file))
					{
						if (writer == null)
							throw new FileLoadException();

						switch (Type)
						{
						case CAQMModel.EAQMType.AQM_9B:
							writer.Write (CAQMModel.Constants.Header_9b_b, 0, CAQMModel.Constants.HeaderSize);
							break;
						case CAQMModel.EAQMType.AQM_911:
						default:
							writer.Write (CAQMModel.Constants.Header_911_b, 0, CAQMModel.Constants.HeaderSize);
							break;
						};

						Header Head = new Header();
						Head.num_frames = Model.GetFrameCount();
						Head.num_skins = Model.GetSkinCount();
						Head.num_st = Model.GetSkinVerticeCount();
						Head.num_tris = Model.GetTriangleCount();
						Head.num_xyz = Model.GetVerticeCount();
						Head.skinwidth = (Model.GetSkinCount() != 0) ? Model.GetSkinAt(0).Width : 1;
						Head.skinheight = (Model.GetSkinCount() != 0) ? Model.GetSkinAt(0).Height : 1;

						Head.id = (('2' << 24) + ('P' << 16) + ('D' << 8) + 'I');
						Head.version = 8;

						Head.Write(writer);

						for (int i = 0; i < Model.GetSkinCount(); i++)
						{
							if (Type == CAQMModel.EAQMType.AQM_911)
							{
								byte b = 100;
								for (int z = 0; z < 3 * 256; ++z)
									writer.Write(b);
							}

							for (int z = 0; z < Head.skinwidth * Head.skinheight; ++z)
								writer.Write((byte)255);
						}

						OpenFileDialog ChooseSkinDlg = new OpenFileDialog();

						ChooseSkinDlg.DefaultExt = "PCX";
						ChooseSkinDlg.Filter = "PCX Files (*.PCX)|*.PCX";

						int BaseDirLen = Model.GetSettings(ESettingType.QuakeRootDirectory, null).Length;
						int SkinNameLen;

						string[] SkinsList = new string[Model.GetSkinCount()];
						for (int i = 0; i < Model.GetSkinCount(); i++)
						{
							int fp = 0;

							if (ChooseSkinDlg.ShowDialog() == DialogResult.Cancel)
								return;

							string SkinName = ChooseSkinDlg.FileName;
							SkinNameLen = SkinName.Length;

							if (BaseDirLen < SkinNameLen) // BaseDir should always be smaller
							{
								for (fp = 0; fp < BaseDirLen; fp++)
								{
									if (Char.ToLower(Model.GetSettings(ESettingType.QuakeRootDirectory, null)[fp]) != Char.ToLower(SkinName[fp]))
										break;
								}
							}

							if (fp < BaseDirLen)    // the loop stopped before comparison ended
							{
								SkinsList[i] = SkinName;
								if (SkinsList[i].Length >= CModelConstants.MaxSkinName - 1)
									SkinsList[i].Remove(CModelConstants.MaxSkinName-1);
								continue;
							}

							SkinsList[i] = SkinName.Substring(BaseDirLen);
						}

						for (int n = 0; n < Model.GetSkinCount(); n++)
							CCString.Write(writer, SkinsList[n], CModelConstants.MaxSkinName);

						for (int i = 0; i < Model.GetSkinVerticeCount(); ++i)
						{
							TRealSkinVertex v = new TRealSkinVertex();
							CPluginSkinVertice SkinVert = Model.GetSkinVerticeAt(i);

							v.s = SkinVert.s * (float)Head.skinwidth;
							v.t = SkinVert.t * (float)Head.skinheight;
							v.Selected = false;
							v.Write(writer);
						}

						for (int i = 0; i < Head.num_tris; ++i)
						{
							TRealTriangle tri = new TRealTriangle();
							CPluginTriangle Triangle = Model.GetTriangleAt(i);

							for (int x = 0; x < 3; ++x)
							{
								tri.SkinVerts[x] = (short)Triangle.SkinVertices[x];
								tri.Vertices[x] = (short)Triangle.Vertices[x];
							}

							tri.Write(writer);
						}

						for (int i = 0; i < Head.num_frames; ++i)
						{
							CCString.Write(writer, Model.GetFrameAt(i).FrameName, 16);

							if (Type == CAQMModel.EAQMType.AQM_911)
								for (int z = 0; z < 12; ++z)
									writer.Write((byte)0);
						}

						// The only thing that has changed is the vertex structure. Bone info has been removed.
						if (Type != CAQMModel.EAQMType.AQM_9B)
						{
							for (int i = 0; i < Head.num_frames; i++)
								WriteFrame(i, Model, writer, Type);
						}
						else
						{
							for (int i = 0; i < Head.num_frames; i++)
							{
								for (int z = 0; z < Head.num_xyz; ++z)
								{
									TVertex Vert = new TVertex();
									CPluginVerticeFrameData v = Model.GetVerticeAt(z).GetFrameDataAt(i);

									Vert.Vector = v.Vector;
									Vert.Write(writer);

									for (int x = 0; x < 20; ++x)
										writer.Write((byte)0);
								}
							}
						}
					}
				}
			}

			// can load all three formats: 911, 9b and vc
			public static void LoadFromAQM(CPluginModel Model, string FileName)
			{
				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Open))
				{
					if (file == null)
						throw new FileNotFoundException();

					using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
					{
						if (reader == null)
							throw new FileLoadException();

						Model.Clear();

						byte[] header = new byte[CAQMModel.Constants.HeaderSize];
						header = reader.ReadBytes(CAQMModel.Constants.HeaderSize);

						string head = "";
						for (int i = 0; i < header.Length; ++i)
						{
							if (header[i] == '\0')
								break;

							head += (char)header[i];
						}

						CAQMModel.EAQMType AQMType;
						switch (head)
						{
						case CAQMModel.Constants.Header_vc:
							AQMType = CAQMModel.EAQMType.AQM_VC;
							break;
						case CAQMModel.Constants.Header_9b:
							AQMType = CAQMModel.EAQMType.AQM_9B;
							break;
						case CAQMModel.Constants.Header_911:
						default:
							AQMType = CAQMModel.EAQMType.AQM_911;
							break;
						};

						Header Head = new Header();
						Head.Read(reader);

						for (int i = 0; i < Head.num_skins; i++)
						{
							if (AQMType == CAQMModel.EAQMType.AQM_911)
								reader.ReadBytes(3 * 256);

							byte[] skinData = new byte[Head.skinwidth * Head.skinheight];
							skinData = reader.ReadBytes(Head.skinwidth * Head.skinheight);
						}

						for (int i = 0; i < Head.num_skins; i++)
						{
							CPluginSkin Skin = Model.CreateSkin();
							Skin.Width = Head.skinwidth;
							Skin.Height = Head.skinheight;
							string Name = CCString.Read(reader, CModelConstants.MaxSkinName);

							Skin.Path = Model.GetSettings(ESettingType.QuakeRootDirectory, null) + Name;

							if (!System.IO.File.Exists(Skin.Path))
							{
								string nam = FileName.Substring(0, FileName.LastIndexOfAny(new char[] { '\\', '/' })).Replace('\\', '/');
								int pos = Name.LastIndexOfAny(new char[] { '\\', '/' });

								if (pos == -1)
									Skin.Path = nam;
								else
								{
									string pn = nam + Name.Substring(pos).Replace('\\', '/');

									if (!pn.EndsWith(Name.Replace('\\', '/')))
										continue;

									Skin.Path = pn;
									if (!System.IO.File.Exists(Skin.Path))
										Skin.Path = "";
								}
							}
						}

						for (int i = 0; i < Head.num_st; ++i)
						{
							TRealSkinVertex v = ReadSkinVert(reader);
							CPluginSkinVertice SkinVert = Model.CreateSkinVertice();
							SkinVert.s = v.s / (float)Head.skinwidth;
							SkinVert.t = v.t / (float)Head.skinheight;
						}

						for (int i = 0; i < Head.num_tris; ++i)
						{
							TRealTriangle tri = ReadTriangle(reader);
							CPluginTriangle Triangle = Model.CreateTriangle();

							for (int x = 0; x < 3; ++x)
							{
								Triangle.SkinVertices[x] = tri.SkinVerts[x];
								Triangle.Vertices[x] = tri.Vertices[x];
							}
						}

						for (int i = 0; i < Head.num_frames; ++i)
						{
							CFrame Frame = new CFrame();
							Frame.ReadFrameInfo(reader);

							CPluginFrame NewFrame = Model.CreateFrame();
							NewFrame.FrameName = Frame.FrameName;

							if (AQMType == CAQMModel.EAQMType.AQM_911)
								reader.ReadBytes(12);
						}

						// The only thing that has changed is the vertex structure. Bone info has been removed.
						if (AQMType != CAQMModel.EAQMType.AQM_9B)
						{
							CFrame[] Frames = new CFrame[Head.num_frames];
							for (int i = 0; i < Head.num_frames; i++)
							{
								Frames[i] = new CFrame();
								Frames[i].ReadFrame(reader, Head.num_xyz, AQMType);
							}

							for (int x = 0; x < Head.num_xyz; ++x)
								Model.CreateVertice();

							for (int i = 0; i < Head.num_frames; i++)
							{
								for (int x = 0; x < Head.num_xyz; ++x )
								{
									Model.GetVerticeAt(x).GetFrameDataAt(i).Vector = Frames[i].Vertices[x].Vector;
									Model.GetVerticeAt(x).GetFrameDataAt(i).Normal = Frames[i].Vertices[x].Normal;
								}
							}
						}
						else
						{
							TVertex[] ReadVerts = new TVertex[Head.num_xyz];

							for (int z = 0; z < Head.num_xyz; z++)
								Model.CreateVertice();

							for (int i = 0; i < Head.num_frames; i++)
							{
								for (int z = 0; z < Head.num_xyz; ++z)
								{
									ReadVerts[z] = new TVertex();
									ReadVerts[z].Read(reader);
									reader.ReadBytes(20);
								}

								for (int z = 0; z < Head.num_xyz; z++)
								{
									CPluginVertice v = Model.GetVerticeAt(z);

									v.GetFrameDataAt(i).Vector = new Vector3(ReadVerts[z].x, ReadVerts[z].y, ReadVerts[z].z);
									//v.Visible = ReadVerts[z].Visible;
									//v.Selected = ReadVerts[z].Selected;
								}
							}
						}
					}
				}
			}
		}

		public class CAQMImportPlugin : CPlugin
		{
			public CAQMImportPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import AQM...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "AQM Models (*.aqm)|*.aqm|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CAQMModel.LoadFromAQM(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CAQMImportPlugin();
			}
		}

		public class CAQMExportPlugin : CPlugin
		{
			public CAQMExportPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export AQM...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "AQM Models (*.aqm)|*.aqm|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CAQMModel.EAQMType Type = CAQMModel.EAQMType.AQM_9B;

				if (MessageBox.Show("Would you like to save this as a 911 AQM? Hit No to save as a 9b AQM", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
					Type = CAQMModel.EAQMType.AQM_911;

				CAQMModel.SaveToAQM(Model, dlg.FileName, Type);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CAQMExportPlugin();
			}
		}
	}
}
