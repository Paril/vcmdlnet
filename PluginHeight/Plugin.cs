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
		public class CHeightMapPlugin : CPlugin
		{
			public CHeightMapPlugin()
				: base(EPluginType.PLUGIN_TOOLS)
			{
				Name = "Height Map...";
			}

			public override bool Execute(CPluginModel Model)
			{
				OpenFileDialog dlg = new OpenFileDialog();
				dlg.Filter = "Supported image files (*.png;*.bmp;*.jpg)|*.png;*.bmp;*.jpg";
				dlg.RestoreDirectory = false;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					Model.Clear();

					CPluginFrame fr = Model.CreateFrame();
					fr.FrameName = "Height";

					Bitmap b = new Bitmap(dlg.FileName);
					float worldScale = (b.Width * b.Height) / 2;
					float hScale = 0.25f;

					for (int y = 0; y < b.Height; ++y)
					{
						for (int x = 0; x < b.Width; ++x)
						{
							Color c = b.GetPixel(x, y);
							float height = ((float)Math.Max(c.R, Math.Max(c.G, c.B))) / 255;

							CPluginVertice v = Model.CreateVertice();
							v.GetFrameDataAt(0).Vector = new Vector3((-((float)b.Width / 2) + x) / b.Width, (-((float)b.Height / 2) + y) / b.Height, height * hScale) * worldScale;
						}
					}

					for (int y = 0; y < b.Height - 1; ++y)
					{
						for (int x = 0; x < b.Width - 1; ++x)
						{
							int vertTopLeft = (y * b.Height) + x;
							int vertTopRight = (y * b.Height) + x + 1;
							int vertBottomLeft = ((y + 1) * b.Height) + x;
							int vertBottomRight = ((y + 1) * b.Height) + x + 1;

							CPluginTriangle tri = Model.CreateTriangle();
							tri.Vertices[0] = vertBottomLeft;
							tri.Vertices[1] = vertTopRight;
							tri.Vertices[2] = vertTopLeft;

							tri = Model.CreateTriangle();
							tri.Vertices[0] = vertBottomLeft;
							tri.Vertices[1] = vertBottomRight;
							tri.Vertices[2] = vertTopRight;
						}
					}

					return true;
				}
				return false;
			}

			public static CPlugin CreatePlugin()
			{
				return new CHeightMapPlugin();
			}
		}
	}
}
