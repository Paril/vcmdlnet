using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCMDL.NET
{
	public enum EPluginType
	{
		PLUGIN_OPEN,
		PLUGIN_SAVE,
		PLUGIN_IMPORT,
		PLUGIN_EXPORT,
		PLUGIN_TOOLS
	}

	public enum EVerticeFlags
	{
		Selected = 1,
		Visible = 2,
		TempSelected = 4
	}

	public abstract class CPluginVerticeFrameData
	{
		public abstract Vector3 Vector
		{
			get;
			set;
		}

		public abstract Vector3 Normal
		{
			get;
			set;
		}
	}

	public abstract class CPluginVertice
	{
		public abstract CPluginVerticeFrameData GetFrameDataAt(int Frame);

		public abstract EVerticeFlags Flags
		{
			get;
			set;
		}
	}

	public enum ESkinVertexFlags
	{
		Selected = 1
	}

	public abstract class CPluginSkinVertice
	{
		public abstract float s
		{
			get;
			set;
		}

		public abstract float t
		{
			get;
			set;
		}

		public abstract ESkinVertexFlags Flags
		{
			get;
			set;
		}
	}

	public abstract class CPluginFrame
	{
		public abstract string FrameName
		{
			get;
			set;
		}
	}

	public enum ETriangleFlags
	{
		Selected = 1,
		Visible = 2,
		SkinSelected = 4,
		TempSelected = 8,
	}

	public abstract class CPluginTriangle
	{
		public abstract int[] Vertices
		{
			get;
			set;
		}

		public abstract int[] SkinVertices
		{
			get;
			set;
		}

		public abstract ETriangleFlags Flags
		{
			get;
			set;
		}
	}
	
	public abstract class CPluginSkin
	{
		public abstract int Width
		{
			get;
			set;
		}

		public abstract int Height
		{
			get;
			set;
		}

		public abstract System.Drawing.Bitmap Bitmap
		{
			get;
			set;
		}

		public abstract string Path
		{
			get;
			set;
		}
	}

	public abstract class CPluginMesh
	{
		public abstract int GetTriangleCount();
		public abstract CPluginTriangle GetTriangleAt(int Index);
		public abstract void RemoveTriangleAt(int Index);
		public abstract CPluginTriangle CreateTriangle();

		public abstract int GetSkinVerticeCount();
		public abstract CPluginSkinVertice GetSkinVerticeAt(int Index);
		public abstract void RemoveSkinVerticeAt(int Index);
		public abstract CPluginSkinVertice CreateSkinVertice();

		public abstract int GetVerticeCount();
		public abstract CPluginVertice GetVerticeAt(int Index);
		public abstract void RemoveVerticeAt(int Index);
		public abstract CPluginVertice CreateVertice();

		public abstract void SetSkin(CPluginSkin skin);
		public abstract CPluginSkin GetSkin();

		public abstract void SetName(string name);
		public abstract string GetName();
	}

	public enum ESettingType
	{
		QuakeRootDirectory
	}

	// interface to internal model stuff
	public abstract class CPluginModel
	{
		public abstract int GetFrameCount();
		public abstract CPluginFrame GetFrameAt(int Index);
		public abstract void RemoveFrameAt(int Index);
		public abstract CPluginFrame CreateFrame();

		public abstract int GetMeshCount();
		public abstract CPluginMesh GetMeshAt(int Index);
		public abstract void RemoveMeshAt(int Index);
		public abstract CPluginMesh CreateMesh();

		public abstract int GetSkinCount();
		public abstract CPluginSkin GetSkinAt(int Index);
		public abstract void RemoveSkinAt(int Index);
		public abstract CPluginSkin CreateSkin();

		public abstract int GetSettingi(ESettingType Setting, object[] Parameters);
		public abstract string GetSettings(ESettingType Setting, object[] Parameters);
		public abstract float GetSettingf(ESettingType Setting, object[] Parameters);

		public abstract void Clear();
	}

	public abstract class CPlugin
	{
		public EPluginType Type;
		public string Name;

		public CPlugin(EPluginType Type)
		{
			this.Type = Type;
		}

		// return true if you want the model to replace g_CurMdl.
		public abstract bool Execute(CPluginModel Model);
	}

	// Convenience
	public class CCString
	{
		public static void Write(System.IO.BinaryWriter writer, string val, int maxSize)
		{
			byte[] strValues = new byte[maxSize];

			for (int i = 0; i < maxSize; ++i)
			{
				if ((val.Length - 1) < i)
					strValues[i] = unchecked((byte)'\0');
				else
					strValues[i] = unchecked((byte)val[i]);
			}

			writer.Write(strValues, 0, maxSize);
		}

		public static string Read(System.IO.BinaryReader reader, int maxSize)
		{
			string str = "";
			byte[] strValues = new byte[maxSize];
			strValues = reader.ReadBytes(maxSize);

			for (int i = 0; i < maxSize; ++i)
			{
				if (strValues[i] == '\0')
					break;

				str += (char)strValues[i];
			}

			return str;
		}

		public static void Write(System.IO.BinaryWriter writer, string val)
		{
			writer.Write(val.Length);
			Write(writer, val, val.Length);
		}

		public static string Read(System.IO.BinaryReader reader)
		{
			return Read(reader, reader.ReadInt32());
		}

		public static int Count(string val)
		{
			return 4 + (val.Length);
		}
	}
}
