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
		public class CRawPlugin : CPlugin
		{
			public CRawPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export Custom Text Vertices...";
			}

			static int DictContainsVert(Dictionary<int, List<int>> VertDict, int i)
			{
				int Key = 0;

				// See if the dictionary already contains this vertice somewhere
				foreach (var lv in VertDict.Values)
				{
					foreach (var iv in lv)
						if (iv == i)
							return Key;

					Key++;
				}

				return -1;
			}

			static void RecursiveCheck(CPluginModel Model, int VerticeIndex, Dictionary<int, List<int>> VertDict, int CurIndex)
			{
				for (int t = 0; t < Model.GetTriangleCount(); ++t)
				{
					for (int z = 0; z < 3; ++z)
					{
						if (Model.GetTriangleAt(t).Vertices[z] == VerticeIndex)
						{
							for (int y = 0; y < 3; ++y)
							{
								if (y == z || DictContainsVert(VertDict, Model.GetTriangleAt(t).Vertices[y]) != -1)
									continue;

								VertDict[CurIndex].Add(Model.GetTriangleAt(t).Vertices[y]);
								RecursiveCheck(Model, Model.GetTriangleAt(t).Vertices[y], VertDict, CurIndex);
							}
							break;
						}
					}
				}
			}

			PluginMS3D.EnterThingy eft = new PluginMS3D.EnterThingy();

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				Dictionary<int, List<int>> VertDict = new Dictionary<int, List<int>>();
				int CurIndex = -1;

				for (int i = 0; i < Model.GetVerticeCount(); ++i)
				{
					if (DictContainsVert(VertDict, i) != -1)
						continue;

					CurIndex++;
					VertDict[CurIndex] = new List<int>();

					RecursiveCheck(Model, i, VertDict, CurIndex);
				}

				if (eft.ShowDialog() == DialogResult.Cancel)
					return false;

				bool OnlySelected = false;
				if (MessageBox.Show("Only process selected vertices?", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
					OnlySelected = true;

				using (System.IO.FileStream file = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file))
					{
						if (writer == null)
							throw new System.IO.FileLoadException();

						writer.Write(eft.BeforeFile.Text.Replace("%v", Model.GetVerticeCount().ToString()));

						foreach (var kvp in VertDict)
						{
							writer.Write(eft.BeforeGroup.Text.Replace("%g", kvp.Value.Count.ToString()));

							foreach (var i in kvp.Value)
							{
								if (OnlySelected && (Model.GetVerticeAt(i).Flags & EVerticeFlags.Selected) == 0)
									continue;

								writer.Write(eft.BeforeVert.Text
									.Replace("%x", Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.x.ToString())
									.Replace("%y", Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.y.ToString())
									.Replace("%z", Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.z.ToString()));
								//writer.WriteLine(Model.GetVerticeAt(i).GetFrameDataAt(0).Vector);

								writer.WriteLine(eft.AfterVert.Text);
							}

							writer.WriteLine(eft.AfterGroup.Text);
						}

						writer.WriteLine(eft.AfterFile.Text);
					}
				}

				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CRawPlugin();
			}
		}

		class CMS3DModel
		{
			struct Header
			{
				public string id; // 10, always "MS3D000000"
				public int version; // 4

				public void Read(System.IO.BinaryReader reader)
				{
					id = CCString.Read(reader, 10);
					version = reader.ReadInt32();
				}
			}

			struct Vertex
			{
				public byte flags; // SELECTED | SELECTED2 | HIDDEN
				public float[] vertex; //
				public sbyte boneId; // -1 = no bone
				public byte referenceCount;

				public void Read(System.IO.BinaryReader reader)
				{
					flags = reader.ReadByte();
					vertex = new float[3];
					for (int i = 0; i < 3; ++i)
						vertex[i] = reader.ReadSingle();
					boneId = reader.ReadSByte();
					referenceCount = reader.ReadByte();
				}

				public CPluginVertice ToVertex(CPluginModel Model)
				{
					CPluginVertice v = Model.CreateVertice();
					v.GetFrameDataAt(0).Vector = new Vector3(vertex[0], vertex[1], vertex[2]);
					return v;
				}
			};

			struct Triangle
			{
				public ushort flags; // SELECTED | SELECTED2 | HIDDEN
				public ushort[] vertexIndices; //
				public float[,] vertexNormals; //
				public float[] s; //
				public float[] t; //
				public byte smoothingGroup; // 1 - 32
				public byte groupIndex; //

				public void Read(System.IO.BinaryReader reader)
				{
					flags = reader.ReadUInt16();
					vertexIndices = new ushort[3];
					for (int i = 0; i < 3; ++i)
						vertexIndices[i] = reader.ReadUInt16();
					vertexNormals = new float[3, 3];
					for (int x = 0; x < 3; ++x)
						for (int y = 0; y < 3; ++y)
							vertexNormals[x, y] = reader.ReadSingle();
					s = new float[3];
					for (int i = 0; i < 3; ++i)
						s[i] = reader.ReadSingle();
					t = new float[3];
					for (int i = 0; i < 3; ++i)
						t[i] = reader.ReadSingle();
					smoothingGroup = reader.ReadByte();
					groupIndex = reader.ReadByte();
				}
			};

			struct Edge
			{
				public ushort[] edgeIndices;

				public void Read(System.IO.BinaryReader reader)
				{
					edgeIndices = new ushort[2];
					for (int i = 0; i < 2; ++i)
						edgeIndices[i] = reader.ReadUInt16();
				}
			};

			struct Group
			{
				public byte flags; // SELECTED | HIDDEN
				public string name; // 32
				public ushort numtriangles; //
				public ushort[] triangleIndices; // the groups group the triangles
				public sbyte materialIndex; // -1 = no material

				public void Read(System.IO.BinaryReader reader)
				{
					flags = reader.ReadByte();
					name = CCString.Read(reader, 32);
					numtriangles = reader.ReadUInt16();
					triangleIndices = new ushort[numtriangles];
					for (int i = 0; i < numtriangles; ++i)
						triangleIndices[i] = reader.ReadUInt16();
					materialIndex = reader.ReadSByte();
				}
			};

			struct Material
			{
				public string name; // 32
				public float[] ambient; //
				public float[] diffuse; //
				public float[] specular; //
				public float[] emissive; //
				public float shininess; // 0.0f - 128.0f
				public float transparency; // 0.0f - 1.0f
				public sbyte mode; // 0, 1, 2 is unused now
				public string texture; // 128, texture.bmp
				public string alphamap; // 128, alpha.bmp

				public void Read(System.IO.BinaryReader reader)
				{
					name = CCString.Read(reader, 32);
					ambient = new float[4];
					for (int i = 0; i < 4; ++i)
						ambient[i] = reader.ReadSingle();
					diffuse = new float[4];
					for (int i = 0; i < 4; ++i)
						diffuse[i] = reader.ReadSingle();
					specular = new float[4];
					for (int i = 0; i < 4; ++i)
						specular[i] = reader.ReadSingle();
					emissive = new float[4];
					for (int i = 0; i < 4; ++i)
						emissive[i] = reader.ReadSingle();
					shininess = reader.ReadSingle();
					transparency = reader.ReadSingle();
					mode = reader.ReadSByte();
					texture = CCString.Read(reader, 128);
					alphamap = CCString.Read(reader, 128);
				}
			};

			struct KeyFrameRotation
			{
				public float time; // time in seconds
				public float[] rotation; // x, y, z angles

				public void Read(System.IO.BinaryReader reader)
				{
					time = reader.ReadSingle();
					rotation = new float[3];
					for (int i = 0; i < 3; ++i)
						rotation[i] = reader.ReadSingle();
				}
			};

			struct KeyFramePosition
			{
				public float time; // time in seconds
				public float[] position; // local position

				public void Read(System.IO.BinaryReader reader)
				{
					time = reader.ReadSingle();
					position = new float[3];
					for (int i = 0; i < 3; ++i)
						position[i] = reader.ReadSingle();
				}
			};

			struct Joint
			{
				public byte flags; // SELECTED | DIRTY
				public string name; // 32
				public string parentName; // 32
				public float[] rotation; // local reference matrix
				public float[] position;

				public ushort numKeyFramesRot; //
				public ushort numKeyFramesTrans; //

				public KeyFrameRotation[] keyFramesRot; // local animation matrices
				public KeyFramePosition[] keyFramesTrans; // local animation matrices

				public void Read(System.IO.BinaryReader reader)
				{
					flags = reader.ReadByte();
					name = CCString.Read(reader, 32);
					parentName = CCString.Read(reader, 32);
					rotation = new float[3];
					for (int i = 0; i < 3; ++i)
						rotation[i] = reader.ReadSingle();
					position = new float[3];
					for (int i = 0; i < 3; ++i)
						position[i] = reader.ReadSingle();

					numKeyFramesRot = reader.ReadUInt16();
					numKeyFramesTrans = reader.ReadUInt16();

					keyFramesRot = new KeyFrameRotation[numKeyFramesRot];
					keyFramesTrans = new KeyFramePosition[numKeyFramesTrans];

					for (int i = 0; i < numKeyFramesRot; ++i)
						keyFramesRot[i].Read(reader);
					for (int i = 0; i < numKeyFramesTrans; ++i)
						keyFramesTrans[i].Read(reader);
				}
			};

			class Model
			{
				public Header ModelHeader = new Header();
				public Vertex[] ModelVertices;
				public Triangle[] ModelTriangles;
				public Edge[] ModelEdges;
				public Group[] Groups;
				public Material[] Materials;
				public Joint[] Joints;
				public float AnimationFPS, CurrentTime;
				public int TotalFrames;

				static void Swap<T>(ref T left, ref T right)
				{
					T temp = right;
					right = left;
					left = temp;
				}

				static uint MAKEDWORD<T>(T low, T high) where T : IConvertible
				{
					return ((uint)(((low.ToUInt16(null))) | ((ushort)((high.ToUInt16(null)))) << 16));
				}

				public void Read(System.IO.BinaryReader reader)
				{
					ModelHeader.Read(reader);

					ModelVertices = new Vertex[reader.ReadUInt16()];
					for (int i = 0; i < ModelVertices.Length; ++i)
						ModelVertices[i].Read(reader);

					ModelTriangles = new Triangle[reader.ReadUInt16()];
					for (int i = 0; i < ModelTriangles.Length; ++i)
						ModelTriangles[i].Read(reader);

					List<uint> setEdgePair = new List<uint>();
					for (int i = 0; i < ModelTriangles.Length; ++i)
					{
						ushort a, b;
						a = ModelTriangles[i].vertexIndices[0];
						b = ModelTriangles[i].vertexIndices[1];
						if (a > b)
							Swap(ref a, ref b);
						if (setEdgePair.IndexOf(MAKEDWORD(a, b)) == -1)
							setEdgePair.Add(MAKEDWORD(a, b));

						a = ModelTriangles[i].vertexIndices[1];
						b = ModelTriangles[i].vertexIndices[2];
						if (a > b)
							Swap(ref a, ref b);
						if (setEdgePair.IndexOf(MAKEDWORD(a, b)) == -1)
							setEdgePair.Add(MAKEDWORD(a, b));

						a = ModelTriangles[i].vertexIndices[2];
						b = ModelTriangles[i].vertexIndices[0];
						if (a > b)
							Swap(ref a, ref b);
						if (setEdgePair.IndexOf(MAKEDWORD(a, b)) == -1)
							setEdgePair.Add(MAKEDWORD(a, b));
					}

					ModelEdges = new Edge[setEdgePair.Count];

					for (int i = 0; i < setEdgePair.Count; ++i)
					{
						ModelEdges[i].edgeIndices = new ushort[2];
						ModelEdges[i].edgeIndices[0] = (ushort)setEdgePair[i];
						ModelEdges[i].edgeIndices[1] = (ushort)((setEdgePair[i] >> 16) & 0xFFFF);
					}

					Groups = new Group[reader.ReadUInt16()];
					for (int i = 0; i < Groups.Length; ++i)
						Groups[i].Read(reader);

					Materials = new Material[reader.ReadUInt16()];
					for (int i = 0; i < Materials.Length; ++i)
						Materials[i].Read(reader);

					AnimationFPS = reader.ReadSingle();
					CurrentTime = reader.ReadSingle();
					TotalFrames = reader.ReadInt32();

					Joints = new Joint[reader.ReadUInt16()];
					for (int i = 0; i < Joints.Length; ++i)
						Joints[i].Read(reader);
				}
			};

			public static void LoadFromMS3D(CPluginModel Model, string FileName)
			{
				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Open))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
					{
						if (reader == null)
							throw new System.IO.FileLoadException();

						Model.Clear();

						Model MS3DModel = new Model();
						MS3DModel.Read(reader);

						CPluginFrame Frame = Model.CreateFrame();

						// Put together
						for (int i = 0; i < MS3DModel.ModelVertices.Length; ++i)
						{
							MS3DModel.ModelVertices[i].ToVertex(Model);
							Model.CreateSkinVertice();
						}

						for (int i = 0; i < MS3DModel.ModelTriangles.Length; ++i)
						{
							CPluginTriangle tri = Model.CreateTriangle();
							for (int x = 0; x < 3; ++x)
							{
								int v = 2 - x;

								tri.Vertices[x] = (short)MS3DModel.ModelTriangles[i].vertexIndices[v];
								tri.SkinVertices[x] = (short)MS3DModel.ModelTriangles[i].vertexIndices[v];
								Model.GetSkinVerticeAt(tri.SkinVertices[x]).s = MS3DModel.ModelTriangles[i].s[v];
								Model.GetSkinVerticeAt(tri.SkinVertices[x]).t = MS3DModel.ModelTriangles[i].t[v];
							}
						}

						Frame.FrameName = "Frame01";
					}
				}
			}
		}

		public class CImportMs3DPlugin : CPlugin
		{
			public CImportMs3DPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import MS3D...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "Milkshape 3D Models (*.ms3d)|*.ms3d";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMS3DModel.LoadFromMS3D(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CImportMs3DPlugin();
			}
		}


		public class CExportRawTextPlugin : CPlugin
		{
			public CExportRawTextPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export Raw Text...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "Text (*.txt)|*.txt";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				using (StreamWriter sw = new StreamWriter(dlg.FileName))
				{
					sw.WriteLine(Model.GetVerticeCount().ToString());
					sw.WriteLine();

					for (int i = 0; i < Model.GetVerticeCount(); ++i)
					{
						var vert = Model.GetVerticeAt(i);
						var data = vert.GetFrameDataAt(0);

						sw.WriteLine("{0} {1} {2}", data.Vector.x, data.Vector.y, data.Vector.z);
					}

					sw.WriteLine();

					for (int i = 0; i < Model.GetVerticeCount(); ++i)
					{
						var vert = Model.GetVerticeAt(i);
						var data = vert.GetFrameDataAt(0);

						sw.WriteLine("{0} {1} {2}", data.Normal.x, data.Normal.y, data.Normal.z);
					}

					sw.WriteLine();
					sw.WriteLine(Model.GetTriangleCount().ToString());
					sw.WriteLine();

					for (int i = 0; i < Model.GetTriangleCount(); ++i)
					{
						var t = Model.GetTriangleAt(i);

						sw.WriteLine("{0} {1} {2} {3} {4} {5}", t.Vertices[0], t.Vertices[1], t.Vertices[2], t.SkinVertices[0], t.SkinVertices[1], t.SkinVertices[2]);
					}

					sw.WriteLine();
					sw.WriteLine(Model.GetSkinVerticeCount().ToString());
					sw.WriteLine();

					for (int i = 0; i < Model.GetSkinVerticeCount(); ++i)
					{
						var t = Model.GetSkinVerticeAt(i);

						sw.WriteLine("{0} {1}", t.s, t.t);
					}
				}

				return false;
			}

			public static CPlugin CreatePlugin()
			{
				return new CExportRawTextPlugin();
			}
		}

		public class CImportPS2RawTextPlugin : CPlugin
		{
			public CImportPS2RawTextPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import PS2 Raw Text...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "Text (*.txt)|*.txt";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				using (StreamReader sw = new StreamReader(dlg.FileName))
				{
					Model.CreateFrame();
					bool reverse = false;
					while (!sw.EndOfStream)
					{
						var line = sw.ReadLine();
						var split = line.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);

						if (split.Length <= 4)
							continue;

						var tri = Model.CreateTriangle();
						int[] indices = new int[3];
						int index = 0;

						for (int i = 0; i < split.Length; i += 2)
						{
							indices[index++] = Model.GetVerticeCount();
							var v = Model.CreateVertice();

							var spl = split[i].Split();
							v.GetFrameDataAt(0).Vector = new Vector3(float.Parse(spl[1]) / 8, float.Parse(spl[2]) / 8, float.Parse(spl[3]) / 128);
						}

						if (!reverse)
							tri.Vertices = indices;
						else
							tri.Vertices = new int[] { indices[2], indices[1], indices[0] };

						reverse = !reverse;
					}
				}

				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CImportPS2RawTextPlugin();
			}
		}
	}
}
