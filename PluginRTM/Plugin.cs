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
		public class RTMModel
		{
			public class Header
			{
				public const int HeaderID = 'W' | ('H' << 8) | ('O' << 16) | ('A' << 24);
				public const int VersionID = '1' | ('0' << 8) | ('0' << 16) | ('0' << 24);

				public long NumTris { get; set; }
			}

			public class Triangle
			{
				public Vector3[] Vertices { get; set; }
				public float[,] TexCoords { get; set; }

				public Triangle(BinaryReader reader)
				{
					Vertices = new Vector3[3];
					TexCoords = new float[3, 2];
				}
			}
		}

		public class CRTMPlugin : CPlugin
		{
			public CRTMPlugin()
				: base(EPluginType.PLUGIN_EXPORT)
			{
				Name = "Export RTM...";
			}

			public override bool Execute(CPluginModel Model)
			{
				SaveFileDialog dlg = new SaveFileDialog();

				dlg.Filter = "All Files|*.rtm";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return false;

				using (System.IO.FileStream file = System.IO.File.Open(dlg.FileName, System.IO.FileMode.Create))
				{
					if (file == null)
						throw new System.IO.FileNotFoundException();

					using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(file))
					{
						if (writer == null)
							throw new System.IO.FileLoadException();

						writer.Write(RTMModel.Header.HeaderID);
						writer.Write(RTMModel.Header.VersionID);

						writer.Write((long)Model.GetFrameCount());
						writer.Write((long)Model.GetTriangleCount());

						// texcoords
						for (int i = 0; i < Model.GetTriangleCount(); ++i)
						{
							var x = Model.GetTriangleAt(i);

							for (int ti = 0; ti < 3; ++ti)
							{
								var tx = Model.GetSkinVerticeAt(x.SkinVertices[ti]);

								writer.Write(tx.s);
								writer.Write(tx.t);
							}
						}

						// triangles/frames
						for (int i = 0; i < Model.GetTriangleCount(); ++i)
						{
							var x = Model.GetTriangleAt(i);

							for (int fi = 0; fi < Model.GetFrameCount(); ++fi)
							{
								for (int ti = 0; ti < 3; ++ti)
								{
									var tx = Model.GetVerticeAt(x.Vertices[ti]);
									var fd = tx.GetFrameDataAt(fi);

									writer.Write(fd.Vector.x);
									writer.Write(fd.Vector.y);
									writer.Write(fd.Vector.z);
								}
							}
						}
					}
				}

				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CRTMPlugin();
			}
		}
	}
}
