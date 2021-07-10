using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VCMDL.NET
{
    public class CQuakePalette
    {
        public Color[] Colors = new Color[256];

        public void Load(string FileName)
        {
            using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Open))
            {
                if (file == null)
                    return;

                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
                {
                    if (reader == null)
                        return;

                    for (int i = 0; i < 256; ++i)
                        Colors[i] = Color.FromArgb(255, reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                }
            }
        }
    }

    // PCX Image class
    public class CPCXImage : IDisposable
    {
        public int width = 0, height = 0;
        public byte[,] Data;
        private Bitmap bitmap = null;
		bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
					bitmap.Dispose();

				disposed = true;
			}
		}

		~CPCXImage()
		{
			Dispose(false);
		}

        public Bitmap GetBitmap(CQuakePalette Pal)
        {
            if (bitmap != null)
                return bitmap;

            bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (FastPixel fp = new FastPixel(bitmap, true))
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        fp.SetPixel(x, y, Pal.Colors[Data[x, y]]);
                    }
                }
            }

            return bitmap;
        }

        public void SetData(byte[] data, int nwidth, int nheight)
        {
            width = nwidth;
            height = nheight;

            Data = new byte[nwidth, nheight];

            int i = 0;
            for (int y = 0; y < height; ++y)
                for (int x = 0; x < width; ++x, ++i)
                    Data[x, y] = data[i];
        }

        public int Load(string FileName)
        {
			try
			{
				using (System.IO.FileStream file = System.IO.File.Open(FileName, System.IO.FileMode.Open))
				{
					if (file == null)
						return 0;

					using (System.IO.BinaryReader reader = new System.IO.BinaryReader(file))
					{
						if (reader == null)
						{
							//   	Application->MessageBox("Could not open PCX file.","Error",MB_OK);
							return 0;
						}

						byte[] Header = new byte[128];
						short x1, y1, x2, y2;

						Header = reader.ReadBytes(128);

						if (Header[0] != 10)
						{
							//		Application->MessageBox("Not a PCX file","Error",MB_OK);
							return 0;
						}
						if (Header[1] != 5)
						{
							//		Application->MessageBox("Must be PCX version 3.0","Error",MB_OK);
							return 0;
						}
						if (Header[2] != 1)
						{
							//		Application->MessageBox("Must be PCX run length encoding.","Error",MB_OK);
							return 0;
						}
						if (Header[3] != 8)
						{
							//		Application->MessageBox("Must be an 8-bit PCX file","Error",MB_OK);
							return 0;
						}

						byte[] Buf = new byte[reader.BaseStream.Length - 128];

						Buf = reader.ReadBytes(Buf.Length);

						x1 = BitConverter.ToInt16(Header, 4);
						y1 = BitConverter.ToInt16(Header, 6);
						x2 = BitConverter.ToInt16(Header, 8);
						y2 = BitConverter.ToInt16(Header, 10);

						int pwidth, pheight;

						pwidth = x2 - x1 + 1;
						pheight = y2 - y1 + 1;

						int x, y = 0;
						byte B;
						int bufIndex = 0;

						width = pwidth;
						height = pheight;

						Data = new byte[pwidth, pheight];

						while (y < pheight)
						{
							x = 0;
							while (x < pwidth)
							{
								B = Buf[bufIndex++];

								if (B >= 192)
								{
									int count = B & 0x3f;
									B = Buf[bufIndex++];
									while ((count--) != 0)
									{
										Data[x, y] = B;
										x++;
									}
								}
								else
								{
									Data[x, y] = B;
									x++;
								}
							}
							y++;
						}
					}
				}
				return 1;
			}
			catch (Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.ToString());
				return 0;
			}
        }
    }
}
