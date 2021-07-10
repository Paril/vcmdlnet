using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public class CModules
	{
		public class CPluginModelInternal : CPluginModel
		{
			TCompleteModel ModelCopy = new TCompleteModel();

			public CPluginModelInternal(TCompleteModel Model)
			{
				ModelCopy.Copy(Model);

				for (int i = 0; i < ModelCopy.Frames.Count; ++i)
				{
					CPluginFrameInternal Frame = new CPluginFrameInternal();
					Frame.FrameName = ModelCopy.Frames[i].FrameName;

					FramesInternal.Add(Frame);
				}

				for (int m = 0; m < Model.Meshes.Count; ++m)
				{
					var mesh = Model.Meshes[m];
					var copyMesh = ModelCopy.Meshes[m];

					CPluginMeshInternal inMesh = new CPluginMeshInternal(this);

					for (int i = 0; i < mesh.Verts.Count; ++i)
					{
						var Vert = new CPluginMeshInternal.CPluginVerticeInternal(FramesInternal.Count);

						for (int x = 0; x < FramesInternal.Count; ++x)
						{
							Vert.FrameData[x].VectorInternal = copyMesh.Verts[i].FrameData[x].Position;
							Vert.FrameData[x].NormalInternal = copyMesh.Verts[i].FrameData[x].Normal;
						}

						Vert.FlagsInternal = mesh.Verts[i].Flags;

						inMesh.VerticeDataInternal.Add(Vert);
					}

					for (int i = 0; i < copyMesh.Tris.Count; ++i)
					{
						inMesh.TrianglesInternal.Add(new CPluginMeshInternal.CPluginTriangleInternal(copyMesh.Tris[i]));
						inMesh.TrianglesInternal[i].FlagsInternal = copyMesh.Tris[i].Flags;
					}

					for (int i = 0; i < copyMesh.SkinVerts.Count; ++i)
						inMesh.SkinVerticesInternal.Add(new CPluginMeshInternal.CPluginSkinVerticeInternal(copyMesh.SkinVerts[i]));
				}

				for (int i = 0; i < ModelCopy.Skins.Count; ++i)
					SkinsInternal.Add(new CPluginSkinInternal(ModelCopy.Skins.GetSkinAt(i).Path, ModelCopy.Skins.GetSkinAt(i).SkinSize.Width, ModelCopy.Skins.GetSkinAt(i).SkinSize.Height));
			}

			public class CPluginVerticeFrameDataInternal : CPluginVerticeFrameData
			{
				public Vector3 VectorInternal = Vector3.Empty, NormalInternal = Vector3.Empty;

				public CPluginVerticeFrameDataInternal(Vector3 _Vector, Vector3 _Normal)
				{
					VectorInternal.x = _Vector.x;
					VectorInternal.y = _Vector.y;
					VectorInternal.z = _Vector.z;

					NormalInternal.x = _Normal.x;
					NormalInternal.y = _Normal.y;
					NormalInternal.z = _Normal.z;
				}

				public override Vector3 Vector
				{
					get { return VectorInternal; }
					set { VectorInternal = value; }
				}

				public override Vector3 Normal
				{
					get { return NormalInternal; }
					set { NormalInternal = value; }
				}
			}

			public class CPluginFrameInternal : CPluginFrame
			{
				public string FrameNameInternal = "";

				public override string FrameName
				{
					get { return FrameNameInternal; }
					set { FrameNameInternal = value; }
				}
			}

			public List<CPluginFrameInternal> FramesInternal = new List<CPluginFrameInternal>();

			public override int GetFrameCount()
			{
				return FramesInternal.Count;
			}

			public override CPluginFrame GetFrameAt(int Index)
			{
				return FramesInternal[Index];
			}

			public override void RemoveFrameAt(int Index)
			{
				FramesInternal.RemoveAt(Index);
			}

			public override CPluginFrame CreateFrame()
			{
				CPluginFrameInternal Frame = new CPluginFrameInternal();
				FramesInternal.Add(Frame);
				return Frame;
			}

			public class CPluginSkinInternal : CPluginSkin
			{
				public string PathInternal = "";
				public int WidthInternal, HeightInternal;
				public System.Drawing.Bitmap PictureData;

				public CPluginSkinInternal()
				{
				}

				public CPluginSkinInternal(string _Path, int _Width, int _Height)
				{
					PathInternal = _Path;
					WidthInternal = _Width;
					HeightInternal = _Height;
				}

				public override int Width
				{
					get { return WidthInternal; }
					set { WidthInternal = value; }
				}

				public override int Height
				{
					get { return HeightInternal; }
					set { HeightInternal = value; }
				}

				public override System.Drawing.Bitmap Bitmap
				{
					get { return PictureData; }
					set { PictureData = value; }
				}

				public override string Path
				{
					get { return PathInternal; }
					set { PathInternal = value; }
				}
			}

			public List<CPluginSkinInternal> SkinsInternal = new List<CPluginSkinInternal>();

			public override int GetSkinCount()
			{
				return SkinsInternal.Count();
			}

			public override CPluginSkin GetSkinAt(int Index)
			{
				return SkinsInternal[Index];
			}

			public int GetSkinIndexOf(CPluginSkin skin)
			{
				return SkinsInternal.IndexOf((CPluginSkinInternal)skin);
			}

			public override void RemoveSkinAt(int Index)
			{
				var skin = GetSkinAt(Index);

				SkinsInternal.RemoveAt(Index);

				foreach (var m in MeshesInternal)
					if (m.GetSkin() == skin)
						m.SetSkin(null);
			}

			public override CPluginSkin CreateSkin()
			{
				CPluginSkinInternal Skin = new CPluginSkinInternal();
				SkinsInternal.Add(Skin);
				return Skin;
			}

			public class CPluginMeshInternal : CPluginMesh
			{
				CPluginModelInternal _model;
				CPluginSkin _skin;
				string _name;

				public CPluginMeshInternal(CPluginModelInternal model)
				{
					_model = model;
					_name = "Mesh " + _model.GetMeshCount();
				}

				public override void SetName(string name)
				{
					_name = name;
				}

				public override string GetName()
				{
					return _name;
				}

				public override void SetSkin(CPluginSkin skin)
				{
					_skin = skin;
				}
				
				public override CPluginSkin GetSkin()
				{
					return _skin;
				}

				public class CPluginTriangleInternal : CPluginTriangle
				{
					public int[] VerticesInternal = new int[3];
					public int[] SkinVerticesInternal = new int[3];
					public ETriangleFlags FlagsInternal = 0;

					public CPluginTriangleInternal()
					{
					}

					public CPluginTriangleInternal(CTriangle Tri)
					{
						for (int i = 0; i < 3; ++i)
							VerticesInternal[i] = (int)Tri.Vertices[i];

						for (int i = 0; i < 3; ++i)
							SkinVerticesInternal[i] = (int)Tri.SkinVerts[i];
					}

					public override int[] Vertices
					{
						get { return VerticesInternal; }
						set { VerticesInternal = value; }
					}

					public override int[] SkinVertices
					{
						get { return SkinVerticesInternal; }
						set { SkinVerticesInternal = value; }
					}

					public override ETriangleFlags Flags
					{
						get { return FlagsInternal; }
						set { FlagsInternal = value; }
					}
				}

				public List<CPluginTriangleInternal> TrianglesInternal = new List<CPluginTriangleInternal>();

				public override int GetTriangleCount()
				{
					return TrianglesInternal.Count;
				}

				public override CPluginTriangle GetTriangleAt(int Index)
				{
					return TrianglesInternal[Index];
				}

				public override void RemoveTriangleAt(int Index)
				{
					TrianglesInternal.RemoveAt(Index);
				}

				public override CPluginTriangle CreateTriangle()
				{
					CPluginTriangleInternal Triangle = new CPluginTriangleInternal();
					TrianglesInternal.Add(Triangle);
					return Triangle;
				}

				public class CPluginSkinVerticeInternal : CPluginSkinVertice
				{
					public float sInternal, tInternal;
					public ESkinVertexFlags FlagsInternal = 0;

					public CPluginSkinVerticeInternal()
					{
					}

					public CPluginSkinVerticeInternal(CSkinVertex Vert)
					{
						sInternal = Vert.s;
						tInternal = Vert.t;
					}

					public override float s
					{
						get { return sInternal; }
						set { sInternal = value; }
					}

					public override float t
					{
						get { return tInternal; }
						set { tInternal = value; }
					}

					public override ESkinVertexFlags Flags
					{
						get { return FlagsInternal; }
						set { FlagsInternal = value; }
					}
				}

				public List<CPluginSkinVerticeInternal> SkinVerticesInternal = new List<CPluginSkinVerticeInternal>();

				public override int GetSkinVerticeCount()
				{
					return SkinVerticesInternal.Count;
				}

				public override CPluginSkinVertice GetSkinVerticeAt(int Index)
				{
					return SkinVerticesInternal[Index];
				}

				public override void RemoveSkinVerticeAt(int Index)
				{
					SkinVerticesInternal.RemoveAt(Index);
				}

				public override CPluginSkinVertice CreateSkinVertice()
				{
					CPluginSkinVerticeInternal SkinVert = new CPluginSkinVerticeInternal();
					SkinVerticesInternal.Add(SkinVert);
					return SkinVert;
				}

				public class CPluginVerticeInternal : CPluginVertice
				{
					public EVerticeFlags FlagsInternal = 0;
					public List<CPluginVerticeFrameDataInternal> FrameData = new List<CPluginVerticeFrameDataInternal>();

					public CPluginVerticeInternal(int Frames)
					{
						for (int i = 0; i < Frames; ++i)
							FrameData.Add(new CPluginVerticeFrameDataInternal(Vector3.Empty, Vector3.Empty));
					}

					public override CPluginVerticeFrameData GetFrameDataAt(int Frame)
					{
						return FrameData[Frame];
					}

					public override EVerticeFlags Flags
					{
						get { return FlagsInternal; }
						set { FlagsInternal = value; }
					}
				}

				public List<CPluginVerticeInternal> VerticeDataInternal = new List<CPluginVerticeInternal>();

				public override CPluginVertice GetVerticeAt(int Index)
				{
					return VerticeDataInternal[Index];
				}

				public override int GetVerticeCount()
				{
					return VerticeDataInternal.Count;
				}

				public override void RemoveVerticeAt(int Index)
				{
					VerticeDataInternal.RemoveAt(Index);
				}

				public override CPluginVertice CreateVertice()
				{
					CPluginVerticeInternal Internal = new CPluginVerticeInternal(_model.GetFrameCount());
					VerticeDataInternal.Add(Internal);
					return Internal;
				}
			}

			public List<CPluginMeshInternal> MeshesInternal = new List<CPluginMeshInternal>();

			public override CPluginMesh GetMeshAt(int Index)
			{
				return MeshesInternal[Index];
			}

			public override int GetMeshCount()
			{
				return MeshesInternal.Count;
			}

			public override void RemoveMeshAt(int Index)
			{
				MeshesInternal.RemoveAt(Index);
			}

			public override CPluginMesh CreateMesh()
			{
				CPluginMeshInternal Internal = new CPluginMeshInternal(this);
				MeshesInternal.Add(Internal);
				return Internal;
			}

			public void Replace(TCompleteModel Model)
			{
				Model.ClearModel(false);

				for (int i = 0; i < FramesInternal.Count; ++i)
				{
					CFrame Frame = new CFrame();
					Frame.FrameName = FramesInternal[i].FrameNameInternal;

					Model.Frames.Add(Frame);
				}

				for (int m = 0; m < MeshesInternal.Count; ++m)
				{
					CMesh mesh = new CMesh();
					var inMesh = MeshesInternal[m];

					mesh.SkinIndex = GetSkinIndexOf(inMesh.GetSkin());
					mesh.Name = inMesh.GetName();

					for (int i = 0; i < inMesh.VerticeDataInternal.Count; ++i)
					{
						CVertice Vert = new CVertice();

						if (i <= mesh.Verts.Count)
							mesh.Verts.Add(Vert);
						else
							mesh.Verts[i] = Vert;

						Vert.Flags = inMesh.VerticeDataInternal[i].FlagsInternal;
						for (int x = 0; x < inMesh.VerticeDataInternal[i].FrameData.Count; ++x)
						{
							CVerticeFrameData v = new CVerticeFrameData();
							v.Position = inMesh.VerticeDataInternal[i].FrameData[x].VectorInternal;
							v.Normal = inMesh.VerticeDataInternal[i].FrameData[x].NormalInternal;

							mesh.Verts[i].FrameData.Add(v);
						}
					}

					for (int i = 0; i < inMesh.TrianglesInternal.Count; ++i)
					{
						CTriangle Triangle = new CTriangle();

						for (int x = 0; x < 3; ++x)
						{
							Triangle.Vertices[x] = inMesh.TrianglesInternal[i].VerticesInternal[x];
							Triangle.SkinVerts[x] = inMesh.TrianglesInternal[i].SkinVerticesInternal[x];

							if (Triangle.SkinVerts[x] < 0)
								Triangle.SkinVerts[x] = 0;
						}

						mesh.Tris.Add(Triangle);
					}

					for (int i = 0; i < inMesh.SkinVerticesInternal.Count; ++i)
					{
						CSkinVertex SkinVert = new CSkinVertex();
						SkinVert.s = inMesh.SkinVerticesInternal[i].sInternal;
						SkinVert.t = inMesh.SkinVerticesInternal[i].tInternal;

						if (SkinVert.s < 0)
							SkinVert.s = 0;
						if (SkinVert.s > 1)
							SkinVert.s = 1;

						if (SkinVert.t < 0)
							SkinVert.t = 0;
						if (SkinVert.t > 1)
							SkinVert.t = 1;

						mesh.SkinVerts.Add(SkinVert);
					}

					Model.Meshes.Add(mesh);
				}

				for (int i = 0; i < SkinsInternal.Count; ++i)
				{
					CSkin Skin = new CSkin(ModelEditor.FillSkins);

					if (SkinsInternal[i].PictureData == null)
					{
						if (SkinsInternal[i].PathInternal != "")
							Skin.Load(SkinsInternal[i].PathInternal);
						if (Skin.Skin == null)
							Skin.MakeEmptySkin(SkinsInternal[i].Width, SkinsInternal[i].Height);

						Skin.Path = SkinsInternal[i].PathInternal;
					}
					else
					{
						Skin.SetBitmap(SkinsInternal[i].Bitmap);
						Skin.Path = SkinsInternal[i].PathInternal;
					}

					Model.Skins.AddSkin(Skin);
				}

				Program.Form_ModelEditor.ModelLoaded();
			}

			public override int GetSettingi(ESettingType Setting, object[] Parameters)
			{
				throw new NotImplementedException("Setting " + Setting.ToString() + " is not implemented as an integral value.");
			}

			public override string GetSettings(ESettingType Setting, object[] Parameters)
			{
				switch (Setting)
				{
					case ESettingType.QuakeRootDirectory:
						return CMDLGlobals.QuakeDataDir;
				}
				throw new NotImplementedException("Setting " + Setting.ToString() + " is not implemented as a string value.");
			}

			public override float GetSettingf(ESettingType Setting, object[] Parameters)
			{
				throw new NotImplementedException("Setting " + Setting.ToString() + " is not implemented as a floating point value.");
			}

			public override void Clear()
			{
				FramesInternal.Clear();
				SkinsInternal.Clear();
				MeshesInternal.Clear();
			}
		}

		public class CModulePlugin : IDisposable
		{
			public CPlugin Plugin;
			public MethodInfo ExecuteFunction;

			public class ModuleToolStripMenuItem : ToolStripMenuItem
			{
				public CModulePlugin Module;

				public ModuleToolStripMenuItem(CModulePlugin _Module) :
					base()
				{
					Module = _Module;
				}
			}
			public ModuleToolStripMenuItem MenuItem;

			public bool Execute(TCompleteModel Model)
			{
				CPluginModelInternal PluginModel = new CPluginModelInternal(Model);
				if ((bool)ExecuteFunction.Invoke(Plugin, new object[] { PluginModel }))
					PluginModel.Replace(Model);

				return true;
			}

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
					{
						if (MenuItem != null)
							MenuItem.Dispose();
					}

					disposed = true;
				}
			}

			~CModulePlugin()
			{
				Dispose(false);
			}
		}

		public class CModule
		{
			public Assembly Module;
			public string Name;
			public List<CModulePlugin> Plugins = new List<CModulePlugin>();

			public CModule(string _Name)
			{
				Module = Assembly.LoadFrom(_Name);
				
				if (Module == null)
					return;

				if (Module.GetType("VCMDL.NET.Plugin") == null)
				{
					Module = null;
					return;
				}

				Name = _Name;

				foreach (Type type in Module.GetType("VCMDL.NET.Plugin").GetNestedTypes())
				{
					if (type.IsSubclassOf(typeof(CPlugin)))
					{
						MethodInfo mi = type.GetMethod("CreatePlugin");
						CPlugin Plugin = (CPlugin)mi.Invoke(null, null);

						if (Plugin == null)
						{
							Module = null;
							return;
						}

						CModulePlugin ModPlug = new CModulePlugin();
						ModPlug.Plugin = Plugin;
						ModPlug.ExecuteFunction = type.GetMethod("Execute");

						if (ModPlug.ExecuteFunction != null)
							Plugins.Add(ModPlug);
					}
				}
			}

			public static void ClickMenuItem(object sender, EventArgs e)
			{
				((CModulePlugin.ModuleToolStripMenuItem)sender).Module.Execute(CMDLGlobals.g_CurMdl);
			}

			public void Init()
			{
				for (int i = 0; i < Plugins.Count; ++i)
				{
					Plugins[i].MenuItem = new CModulePlugin.ModuleToolStripMenuItem(Plugins[i]);
					Plugins[i].MenuItem.Text = Plugins[i].Plugin.Name;
					Plugins[i].MenuItem.Click += ClickMenuItem;

					switch (Plugins[i].Plugin.Type)
					{
					case EPluginType.PLUGIN_TOOLS:
						Program.Form_ModelEditor.toolsToolStripMenuItem.DropDownItems.Add(Plugins[i].MenuItem);
						break;
					case EPluginType.PLUGIN_EXPORT:
						Program.Form_ModelEditor.exportToolStripMenuItem.DropDownItems.Add(Plugins[i].MenuItem);
						break;
					case EPluginType.PLUGIN_IMPORT:
						Program.Form_ModelEditor.importToolStripMenuItem.DropDownItems.Add(Plugins[i].MenuItem);
						break;
					}
				}
			}

			public bool Valid() { return Module != null; }
		};

		public static List<CModule> Modules = new List<CModule>();

		public static void LoadModule(string Name)
		{
			CModule Module = new CModule(Name);
			if (Module.Valid())
			{
				Modules.Add(Module);
				Module.Init();
			}
		}

		public static void FindPlugins()
		{
			string[] filePaths = Directory.GetFiles(".\\Plugins\\", "*.dll", SearchOption.AllDirectories);

			foreach (string file in filePaths)
			{
				if (file.EndsWith(".dll"))
					LoadModule(file);
			}
		}
	}
}
