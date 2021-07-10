using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCMDL.NET
{
	public class CompressionStream : IDisposable
	{
		public System.IO.MemoryStream MemoryStream = null;
		string Path = "";
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
					MemoryStream.Dispose();

				disposed = true;
			}
		}

		~CompressionStream()
		{
			Dispose(false);
		}

		public CompressionStream(string _Path)
		{
			Path = _Path;
		}

		public void StartWrite()
		{
			MemoryStream = new System.IO.MemoryStream();
		}

		public void EndWrite()
		{
            // Create the compressed file.
            using (System.IO.FileStream outFile = System.IO.File.Create(Path))
			{
                using (System.IO.Compression.GZipStream GZipStream = 
                    new System.IO.Compression.GZipStream(outFile, System.IO.Compression.CompressionMode.Compress))
				{
                    // Copy the source file into the compression stream.
                    MemoryStream.Seek(0, System.IO.SeekOrigin.Begin);
					GZipStream.Write(MemoryStream.ToArray(), 0, (int)MemoryStream.Length);
				}
            }

			MemoryStream.Dispose();
			MemoryStream = null;
		}

		public void StartRead()
		{
			MemoryStream = new System.IO.MemoryStream();

            using (System.IO.FileStream InFile = System.IO.File.Open(Path, System.IO.FileMode.Open))
			{
				using (System.IO.Compression.GZipStream GZipStream = new System.IO.Compression.GZipStream(InFile, System.IO.Compression.CompressionMode.Decompress))
				{
					byte[] buffer = new byte[4096];
					int numRead = GZipStream.Read(buffer, 0, buffer.Length);
					while (numRead != 0)
					{
						MemoryStream.Write(buffer, 0, numRead);
						numRead = GZipStream.Read(buffer, 0, buffer.Length);
					}

					MemoryStream.Position = 0;
				}
			}
		}

		public void EndRead()
		{
			MemoryStream.Dispose();
			MemoryStream = null;
		}
	}
}
