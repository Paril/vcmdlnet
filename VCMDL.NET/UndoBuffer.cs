using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCMDL.NET
{
	public enum EUndoBufferType
	{
		BufferModel,
		BufferSkin
	}

	// Base undo index class
	public abstract class CUndoIndex
	{
		public EUndoBufferType Type;

		public CUndoIndex (EUndoBufferType _Type)
		{
			Type = _Type;
		}

		public abstract void Undo ();
		public abstract void Redo ();
	}

	// Base undo buffer class
	public class CUndoBuffer
	{
		public List<CUndoIndex> Buffer = new List<CUndoIndex> ();

		public virtual void Undo ()
		{
			for (int i = 0; i < Buffer.Count; ++i)
				Buffer[i].Undo ();
		}

		public virtual void Redo ()
		{
			for (int i = 0; i < Buffer.Count; ++i)
				Buffer[i].Redo ();
		}

		public virtual void Add (CUndoIndex Index)
		{
			Buffer.Add (Index);
		}

		public void Clear()
		{
			Buffer.Clear();
		}
	}

	// Undo handler
	public class CUndoHandler
	{
		public int MaxSize = 128;
		private int BufferIndex = -1;
		private List<CUndoBuffer> UndoList = new List<CUndoBuffer> ();

		public void Undo ()
		{
			if (BufferIndex == 0)
				return;

			if (BufferIndex == -1)
				BufferIndex = UndoList.Count - 1;
			else
				BufferIndex--;

			UndoList[BufferIndex].Undo ();
		}

		public void Redo ()
		{
			if (BufferIndex == -1)
				return;

			UndoList[BufferIndex++].Redo ();

			if (BufferIndex >= UndoList.Count)
				BufferIndex = -1;
		}

		public void Clear ()
		{
			BufferIndex = -1;
			UndoList.Clear ();
		}

		public void Add (CUndoBuffer Buf)
		{
			if (BufferIndex == -1)
			{
				if (UndoList.Count > MaxSize)
					UndoList.RemoveAt (0);
			}
			else
				// if we had already undo'ed to another location we need
				// to remove everything ahead of us
				UndoList.RemoveRange (BufferIndex, (UndoList.Count - (BufferIndex)));

			if (UndoList.Count == 0)
				BufferIndex = -1;

			UndoList.Add (Buf);
		}
	}

	public class CSkinVertMovedUndoIndex : CUndoIndex
	{
		public Tuple<int, int> skinVertIndex;
		public System.Drawing.PointF oldPosition, newPosition;

		public CSkinVertMovedUndoIndex (Tuple<int, int> _skinVertIndex, System.Drawing.PointF _oldPosition, System.Drawing.PointF _newPosition) :
			base (EUndoBufferType.BufferSkin)
		{
			skinVertIndex = _skinVertIndex;
			oldPosition = _oldPosition;
			newPosition = _newPosition;
		}

		public override void Undo ()
		{
			CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].s = oldPosition.X;
			CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].t = oldPosition.Y;
		}

		public override void Redo ()
		{
			CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].s = newPosition.X;
			CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].t = newPosition.Y;
		}
	}

	public class CSkinVertSelectedUndoIndex : CUndoIndex
	{
		public Tuple<int, int> skinVertIndex;
		public bool Selected;

		public CSkinVertSelectedUndoIndex(Tuple<int, int> _skinVertIndex, bool _Selected) :
			base (EUndoBufferType.BufferSkin)
		{
			skinVertIndex = _skinVertIndex;
			Selected = _Selected;
		}

		public override void Undo ()
		{
			if (!Selected)
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].Flags |= ESkinVertexFlags.Selected;
			else
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].Flags &= ~ESkinVertexFlags.Selected;
		}

		public override void Redo ()
		{
			if (Selected)
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].Flags |= ESkinVertexFlags.Selected;
			else
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].SkinVerts[skinVertIndex.Item2].Flags &= ~ESkinVertexFlags.Selected;
		}
	}

	public class CSkinTrisSelectedUndoIndex : CUndoIndex
	{
		public Tuple<int, int> skinVertIndex;
		public bool Selected;

		public CSkinTrisSelectedUndoIndex(Tuple<int, int> _skinVertIndex, bool _Selected) :
			base (EUndoBufferType.BufferSkin)
		{
			skinVertIndex = _skinVertIndex;
			Selected = _Selected;
		}

		public override void Undo ()
		{
			if (!Selected)
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].Tris[skinVertIndex.Item2].Flags |= ETriangleFlags.SkinSelected;
			else
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].Tris[skinVertIndex.Item2].Flags &= ~ETriangleFlags.SkinSelected;
		}

		public override void Redo ()
		{
			if (Selected)
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].Tris[skinVertIndex.Item2].Flags |= ETriangleFlags.SkinSelected;
			else
				CMDLGlobals.g_CurMdl.Meshes[skinVertIndex.Item1].Tris[skinVertIndex.Item2].Flags &= ~ETriangleFlags.SkinSelected;
		}
	}

	public static class UndoHandlers
	{
		public static CUndoHandler SkinUndoHandler = new CUndoHandler ();
	}
}
