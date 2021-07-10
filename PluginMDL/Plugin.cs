using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace VCMDL.NET
{
	public class Plugin
	{
		class CMDLModel
		{
			public static Color[] ColorMap = new Color[256] 
			{
				Color.FromArgb(  0,   0,   0), Color.FromArgb( 15,  15,  15), Color.FromArgb( 31,  31,  31), Color.FromArgb( 47,  47,  47), 
				Color.FromArgb( 63,  63,  63), Color.FromArgb( 75,  75,  75), Color.FromArgb( 91,  91,  91), Color.FromArgb(107, 107, 107), 
				Color.FromArgb(123, 123, 123), Color.FromArgb(139, 139, 139), Color.FromArgb(155, 155, 155), Color.FromArgb(171, 171, 171), 
				Color.FromArgb(187, 187, 187), Color.FromArgb(203, 203, 203), Color.FromArgb(219, 219, 219), Color.FromArgb(235, 235, 235), 
				Color.FromArgb( 15,  11,   7), Color.FromArgb( 23,  15,  11), Color.FromArgb( 31,  23,  11), Color.FromArgb( 39,  27,  15), 
				Color.FromArgb( 47,  35,  19), Color.FromArgb( 55,  43,  23), Color.FromArgb( 63,  47,  23), Color.FromArgb( 75,  55,  27), 
				Color.FromArgb( 83,  59,  27), Color.FromArgb( 91,  67,  31), Color.FromArgb( 99,  75,  31), Color.FromArgb(107,  83,  31), 
				Color.FromArgb(115,  87,  31), Color.FromArgb(123,  95,  35), Color.FromArgb(131, 103,  35), Color.FromArgb(143, 111,  35), 
				Color.FromArgb( 11,  11,  15), Color.FromArgb( 19,  19,  27), Color.FromArgb( 27,  27,  39), Color.FromArgb( 39,  39,  51), 
				Color.FromArgb( 47,  47,  63), Color.FromArgb( 55,  55,  75), Color.FromArgb( 63,  63,  87), Color.FromArgb( 71,  71, 103), 
				Color.FromArgb( 79,  79, 115), Color.FromArgb( 91,  91, 127), Color.FromArgb( 99,  99, 139), Color.FromArgb(107, 107, 151), 
				Color.FromArgb(115, 115, 163), Color.FromArgb(123, 123, 175), Color.FromArgb(131, 131, 187), Color.FromArgb(139, 139, 203), 
				Color.FromArgb(  0,   0,   0), Color.FromArgb(  7,   7,   0), Color.FromArgb( 11,  11,   0), Color.FromArgb( 19,  19,   0), 
				Color.FromArgb( 27,  27,   0), Color.FromArgb( 35,  35,   0), Color.FromArgb( 43,  43,   7), Color.FromArgb( 47,  47,   7), 
				Color.FromArgb( 55,  55,   7), Color.FromArgb( 63,  63,   7), Color.FromArgb( 71,  71,   7), Color.FromArgb( 75,  75,  11), 
				Color.FromArgb( 83,  83,  11), Color.FromArgb( 91,  91,  11), Color.FromArgb( 99,  99,  11), Color.FromArgb(107, 107,  15), 
				Color.FromArgb(  7,   0,   0), Color.FromArgb( 15,   0,   0), Color.FromArgb( 23,   0,   0), Color.FromArgb( 31,   0,   0), 
				Color.FromArgb( 39,   0,   0), Color.FromArgb( 47,   0,   0), Color.FromArgb( 55,   0,   0), Color.FromArgb( 63,   0,   0), 
				Color.FromArgb( 71,   0,   0), Color.FromArgb( 79,   0,   0), Color.FromArgb( 87,   0,   0), Color.FromArgb( 95,   0,   0), 
				Color.FromArgb(103,   0,   0), Color.FromArgb(111,   0,   0), Color.FromArgb(119,   0,   0), Color.FromArgb(127,   0,   0), 
				Color.FromArgb( 19,  19,   0), Color.FromArgb( 27,  27,   0), Color.FromArgb( 35,  35,   0), Color.FromArgb( 47,  43,   0), 
				Color.FromArgb( 55,  47,   0), Color.FromArgb( 67,  55,   0), Color.FromArgb( 75,  59,   7), Color.FromArgb( 87,  67,   7), 
				Color.FromArgb( 95,  71,   7), Color.FromArgb(107,  75,  11), Color.FromArgb(119,  83,  15), Color.FromArgb(131,  87,  19), 
				Color.FromArgb(139,  91,  19), Color.FromArgb(151,  95,  27), Color.FromArgb(163,  99,  31), Color.FromArgb(175, 103,  35), 
				Color.FromArgb( 35,  19,   7), Color.FromArgb( 47,  23,  11), Color.FromArgb( 59,  31,  15), Color.FromArgb( 75,  35,  19), 
				Color.FromArgb( 87,  43,  23), Color.FromArgb( 99,  47,  31), Color.FromArgb(115,  55,  35), Color.FromArgb(127,  59,  43), 
				Color.FromArgb(143,  67,  51), Color.FromArgb(159,  79,  51), Color.FromArgb(175,  99,  47), Color.FromArgb(191, 119,  47), 
				Color.FromArgb(207, 143,  43), Color.FromArgb(223, 171,  39), Color.FromArgb(239, 203,  31), Color.FromArgb(255, 243,  27), 
				Color.FromArgb( 11,   7,   0), Color.FromArgb( 27,  19,   0), Color.FromArgb( 43,  35,  15), Color.FromArgb( 55,  43,  19), 
				Color.FromArgb( 71,  51,  27), Color.FromArgb( 83,  55,  35), Color.FromArgb( 99,  63,  43), Color.FromArgb(111,  71,  51), 
				Color.FromArgb(127,  83,  63), Color.FromArgb(139,  95,  71), Color.FromArgb(155, 107,  83), Color.FromArgb(167, 123,  95), 
				Color.FromArgb(183, 135, 107), Color.FromArgb(195, 147, 123), Color.FromArgb(211, 163, 139), Color.FromArgb(227, 179, 151), 
				Color.FromArgb(171, 139, 163), Color.FromArgb(159, 127, 151), Color.FromArgb(147, 115, 135), Color.FromArgb(139, 103, 123), 
				Color.FromArgb(127,  91, 111), Color.FromArgb(119,  83,  99), Color.FromArgb(107,  75,  87), Color.FromArgb( 95,  63,  75), 
				Color.FromArgb( 87,  55,  67), Color.FromArgb( 75,  47,  55), Color.FromArgb( 67,  39,  47), Color.FromArgb( 55,  31,  35), 
				Color.FromArgb( 43,  23,  27), Color.FromArgb( 35,  19,  19), Color.FromArgb( 23,  11,  11), Color.FromArgb( 15,   7,   7), 
				Color.FromArgb(187, 115, 159), Color.FromArgb(175, 107, 143), Color.FromArgb(163,  95, 131), Color.FromArgb(151,  87, 119), 
				Color.FromArgb(139,  79, 107), Color.FromArgb(127,  75,  95), Color.FromArgb(115,  67,  83), Color.FromArgb(107,  59,  75), 
				Color.FromArgb( 95,  51,  63), Color.FromArgb( 83,  43,  55), Color.FromArgb( 71,  35,  43), Color.FromArgb( 59,  31,  35), 
				Color.FromArgb( 47,  23,  27), Color.FromArgb( 35,  19,  19), Color.FromArgb( 23,  11,  11), Color.FromArgb( 15,   7,   7), 
				Color.FromArgb(219, 195, 187), Color.FromArgb(203, 179, 167), Color.FromArgb(191, 163, 155), Color.FromArgb(175, 151, 139), 
				Color.FromArgb(163, 135, 123), Color.FromArgb(151, 123, 111), Color.FromArgb(135, 111,  95), Color.FromArgb(123,  99,  83), 
				Color.FromArgb(107,  87,  71), Color.FromArgb( 95,  75,  59), Color.FromArgb( 83,  63,  51), Color.FromArgb( 67,  51,  39), 
				Color.FromArgb( 55,  43,  31), Color.FromArgb( 39,  31,  23), Color.FromArgb( 27,  19,  15), Color.FromArgb( 15,  11,   7), 
				Color.FromArgb(111, 131, 123), Color.FromArgb(103, 123, 111), Color.FromArgb( 95, 115, 103), Color.FromArgb( 87, 107,  95), 
				Color.FromArgb( 79,  99,  87), Color.FromArgb( 71,  91,  79), Color.FromArgb( 63,  83,  71), Color.FromArgb( 55,  75,  63), 
				Color.FromArgb( 47,  67,  55), Color.FromArgb( 43,  59,  47), Color.FromArgb( 35,  51,  39), Color.FromArgb( 31,  43,  31), 
				Color.FromArgb( 23,  35,  23), Color.FromArgb( 15,  27,  19), Color.FromArgb( 11,  19,  11), Color.FromArgb(  7,  11,   7), 
				Color.FromArgb(255, 243,  27), Color.FromArgb(239, 223,  23), Color.FromArgb(219, 203,  19), Color.FromArgb(203, 183,  15), 
				Color.FromArgb(187, 167,  15), Color.FromArgb(171, 151,  11), Color.FromArgb(155, 131,   7), Color.FromArgb(139, 115,   7), 
				Color.FromArgb(123,  99,   7), Color.FromArgb(107,  83,   0), Color.FromArgb( 91,  71,   0), Color.FromArgb( 75,  55,   0), 
				Color.FromArgb( 59,  43,   0), Color.FromArgb( 43,  31,   0), Color.FromArgb( 27,  15,   0), Color.FromArgb( 11,   7,   0), 
				Color.FromArgb(  0,   0, 255), Color.FromArgb( 11,  11, 239), Color.FromArgb( 19,  19, 223), Color.FromArgb( 27,  27, 207), 
				Color.FromArgb( 35,  35, 191), Color.FromArgb( 43,  43, 175), Color.FromArgb( 47,  47, 159), Color.FromArgb( 47,  47, 143), 
				Color.FromArgb( 47,  47, 127), Color.FromArgb( 47,  47, 111), Color.FromArgb( 47,  47,  95), Color.FromArgb( 43,  43,  79), 
				Color.FromArgb( 35,  35,  63), Color.FromArgb( 27,  27,  47), Color.FromArgb( 19,  19,  31), Color.FromArgb( 11,  11,  15), 
				Color.FromArgb( 43,   0,   0), Color.FromArgb( 59,   0,   0), Color.FromArgb( 75,   7,   0), Color.FromArgb( 95,   7,   0), 
				Color.FromArgb(111,  15,   0), Color.FromArgb(127,  23,   7), Color.FromArgb(147,  31,   7), Color.FromArgb(163,  39,  11), 
				Color.FromArgb(183,  51,  15), Color.FromArgb(195,  75,  27), Color.FromArgb(207,  99,  43), Color.FromArgb(219, 127,  59), 
				Color.FromArgb(227, 151,  79), Color.FromArgb(231, 171,  95), Color.FromArgb(239, 191, 119), Color.FromArgb(247, 211, 139), 
				Color.FromArgb(167, 123,  59), Color.FromArgb(183, 155,  55), Color.FromArgb(199, 195,  55), Color.FromArgb(231, 227,  87), 
				Color.FromArgb(127, 191, 255), Color.FromArgb(171, 231, 255), Color.FromArgb(215, 255, 255), Color.FromArgb(103,   0,   0), 
				Color.FromArgb(139,   0,   0), Color.FromArgb(179,   0,   0), Color.FromArgb(215,   0,   0), Color.FromArgb(255,   0,   0), 
				Color.FromArgb(255, 243, 147), Color.FromArgb(255, 247, 199), Color.FromArgb(255, 255, 255), Color.FromArgb(159,  91,  83)
			};

			public class Constants
			{
				public const int Header = (('O' << 24) + ('P' << 16) + ('D' << 8) + 'I'),
				Version = 6,

				MaxTriangles = 2048,
				MaxVerts = 1024,
				MaxFrames = 256,
				MaxSkins = 32,
				MaxSkinName = 64,
				MaxFrameName = 16;
			}

			public class Header
			{
				public int id = 0;
				public int version = 0;

				public Vector3 scale = Vector3.Empty;
				public Vector3 translate = Vector3.Empty;
				public float boundingradius = 0;
				public Vector3 eyeposition = Vector3.Empty;

				public int num_skins = 0;
				public int skinwidth = 0;
				public int skinheight = 0;

				public int num_verts = 0;
				public int num_tris = 0;
				public int num_frames = 0;

				public int synctype = 0;
				public int flags = 0;
				public float size = 0;

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(id);
					writer.Write(version);
					scale.Write(writer);
					translate.Write(writer);
					writer.Write(boundingradius);
					eyeposition.Write(writer);
					writer.Write(num_skins);
					writer.Write(skinwidth);
					writer.Write(skinheight);
					writer.Write(num_verts);
					writer.Write(num_tris);
					writer.Write(num_frames);
					writer.Write(synctype);
					writer.Write(flags);
					writer.Write(size);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					id = reader.ReadInt32();
					version = reader.ReadInt32();
					scale.Read(reader);
					translate.Read(reader);
					boundingradius = reader.ReadSingle();
					eyeposition.Read(reader);
					num_skins = reader.ReadInt32();
					skinwidth = reader.ReadInt32();
					skinheight = reader.ReadInt32();
					num_verts = reader.ReadInt32();
					num_tris = reader.ReadInt32();
					num_frames = reader.ReadInt32();
					synctype = reader.ReadInt32();
					flags = reader.ReadInt32();
					size = reader.ReadSingle();
				}

				public const int StructureSize = 84;

				public void Clear()
				{
					id = version = skinwidth = skinheight = num_skins = num_verts = num_tris= num_frames = synctype = flags = 0;
					size = boundingradius = 0;
					scale = translate = eyeposition = Vector3.Empty;
				}
			};

			public struct Skin
			{
				public int group;
				public byte[] data;

				public void Write(System.IO.BinaryWriter writer, int size)
				{
					writer.Write(group);
					writer.Write(data, 0, size);
				}

				public void Read(System.IO.BinaryReader reader, int size)
				{
					group = reader.ReadInt32();
					data = reader.ReadBytes(size);
				}
			};

			public struct SkinVertex
			{
				public int onseam;
				public int s, t;
				public CPluginSkinVertice BackFaceVert;
				public int BackFaceIndex;

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(onseam);
					writer.Write(s);
					writer.Write(t);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					onseam = reader.ReadInt32();
					s = reader.ReadInt32();
					t = reader.ReadInt32();
				}
			};

			public struct Triangle
			{
				public int facesfront;
				public int[] vec;

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(facesfront);
					for (int i = 0; i < 3; ++i)
						writer.Write(vec[i]);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					vec = new int[3];
					facesfront = reader.ReadInt32();
					for (int i = 0; i < 3; ++i)
						vec[i] = reader.ReadInt32();
				}
			};

			public struct AnimVertex
			{
				public byte[] v;    // X,Y,Z coordinate, packed on 0-255
				public byte lightnormalindex;     // index of the vertex normal

				public const int StructureSize = 4;

				public void Init()
				{
					v = new byte[3];
				}

				public void Write(System.IO.BinaryWriter writer)
				{
					writer.Write(v, 0, 3);
					writer.Write(lightnormalindex);
				}

				public void Read(System.IO.BinaryReader reader)
				{
					Init();

					v = reader.ReadBytes(3);
					lightnormalindex = reader.ReadByte();
				}
			};

			public class Frame
			{
				public AnimVertex bboxmin = new AnimVertex();
				public AnimVertex bboxmax = new AnimVertex();
				public string name = "";
				public List<AnimVertex> verts = new List<AnimVertex>();

				public void Write(System.IO.BinaryWriter writer)
				{
					bboxmin.Write(writer);
					bboxmax.Write(writer);

					CCString.Write(writer, name, Constants.MaxFrameName);

					for (int i = 0; i < verts.Count; ++i)
						verts[i].Write(writer);
				}

				public void Read(System.IO.BinaryReader reader, int count)
				{
					bboxmin.Read(reader);
					bboxmax.Read(reader);

					name = CCString.Read(reader, Constants.MaxFrameName);

					for (int i = 0; i < count; ++i)
					{
						AnimVertex v = new AnimVertex();
						v.Read(reader);
						verts.Add(v);
					}
				}
			};

			class MDLTooManySkinsException : Exception
			{
				public MDLTooManySkinsException()
					: base("Model has too many skins to export to MDL, the maximum allowed is "+Constants.MaxSkins.ToString())
				{
				}
			}

			class MDLTooManyFramesException : Exception
			{
				public MDLTooManyFramesException()
					: base("Model has too many frames to export to MDL, the maximum allowed is "+Constants.MaxFrames.ToString())
				{
				}
			}

			class MDLNotFileException : Exception
			{
				public MDLNotFileException()
					: base("File is not a valid MDL model")
				{
				}
			}

			class MDLSkinGroupsNotSupportedException : Exception
			{
				public MDLSkinGroupsNotSupportedException()
					: base("Skin and frame groups are not supported")
				{
				}
			}

			public static void SaveToMDL(CPluginModel Model, string FileName)
			{
			}
			public static void LoadFromMDL(CPluginModel Model, string FileName)
			{
				CMDLModel.Header tHead = new CMDLModel.Header();

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

						if (tHead.id != CMDLModel.Constants.Header || tHead.version != CMDLModel.Constants.Version)
							throw new MDLNotFileException();

						if (tHead.num_skins > CMDLModel.Constants.MaxSkins)
							throw new MDLTooManySkinsException();
						if (tHead.num_frames > CMDLModel.Constants.MaxFrames)
							throw new MDLTooManyFramesException();

						for (int n = 0; n < tHead.num_skins; n++)
						{
							CMDLModel.Skin skin = new CMDLModel.Skin();

							skin.Read(reader, tHead.skinwidth * tHead.skinheight);

							if (skin.group != 0)
								throw new MDLSkinGroupsNotSupportedException();

							// Compile into a bitmap
							Bitmap bmp = new Bitmap(tHead.skinwidth, tHead.skinheight);

							for (int y = 0; y < tHead.skinheight; ++y)
							{
								for (int x = 0; x < tHead.skinwidth; ++x)
								{
									//RealData[((tHead.skinwidth * 4) * y) + rx] = ColorMap[skin.data[(tHead.skinwidth * y) + x]].B;
									//RealData[((tHead.skinwidth * 4) * y) + rx + 1] = ColorMap[skin.data[(tHead.skinwidth * y) + x]].G;
									//RealData[((tHead.skinwidth * 4) * y) + rx + 2] = ColorMap[skin.data[(tHead.skinwidth * y) + x]].R;
									//RealData[((tHead.skinwidth * 4) * y) + rx + 3] = 255;
									bmp.SetPixel(x, y, ColorMap[skin.data[(tHead.skinwidth * y) + x]]);
								}
							}

							CPluginSkin Skin = Model.CreateSkin();
							Skin.Bitmap = bmp;
							Skin.Width = tHead.skinwidth;
							Skin.Height = tHead.skinheight;
						}

						CMDLModel.SkinVertex[] tempskinverts = new CMDLModel.SkinVertex[tHead.num_verts];

						for (int i = 0; i < tHead.num_verts; ++i)
							tempskinverts[i].Read(reader);

						for (int i = 0; i < tHead.num_frames; ++i)
							Model.CreateFrame();

						for (int i = 0; i < tHead.num_verts; i++)
						{
							CPluginSkinVertice rsk = Model.CreateSkinVertice();
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

							Model.CreateVertice();
						}

						CMDLModel.Triangle[] temptris = new CMDLModel.Triangle[tHead.num_tris];

						for (int i = 0; i < tHead.num_tris; ++i)
							temptris[i].Read(reader);

						for (int i = 0; i < tHead.num_tris; i++)
						{
							CPluginTriangle rt = Model.CreateTriangle();

							rt.Vertices[0] = (short)temptris[i].vec[0];
							rt.Vertices[1] = (short)temptris[i].vec[1];
							rt.Vertices[2] = (short)temptris[i].vec[2];
							rt.SkinVertices[0] = rt.Vertices[0];
							rt.SkinVertices[1] = rt.Vertices[1];
							rt.SkinVertices[2] = rt.Vertices[2];

							// Calculate the backface skinvert positioning
							for (int j = 0; j < 3; ++j)
							{
								if (temptris[i].facesfront == 0 && tempskinverts[rt.Vertices[j]].onseam != 0)
								{
									if (tempskinverts[rt.Vertices[j]].BackFaceVert == null)
									{
										CPluginSkinVertice nSkinVertex = Model.CreateSkinVertice();
										nSkinVertex.s = Model.GetSkinVerticeAt(rt.Vertices[j]).s + 0.5f;
										nSkinVertex.t = Model.GetSkinVerticeAt(rt.Vertices[j]).t;

										tempskinverts[rt.Vertices[j]].BackFaceVert = nSkinVertex;
										tempskinverts[rt.Vertices[j]].BackFaceIndex = Model.GetSkinVerticeCount() - 1;
									}

									rt.SkinVertices[j] = (short)tempskinverts[rt.Vertices[j]].BackFaceIndex;
								}
							}
						}

						for (int n = 0; n < tHead.num_frames; n++)
						{
							CMDLModel.Frame Frame = new CMDLModel.Frame();
							int type = reader.ReadInt32();

							if (type != 0)
								throw new MDLSkinGroupsNotSupportedException();

							Frame.Read(reader, tHead.num_verts);

							CPluginFrame TheFrame = Model.GetFrameAt(n);

							TheFrame.FrameName = Frame.name;
							for (int v = 0; v < tHead.num_verts; v++)
							{
								CPluginVertice rv = Model.GetVerticeAt(v);
								float newx, newy, newz;

								newx = Frame.verts[v].v[0];
								newy = Frame.verts[v].v[1];
								newz = Frame.verts[v].v[2];
								newx *= tHead.scale[0];
								newy *= tHead.scale[1];
								newz *= tHead.scale[2];
								newx += tHead.translate[0];
								newy += tHead.translate[1];
								newz += tHead.translate[2];

								rv.GetFrameDataAt(n).Vector = new Vector3(newx, newy, newz);
							}
						}
					}
				}
			}
		}

		/*public class CMDLExportPlugin : CPlugin
		{
			public CMDLExportPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export MDL...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "MDL Models (*.mdl)|*.mdl|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMDLModel.SaveToMDL(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CMDLExportPlugin();
			}
		}*/

		public class CMDLImportPlugin : CPlugin
		{
			public CMDLImportPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import MDL...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "MDL Models (*.mdl)|*.mdl|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMDLModel.LoadFromMDL(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CMDLImportPlugin();
			}
		}
	}
}
