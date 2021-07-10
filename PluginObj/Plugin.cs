using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public class Plugin
	{
		public class CExportOBJPlugin : CPlugin
		{
			public CExportOBJPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export OBJ...";
			}

			static void WriteHeader(System.IO.StreamWriter writer, CPluginModel Model)
			{
				writer.WriteLine("# Wavefront OBJ model, exported by VCMDL.NET");
				writer.WriteLine("# Notes:");
				writer.WriteLine("# Vertice count: "+Model.GetVerticeCount());
				writer.WriteLine("# Triangle count: "+Model.GetTriangleCount()+"\n\n");
			}

			static void WriteVertices(System.IO.StreamWriter writer, CPluginModel Model)
			{
				for (int i = 0; i < Model.GetVerticeCount(); ++i)
					writer.WriteLine("v "+Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.x.ToString() + " "+Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.z.ToString()+" "+Model.GetVerticeAt(i).GetFrameDataAt(0).Vector.y.ToString());
			}

			static void WriteTexCoords(System.IO.StreamWriter writer, CPluginModel Model)
			{
				for (int i = 0; i < Model.GetSkinVerticeCount(); ++i)
					writer.WriteLine("vt "+Model.GetSkinVerticeAt(i).s.ToString()+" "+(1-Model.GetSkinVerticeAt(i).t).ToString());
			}

			static void WriteNormals(System.IO.StreamWriter writer, CPluginModel Model)
			{
				for (int i = 0; i < Model.GetVerticeCount(); ++i)
					writer.WriteLine("vn "+Model.GetVerticeAt(i).GetFrameDataAt(0).Normal.x.ToString() + " "+Model.GetVerticeAt(i).GetFrameDataAt(0).Normal.z.ToString()+" "+Model.GetVerticeAt(i).GetFrameDataAt(0).Normal.y.ToString());
			}

			static void WriteFaces(System.IO.StreamWriter writer, CPluginModel Model)
			{
				writer.WriteLine("g Model\ns 1");
				for (int i = 0; i < Model.GetTriangleCount(); ++i)
				{
					writer.Write("f");
					for (int z = 0; z < 3; ++z)
						writer.Write(" "+(Model.GetTriangleAt(i).Vertices[z]+1).ToString()+"/"+(Model.GetTriangleAt(i).SkinVertices[z]+1).ToString()+"/"+(Model.GetTriangleAt(i).Vertices[z]+1).ToString());
					writer.Write("\n");
				}
			}

			public static void SaveToOBJ(CPluginModel Model, string FileName)
			{
				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file))
					{
						if (writer == null)
							throw new System.IO.FileLoadException();

						WriteHeader(writer, Model);
						WriteVertices(writer, Model);
						writer.Write("\n");
						WriteTexCoords(writer, Model);
						writer.Write("\n");
						WriteNormals(writer, Model);
						writer.Write("\n");
						WriteFaces(writer, Model);
					}
				}
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "OBJ Models (*.obj)|*.obj|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				SaveToOBJ(Model, dlg.FileName);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CExportOBJPlugin();
			}
		}

		public class CImportRawPlugin : CPlugin
		{
			public CImportRawPlugin()
				: base(EPluginType.PLUGIN_IMPORT)
			{
				Name = "Import Raw ASCII...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();

				dlg.Filter = "All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				using (System.IO.FileStream file = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Open))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
					{
						if (reader == null)
							throw new System.IO.FileLoadException();

						Model.Clear();

						CPluginFrame Frame = Model.CreateFrame();

						int NumBrushes = int.Parse(reader.ReadLine());

						int GlobVerticeNumber = 0;
						for (int i = 0; i < NumBrushes; ++i)
						{
							int NumSides = int.Parse(reader.ReadLine());

							for (int z = 0; z < NumSides; ++z)
							{
								int NumPoints = int.Parse(reader.ReadLine());

								int NumFacesCreatedHere = 0,
									PointsTallied = 0,
									LastFacesPoint = 0,
									WindingFace = 0;

								for (int p = 0; p < NumPoints; ++p)
								{
									string[] Points = reader.ReadLine().Split(' ');

									Vector3 v = new Vector3(float.Parse(Points[0]),
																float.Parse(Points[1]),
																float.Parse(Points[2]));

									CPluginVertice Vert = Model.CreateVertice();
									Vert.GetFrameDataAt(0).Vector = v;

									PointsTallied++;

									if (PointsTallied == 1 && NumFacesCreatedHere != 0)
									{
										NumFacesCreatedHere++;

										CPluginTriangle Tri = Model.CreateTriangle();
										Tri.Vertices[0] = WindingFace;
										Tri.Vertices[1] = GlobVerticeNumber - 1;
										Tri.Vertices[2] = GlobVerticeNumber;

										PointsTallied = 0;
									}
									else if (PointsTallied == 3 && NumFacesCreatedHere == 0)
									{
										NumFacesCreatedHere++;

										CPluginTriangle Tri = Model.CreateTriangle();
										Tri.Vertices[0] = GlobVerticeNumber - 2;
										Tri.Vertices[1] = GlobVerticeNumber - 1;
										Tri.Vertices[2] = GlobVerticeNumber;

										WindingFace = GlobVerticeNumber - 2;

										PointsTallied = 0;
									}

									LastFacesPoint++;
									GlobVerticeNumber++;
								}
							}
						}

						CPluginSkinVertice pv = Model.CreateSkinVertice();
						pv.s = pv.t = 0;
					}
				}
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CImportRawPlugin();
			}
		}

		public class CExportPlyFormat : CPlugin
		{
			public CExportPlyFormat()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export PLY...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "PLY Files (*.ply)|*.ply|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				using (System.IO.FileStream file = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file))
					{
						if (writer == null)
							throw new System.IO.FileLoadException();

						// write header
						writer.Write("ply\nformat ascii 1.0\nelement vertex "+Model.GetVerticeCount().ToString()+"\nproperty float32 x\nproperty float32 y\nproperty float32 z\nelement face "+Model.GetTriangleCount().ToString()+"\nproperty list uint8 int32 vertex_indices\nend_header\n");

						for (int i = 0; i < Model.GetVerticeCount(); ++i)
						{
							CPluginVerticeFrameData fd = Model.GetVerticeAt(i).GetFrameDataAt(0);
							writer.WriteLine(fd.Vector.ToString());
						}

						for (int i = 0; i < Model.GetTriangleCount(); ++i)
						{
							CPluginTriangle tri = Model.GetTriangleAt(i);
							writer.WriteLine("3 "+tri.Vertices[0]+" "+tri.Vertices[1]+" "+tri.Vertices[2]);
						}
					}
				}
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CExportPlyFormat();
			}
		}
	}
}
