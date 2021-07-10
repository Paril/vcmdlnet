using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace VCMDL.NET
{
	public enum ESelectType
	{
		Vertex,
		Face,
		Bone,
	}

	// blue = x
	// red = y
	// green = z
	public enum EViewport
	{
		XYViewport,
		ZYViewport,
		XZViewport,
		XYZViewport
	}

	public enum EAxis
	{
		X = 1,
		Y = 2,
		Z = 4
	}

	public class CMDLGlobals
	{
		public static int g_CurFrame = 0;
		public static int g_CurSkin = 0;
		public static TCompleteModel g_CurMdl = new TCompleteModel();
		public static EActionType g_MainActionMode = EActionType.Select;
		public static string QuakeDataDir = "c:\\quake2\\";
		public static CQuakePalette g_Palette = new CQuakePalette();
		public static int g_ZoomFactor = 1;
		public static float g_Zoom2DFactor = 2;
		public static bool g_Panning = false, g_Zooming = false;
		public static Vector3 g_PanPosition = Vector3.Empty, g_OldPanPosition = Vector3.Empty;
		public static ESelectType g_ModelSelectType = ESelectType.Vertex;
		public static EViewport g_SelectedViewport = 0;
		public static EAxis g_Axis = EAxis.X|EAxis.Y|EAxis.Z;
		public static int g_GridSlices = 16, g_GridSize = 32;
		public static bool g_SyncSelections = false;
		public static Vector3 g_PlanePosition = Vector3.Empty;
	}

	public class CSelectionList
	{
		public bool[] Dest;
		public int curIndex = 0;

		public CSelectionList(int Size)
		{
			Dest = new bool[Size];
		}
	}

	public class CFrame
	{
		public string FrameName = "";
	};

	public delegate void FillSkinDataDelegate (CSkin Skin);

	public class CSkin : IDisposable
	{
		public Size SkinSize = Size.Empty;
		public Bitmap Skin = null;
		public string Path = "";
		public int TextureID = 0;
		public FillSkinDataDelegate FillSkinData;

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
					Skin.Dispose();

				disposed = true;
			}
		}

		~CSkin()
		{
			Dispose(false);
		}

		public CSkin(FillSkinDataDelegate _FillSkinData)
		{
			FillSkinData = _FillSkinData;
			Gl.glGenTextures(1, out TextureID);
		}

		public void Delete()
		{
			Gl.glDeleteTextures(1, ref TextureID);
		}

		public bool Load(string SkinPath)
		{
			Path = SkinPath;
			Skin = SkinEditor.LoadTexture(SkinPath);

			if (Skin == null)
				return false;

			SkinSize = Skin.Size;
			FillSkinData(this);
			return true;
		}

		public void MakeEmptySkin(int Width, int Height)
		{
			if (Width <= 0 || Height <= 0)
				Width = Height = 1;

			SkinSize = new Size(Width, Height);
			Skin = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			Path = "";

			FillSkinData(this);
		}

		public void SetBitmap(Bitmap bitmap)
		{
			SkinSize = bitmap.Size;
			Skin = bitmap;
			Path = "";

			FillSkinData(this);
		}

		public void Fill()
		{
			Bitmap textureImage = (Skin != null) ? new Bitmap(Skin, SkinSize.Width, SkinSize.Height) : new Bitmap(1, 1);

			//  Bind the texture.
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, TextureID);

			//  Tell OpenGL where the texture data is.
			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, 3, textureImage.Width, textureImage.Height, 0, Gl.GL_BGRA_EXT, Gl.GL_UNSIGNED_BYTE,
				textureImage.LockBits(new Rectangle(0, 0, textureImage.Width, textureImage.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb).Scan0);

			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);	// Linear Filtering
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);	// Linear Filtering
		}

		public void Bind()
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, TextureID);
		}
	};

	public class CSkinList
	{
		List<CSkin> Skins = new List<CSkin>();

		public int Count
		{
			get
			{
				return Skins.Count;
			}
		}

		public Size LargestSize
		{
			get
			{
				Size sz = new Size(1, 1);

				for (int i = 0; i < Skins.Count; ++i)
				{
					if (sz.Width < Skins[i].SkinSize.Width)
						sz.Width = Skins[i].SkinSize.Width;
					if (sz.Height < Skins[i].SkinSize.Height)
						sz.Height = Skins[i].SkinSize.Height;
				}

				return sz;
			}
		}

		public Size SizeForSkin(int mesh)
		{
			if (mesh < Count)
				return GetSkinAt(mesh).SkinSize;

			return LargestSize;
		}

		public void InsertSkin(int Index, CSkin Skin)
		{
			Skins.Insert(Index, Skin);
		}

		public void AddSkin(CSkin Skin)
		{
			InsertSkin(Skins.Count, Skin);
		}

		public CSkin GetSkinAt(int Index)
		{
			return Skins[Index];
		}

		public void RemoveSkin(int Index)
		{
			Skins[Index].Delete();
			Skins.RemoveAt(Index);

			if (CMDLGlobals.g_CurSkin == Index && CMDLGlobals.g_CurSkin != 0)
				CMDLGlobals.g_CurSkin--;
		}

		public void Clear()
		{
			for (int i = 0; i < Skins.Count; ++i)
				Skins[i].Delete();
			Skins.Clear();
		}
	};

	public class TCompleteModel
	{
		public const uint CALCNORMS_ALL = 1;
		public const uint CALCNORMS_SELECTED = 2;

		public enum TMode { imAppendVerts, imAppendFrame, imReplaceFrame };
		public enum CSelectionSource { ssVertex, ssTriangle, ssSkinVert, ssSkinTri };

		public class TImportMode
		{
			public TMode Mode = TMode.imAppendVerts;
			public int Frame = 0;	// for the two frame import modes

			public TImportMode(TMode M, int f = 0) { Mode = M; Frame = f; }
		};

		public CSkinList Skins = new CSkinList();
		public List<CMesh> Meshes = new List<CMesh>();
		public List<CFrame> Frames = new List<CFrame>();
		public List<CBone> Bones = new List<CBone>();

		public TCompleteModel()
		{
		}

		public TCompleteModel(TCompleteModel Src)
		{
			Copy(Src);
		}

		public void ClearModel(bool isNew = true)
		{
			Skins.Clear();
			Meshes.Clear();
			Frames.Clear();
			Bones.Clear();

			if (isNew)
			{
				Meshes.Add(new CMesh());
				Meshes[0].Name = "Default";

				Frames.Add(new CFrame());
				Frames[0].FrameName = "Frame 1";
			}
		}

		public bool Valid()
		{
			return (Frames.Count != 0);
		}

		public void Copy(TCompleteModel Src)
		{
			Skins = Src.Skins;
			Meshes = Src.Meshes;
			Frames = Src.Frames;
			Bones = Src.Bones;
		}

		// Selection related functions
		public Vector3 GetSelectionCentre(ESelectType S)
		{
			Vector3 V = Vector3.Empty;

			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];
				int ns = 0;

				if (S == ESelectType.Vertex)
				{
					for (int n = 0; n < mesh.Verts.Count; n++)
					{
						if ((mesh.Verts[n].Flags & EVerticeFlags.Selected) != 0)
						{
							V += mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position;
							ns++;
						}
					}
				}
				else if (S == ESelectType.Face)
				{
					bool[] vertsel = new bool[mesh.Verts.Count];

					for (int n = 0; n < mesh.Verts.Count; n++)
						vertsel[n] = false;

					for (int n = 0; n < mesh.Tris.Count; n++)
					{
						if ((mesh.Tris[n].Flags & ETriangleFlags.Selected) != 0)
						{
							vertsel[mesh.Tris[n].Vertices[0]] = true;
							vertsel[mesh.Tris[n].Vertices[1]] = true;
							vertsel[mesh.Tris[n].Vertices[2]] = true;
						}
					}

					for (int n = 0; n < mesh.Verts.Count; n++)
					{
						if (vertsel[n])
						{
							V += mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position;
							ns++;
						}
					}
				}

				if (ns > 0)
					V /= ns;
			}

			return V;
		}

		public Vector3 GetCentre()
		{
			Vector3 V = Vector3.Empty;

			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				for (int n = 0; n < mesh.Verts.Count; n++)
					V += (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position / mesh.Verts.Count);
			}

			return V;
		}

		public bool AnythingSelected()
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				for (int m = 0; m < Meshes.Count; ++m)
					for (int i = 0; i < Meshes[m].Verts.Count; i++)
						if ((Meshes[m].Verts[i].Flags & EVerticeFlags.Selected) != 0)
							return true;

				return false;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
			{
				for (int m = 0; m < Meshes.Count; ++m)
					for (int i = 0; i < Meshes[m].Tris.Count; i++)
						if ((Meshes[m].Tris[i].Flags & ETriangleFlags.Selected) != 0)
							return true;

				return false;
			}
			/*else if (CMDLGlobals.g_MainSelectMode == SelectType.stBone)
			{
				return Bones.AnythingSelected();
			}*/
			return false;
		}

		// Generalised stuffs
		public void CalcNormals(uint Action = CALCNORMS_ALL, int FrameNo = -1)
		{
			if (!(FrameNo >= 0 && FrameNo < Frames.Count))
				return;

			Vector3 va = Vector3.Empty,
					vb = Vector3.Empty,
					vc = Vector3.Empty,
					vd = Vector3.Empty,
					ve = Vector3.Empty;

			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				if (Action == CALCNORMS_ALL)
				{
					for (int n = 0; n < mesh.Verts.Count; n++)
						mesh.Verts[n].FrameData[FrameNo].Normal = Vector3.Empty;
				}

				for (int n = 0; n < mesh.Tris.Count; n++)
				{
					if (Action == CALCNORMS_ALL || (mesh.Tris[n].Flags & ETriangleFlags.Selected) != 0)
					{
						va = mesh.Verts[mesh.Tris[n].Vertices[0]].FrameData[FrameNo].Position;
						vb = mesh.Verts[mesh.Tris[n].Vertices[1]].FrameData[FrameNo].Position;
						vc = mesh.Verts[mesh.Tris[n].Vertices[2]].FrameData[FrameNo].Position;

						vd = vb - va;
						ve = vc - vb;

						mesh.Tris[n].Normal = vd.CrossProduct(ve);
						mesh.Tris[n].Normal.Normalize();
						mesh.Tris[n].Centre = (va + vb + vc) * (float)(1 / 3.0);

						mesh.Verts[mesh.Tris[n].Vertices[0]].FrameData[FrameNo].Normal += mesh.Tris[n].Normal;
						mesh.Verts[mesh.Tris[n].Vertices[1]].FrameData[FrameNo].Normal += mesh.Tris[n].Normal;
						mesh.Verts[mesh.Tris[n].Vertices[2]].FrameData[FrameNo].Normal += mesh.Tris[n].Normal;
					}
				}

				if (Action == CALCNORMS_ALL)
				{
					for (int n = 0; n < mesh.Verts.Count; n++)
						mesh.Verts[n].FrameData[FrameNo].Normal.Normalize();
				}
			}
		}

		public void CalcAllNormals()
		{
			for (int i = 0; i < Frames.Count; ++i)
				CalcNormals(CALCNORMS_ALL, i);
		}

		public void DeleteUnusedSkinVerts()
		{
			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];
				int num_st_new = mesh.SkinVerts.Count;
				int[] st_to_new = new int[mesh.SkinVerts.Count];
				int targ;
				CSelectionList to_delete = new CSelectionList(mesh.SkinVerts.Count);
				int v, t;
				CSkinVertex[] tempskinverts;

				for (v = 0; v < mesh.SkinVerts.Count; v++)
					to_delete.Dest[v] = true;

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					to_delete.Dest[mesh.Tris[t].SkinVerts[0]] = false;
					to_delete.Dest[mesh.Tris[t].SkinVerts[1]] = false;
					to_delete.Dest[mesh.Tris[t].SkinVerts[2]] = false;
				}

				targ = 0;
				for (v = 0; v < mesh.SkinVerts.Count; v++)
				{
					if (to_delete.Dest[v])
						num_st_new--;
					else
						st_to_new[v] = targ++;
				}

				tempskinverts = new CSkinVertex[num_st_new];
				for (t = 0; t < mesh.Tris.Count; t++)
				{
					mesh.Tris[t].SkinVerts[0] = st_to_new[mesh.Tris[t].SkinVerts[0]];
					mesh.Tris[t].SkinVerts[1] = st_to_new[mesh.Tris[t].SkinVerts[1]];
					mesh.Tris[t].SkinVerts[2] = st_to_new[mesh.Tris[t].SkinVerts[2]];
				}

				for (v = 0; v < mesh.SkinVerts.Count; v++)
					if (to_delete.Dest[v] == false)
						tempskinverts[st_to_new[v]] = mesh.SkinVerts[v];

				mesh.SkinVerts = tempskinverts.ToList<CSkinVertex>();
			}
		}

		public int GetSkinVerticesFrom3DVertices(SkinVertPos SVP, double Tolerance, float MinGetS, float MinGetT, float MaxGetS, float MaxGetT, bool Mirror)
		{
			CalcNormals();

			int t, v;

			bool[] vertsel, trisel;
			int num_st_toadd = 0, num_st_new;
			int[] xyz_to_st;
			CSkinVertex[] tempskinverts;

			Vector3 ProjVect = Vector3.Empty, UpVect = Vector3.Empty, RightVect = Vector3.Empty;
			double CosTol;

			CosTol = Math.Cos(VCMath.degreesToRadians((float)Tolerance));
			if (SVP == SkinVertPos.Front)
			{
				ProjVect.Set(-1, 0, 0);
				RightVect.Set(0, 1, 0);
				UpVect.Set(0, 0, -1);
			}
			else if (SVP == SkinVertPos.Back)
			{
				ProjVect.Set(1, 0, 0);
				if (Mirror)
					RightVect.Set(0, 1, 0);
				else
					RightVect.Set(0, -1, 0);
				UpVect.Set(0, 0, -1);
			}
			else if (SVP == SkinVertPos.Right)
			{
				ProjVect.Set(0, -1, 0);
				RightVect.Set(1, 0, 0);
				UpVect.Set(0, 0, -1);
			}
			else if (SVP == SkinVertPos.Left)
			{
				ProjVect.Set(0, 1, 0);
				if (Mirror)
					RightVect.Set(1, 0, 0);
				else
					RightVect.Set(-1, 0, 0);
				UpVect.Set(0, 0, -1);
			}
			else if (SVP == SkinVertPos.Top)
			{
				ProjVect.Set(0, 0, -1);
				RightVect.Set(0, 1, 0);
				UpVect.Set(1, 0, 0);
			}
			else if (SVP == SkinVertPos.Bottom)
			{
				ProjVect.Set(0, 0, 1);
				if (Mirror)
					RightVect.Set(0, -1, 0);
				else
					RightVect.Set(0, 1, 0);
				UpVect.Set(-1, 0, 0);
			}

			// add others here
			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				vertsel = new bool[mesh.Verts.Count];

				for (v = 0; v < mesh.Verts.Count; v++)
					vertsel[v] = false;

				trisel = new bool[mesh.Tris.Count];

				CalcAllNormals();

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					Vector3 Normal = new Vector3(mesh.Tris[t].Normal.x, mesh.Tris[t].Normal.y, mesh.Tris[t].Normal.z);
					double product;

					product = ProjVect.DotProduct(Normal);

					trisel[t] = (mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0;

					if (product < CosTol)
						trisel[t] = false;
				}

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					if (trisel[t])
					{
						vertsel[mesh.Tris[t].Vertices[0]] = true;
						vertsel[mesh.Tris[t].Vertices[1]] = true;
						vertsel[mesh.Tris[t].Vertices[2]] = true;
					}

				}

				for (v = 0; v < mesh.Verts.Count; v++)
				{
					if (vertsel[v])
						num_st_toadd++;
				}

				num_st_new = mesh.SkinVerts.Count + num_st_toadd;

				tempskinverts = new CSkinVertex[num_st_new];
				for (int i = 0; i < num_st_new; ++i)
				{
					tempskinverts[i] = new CSkinVertex();
					if (i < mesh.SkinVerts.Count)
					{
						tempskinverts[i].Flags = mesh.SkinVerts[i].Flags;
						tempskinverts[i].s = mesh.SkinVerts[i].s;
						tempskinverts[i].t = mesh.SkinVerts[i].t;
					}
				}

				xyz_to_st = new int[mesh.Verts.Count];

				int temp = mesh.SkinVerts.Count;
				for (v = 0; v < mesh.Verts.Count; v++)
				{
					if (vertsel[v])
						xyz_to_st[v] = temp++;
				}

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					if (trisel[t])
					{
						mesh.Tris[t].SkinVerts[0] = xyz_to_st[mesh.Tris[t].Vertices[0]];
						mesh.Tris[t].SkinVerts[1] = xyz_to_st[mesh.Tris[t].Vertices[1]];
						mesh.Tris[t].SkinVerts[2] = xyz_to_st[mesh.Tris[t].Vertices[2]];
					}
				}

				float minx = 100000, miny = 100000,
				  maxx = -100000, maxy = -100000;

				for (v = 0; v < mesh.Verts.Count; v++)
				{
					if (vertsel[v])
					{
						Vector3 VertVect = mesh.Verts[v].FrameData[CMDLGlobals.g_CurFrame].Position;

						tempskinverts[xyz_to_st[v]].s = VertVect.DotProduct(RightVect);
						tempskinverts[xyz_to_st[v]].t = VertVect.DotProduct(UpVect);
						tempskinverts[xyz_to_st[v]].Flags |= ESkinVertexFlags.Selected;
					}
				}

				for (v = 0; v < mesh.Verts.Count; v++)
				{
					if (vertsel[v])
					{
						if (tempskinverts[xyz_to_st[v]].s < minx)
							minx = tempskinverts[xyz_to_st[v]].s;
						if (tempskinverts[xyz_to_st[v]].s > maxx)
							maxx = tempskinverts[xyz_to_st[v]].s;
						if (tempskinverts[xyz_to_st[v]].t < miny)
							miny = tempskinverts[xyz_to_st[v]].t;
						if (tempskinverts[xyz_to_st[v]].t > maxy)
							maxy = tempskinverts[xyz_to_st[v]].t;
					}
				}

				double divS, divT, mulS, mulT;
				divS = maxx - minx;
				divT = maxy - miny;
				mulS = MaxGetS - MinGetS;
				mulT = MaxGetT - MinGetT;

				for (v = 0; v < mesh.Verts.Count; v++)
				{
					if (vertsel[v])
					{
						double v_s, v_t;
						v_s = tempskinverts[xyz_to_st[v]].s;
						v_t = tempskinverts[xyz_to_st[v]].t;

						v_s = mulS * (v_s - minx) / divS + MinGetS;
						v_t = mulT * (v_t - miny) / divT + MinGetT;

						tempskinverts[xyz_to_st[v]].s = (float)v_s / CMDLGlobals.g_CurMdl.Skins.SizeForSkin(m).Width;
						tempskinverts[xyz_to_st[v]].t = (float)v_t / CMDLGlobals.g_CurMdl.Skins.SizeForSkin(m).Height;
					}
				}

				mesh.SkinVerts = tempskinverts.ToList<CSkinVertex>();
			}

			DeleteUnusedSkinVerts();
			return 1;
		}

		public class QMFConstants
		{
			public const int Header = (('F' << 24) + ('M' << 16) + ('Q' << 8) + 'A'),
								Version = 1;
		};

		public enum QMFChunkNames
		{
			VerticeChunk		= 1,
			FaceChunk			= 2,
			SkinVertChunk		= 3,
			SkinsChunk			= 4,
			EndChunk			= -1
		};

		// File Related Stuff
		public void LoadFromQMF(string FileName, bool Merge)
		{
			/*CompressionStream cs = new CompressionStream(FileName);
			cs.StartRead();

			System.IO.BinaryReader rd = new System.IO.BinaryReader(cs.MemoryStream);
			{
				int head = rd.ReadInt32();
				int ver = rd.ReadInt32();

				if (head != QMFConstants.Header)
					return;

				ClearModel();

				QMFChunkNames chunk;

				while ((chunk = (QMFChunkNames)rd.ReadInt32()) != QMFChunkNames.EndChunk)
				{
					int len = rd.ReadInt32();

					switch (chunk)
					{
					case QMFChunkNames.VerticeChunk:
						{
							int frameCount = rd.ReadInt32();
							int vertCount = 0;

							if (frameCount != 0)
								vertCount = rd.ReadInt32();

							for (int i = 0; i < frameCount; ++i)
							{
								CFrame Frame = new CFrame();
								Frame.FrameName = CCString.Read(rd);
								Frames.Add(Frame);
							}

							for (int z = 0; z < vertCount; ++z)
							{
								CVertice Vert = new CVertice();
								Vert.Flags = (EVerticeFlags)rd.ReadInt32();

								for (int i = 0; i < frameCount; ++i)
								{
									Vert.FrameData.Add(new CVerticeFrameData());
									Vert.FrameData[i].Position.ReadCompressed(rd);
								}

								Verts.Add(Vert);
							}
						}
						break;
					case QMFChunkNames.FaceChunk:
						{
							int trisCount = rd.ReadInt32();

							for (int i = 0; i < trisCount; ++i)
							{
								CTriangle Tri = new CTriangle();
								Tri.Flags = (ETriangleFlags)rd.ReadInt32();

								for (int z = 0; z < 3; ++z)
									Tri.Vertices[z] = rd.ReadInt32();

								for (int z = 0; z < 3; ++z)
									Tri.SkinVerts[z] = rd.ReadInt32();

								mesh.Tris.Add(Tri);
							}
						}
						break;
					case QMFChunkNames.SkinVertChunk:
						{
							int stCount = rd.ReadInt32();

							for (int i = 0; i < stCount; ++i)
							{
								CSkinVertex SkinVert = new CSkinVertex();

								SkinVert.Flags = (ESkinVertexFlags)rd.ReadInt32();

								SkinVert.s = rd.ReadSingle();
								SkinVert.t = rd.ReadSingle();

								SkinVerts.Add(SkinVert);
							}
						}
						break;
					case QMFChunkNames.SkinsChunk:
						{
							int skinCount = rd.ReadInt32();

							for (int i = 0; i < skinCount; ++i)
							{
								CSkin Skin = new CSkin(ModelEditor.FillSkins);

								string Path = CCString.Read(rd);
								int Width = rd.ReadInt32();
								int Height = rd.ReadInt32();
								Bitmap bmp = new Bitmap(Width, Height);

								using (FastPixel fp = new FastPixel(bmp, true))
								{
									for (int y = 0; y < Height; ++y)
										for (int x = 0; x < Width; ++x)
											fp.SetPixel(x, y, Color.FromArgb(rd.ReadByte(), rd.ReadByte(), rd.ReadByte(), rd.ReadByte()));
								}

								Skin.SetBitmap(bmp);
								Skin.Path = Path;

								Skins.AddSkin(Skin);
							}
						}
						break;
					default:
						// Skip it if we don't know what it is
						rd.BaseStream.Seek(len, System.IO.SeekOrigin.Current);
						break;
					}
				}
			}

			cs.EndRead();*/
		}

		public void SaveToQMF(string FileName)
		{
			/*CompressionStream cs = new CompressionStream(FileName);
			cs.StartWrite();

			System.IO.BinaryWriter wr = new System.IO.BinaryWriter(cs.MemoryStream);
			{
				wr.Write(QMFConstants.Header);
				wr.Write(QMFConstants.Version);

				// vertice chunk
				wr.Write((int)QMFChunkNames.VerticeChunk);

				// Calculate write amount (non-constant)
				int totalWriteAmount = 12;

				int writeAmount = sizeof(int) + sizeof(int) + sizeof(int);

				for (int i = 0; i < Frames.Count; ++i)
					writeAmount += CCString.Count(Frames[i].FrameName);

				for (int z = 0; z < Verts.Count; ++z)
				{
					writeAmount += sizeof(int);

					for (int i = 0; i < Frames.Count; ++i)
						writeAmount += sizeof(short) + sizeof(short) + sizeof(short);
				}

				wr.Write(writeAmount);

				wr.Write(Frames.Count);

				if (Frames.Count != 0)
					wr.Write(Verts.Count);

				for (int i = 0; i < Frames.Count; ++i)
					CCString.Write(wr, Frames[i].FrameName);

				for (int z = 0; z < Verts.Count; ++z)
				{
					wr.Write((int)Verts[z].Flags);

					for (int i = 0; i < Frames.Count; ++i)
						Verts[z].FrameData[i].Position.WriteCompressed(wr);
				}

				totalWriteAmount += writeAmount;
				if (wr.BaseStream.Position - totalWriteAmount != 0)
					throw new Exception("Write mishap");

				// face chunk
				wr.Write((int)QMFChunkNames.FaceChunk);

				// Calculate write amount
				writeAmount = sizeof(int) + (mesh.Tris.Count * (sizeof(int) + (sizeof(int) * 3) + (sizeof(int) * 3)));
				wr.Write(writeAmount);

				wr.Write(mesh.Tris.Count);

				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					wr.Write((int)mesh.Tris[i].Flags);

					for (int z = 0; z < 3; ++z)
						wr.Write(mesh.Tris[i].Vertices[z]);

					for (int z = 0; z < 3; ++z)
						wr.Write(mesh.Tris[i].SkinVerts[z]);
				}

				totalWriteAmount += writeAmount + 8;
				if (wr.BaseStream.Position - totalWriteAmount != 0)
					throw new Exception("Write mishap");

				// skin vertices chunk
				wr.Write((int)QMFChunkNames.SkinVertChunk);

				// Calculate write amount
				writeAmount = sizeof(int) + ((sizeof(int) + sizeof(float) + sizeof(float)) * SkinVerts.Count);
				wr.Write(writeAmount);

				wr.Write(SkinVerts.Count);

				for (int i = 0; i < SkinVerts.Count; ++i)
				{
					wr.Write((int)SkinVerts[i].Flags);

					// Use a compression scheme here
					wr.Write(SkinVerts[i].s);
					wr.Write(SkinVerts[i].t);
				}

				totalWriteAmount += writeAmount + 8;
				if (wr.BaseStream.Position - totalWriteAmount != 0)
					throw new Exception("Write mishap");

				// skin chunk
				wr.Write((int)QMFChunkNames.SkinsChunk);

				// calculate write amount (not constant)
				writeAmount = 4;

				for (int i = 0; i < Skins.Count; ++i)
				{
					writeAmount += 8 + ((Skins.GetSkinAt(i).SkinSize.Width * Skins.GetSkinAt(i).SkinSize.Height) * 4);
					writeAmount += CCString.Count(Skins.GetSkinAt(i).Path);
				}

				wr.Write(writeAmount);

				wr.Write(Skins.Count);

				for (int i = 0; i < Skins.Count; ++i)
				{
					CCString.Write(wr, Skins.GetSkinAt(i).Path);

					wr.Write(Skins.GetSkinAt(i).Skin.Width);
					wr.Write(Skins.GetSkinAt(i).Skin.Height);

					using (FastPixel fp = new FastPixel(Skins.GetSkinAt(i).Skin, true))
					{
						for (int h = 0; h < fp.Height; ++h)
						{
							for (int w = 0; w < fp.Width; ++w)
							{
								Color px = fp.GetPixel(w, h);

								wr.Write(px.A);
								wr.Write(px.R);
								wr.Write(px.G);
								wr.Write(px.B);
							}
						}
					}
				}

				totalWriteAmount += writeAmount + 8;
				if (wr.BaseStream.Position - totalWriteAmount != 0)
					throw new Exception("Write mishap");
				wr.Write((int)QMFChunkNames.EndChunk);
			}

			cs.EndWrite();*/
		}

		public void CalculateBBox(ref Vector3 Mins, ref Vector3 Maxs)
		{
			if (Frames.Count == 0)
				return;

			Vector3[] LowestValues = new Vector3[3], HighestValues = new Vector3[3];

			for (int i = 0; i < 3; ++i)
			{
				LowestValues[i] = new Vector3(99999, 99999, 99999);
				HighestValues[i] = new Vector3(-99999, -99999, -99999);
			}

			for (int m = 0; m < Meshes.Count; ++m)
			{
				for (int i = 0; i < Meshes[m].Verts.Count; ++i)
				{
					Vector3 vertex = Meshes[m].Verts[i].FrameData[0].Position;

					if (vertex.x < LowestValues[0].x)
						LowestValues[0] = vertex;
					if (vertex.y < LowestValues[1].y)
						LowestValues[1] = vertex;
					if (vertex.z < LowestValues[2].z)
						LowestValues[2] = vertex;

					if (vertex.x > HighestValues[0].x)
						HighestValues[0] = vertex;
					if (vertex.y > HighestValues[1].y)
						HighestValues[1] = vertex;
					if (vertex.z > HighestValues[2].z)
						HighestValues[2] = vertex;
				}
			}

			Mins = new Vector3(LowestValues[0].x, LowestValues[2].z, LowestValues[1].y);
			Maxs = new Vector3(HighestValues[0].x, HighestValues[2].z, HighestValues[1].y);
		}

		public static Vector3 CalculateInterpolation(Vector3 Current, Vector3 Next, float Interp)
		{
			return Current + Interp * (Next - Current);
		}

		public void Draw(bool Is3DView, COpenGlControlData glData)
		{
			PGl.glColor(Color.White);

			if ((glData.Flags & EControlDataFlags.WireFrame) != 0)
			{
				PGl.glColor(Color.Black);
				Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_LINE);
			}

			Gl.glFrontFace(Gl.GL_CW);

			bool ShowBackfaces = (glData.Flags & EControlDataFlags.ShowBackfaces) == 0;

			if (ShowBackfaces)
			{
				Gl.glEnable(Gl.GL_CULL_FACE);
				Gl.glCullFace(Gl.GL_BACK);
			}

			if (Frames.Count != 0)
			{
				Program.Form_ModelEditor.EnableBlend();
				Program.Form_ModelEditor.DisableLighting(glData);

				DrawTicks(glData);

				DrawNormals(glData);

				DrawBones(glData);

				DrawFaces(Is3DView, glData, false, false, delegate(CTriangle Tri)
				{
					if ((Tri.Flags & ETriangleFlags.Selected) == 0 &&
						(Tri.Flags & ETriangleFlags.TempSelected) == 0)
						return true;

					if ((Tri.Flags & ETriangleFlags.TempSelected) == 0)
						PGl.glColor(Color.Yellow);
					else
					{
						PGl.glColor(Color.Purple);
						Tri.Flags &= ~ETriangleFlags.TempSelected;
					}

					return false;
				});
				
				PGl.glColor(Color.White);

				if ((glData.Flags & EControlDataFlags.WireFrame) != 0)
					PGl.glColor(Color.Black);

				Program.Form_ModelEditor.EnableLighting(glData);

				DrawFaces(Is3DView, glData, true, true, null);
			}

			if (ShowBackfaces)
				Gl.glDisable(Gl.GL_CULL_FACE);

			if ((glData.Flags & EControlDataFlags.WireFrame) != 0)
				Gl.glPolygonMode(Gl.GL_FRONT_AND_BACK, Gl.GL_FILL);

			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
			Program.Form_ModelEditor.DisableLighting(glData);
		}

		private void DrawBones(COpenGlControlData glData)
		{
			foreach (CBone b in Bones)
			{
				Gl.glPushMatrix();
				ModelEditor.DrawJoint(b.Position, b.Angles, ref b.Flags);
				Gl.glPopMatrix();
			}
		}

		private void DrawTicks(COpenGlControlData glData)
		{
			Gl.glPointSize(4);
			if ((glData.Flags & EControlDataFlags.ShowVerticeTicks) != 0)
			{
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);

				Gl.glBegin(Gl.GL_POINTS);

				for (int m = 0; m < Meshes.Count; ++m)
				{
					var mesh = Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
					{
						Vector3 vertexPosition = Vector3.Empty;
						if (Program.Form_ModelEditor.Animating && ModelEditor.theViewTab.IsInterpolating())
						{
							if (CMDLGlobals.g_CurFrame + 1 > Program.Form_ModelEditor.AnimateEnd)
								vertexPosition = CalculateInterpolation(mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame].Position, mesh.Verts[i].FrameData[Program.Form_ModelEditor.AnimateStart].Position, Program.Form_ModelEditor.InterpolatePercent);
							else
								vertexPosition = CalculateInterpolation(mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame].Position, mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame + 1].Position, Program.Form_ModelEditor.InterpolatePercent);
						}
						else
							vertexPosition = mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame].Position;

						if ((mesh.Verts[i].Flags & EVerticeFlags.TempSelected) != 0)
						{
							PGl.glColor(Color.Purple);
							mesh.Verts[i].Flags &= ~EVerticeFlags.TempSelected;
						}
						else if ((mesh.Verts[i].Flags & EVerticeFlags.Selected) != 0)
							PGl.glColor(Color.Yellow);
						else
							PGl.glColor(Color.SaddleBrown);

						PGl.glVertex(vertexPosition);
					}
				}

				Gl.glEnd();
			}
			Gl.glPointSize(1);
		}

		private void DrawNormals(COpenGlControlData glData)
		{
			if ((glData.Flags & EControlDataFlags.NormalsAll) != 0 ||
					(glData.Flags & EControlDataFlags.NormalsSelected) != 0)
			{
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);

				Gl.glBegin(Gl.GL_LINES);
				for (int m = 0; m < Meshes.Count; ++m)
				{
					var mesh = Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						CTriangle tri = mesh.Tris[i];

						if ((glData.Flags & EControlDataFlags.NormalsSelected) != 0 &&
								(tri.Flags & ETriangleFlags.Selected) == 0)
							continue;

						PGl.glColor(Color.Goldenrod);

						Vector3 centerLine = mesh.Tris[i].Centre;
						PGl.glVertex(centerLine);

						centerLine = mesh.Tris[i].Centre + (-mesh.Tris[i].Normal / 2.0f);
						PGl.glVertex(centerLine);

						PGl.glColor(Color.Firebrick);

						centerLine = mesh.Tris[i].Centre + (-mesh.Tris[i].Normal / 2.0f);
						PGl.glVertex(centerLine);

						centerLine = mesh.Tris[i].Centre + (-mesh.Tris[i].Normal);
						PGl.glVertex(centerLine);
					}
					Gl.glEnd();
				}
			}
		}

		delegate bool CheckTriangleDelegate (CTriangle Tri);

		private void DrawFaces(bool Is3DView, COpenGlControlData glData, bool Normals, bool Textured, CheckTriangleDelegate CheckTri)
		{
			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				if ((glData.Flags & EControlDataFlags.Textured) != 0 && m < Skins.Count)
					Skins.GetSkinAt(mesh.SkinIndex).Bind();

				Gl.glBegin(Gl.GL_TRIANGLES);
				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					CTriangle tri = mesh.Tris[i];

					if (CheckTri != null && CheckTri(tri))
						continue;

					for (int z = 0; z < 3; ++z)
					{
						if (mesh.Verts[tri.Vertices[z]].Bone != -1)
							continue;

						Vector3 vertexPosition = Vector3.Empty;
						if (Program.Form_ModelEditor.Animating && ModelEditor.theViewTab.IsInterpolating())
						{
							if (CMDLGlobals.g_CurFrame + 1 > Program.Form_ModelEditor.AnimateEnd)
								vertexPosition = CalculateInterpolation(mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Position, mesh.Verts[tri.Vertices[z]].FrameData[Program.Form_ModelEditor.AnimateStart].Position, Program.Form_ModelEditor.InterpolatePercent);
							else
								vertexPosition = CalculateInterpolation(mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Position, mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame + 1].Position, Program.Form_ModelEditor.InterpolatePercent);
						}
						else
							vertexPosition = mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Position;

						if (Normals)
						{
							if (Is3DView)
								Gl.glNormal3f(
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.x,
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.y,
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.z);
							else
								Gl.glNormal3f(
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.x,
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.y,
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.z);
						}

						if (Textured && (glData.Flags & EControlDataFlags.Textured) != 0 && mesh.SkinVerts.Count != 0)
							Gl.glTexCoord2f(mesh.SkinVerts[tri.SkinVerts[z]].s, mesh.SkinVerts[tri.SkinVerts[z]].t);

						PGl.glVertex(vertexPosition);
					}
				}
				Gl.glEnd();

				Gl.glBegin(Gl.GL_TRIANGLES);
				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					CTriangle tri = mesh.Tris[i];

					if (CheckTri != null && CheckTri(tri))
						continue;

					for (int z = 0; z < 3; ++z)
					{
						if (mesh.Verts[tri.Vertices[z]].Bone == -1)
							continue;

						Vector3 ang = Bones[mesh.Verts[tri.Vertices[z]].Bone].Angles - mesh.Verts[tri.Vertices[z]].BoneBaseAngles;
						Vector3 localPosition = (mesh.Verts[tri.Vertices[z]].BoneBasePosition - Bones[mesh.Verts[tri.Vertices[z]].Bone].Position);
						Vector3 vertexPosition = (localPosition)
							.Rotate(new Vector3(1, 0, 0), ang.x)
							.Rotate(new Vector3(0, 1, 0), ang.y)
							.Rotate(new Vector3(0, 0, 1), ang.z)
							+ Bones[mesh.Verts[tri.Vertices[z]].Bone].Position;

						if (Normals)
						{
							if (Is3DView)
								Gl.glNormal3f(
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.x,
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.y,
									-mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.z);
							else
								Gl.glNormal3f(
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.x,
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.y,
									mesh.Verts[tri.Vertices[z]].FrameData[CMDLGlobals.g_CurFrame].Normal.z);
						}

						if (Textured && (glData.Flags & EControlDataFlags.Textured) != 0 && mesh.SkinVerts.Count != 0)
							Gl.glTexCoord2f(mesh.SkinVerts[tri.SkinVerts[z]].s, mesh.SkinVerts[tri.SkinVerts[z]].t);

						PGl.glVertex(vertexPosition);
					}
				}
				Gl.glEnd();
			}
		}

		public PointF SkinVerticeToPoint(int mesh, int skinVertIndex)
		{
			return new PointF(Meshes[mesh].SkinVerts[skinVertIndex].s * Skins.SizeForSkin(mesh).Width,
								Meshes[mesh].SkinVerts[skinVertIndex].t * Skins.SizeForSkin(mesh).Height);
		}

		public void CreateVertex(int mesh, Vector3 createVertexPos)
		{
			CVertice v = new CVertice();

			for (int i = 0; i < Frames.Count; ++i)
			{
				CVerticeFrameData fd = new CVerticeFrameData();
				fd.Position = createVertexPos;
				v.FrameData.Add(fd);
			}

			Meshes[mesh].Verts.Add(v);
			Program.Form_ModelEditor.ModelUpdated();
		}

		public void CreateFace(int mesh, int[] BuildingFaceVerts)
		{
			CTriangle tri = new CTriangle();

			tri.Vertices[0] = BuildingFaceVerts[0];
			tri.Vertices[1] = BuildingFaceVerts[1];
			tri.Vertices[2] = BuildingFaceVerts[2];

			Meshes[mesh].Tris.Add(tri);
			Program.Form_ModelEditor.ModelUpdated();
		}

		public void WeldSelected()
		{
			if (CMDLGlobals.g_ModelSelectType != ESelectType.Vertex)
				return;

			int NumSel = 0, First=-1, NumNewVerts;
			double Newx=0, Newy=0, Newz=0;

			List<CSelectedVertice> Sel = ModelEditor.RetrieveSelectedVertices();

			int meshIndex = -1;
			foreach (CSelectedVertice v in Sel)
			{
				NumSel++;
				if (First == -1)
					First = v.Index;

				Newx += Meshes[v.Mesh].Verts[v.Index].FrameData[CMDLGlobals.g_CurFrame].Position.x;
				Newy += Meshes[v.Mesh].Verts[v.Index].FrameData[CMDLGlobals.g_CurFrame].Position.y;
				Newz += Meshes[v.Mesh].Verts[v.Index].FrameData[CMDLGlobals.g_CurFrame].Position.z;

				if (meshIndex == -1)
					meshIndex = v.Mesh;
				else if (meshIndex != v.Mesh)
					return; // FIXME TODO
			}

			if (NumSel == 0)
				return;

			Newx /= NumSel;
			Newy /= NumSel;
			Newz /= NumSel;

			int[] TargVert = new int[Meshes[meshIndex].Verts.Count];

			{
				int t=0;
				for (int v=0; v < Meshes[meshIndex].Verts.Count; v++)
				{
					if (ModelEditor.SelectedVertsContains(Sel, new CSelectedVertice(meshIndex, v, null)) && v != First)
						TargVert[v] = First;
					else
					{
						TargVert[v] =  t;
						t++;
					}
				}

				NumNewVerts = t;
			}

			for (int t=0; t < Meshes[meshIndex].Tris.Count; t++)
			{
				Meshes[meshIndex].Tris[t].Vertices[0] = TargVert[Meshes[meshIndex].Tris[t].Vertices[0]];
				Meshes[meshIndex].Tris[t].Vertices[1] = TargVert[Meshes[meshIndex].Tris[t].Vertices[1]];
				Meshes[meshIndex].Tris[t].Vertices[2] = TargVert[Meshes[meshIndex].Tris[t].Vertices[2]];
			}

			CVertice[] V = new CVertice[NumNewVerts];

			for (int v = 0; v < NumNewVerts; ++v)
				V[v] = new CVertice();

			for (int F=0; F<Frames.Count; F++)
			{
				Newx = Newy = Newz = 0;

				for (int v = 0; v < Meshes[meshIndex].Verts.Count; ++v)
				{
					V[TargVert[v]].FrameData.Add(Meshes[meshIndex].Verts[v].FrameData[F]);
					if (TargVert[v] == First)
					{
						Newx += Meshes[meshIndex].Verts[v].FrameData[F].Position.x;
						Newy += Meshes[meshIndex].Verts[v].FrameData[F].Position.y;
						Newz += Meshes[meshIndex].Verts[v].FrameData[F].Position.z;
					}
				}

				Newx /= NumSel;
				Newy /= NumSel;
				Newz /= NumSel;

				V[First].FrameData[F].Position.x = (float)Newx;
				V[First].FrameData[F].Position.y = (float)Newy;
				V[First].FrameData[F].Position.z = (float)Newz;
			}

			Meshes[meshIndex].Verts = V.ToList<CVertice>();
			CalcAllNormals();

			Meshes[meshIndex].Verts[First].Flags |= EVerticeFlags.Selected;

			Program.Form_ModelEditor.ModelUpdated();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		public void DeleteSelected()
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				if (MessageBox.Show("This will delete all the selected vertices and faces using these. Are you sure?", "Confirmation", MessageBoxButtons.YesNo)==DialogResult.No)
					return;

				int newverts=0, newtris=0, numverts=0, numtris=0;
				int n;
				bool[] vertsdel, trisdel;

				for (int m = 0; m < Meshes.Count; ++m)
				{
					var mesh = Meshes[m];

					vertsdel = new bool[mesh.Verts.Count];
					trisdel = new bool[mesh.Tris.Count];

					for (n = 0; n < mesh.Verts.Count; n++)
					{
						if ((mesh.Verts[n].Flags & EVerticeFlags.Selected) != 0)
							vertsdel[n] = true;
						else
						{
							vertsdel[n] = false;
							numverts++;
						}
					}

					for (n = 0; n < mesh.Tris.Count; n++)
					{
						if (vertsdel[mesh.Tris[n].Vertices[0]] || vertsdel[mesh.Tris[n].Vertices[1]] || vertsdel[mesh.Tris[n].Vertices[2]])
							trisdel[n] = true;
						else
						{
							trisdel[n] = false;
							numtris++;
						}
					}

					CTriangle[] tempTris = new CTriangle[numtris];
					CVertice[] tempVerts = new CVertice[numverts];

					for (n = 0; n < numverts; n++)
					{
						tempVerts[n] = new CVertice();

						for (int f = 0; f < Frames.Count; ++f)
							tempVerts[n].FrameData.Add(new CVerticeFrameData());
					}

					for (n = 0; n < mesh.Verts.Count; n++)
					{
						if (vertsdel[n] == false)
						{
							for (int f=0; f < Frames.Count; f++)
								tempVerts[newverts].FrameData[f] = mesh.Verts[n].FrameData[f];

							for (int t=0; t < mesh.Tris.Count; t++)
							{
								for (int i = 0; i < 3; ++i)
								{
									if (mesh.Tris[t].Vertices[i] == n)
										mesh.Tris[t].Vertices[i] = newverts;
								}
							}

							newverts++;
						}
					}

					for (n = 0; n < mesh.Tris.Count; n++)
					{
						if (trisdel[n] == false)
						{
							tempTris[newtris] = mesh.Tris[n];
							newtris++;
						}
					}

					mesh.Tris.Clear();
					mesh.Verts.Clear();

					mesh.Tris = tempTris.ToList<CTriangle>();
					mesh.Verts = tempVerts.ToList<CVertice>();
				}
			}
			else
			{
				int DelIsolated = (int)DialogResult.No;

				if (MessageBox.Show("This will delete all the selected faces. Are you sure?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
					return;

				DelIsolated = (int)MessageBox.Show("Do you want do delete any isolated vertices?", "Delete Isolated", MessageBoxButtons.YesNo);
				if (DelIsolated == (int)DialogResult.No)
					DelIsolated = 0;

				int newtris, numtris=0, numverts;
				int n;
				for (int m = 0; m < Meshes.Count; ++m)
				{
					var mesh = Meshes[m];

					bool[] trisdel = new bool[mesh.Tris.Count];

					for (n = 0; n < mesh.Tris.Count; n++)
					{
						if ((mesh.Tris[n].Flags & ETriangleFlags.Selected) != 0)
							trisdel[n] = true;
						else
						{
							trisdel[n] = false;
							numtris++;
						}
					}

					CTriangle[] tempTris = new CTriangle[numtris];

					newtris = 0;
					for (n = 0; n < mesh.Tris.Count; n++)
					{
						if (trisdel[n] == false)
						{
							tempTris[newtris] = mesh.Tris[n];
							newtris++;
						}
					}

					mesh.Tris = tempTris.ToList<CTriangle>();

					if (DelIsolated != 0)
					{
						bool[] vertsdel = new bool[mesh.Verts.Count];

						for (n = 0; n < mesh.Verts.Count; n++)
							vertsdel[n] = true;

						for (n = 0; n < mesh.Tris.Count; n++)
						{
							for (int i = 0; i < 3; ++i)
								vertsdel[mesh.Tris[n].Vertices[i]] = false;
						}

						numverts = mesh.Verts.Count;

						for (n = 0; n < mesh.Verts.Count; n++)
						{
							if (vertsdel[n])
								numverts--;
						}

						int[] targVerts = new int[mesh.Verts.Count];

						{
							int t=0;
							for (n = 0; n < mesh.Verts.Count; n++)
							{
								if (vertsdel[n] == false)
								{
									targVerts[n] = t;
									t++;
								}
							}
						}


						CVertice[] tempVerts = new CVertice[numverts];

						for (n = 0; n < numverts; n++)
							tempVerts[n] = new CVertice();

						for (int f=0; f < Frames.Count; f++)
						{
							int t=0;

							for (n = 0; n < mesh.Verts.Count; n++)
							{
								if (vertsdel[n] == false)
								{
									tempVerts[t].FrameData.Add(mesh.Verts[n].FrameData[f]);
									t++;
								}
							}
						}

						for (n = 0; n < mesh.Tris.Count; n++)
						{
							tempTris[n].Vertices[0] = targVerts[mesh.Tris[n].Vertices[0]];
							tempTris[n].Vertices[1] = targVerts[mesh.Tris[n].Vertices[1]];
							tempTris[n].Vertices[2] = targVerts[mesh.Tris[n].Vertices[2]];
						}

						mesh.Verts = tempVerts.ToList<CVertice>();
						mesh.Tris = tempTris.ToList<CTriangle>();
					}
				}
			}

			Program.Form_ModelEditor.ModelUpdated();
		}

		public void FlipNormals()
		{
			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
						mesh.Tris[i].Flip();
				}
			}

			CalcNormals(CALCNORMS_SELECTED, CMDLGlobals.g_CurFrame);
		}

		public void Subdivide()
		{
			/*if (CMDLGlobals.g_ModelSelectType != ESelectType.Face)
				return;

			// do subdivision
			int NumNewXYZ=0, NumNewST=0;

			bool[] DoSkin = new bool[mesh.Tris.Count];
			int[] TriToVert = new int[mesh.Tris.Count];
			int[] TriToST = new int[mesh.Tris.Count];

			for (int t=0; t<mesh.Tris.Count; t++)
			{
				DoSkin[t] = false;
				if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					NumNewXYZ++;
				{
					if (mesh.Tris[t].SkinVerts[0] != mesh.Tris[t].SkinVerts[1] &&
					 mesh.Tris[t].SkinVerts[1] != mesh.Tris[t].SkinVerts[2] &&
					 mesh.Tris[t].SkinVerts[2] != mesh.Tris[t].SkinVerts[0])
						DoSkin[t] = true;
				}
			}

			{
				int Targ=0;
				for (int t=0; t<mesh.Tris.Count; t++)
					if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					{
						TriToVert[t] = Verts.Count + Targ;
						TriToST[t] = SkinVerts.Count + Targ;
						Targ++;
					}
					else
					{
						TriToVert[t] = -1;
						TriToST[t] = -1;
					}
			}

			// Add in the vertices for all the frames
			// todo: add in skin vertices addition

			{
				CVertice[] v = new CVertice[Verts.Count + NumNewXYZ];
				for (int i = 0; i < Verts.Count + NumNewXYZ; ++i)
				{
					if (i < Verts.Count)
						v[i] = Verts[i];
					else
						v[i] = new CVertice();
				}

				for (int F=0; F<Frames.Count; F++)
				{
					for (int t=0; t<mesh.Tris.Count; t++)
					{
						if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
						{
							double targx, targy, targz;
							targx = targy = targz = 0.0;

							targx += Verts[mesh.Tris[t].Vertices[0]].FrameData[F].Position.x;
							targx += Verts[mesh.Tris[t].Vertices[1]].FrameData[F].Position.x;
							targx += Verts[mesh.Tris[t].Vertices[2]].FrameData[F].Position.x;
							targy += Verts[mesh.Tris[t].Vertices[0]].FrameData[F].Position.y;
							targy += Verts[mesh.Tris[t].Vertices[1]].FrameData[F].Position.y;
							targy += Verts[mesh.Tris[t].Vertices[2]].FrameData[F].Position.y;
							targz += Verts[mesh.Tris[t].Vertices[0]].FrameData[F].Position.z;
							targz += Verts[mesh.Tris[t].Vertices[1]].FrameData[F].Position.z;
							targz += Verts[mesh.Tris[t].Vertices[2]].FrameData[F].Position.z;

							if (v[TriToVert[t]].FrameData.Count <= F)
								v[TriToVert[t]].FrameData.Add(new CVerticeFrameData());

							v[TriToVert[t]].FrameData[F].Position.x = (float)(targx/3.0);
							v[TriToVert[t]].FrameData[F].Position.y = (float)(targy/3.0);
							v[TriToVert[t]].FrameData[F].Position.z = (float)(targz/3.0);
						}
					}
				}

				Verts = v.ToList<CVertice>();
			}

			// set up the skin vertices
			{
				int t;

				for (t=0; t<mesh.Tris.Count; t++)
				{
					if (DoSkin[t])
						NumNewST++;
				}

				CSkinVertex[] V = new CSkinVertex[SkinVerts.Count+NumNewST];

				for (int i = 0; i < SkinVerts.Count+NumNewST; ++i)
				{
					if (i < SkinVerts.Count)
						V[i] = SkinVerts[i];
					else
						V[i] = new CSkinVertex();
				}

				for (t=0; t<mesh.Tris.Count; t++)
				{
					if (DoSkin[t] && (mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					{
						double targs, targt;
						targs = targt = 0.0;

						targs += SkinVerts[mesh.Tris[t].SkinVerts[0]].s;
						targs += SkinVerts[mesh.Tris[t].SkinVerts[1]].s;
						targs += SkinVerts[mesh.Tris[t].SkinVerts[2]].s;
						targt += SkinVerts[mesh.Tris[t].SkinVerts[0]].t;
						targt += SkinVerts[mesh.Tris[t].SkinVerts[1]].t;
						targt += SkinVerts[mesh.Tris[t].SkinVerts[2]].t;

						V[TriToST[t]].s = (float)(targs/3.0);
						V[TriToST[t]].t = (float)(targt/3.0);
					}
				}

				SkinVerts = V.ToList<CSkinVertex>();
			}

			// set up the new triangles, and reorganise the old ones.
			// todo: add in skin triangle set up

			{
				int TrisDone=0;

				CTriangle[] T = new CTriangle[mesh.Tris.Count + NumNewXYZ*2];

				for (int i = 0; i < mesh.Tris.Count + NumNewXYZ*2; ++i)
				{
					T[i] = new CTriangle();
				}

				for (int t=0; t<mesh.Tris.Count; t++)
				{
					if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					{
						int NewTri1, NewTri2;
						NewTri1 = mesh.Tris.Count+TrisDone++;
						NewTri2 = mesh.Tris.Count+TrisDone++;
						
						T[t].Vertices[0] = mesh.Tris[t].Vertices[0];
						T[t].Vertices[1] = TriToVert[t];
						T[t].Vertices[2] = mesh.Tris[t].Vertices[2];

						T[NewTri1].Vertices[0] = mesh.Tris[t].Vertices[0];
						T[NewTri1].Vertices[1] = mesh.Tris[t].Vertices[1];
						T[NewTri1].Vertices[2] = TriToVert[t];

						T[NewTri2].Vertices[0] = TriToVert[t];
						T[NewTri2].Vertices[1] = mesh.Tris[t].Vertices[1];
						T[NewTri2].Vertices[2] = mesh.Tris[t].Vertices[2];

						T[t].SkinVerts[0] = mesh.Tris[t].SkinVerts[0];
						T[t].SkinVerts[1] = mesh.Tris[t].SkinVerts[1];
						T[t].SkinVerts[2] = mesh.Tris[t].SkinVerts[2];

						if (DoSkin[t])
						{
							T[t].SkinVerts[0] = mesh.Tris[t].SkinVerts[0];
							T[t].SkinVerts[1] = TriToST[t];
							T[t].SkinVerts[2] = mesh.Tris[t].SkinVerts[2];

							T[NewTri1].SkinVerts[0] = mesh.Tris[t].SkinVerts[0];
							T[NewTri1].SkinVerts[1] = mesh.Tris[t].SkinVerts[1];
							T[NewTri1].SkinVerts[2] = TriToST[t];

							T[NewTri2].SkinVerts[0] = TriToST[t];
							T[NewTri2].SkinVerts[1] = mesh.Tris[t].SkinVerts[1];
							T[NewTri2].SkinVerts[2] = mesh.Tris[t].SkinVerts[2];
						}
					}
					else
						T[t] = mesh.Tris[t];
				}

				mesh.Tris = T.ToList<CTriangle>();
			}

			CalcAllNormals();
			Program.Form_ModelEditor.ModelUpdated();*/
		}

		public void DeleteFrame(int p)
		{
			Frames.RemoveAt(p);

			for (int m = 0; m < Meshes.Count; ++m)
				for (int x = 0; x < Meshes[m].Verts.Count; ++x)
					Meshes[m].Verts[x].FrameData.RemoveAt(p);

			if (CMDLGlobals.g_CurFrame == p)
			{
				if (CMDLGlobals.g_CurFrame > 0)
					CMDLGlobals.g_CurFrame--;
				else
					CMDLGlobals.g_CurFrame++;
			}
		}

		public void DeleteFrames(List<int> FrameIndices)
		{
			FrameIndices.ReverseSort();

			for (int i = 0; i < FrameIndices.Count; ++i)
				DeleteFrame(FrameIndices[i]);

			if (CMDLGlobals.g_CurFrame >= Frames.Count)
				Program.Form_ModelEditor.SetFrame(Frames.Count - 1);
			else
				Program.Form_ModelEditor.RedrawAllViews();
		}

		public void TurnEdge()
		{
			int SelCount = 0;
			CTriangle[] tris = new CTriangle[2];

			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						if (SelCount == 2)
						{
							MessageBox.Show("Only two faces need to be selected to turn an edge.");
							return;
						}

						tris[SelCount++] = mesh.Tris[i];
					}
				}

				if (SelCount != 2)
				{
					MessageBox.Show("At least two faces need to be selected to turn an edge.");
					return;
				}

				int[] SharedVerts = new int[2] { -1, -1 };

				for (int i = 0; i < 3; ++i)
				{
					for (int z = 0; z < 3; ++z)
					{
						if (tris[0].Vertices[i] == tris[1].Vertices[z])
						{
							if (SharedVerts[0] == -1)
							{
								SharedVerts[0] = tris[0].Vertices[i];
								break;
							}
							else if (SharedVerts[1] == -1 && tris[0].Vertices[i] != SharedVerts[0])
							{
								SharedVerts[1] = tris[0].Vertices[i];
								break;
							}
						}
					}

					if (SharedVerts[0] != -1 && SharedVerts[1] != -1)
						break;
				}

				int[] UniqueVerts = new int[2] { -1, -1 };

				for (int i = 0; i < 3; ++i)
				{
					if (tris[0].Vertices[i] != SharedVerts[0] && tris[0].Vertices[i] != SharedVerts[1])
						UniqueVerts[0] = tris[0].Vertices[i];
					if (tris[1].Vertices[i] != SharedVerts[0] && tris[1].Vertices[i] != SharedVerts[1])
						UniqueVerts[1] = tris[1].Vertices[i];
				}

				for (int i = 0; i < 3; ++i)
				{
					if (tris[0].Vertices[i] == SharedVerts[0])
						tris[0].Vertices[i] = UniqueVerts[0];
					else if (tris[0].Vertices[i] == SharedVerts[1])
						tris[0].Vertices[i] = UniqueVerts[1];
					else if (tris[0].Vertices[i] == UniqueVerts[0])
						tris[0].Vertices[i] = SharedVerts[0];
					else if (tris[0].Vertices[i] == UniqueVerts[1])
						tris[0].Vertices[i] = SharedVerts[1];

					if (tris[1].Vertices[i] == SharedVerts[0])
						tris[1].Vertices[i] = UniqueVerts[0];
					else if (tris[1].Vertices[i] == SharedVerts[1])
						tris[1].Vertices[i] = UniqueVerts[1];
					else if (tris[1].Vertices[i] == UniqueVerts[0])
						tris[1].Vertices[i] = SharedVerts[0];
					else if (tris[1].Vertices[i] == UniqueVerts[1])
						tris[1].Vertices[i] = SharedVerts[1];
				}

				SharedVerts = new int[2] { -1, -1 };

				for (int i = 0; i < 3; ++i)
				{
					for (int z = 0; z < 3; ++z)
					{
						if (tris[0].SkinVerts[i] == tris[1].SkinVerts[z])
						{
							if (SharedVerts[0] == -1)
							{
								SharedVerts[0] = tris[0].SkinVerts[i];
								break;
							}
							else if (SharedVerts[1] == -1 && tris[0].SkinVerts[i] != SharedVerts[0])
							{
								SharedVerts[1] = tris[0].SkinVerts[i];
								break;
							}
						}
					}

					if (SharedVerts[0] != -1 && SharedVerts[1] != -1)
						break;
				}

				UniqueVerts = new int[2] { -1, -1 };

				for (int i = 0; i < 3; ++i)
				{
					if (tris[0].SkinVerts[i] != SharedVerts[0] && tris[0].SkinVerts[i] != SharedVerts[1])
						UniqueVerts[0] = tris[0].SkinVerts[i];
					if (tris[1].SkinVerts[i] != SharedVerts[0] && tris[1].SkinVerts[i] != SharedVerts[1])
						UniqueVerts[1] = tris[1].SkinVerts[i];
				}

				for (int i = 0; i < 3; ++i)
				{
					if (tris[0].SkinVerts[i] == SharedVerts[0])
						tris[0].SkinVerts[i] = UniqueVerts[0];
					else if (tris[0].SkinVerts[i] == SharedVerts[1])
						tris[0].SkinVerts[i] = UniqueVerts[1];
					else if (tris[0].SkinVerts[i] == UniqueVerts[0])
						tris[0].SkinVerts[i] = SharedVerts[0];
					else if (tris[0].SkinVerts[i] == UniqueVerts[1])
						tris[0].SkinVerts[i] = SharedVerts[1];

					if (tris[1].SkinVerts[i] == SharedVerts[0])
						tris[1].SkinVerts[i] = UniqueVerts[0];
					else if (tris[1].SkinVerts[i] == SharedVerts[1])
						tris[1].SkinVerts[i] = UniqueVerts[1];
					else if (tris[1].SkinVerts[i] == UniqueVerts[0])
						tris[1].SkinVerts[i] = SharedVerts[0];
					else if (tris[1].SkinVerts[i] == UniqueVerts[1])
						tris[1].SkinVerts[i] = SharedVerts[1];
				}

				FlipNormals();
			}
		}

		class TExtrudeTri
		{
			public bool[] OnEdge = new bool[3];
		}

		static bool SamePair(int a1, int b1, int a2, int b2)
		{
			if ((a1 == a2 || a1 == b2) && (b1 == a2 || b1 == b2))
				return true;
			else return false;
		}

		public void ExtrudeSelected(float D, bool AutoDir, Vector3 CustomDir)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				MessageBox.Show("Must be on triangle mode", "Error", MessageBoxButtons.OK);
				return;
			}

			for (int m = 0; m < Meshes.Count; ++m)
			{
				CMesh mesh = Meshes[m];
				int num;
				int[] targvert;
				TExtrudeTri[] extTris;
				bool[] vertneeded;
				int i, f;

				CTriangle[] tempTris;
				CVertice[] tempFrames;

				int numnewverts, numnewtris, curtri;

				int numoldtris = mesh.Tris.Count, numoldverts = mesh.Verts.Count;

				vertneeded = new bool[numoldverts];
				extTris = new TExtrudeTri[numoldtris];
				targvert = new int[numoldverts];

				for (i = 0; i < numoldtris; ++i)
					extTris[i] = new TExtrudeTri();

				Vector3 dir = Vector3.Empty;

				num = 0;
				numnewtris = 0;
				for (i = 0; i < numoldtris; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						dir -= mesh.Tris[i].Normal;
						num++;
					}
				}

				if (num == 0)
				{
					MessageBox.Show("No faces are selected.", "Error", MessageBoxButtons.OK);
					return;
				}

				if (AutoDir)
				{
					dir /= num;
					dir.Normalize();
				}
				else
					dir = CustomDir;

				Vector3 ofs = D * dir;

				for (i = 0; i < numoldtris; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						extTris[i].OnEdge[0] = true;
						extTris[i].OnEdge[1] = true;
						extTris[i].OnEdge[2] = true;
					}
					else
					{
						extTris[i].OnEdge[0] = false;
						extTris[i].OnEdge[1] = false;
						extTris[i].OnEdge[2] = false;
					}
				}

				for (int t1=0; t1 < numoldtris - 1; t1++)
				{
					for (int t2=t1 + 1; t2 < numoldtris; t2++)
					{
						if ((mesh.Tris[t1].Flags & ETriangleFlags.Selected) != 0 && (mesh.Tris[t2].Flags & ETriangleFlags.Selected) != 0)
						{
							if (SamePair(mesh.Tris[t1].Vertices[0], mesh.Tris[t1].Vertices[1],
										  mesh.Tris[t2].Vertices[0], mesh.Tris[t2].Vertices[1]))
							{
								extTris[t1].OnEdge[0] = false;
								extTris[t2].OnEdge[0] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[0], mesh.Tris[t1].Vertices[1],
										  mesh.Tris[t2].Vertices[1], mesh.Tris[t2].Vertices[2]))
							{
								extTris[t1].OnEdge[0] = false;
								extTris[t2].OnEdge[1] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[0], mesh.Tris[t1].Vertices[1],
										  mesh.Tris[t2].Vertices[2], mesh.Tris[t2].Vertices[0]))
							{
								extTris[t1].OnEdge[0] = false;
								extTris[t2].OnEdge[2] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[1], mesh.Tris[t1].Vertices[2],
										  mesh.Tris[t2].Vertices[0], mesh.Tris[t2].Vertices[1]))
							{
								extTris[t1].OnEdge[1] = false;
								extTris[t2].OnEdge[0] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[1], mesh.Tris[t1].Vertices[2],
										  mesh.Tris[t2].Vertices[1], mesh.Tris[t2].Vertices[2]))
							{
								extTris[t1].OnEdge[1] = false;
								extTris[t2].OnEdge[1] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[1], mesh.Tris[t1].Vertices[2],
										  mesh.Tris[t2].Vertices[2], mesh.Tris[t2].Vertices[0]))
							{
								extTris[t1].OnEdge[1] = false;
								extTris[t2].OnEdge[2] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[2], mesh.Tris[t1].Vertices[0],
										  mesh.Tris[t2].Vertices[0], mesh.Tris[t2].Vertices[1]))
							{
								extTris[t1].OnEdge[2] = false;
								extTris[t2].OnEdge[0] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[2], mesh.Tris[t1].Vertices[0],
										  mesh.Tris[t2].Vertices[1], mesh.Tris[t2].Vertices[2]))
							{
								extTris[t1].OnEdge[2] = false;
								extTris[t2].OnEdge[1] = false;
							}
							if (SamePair(mesh.Tris[t1].Vertices[2], mesh.Tris[t1].Vertices[0],
										  mesh.Tris[t2].Vertices[2], mesh.Tris[t2].Vertices[0]))
							{
								extTris[t1].OnEdge[2] = false;
								extTris[t2].OnEdge[2] = false;
							}
						}
					}
				}

				for (i = 0; i < numoldverts; i++)
					vertneeded[i] = false;

				for (i = 0; i < numoldtris; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						vertneeded[mesh.Tris[i].Vertices[0]] = true;
						vertneeded[mesh.Tris[i].Vertices[1]] = true;
						vertneeded[mesh.Tris[i].Vertices[2]] = true;
					}
				}

				numnewverts = 0;
				for (i = 0; i < numoldverts; i++)
				{
					if (vertneeded[i])
					{
						targvert[i] = (numnewverts + numoldverts);
						numnewverts++;
					}
					else targvert[i] = 0;
				}

				for (i = 0; i < numoldtris; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						if (extTris[i].OnEdge[0])
							numnewtris += 2;
						if (extTris[i].OnEdge[1])
							numnewtris += 2;
						if (extTris[i].OnEdge[2])
							numnewtris += 2;
					}
				}

				tempTris = new CTriangle[numoldtris + numnewtris];
				for (i = 0; i < numoldtris + numnewtris; ++i)
				{
					if (i < numoldtris)
					{
						tempTris[i] = new CTriangle();
						for (int v = 0; v < 3; ++v)
						{
							tempTris[i].Vertices[v] = mesh.Tris[i].Vertices[v];
							tempTris[i].SkinVerts[v] = mesh.Tris[i].SkinVerts[v];
						}
						tempTris[i].Normal = mesh.Tris[i].Normal;
						tempTris[i].Centre = mesh.Tris[i].Centre;
						tempTris[i].Flags = mesh.Tris[i].Flags;
					}
					else
						tempTris[i] = new CTriangle();
				}

				tempFrames = new CVertice[numoldverts + numnewverts];
				for (i = 0; i < numoldverts + numnewverts; ++i)
				{
					if (i < numoldverts)
					{
						tempFrames[i] = new CVertice();
						tempFrames[i].Flags = mesh.Verts[i].Flags;
						tempFrames[i].FrameData = mesh.Verts[i].FrameData;
					}
					else
					{
						tempFrames[i] = new CVertice();
						for (int c = 0; c < Frames.Count; ++c)
							tempFrames[i].FrameData.Add(new CVerticeFrameData());
					}
				}

				for (f = 0; f < Frames.Count; f++)
				{
					if (AutoDir)
						dir = Vector3.Empty;

					CalcNormals(CALCNORMS_ALL, f);

					if (AutoDir)
					{
						for (i = 0; i < numoldtris; i++)
						{
							if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
								dir -= mesh.Tris[i].Normal;
						}

						dir /= num;
						dir.Normalize();

						ofs = D * dir;
					}

					for (i = 0; i < numoldverts; i++)
					{
						if (vertneeded[i])
							tempFrames[targvert[i]].FrameData[f].Position = mesh.Verts[i].FrameData[f].Position + ofs;
					}
				}

				for (i = 0; i < numoldtris; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						tempTris[i].Vertices[0] = targvert[mesh.Tris[i].Vertices[0]];
						tempTris[i].Vertices[1] = targvert[mesh.Tris[i].Vertices[1]];
						tempTris[i].Vertices[2] = targvert[mesh.Tris[i].Vertices[2]];
					}
				}

				curtri = numoldtris;
				for (i = 0; i < numoldtris; i++)
				{
					if (extTris[i].OnEdge[0])
					{
						tempTris[curtri].Vertices[0] = mesh.Tris[i].Vertices[0];
						tempTris[curtri].Vertices[1] = mesh.Tris[i].Vertices[1];
						tempTris[curtri].Vertices[2] = targvert[mesh.Tris[i].Vertices[1]];
						curtri++;

						tempTris[curtri].Vertices[0] = targvert[mesh.Tris[i].Vertices[1]];
						tempTris[curtri].Vertices[1] = targvert[mesh.Tris[i].Vertices[0]];
						tempTris[curtri].Vertices[2] = mesh.Tris[i].Vertices[0];
						curtri++;
					}
					if (extTris[i].OnEdge[1])
					{
						tempTris[curtri].Vertices[1] = mesh.Tris[i].Vertices[1];
						tempTris[curtri].Vertices[2] = mesh.Tris[i].Vertices[2];
						tempTris[curtri].Vertices[0] = targvert[mesh.Tris[i].Vertices[2]];
						curtri++;

						tempTris[curtri].Vertices[1] = targvert[mesh.Tris[i].Vertices[2]];
						tempTris[curtri].Vertices[2] = targvert[mesh.Tris[i].Vertices[1]];
						tempTris[curtri].Vertices[0] = mesh.Tris[i].Vertices[1];
						curtri++;
					}
					if (extTris[i].OnEdge[2])
					{
						tempTris[curtri].Vertices[2] = mesh.Tris[i].Vertices[2];
						tempTris[curtri].Vertices[0] = mesh.Tris[i].Vertices[0];
						tempTris[curtri].Vertices[1] = targvert[mesh.Tris[i].Vertices[0]];
						curtri++;

						tempTris[curtri].Vertices[2] = targvert[mesh.Tris[i].Vertices[0]];
						tempTris[curtri].Vertices[0] = targvert[mesh.Tris[i].Vertices[2]];
						tempTris[curtri].Vertices[1] = mesh.Tris[i].Vertices[2];
						curtri++;
					}
				}

				mesh.Tris = tempTris.ToList<CTriangle>();
				mesh.Verts = tempFrames.ToList<CVertice>();

				for (i = numoldtris; i < numoldtris + numnewtris; i++)
					mesh.Tris[i].Flags &= ~ETriangleFlags.Selected;

				for (i = 0; i < numnewtris; i++)
				{
					mesh.Tris[i + numoldtris].SkinVerts[0] = 0;
					mesh.Tris[i + numoldtris].SkinVerts[1] = 0;
					mesh.Tris[i + numoldtris].SkinVerts[2] = 0;
				}

				CalcAllNormals();
			}
		}

		public class CTriangleUnify
		{
			public CTriangle Tri;
			public bool Clockwise;
		}

		public void UnifyNormals()
		{
			int SelectedNum = 0;

			CalcAllNormals();

			for (int m = 0; m < Meshes.Count; ++m)
			{
				var mesh = Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
						SelectedNum++;
				}

				if (SelectedNum <= 1)
					continue;

				CTriangleUnify[] unified = new CTriangleUnify[SelectedNum];

				for (int i = 0, z = 0; i < mesh.Tris.Count; ++i)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
					{
						unified[z] = new CTriangleUnify();
						unified[z].Tri = mesh.Tris[i];
						z++;
					}
				}
			}
		}

		bool IsVertUsedByUnselected(int m, int n)
		{
			var mesh = Meshes[m];

			for (int t = 0; t < mesh.Tris.Count; t++)
			{
				if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) == 0)
				{
					if (mesh.Tris[t].Vertices[0] == n ||
						mesh.Tris[t].Vertices[1] == n ||
						mesh.Tris[t].Vertices[2] == n)
						return true;
				}
			}

			return false;
		}

		public void Detach()
		{
			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; i++)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
						goto skip;
				}

				continue;
skip:

				int[] ToVerts = new int[mesh.Verts.Count];
				int VertTop, t;
				CVertice[] NewVerts;

				for (int i = 0; i < mesh.Verts.Count; i++)
					ToVerts[i] = -1;

				VertTop = mesh.Verts.Count;

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					{
						for (int i = 0; i < 3; ++i)
						{
							if (IsVertUsedByUnselected(m, mesh.Tris[t].Vertices[i]))
							{
								if (ToVerts[mesh.Tris[t].Vertices[i]] == -1)
									ToVerts[mesh.Tris[t].Vertices[i]] = VertTop++;
							}
						}
					}
				}

				if (VertTop == mesh.Verts.Count)
					continue;

				NewVerts = new CVertice[VertTop];

				for (int i = 0; i < mesh.Verts.Count; ++i)
					NewVerts[i] = mesh.Verts[i];

				for (int v = 0; v < mesh.Verts.Count; v++)
				{
					if (ToVerts[v] != -1)
						NewVerts[ToVerts[v]] = NewVerts[v].Copy();
				}

				for (t = 0; t < mesh.Tris.Count; t++)
				{
					if ((mesh.Tris[t].Flags & ETriangleFlags.Selected) != 0)
					{
						for (int i = 0; i < 3; ++i)
						{
							if (ToVerts[mesh.Tris[t].Vertices[i]] != -1)
								mesh.Tris[t].Vertices[i] = ToVerts[mesh.Tris[t].Vertices[i]];
						}
					}
				}

				mesh.Verts = NewVerts.ToList<CVertice>();
			}
		}

		public void Fit(bool All)
		{
			int size;
			Vector3 min, max, zoom = Vector3.Empty;
			bool NoSel = true;

			min = new Vector3(100000, 100000, 100000);
			max = new Vector3(-100000, -100000, -100000);

			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

				bool[] vis = new bool[mesh.Verts.Count];

				for (int i=0; i < mesh.Verts.Count; i++)
					vis[i] = (All) ? true : false;

				if (!All)
				{
					if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
					{
						for (int i=0; i < mesh.Verts.Count; i++)
							vis[i] = (mesh.Verts[i].Flags & EVerticeFlags.Selected) != 0;
					}

					if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
					{
						for (int i=0; i < mesh.Tris.Count; i++)
						{
							if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
							{
								vis[mesh.Tris[i].Vertices[0]] = true;
								vis[mesh.Tris[i].Vertices[1]] = true;
								vis[mesh.Tris[i].Vertices[2]] = true;
							}
						}
					}
				}

				for (int n=0; n < mesh.Verts.Count; n++)
				{
					if (All && vis[n] || !All)
					{
						NoSel = false;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.x < min.x)
							min.x = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.x;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.x > max.x)
							max.x = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.x;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.y < min.y)
							min.y = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.y;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.y > max.y)
							max.y = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.y;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.z < min.z)
							min.z = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.z;
						if (mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.z > max.z)
							max.z = mesh.Verts[n].FrameData[CMDLGlobals.g_CurFrame].Position.z;
					}
				}
			}

			int vwidth = (Program.Form_ModelEditor.simpleOpenGlControl2.Width + 
				Program.Form_ModelEditor.simpleOpenGlControl3.Width +
				Program.Form_ModelEditor.simpleOpenGlControl4.Width) / 3;
			int vheight = (Program.Form_ModelEditor.simpleOpenGlControl2.Height + 
				Program.Form_ModelEditor.simpleOpenGlControl3.Height +
				Program.Form_ModelEditor.simpleOpenGlControl4.Height) / 3;

			if (vheight>vwidth) size = vwidth;
			else size = vheight;

			if (NoSel)
			{
				CMDLGlobals.g_PanPosition = Vector3.Empty;
				CMDLGlobals.g_Zoom2DFactor = 2;
			}
			else
			{
				CMDLGlobals.g_PanPosition.x = (min.x+max.x)/2;
				CMDLGlobals.g_PanPosition.y = -(min.y+max.y)/2;
				CMDLGlobals.g_PanPosition.z = -(min.z+max.z)/2;

				CMDLGlobals.g_OldPanPosition = CMDLGlobals.g_PanPosition;
				Program.Form_ModelEditor.Cam.lookAt(Program.Form_ModelEditor.Cam.getPosition(), new Vector3(CMDLGlobals.g_PanPosition.x, -CMDLGlobals.g_PanPosition.z, CMDLGlobals.g_PanPosition.y), new Vector3(0, 1, 0));

				if (max.z - min.z == 0)
				{
					min.z = -10;
					max.z = 10;
				}
				if (max.x - min.x == 0)
				{
					min.x = -10;
					max.x = 10;
				}
				if (max.y - min.y == 0)
				{
					min.y = -10;
					max.y = 10;
				}

				zoom.x = (float)(0.8*size/(max.x-min.x));
				zoom.y = (float)(0.8*size/(max.y-min.y));
				zoom.z = (float)(0.8*size/(max.z-min.z));
				CMDLGlobals.g_Zoom2DFactor = zoom.x;
				if (zoom.y<CMDLGlobals.g_Zoom2DFactor)
					CMDLGlobals.g_Zoom2DFactor = zoom.y;
				if (zoom.z<CMDLGlobals.g_Zoom2DFactor)
					CMDLGlobals.g_Zoom2DFactor = zoom.z;
			}

			if (CMDLGlobals.g_Zoom2DFactor == 0)
				CMDLGlobals.g_Zoom2DFactor = 1;
		}

		public void CreateBone(Vector3 pos)
		{
			CBone bon = new CBone();
			bon.Position = pos;

			int bo = -1;

			for (int i = 0; i < Bones.Count; ++i)
			{
				CBone bone = Bones[i];
				if ((bone.Flags & EBoneFlags.Selected) != 0)
				{
					if (bo != -1)
					{
						bo = -1;
						break;
					}

					bo = i;
				}
			}

			if (bo != -1)
			{
				bon.Parent = Bones[bo];

				Vector3 subt = (bon.Parent.Position - bon.Position).ToAngles();
				bon.Parent.Angles = subt;
				bon.Angles = subt;
			}

			Bones.Add(bon);

			foreach (CBone b in Bones)
				b.Flags &= ~EBoneFlags.Selected;

			bon.Flags |= EBoneFlags.Selected;
		}
	};
}
