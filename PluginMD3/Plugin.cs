using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VCMDL.NET
{
	class CMD3Model
	{
		public class Constants
		{
			public const int ID = (('3' << 24) + ('P' << 16) + ('D' << 8) + 'I'),
				Version = 15,
				MaxFrames = 1024,
				MaxTags = 16,
				MaxSurfaces = 32,
				MaxShaders = 256,
				MaxVertices = 4096,
				MaxTriangles = 8192;
		}

		public class Header
		{
			public int Ident = 0;
			public int Version = 0;
			public string Name = "";
			public int Flags = 0;
			public int NumFrames = 0;
			public int NumTags = 0;
			public int NumSurfaces = 0;
			public int NumSkins = 0;
			public int OfsFrames = 0;
			public int OfsTags = 0;
			public int OfsSurfaces = 0;
			public int OfsEnd = 0;

			public void Read(System.IO.BinaryReader reader)
			{
				Ident = reader.ReadInt32();
				Version = reader.ReadInt32();
				Name = CCString.Read(reader, 64);
				Flags = reader.ReadInt32();
				NumFrames = reader.ReadInt32();
				NumTags = reader.ReadInt32();
				NumSurfaces = reader.ReadInt32();
				NumSkins = reader.ReadInt32();
				OfsFrames = reader.ReadInt32();
				OfsTags = reader.ReadInt32();
				OfsSurfaces = reader.ReadInt32();
				OfsEnd = reader.ReadInt32();
			}
		}

		public class Frame
		{
			public Vector3 MinBounds = Vector3.Empty;
			public Vector3 MaxBounds = Vector3.Empty;
			public Vector3 LocalOrigin = Vector3.Empty;
			public float Radius;
			public string Name = "";

			public void Read(System.IO.BinaryReader reader)
			{
				MinBounds.Read(reader);
				MaxBounds.Read(reader);
				LocalOrigin.Read(reader);
				Radius = reader.ReadSingle();
				Name = CCString.Read(reader, 16);
			}
		}

		public class Tag
		{
			public string Name = "";
			public Vector3 Origin = Vector3.Empty;
			public Matrix3 Axis = new Matrix3();

			public void Read(System.IO.BinaryReader reader)
			{
				Name = CCString.Read(reader, 64);
				Origin.Read(reader);
				Axis.Read(reader);
			}
		}

		public class Surface
		{
			public int Ident;
			public string Name = "";
			public int Flags;
			public int NumFrames;
			public int NumShaders;
			public int NumVerts;
			public int NumTriangles;
			public int OfsTriangles;
			public int OfsShaders;
			public int OfsSkinVertices;
			public int OfsVertices;
			public int OfsEnd;
			public List<Shader> Shaders = new List<Shader>();
			public List<Triangle> Triangles = new List<Triangle>();
			public List<SkinVertice> SkinVertices = new List<SkinVertice>();
			public List<Vertice>[] Vertices;

			public void Read(System.IO.BinaryReader reader)
			{
				int tell = (int)reader.BaseStream.Position;

				Ident = reader.ReadInt32();
				Name = CCString.Read(reader, 64);
				Flags = reader.ReadInt32();
				NumFrames = reader.ReadInt32();
				NumShaders = reader.ReadInt32();
				NumVerts = reader.ReadInt32();
				NumTriangles = reader.ReadInt32();
				OfsTriangles = reader.ReadInt32();
				OfsShaders = reader.ReadInt32();
				OfsSkinVertices = reader.ReadInt32();
				OfsVertices = reader.ReadInt32();
				OfsEnd = reader.ReadInt32();

				reader.BaseStream.Seek(tell + OfsTriangles, SeekOrigin.Begin);
				for (int i = 0; i < NumTriangles; ++i)
					Triangles.Add(Triangle.Read(reader));

				reader.BaseStream.Seek(tell + OfsShaders, SeekOrigin.Begin);
				for (int i = 0; i < NumShaders; ++i)
					Shaders.Add(Shader.Read(reader));

				reader.BaseStream.Seek(tell + OfsSkinVertices, SeekOrigin.Begin);
				for (int i = 0; i < NumVerts; ++i)
					SkinVertices.Add(SkinVertice.Read(reader));

				Vertices = new List<Vertice>[NumFrames];

				reader.BaseStream.Seek(tell + OfsVertices, SeekOrigin.Begin);
				for (int z = 0; z < NumFrames; ++z)
				{
					Vertices[z] = new List<Vertice>();
					for (int i = 0; i < NumVerts; ++i)
						Vertices[z].Add(Vertice.Read(reader));
				}
			}
		}

		public class Shader
		{
			public string Name;
			public int Index;

			public static Shader Read(System.IO.BinaryReader reader)
			{
				Shader shader = new Shader();
				shader.Name = CCString.Read(reader, 64);
				shader.Index = reader.ReadInt32();
				return shader;
			}
		}

		public class Triangle
		{
			public int[] Indexes = new int[3];

			public static Triangle Read(System.IO.BinaryReader reader)
			{
				Triangle triangle = new Triangle();
				for (int i = 0; i < 3; ++i)
					triangle.Indexes[i] = reader.ReadInt32();
				return triangle;
			}
		}

		public class SkinVertice
		{
			public float s;
			public float t;

			public static SkinVertice Read(System.IO.BinaryReader reader)
			{
				SkinVertice vertice = new SkinVertice();
				vertice.s = reader.ReadSingle();
				vertice.t = reader.ReadSingle();
				return vertice;
			}
		}

		public class Vertice
		{
			public Vector3 Coord;
			public Vector3 Normal;

			public static Vertice Read(System.IO.BinaryReader reader)
			{
				Vertice vertice = new Vertice();

				vertice.Coord = new Vector3(((float)reader.ReadInt16()) * 1.0f/64, ((float)reader.ReadInt16()) * 1.0f/64, ((float)reader.ReadInt16()) * 1.0f/64);

				float lat = reader.ReadByte() * (float)(2 * Math.PI) / 255;
				float lng = reader.ReadByte() * (float)(2 * Math.PI) / 255;
				vertice.Normal = new Vector3((float)(Math.Cos(lat) * Math.Sin(lng)), (float)(Math.Sin(lat) * Math.Sin(lng)), (float)Math.Cos(lng));

				return vertice;
			}
		}

		public static void LoadFromMD3(CPluginModel Model, string FileName)
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

					Header Head = new Header();
					Head.Read(reader);

					if (Head.Ident != CMD3Model.Constants.ID || Head.Version != CMD3Model.Constants.Version)
						return;

					reader.BaseStream.Seek(Head.OfsFrames, SeekOrigin.Begin);
					for (int i = 0; i < Head.NumFrames; ++i)
					{
						Frame Frame = new Frame();
						Frame.Read(reader);

						var frame = Model.CreateFrame();
						frame.FrameName = Frame.Name;
					}

					reader.BaseStream.Seek(Head.OfsTags, SeekOrigin.Begin);
					List<Tag> Tags = new List<Tag>();
					for (int z = 0; z < Head.NumFrames; ++z)
					{
						for (int i = 0; i < Head.NumTags; ++i)
						{
							Tag Tag = new Tag();
							Tag.Read(reader);
							Tags.Add(Tag);
						}
					}

					int meshofs = Head.OfsSurfaces;

					List<Surface> Surfaces = new List<Surface>();
					for (int i = 0; i < Head.NumSurfaces; ++i)
					{
						reader.BaseStream.Seek(meshofs, SeekOrigin.Begin);

						Surface Surface = new Surface();
						Surface.Read(reader);
						Surfaces.Add(Surface);

						meshofs += Surface.OfsEnd;
					}

					var matDict = new Dictionary<string, CPluginSkin>();

					for (int f = 0; f < Head.NumSurfaces; ++f)
					{
						if (Surfaces[f].NumShaders != 0)
						{
							if (!matDict.ContainsKey(Surfaces[f].Shaders[0].Name))
							{
								var skin = Model.CreateSkin();
								skin.Bitmap = null;
								skin.Path = Surfaces[f].Shaders[0].Name;
								skin.Width = skin.Height = 0;

								matDict.Add(Surfaces[f].Shaders[0].Name, skin);
							}
						}
					}

					for (int i = 0; i < Head.NumSurfaces; ++i)
					{
						CMD3Model.Surface Surface = Surfaces[i];
						var mesh = Model.CreateMesh();
						mesh.SetName(Surface.Name);

						if (Surface.NumShaders != 0)
							mesh.SetSkin(matDict[Surface.Shaders[0].Name]);

						for (int x = 0; x < Surface.NumVerts; ++x)
						{
							CPluginVertice v = mesh.CreateVertice();

							for (int f = 0; f < Head.NumFrames; ++f)
							{
								v.GetFrameDataAt(f).Vector = Surface.Vertices[f][x].Coord;
								v.GetFrameDataAt(f).Normal = Surface.Vertices[f][x].Normal;
							}
						}

						for (int x = 0; x < Surface.NumVerts; ++x)
						{
							CPluginSkinVertice sv = mesh.CreateSkinVertice();
							sv.s = Surface.SkinVertices[x].s;
							sv.t = Surface.SkinVertices[x].t;
						}

						for (int x = 0; x < Surface.NumTriangles; ++x)
						{
							CPluginTriangle tri = mesh.CreateTriangle();
							for (int z = 0; z < 3; ++z)
								tri.SkinVertices[z] = tri.Vertices[z] = Surface.Triangles[x].Indexes[z];
						}
					}
				}
			}
		}
	}

	public class Plugin
	{
		public class CMD3ImportPlugin : CPlugin
		{
			public CMD3ImportPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import MD3...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "MD3 Models (*.md3)|*.md3|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				CMD3Model.LoadFromMD3(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CMD3ImportPlugin();
			}
		}
	}
}
