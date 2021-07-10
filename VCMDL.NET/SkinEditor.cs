using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public partial class SkinEditor : Form
	{
		public enum ESkinActionType
		{
			EActionSelect,
			EActionMove,
			EActionRotate,
			EActionScale
		}
		ESkinActionType Action = ESkinActionType.EActionSelect;

		static CheckBox[] boxList;

		ESelectType SelectMode = ESelectType.Vertex;

		public void SwitchSelectionType (ESelectType Wanted)
		{
			if (SelectMode == Wanted)
				return;

			CheckBox wantedBox = (Wanted == ESelectType.Vertex) ? button5 : button6;
			CheckBox curBox = (SelectMode == ESelectType.Vertex) ? button5 : button6;

			curBox.CheckState = CheckState.Unchecked;
			wantedBox.CheckState = CheckState.Checked;
			SelectMode = Wanted;

			ImageUpdated ();
		}

		public void SwitchCurrentAction (ESkinActionType Wanted)
		{
			if (boxList == null)
				boxList = new CheckBox[] 
                {
                    Program.Form_SkinEditor.checkBox1,
                    Program.Form_SkinEditor.checkBox4,
                    Program.Form_SkinEditor.checkBox5,
                    Program.Form_SkinEditor.checkBox6
                };

			CheckBox curBox = boxList[(int)Action];

			if (Action == Wanted)
				return;

			CheckBox wantedBox = boxList[(int)Wanted];
			curBox.CheckState = CheckState.Unchecked;
			wantedBox.CheckState = CheckState.Checked;
			Action = Wanted;
		}

		public SkinEditor ()
		{
			InitializeComponent ();
		}

		CControlMouseMoveHook pictureBox1_MouseHook;

		private void SkinEditor_Load (object sender, EventArgs e)
		{
			pictureBox1_MouseHook = new CControlMouseMoveHook (
			pictureBox1,
			pictureBox1_MouseDown,
			pictureBox1_MouseUp,
			pictureBox1_MouseMove,
			null);

			KeyPreview = true;
		}

		protected override bool ProcessKeyPreview(ref Message m)
		{
			if (m.Msg == (int)WM.KEYDOWN)
			{
				switch ((Keys)m.WParam)
				{
				case Keys.A:
					selectAllToolStripMenuItem_Click(null, null);
					break;
				case Keys.OemQuestion:
					selectNoneToolStripMenuItem_Click(null, null);
					break;
				case Keys.I:
					selectInverseToolStripMenuItem_Click(null, null);
					break;
				case Keys.OemCloseBrackets:
					selectConnectedToolStripMenuItem_Click(null, null);
					break;
				case Keys.OemOpenBrackets:
					selectTouchingToolStripMenuItem_Click(null, null);
					break;
				case Keys.W:
					button8_Click(null, null);
					break;
				}
			}

			return base.ProcessKeyPreview(ref m);
		}

		private Bitmap drawnBitmap = null;

		Rectangle SquareFromPoint(PointF pt)
		{
			return new Rectangle(
					(int)(pt.X - ((2 * CMDLGlobals.g_ZoomFactor) / 2)),
					(int)(pt.Y - ((2 * CMDLGlobals.g_ZoomFactor) / 2)),
					2 * CMDLGlobals.g_ZoomFactor,
					2 * CMDLGlobals.g_ZoomFactor);
		}

		public void ImageUpdated ()
		{
			if (!CMDLGlobals.g_CurMdl.Valid() || CMDLGlobals.g_CurMdl.Skins.Count == 0 || (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width == 0 || CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height == 0))
				return;

			CSkin img = CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin);
			Bitmap tickMap = new Bitmap(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width * CMDLGlobals.g_ZoomFactor, CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height * CMDLGlobals.g_ZoomFactor, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			using (Graphics g = Graphics.FromImage (tickMap))
			{
				if (img != null)
					g.DrawImage (img.Skin, new Rectangle (0, 0, tickMap.Width, tickMap.Height));

				for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; ++i)
				{
					int[] vertsIndex = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].SkinVerts;
					PointF[] verts = new PointF[] {CMDLGlobals.g_CurMdl.SkinVerticeToPoint(CMDLGlobals.g_CurSkin, vertsIndex[0]),
                                                   CMDLGlobals.g_CurMdl.SkinVerticeToPoint(CMDLGlobals.g_CurSkin, vertsIndex[1]),
                                                   CMDLGlobals.g_CurMdl.SkinVerticeToPoint(CMDLGlobals.g_CurSkin, vertsIndex[2])};

					for (int z = 0; z < 3; ++z)
					{
						verts[z].X *= CMDLGlobals.g_ZoomFactor;
						verts[z].Y *= CMDLGlobals.g_ZoomFactor;
					}

					Pen color = (SelectMode == ESelectType.Face && ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags & ETriangleFlags.SkinSelected) != 0)) ? Pens.Yellow : Pens.Gray;

					// Draw lines between the three verts in counter-clockwise order
					g.DrawLine (color, verts[0], verts[2]);
					g.DrawLine (color, verts[2], verts[1]);
					g.DrawLine (color, verts[1], verts[0]);
				}

				for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
				{
					PointF vert = new PointF(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].s * tickMap.Width, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].t * tickMap.Height);

					if (SelectMode == ESelectType.Vertex && ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags & ESkinVertexFlags.Selected) != 0))
						g.FillRectangle(Brushes.Yellow, SquareFromPoint(vert));
					else
						g.FillRectangle(Brushes.SaddleBrown, SquareFromPoint(vert));
				}
			}

			if (img != null)
			{
				pictureBox1.Size = tickMap.Size;
				pictureBox1.Image = tickMap;
				drawnBitmap = tickMap;
			}
		}

		bool bHaveMouse = false;
		Point ptOriginal = new Point ();
		Point ptLast = new Point ();

		public static Rectangle ReverseTriangle (Point start, Point end)
		{
			// end = bottom-right corner
			if (end.X >= start.X && start.Y < end.Y)
				return new Rectangle (start.X, start.Y, end.X - start.X, end.Y - start.Y);
			// end = bottom-left corner
			else if (end.X < start.X && start.Y < end.Y)
				return new Rectangle (end.X, start.Y, start.X - end.X, end.Y - start.Y);
			// end = top-left corner
			else if (end.X < start.X && start.Y >= end.Y)
				return new Rectangle (end.X, end.Y, start.X - end.X, start.Y - end.Y);

			// end = top-right corner
			return new Rectangle (start.X, end.Y, end.X - start.X, start.Y - end.Y);
		}

		Point LastClickLocation = new Point (-1, -1);
		public void DrawImageSelection (Graphics g)
		{
			if (LastClickLocation == new Point (-1, -1))
				return;

			switch (Action)
			{
				case ESkinActionType.EActionSelect:
					g.DrawRectangle (Pens.Gray, ReverseTriangle (ptOriginal, ptLast));
					break;
				case ESkinActionType.EActionRotate:
					g.DrawEllipse (Pens.White, LastClickLocation.X - 3, LastClickLocation.Y - 3, 6, 6);
					break;
			}
		}

		CUndoBuffer PreClickUndoBuffer;

		private void pictureBox1_MouseDown (object sender, HookMouseEventArgs e)
		{
			// Make a note that we "have the mouse".
			bHaveMouse = true;
			// Store the "starting point" for this rubber-band rectangle.
			ptOriginal = e.MouseEvent.Location;
			// Special value lets us know that no previous
			// rectangle needs to be erased.
			ptLast.X = -1;
			ptLast.Y = -1;

			PreClickUndoBuffer = new CUndoBuffer();

			List<CSelectedVertice> verts = RetrieveSelectedVertices ();
			switch (Action)
			{
				case ESkinActionType.EActionSelect:
					break;
				case ESkinActionType.EActionRotate:
				case ESkinActionType.EActionMove:
				case ESkinActionType.EActionScale:
					foreach (CSelectedVertice v in verts)
						PreClickUndoBuffer.Add(new CSkinVertMovedUndoIndex(Tuple.Create(CMDLGlobals.g_CurSkin, v.Index), new PointF(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v.Index].s, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v.Index].t), PointF.Empty));
					break;
			}
		}

		private void pictureBox1_MouseUp (object sender, HookMouseEventArgs e)
		{
			// Set internal flag to know we no longer "have the mouse".
			bHaveMouse = false;

			if (ptLast.X == -1 && ptLast.Y == -1)
			{
				ptLast = ptOriginal;

				ptOriginal.X -= 1;
				ptOriginal.Y -= 1;

				ptLast.X += 1;
				ptLast.Y += 1;
			}

			if (PreClickUndoBuffer == null)
				return;

			if (Action == ESkinActionType.EActionSelect)
			{
				Rectangle rect = ReverseTriangle (ptOriginal, ptLast);

				switch (SelectMode)
				{
					case ESelectType.Vertex:
						for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
						{
							PointF vert = new PointF(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].s * (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width * CMDLGlobals.g_ZoomFactor),
								CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].t * (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height * CMDLGlobals.g_ZoomFactor));

							Rectangle ptRect = new Rectangle((int)(vert.X - ((2 * CMDLGlobals.g_ZoomFactor) / 2)), (int)(vert.Y - ((2 * CMDLGlobals.g_ZoomFactor) / 2)), 2 * CMDLGlobals.g_ZoomFactor, 2 * CMDLGlobals.g_ZoomFactor);

							if (ptRect.IntersectsWith (rect))
							{
								if ((ModifierKeys & Keys.Alt) != 0)
									CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags &= ~ESkinVertexFlags.Selected;
								else
									CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags |= ESkinVertexFlags.Selected;
								PreClickUndoBuffer.Add(new CSkinVertSelectedUndoIndex(Tuple.Create(CMDLGlobals.g_CurSkin, i), (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags & ESkinVertexFlags.Selected) != 0));
							}
						}
						break;
					case ESelectType.Face:
						for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
						{
							for (int i = 0; i < 3; ++i)
							{
								PointF vert = new PointF(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i]].s * (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width * CMDLGlobals.g_ZoomFactor),
									CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i]].t * (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height * CMDLGlobals.g_ZoomFactor));

								Rectangle ptRect = new Rectangle((int)(vert.X - ((2 * CMDLGlobals.g_ZoomFactor) / 2)), (int)(vert.Y - ((2 * CMDLGlobals.g_ZoomFactor) / 2)), 2 * CMDLGlobals.g_ZoomFactor, 2 * CMDLGlobals.g_ZoomFactor);

								if (ptRect.IntersectsWith (rect))
								{
									if ((ModifierKeys & Keys.Alt) == 0)
									{
										CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags |= ETriangleFlags.SkinSelected;
										if (CMDLGlobals.g_SyncSelections)
											CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags |= ETriangleFlags.Selected;
									}
									else
									{
										CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags &= ~ETriangleFlags.SkinSelected;
										if (CMDLGlobals.g_SyncSelections)
											CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags &= ~ETriangleFlags.Selected;
									}

									PreClickUndoBuffer.Add(new CSkinTrisSelectedUndoIndex(Tuple.Create(CMDLGlobals.g_CurSkin, t), (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags & ETriangleFlags.SkinSelected) != 0));
									break;
								}
							}
						}
						break;
				}
			}

			// Set flags to know that there is no "previous" line to reverse.
			ptLast.X = -1;
			ptLast.Y = -1;
			ptOriginal.X = -1;
			ptOriginal.Y = -1;
			LastClickLocation = new Point (-1, -1);

			ImageUpdated ();
			pictureBox1.Invalidate ();

			if (PreClickUndoBuffer != null && PreClickUndoBuffer.Buffer.Count != 0)
			{
				if (Action != ESkinActionType.EActionSelect)
				{
					List<CSelectedVertice> verts = RetrieveSelectedVertices ();

					int i = 0;
					foreach (CSelectedVertice v in verts)
					{
						CSkinVertMovedUndoIndex ind = (CSkinVertMovedUndoIndex)PreClickUndoBuffer.Buffer[i++];
						ind.newPosition = new PointF(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v.Index].s, CMDLGlobals.g_CurMdl.Meshes[1].SkinVerts[v.Index].t);
					}
				}

				UndoHandlers.SkinUndoHandler.Add (PreClickUndoBuffer);
			}
		}

		List<CSelectedVertice> RetrieveSelectedVertices ()
		{
			List<CSelectedVertice> verts = new List<CSelectedVertice> ();

			switch (SelectMode)
			{
				case ESelectType.Vertex:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
					{
						CSkinVertex Vert = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i];

						if ((Vert.Flags & ESkinVertexFlags.Selected) != 0)
							verts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, i, null));
					}
					break;
				case ESelectType.Face:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; i++)
					{
						if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags & ETriangleFlags.SkinSelected) != 0)
						{
							verts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].SkinVerts[0], null));
							verts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].SkinVerts[1], null));
							verts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].SkinVerts[2], null));
						}								   
					}
					break;
			}

			return verts;
		}

		void MoveSkinVerts (HookMouseEventArgs e)
		{
			Point mov = e.MoveDelta;

			if (checkBox2.Checked == false)
				mov.X = 0;
			if (checkBox3.Checked == false)
				mov.Y = 0;

			List<CSelectedVertice> selVerts = RetrieveSelectedVertices ();
			float xMov = (((float)mov.X) / CMDLGlobals.g_ZoomFactor) / (float)CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width;
			float yMov = (((float)mov.Y) / CMDLGlobals.g_ZoomFactor) / (float)CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height;

			for (int i = 0; i < selVerts.Count; ++i)
			{
				CSkinVertex Vert = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[selVerts[i].Index];

				PointF movPnt = new PointF (Vert.s + xMov, Vert.t + yMov);
				selVerts[i].Data = movPnt;

				// try the move
				if ((movPnt.X < 0 || movPnt.X >= 1) ||
					(movPnt.Y < 0 || movPnt.Y >= 1))
					return;
			}

			// Build undo buffer
			foreach (CSelectedVertice v in selVerts)
			{
				CSkinVertex Vert = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v.Index];
				Vert.s = ((PointF)v.Data).X;
				Vert.t = ((PointF)v.Data).Y;
			}

			ImageUpdated ();
		}

		void RotateSkinVerts (HookMouseEventArgs e)
		{
			List<CSelectedVertice> RotateList = RetrieveSelectedVertices ();

			float g_DownS = (pictureBox1.PointToClient(e.ClickPos).X / CMDLGlobals.g_ZoomFactor);
			float g_DownT = (pictureBox1.PointToClient(e.ClickPos).Y / CMDLGlobals.g_ZoomFactor);
			float ang = (float)(Math.PI / 360.0 * -e.MoveDelta.Y / CMDLGlobals.g_ZoomFactor);

			for (int i = 0; i < RotateList.Count; i++)
			{
				CSkinVertex vert = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[RotateList[i].Index];

				// Calculate & see if it works
				float nx = (float)(((vert.s * CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Width) - g_DownS) * Math.Cos(ang) + ((vert.t * CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Height) - g_DownT) * Math.Sin(ang));
				float ny = (float)(((vert.t * CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Height) - g_DownT) * Math.Cos(ang) - ((vert.s * CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Width) - g_DownS) * Math.Sin(ang));
				nx += g_DownS;
				ny += g_DownT;

				if (nx < 0 || nx >= CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Width ||
					ny < 0 || ny >= CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Height)
					return;

				RotateList[i].Data = new PointF(nx / CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Width, ny / CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SkinSize.Height);
			}

			foreach (CSelectedVertice ind in RotateList)
			{
				CSkinVertex skv = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[ind.Index];

				skv.s = ((PointF)ind.Data).X;
				skv.t = ((PointF)ind.Data).Y;
			}

			ImageUpdated ();
		}

		void ScaleSkinVerts (HookMouseEventArgs e)
		{
			double ScaleX = 1, ScaleY = 1;

			if (checkBox2.Checked)
				ScaleX = (1 + 0.01 * -e.MoveDelta.Y);
			if (checkBox3.Checked)
				ScaleY = (1 + 0.01 * -e.MoveDelta.Y);

			List<CSelectedVertice> RotateList = RetrieveSelectedVertices ();

			float g_DownS = (pictureBox1.PointToClient(e.ClickPos).X / CMDLGlobals.g_ZoomFactor) / (float)CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width;
			float g_DownT = (pictureBox1.PointToClient(e.ClickPos).Y / CMDLGlobals.g_ZoomFactor) / (float)CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height;

			for (int i = 0; i < RotateList.Count; i++)
			{
				// Calculate & see if it works
				float s = (float)((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[RotateList[i].Index].s - g_DownS) * ScaleX + g_DownS);
				float t = (float)((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[RotateList[i].Index].t - g_DownT) * ScaleY + g_DownT);

				if (s < 0 || s >= 1 || t < 0 || t >= 1)
					return;

				RotateList[i].Data = new PointF(s, t);
			}

			foreach (CSelectedVertice ind in RotateList)
			{
				CSkinVertex skv = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[ind.Index];

				skv.s = ((PointF)ind.Data).X;
				skv.t = ((PointF)ind.Data).Y;
			}

			ImageUpdated ();
		}

		private void pictureBox1_MouseMove (object sender, HookMouseEventArgs e)
		{
			Point ptCurrent = e.MouseEvent.Location;
			// If we "have the mouse", then we draw our lines.
			if (bHaveMouse)
			{
				// Update last point.
				ptLast = ptCurrent;
				LastClickLocation = pictureBox1.PointToClient (e.ClickPos);

				switch (Action)
				{
					case ESkinActionType.EActionSelect:
						// Draw new lines.
						pictureBox1.Invalidate ();
						break;
					case ESkinActionType.EActionMove:
						MoveSkinVerts (e);
						break;
					case ESkinActionType.EActionRotate:
						RotateSkinVerts (e);
						break;
					case ESkinActionType.EActionScale:
						ScaleSkinVerts (e);
						break;
				}
				Program.Form_ModelEditor.RedrawAllViews();
			}
		}

		public void SetSkin (int Skin)
		{
			if (Skin < 0 || Skin > CMDLGlobals.g_CurMdl.Skins.Count - 1)
				return;

			CMDLGlobals.g_CurSkin = Skin;
			label1.Text = Skin.ToString ();
			label5.Text = CMDLGlobals.g_CurMdl.Skins.GetSkinAt(Skin).Path;

			ImageUpdated ();
		}

		public void ModelLoaded ()
		{
			SetSkin (0);
		}

		private void button4_Click (object sender, EventArgs e)
		{
			SetSkin (CMDLGlobals.g_CurSkin + 1);
		}

		private void button3_Click (object sender, EventArgs e)
		{
			SetSkin (CMDLGlobals.g_CurSkin - 1);
		}

		private void button1_Click (object sender, EventArgs e)
		{
			CMDLGlobals.g_ZoomFactor++;
			ImageUpdated ();
		}

		private void button2_Click (object sender, EventArgs e)
		{
			if (CMDLGlobals.g_ZoomFactor == 1)
				return;

			CMDLGlobals.g_ZoomFactor--;
			ImageUpdated ();
		}

		public static Bitmap LoadTexture(string FileName)
		{
			if (FileName.EndsWith(".pcx"))
			{
				CPCXImage pcx = new CPCXImage();

				if (pcx.Load(FileName) == 0)
					return null;

				return pcx.GetBitmap(CMDLGlobals.g_Palette);
			}

			if (FileName.EndsWith(".tga"))
			{
				try
				{
					return TargaImage.LoadTargaImage(FileName);
				}
				catch
				{
					return null;
				}
			}
			else
			{
				try
				{
					return new Bitmap(FileName);
				}
				catch
				{
					return null;
				}
			}
		}

		private void skinImageToolStripMenuItem_Click (object sender, EventArgs e)
		{
			using (OpenFileDialog ChooseSkinDlg = new OpenFileDialog())
			{
				ChooseSkinDlg.DefaultExt = "pcx";
				ChooseSkinDlg.Filter = "Supported image files (*.pcx;*.png;*.bmp;*.jpg;*.tga)|*.pcx;*.png;*.bmp;*.jpg;*.tga";

				int BaseDirLen = CMDLGlobals.QuakeDataDir.Length;

				if (CMDLGlobals.g_CurMdl.Skins.Count == 0)
				{
					CSkin BlankSkin = new CSkin(ModelEditor.FillSkins);
					BlankSkin.MakeEmptySkin(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width, CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height);
					CMDLGlobals.g_CurMdl.Skins.AddSkin(BlankSkin);
				}

				if (ChooseSkinDlg.ShowDialog() == DialogResult.Cancel)
					return;

				Bitmap bmp = LoadTexture(ChooseSkinDlg.FileName);
				if (bmp == null)
				{
					CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).MakeEmptySkin(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width, CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height);
					CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Path = ChooseSkinDlg.FileName;
					return;
				}

				CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).SetBitmap(bmp);
				CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Path = ChooseSkinDlg.FileName;
				ImageUpdated();
			}
		}

		private void SkinEditor_FormClosing (object sender, FormClosingEventArgs e)
		{
			if (!Program.ClosingFinal)
			{
				e.Cancel = true;
				Hide ();
			}
		}

		private void pictureBox1_Paint (object sender, PaintEventArgs e)
		{
			DrawImageSelection (e.Graphics);
		}

		private void checkBox1_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchCurrentAction (ESkinActionType.EActionSelect);
		}

		private void checkBox4_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchCurrentAction (ESkinActionType.EActionMove);
		}

		private void checkBox5_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchCurrentAction (ESkinActionType.EActionRotate);
		}

		private void checkBox6_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchCurrentAction (ESkinActionType.EActionScale);
		}

		private void button5_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchSelectionType (ESelectType.Vertex);
		}

		private void button6_MouseUp (object sender, MouseEventArgs e)
		{
			SwitchSelectionType (ESelectType.Face);
		}

		private void selectAllToolStripMenuItem_Click (object sender, EventArgs e)
		{
			switch (SelectMode)
			{
				case ESelectType.Vertex:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags |= ESkinVertexFlags.Selected;
					break;
				case ESelectType.Face:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags |= ETriangleFlags.SkinSelected;
					break;
			}

			ImageUpdated ();
		}

		private void selectNoneToolStripMenuItem_Click (object sender, EventArgs e)
		{
			switch (SelectMode)
			{
				case ESelectType.Vertex:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags &= ~ESkinVertexFlags.Selected;
					break;
				case ESelectType.Face:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags &= ~ETriangleFlags.SkinSelected;
					break;
			}

			ImageUpdated ();
		}

		private void selectInverseToolStripMenuItem_Click (object sender, EventArgs e)
		{
			switch (SelectMode)
			{
				case ESelectType.Vertex:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags ^= ESkinVertexFlags.Selected;
					break;
				case ESelectType.Face:
					for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; ++i)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags ^= ETriangleFlags.SkinSelected;
					break;
			}

			ImageUpdated ();
		}

		private void selectConnectedToolStripMenuItem_Click (object sender, EventArgs e)
		{
			// this one is the same for both
			bool changed = true;

			while (changed)
			{
				changed = false;
				for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
					if (
					   !((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]].Flags & ESkinVertexFlags.Selected) != 0 &&
						 (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]].Flags & ESkinVertexFlags.Selected) != 0 &&
						 (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]].Flags & ESkinVertexFlags.Selected) != 0)
									  &&
					   ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]].Flags & ESkinVertexFlags.Selected) != 0 ||
						(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]].Flags & ESkinVertexFlags.Selected) != 0 ||
						(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]].Flags & ESkinVertexFlags.Selected) != 0))
					{
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]].Flags |= ESkinVertexFlags.Selected;
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]].Flags |= ESkinVertexFlags.Selected;
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]].Flags |= ESkinVertexFlags.Selected;
						changed = true;
					}
			}

			ImageUpdated ();
		}

		private void selectTouchingToolStripMenuItem_Click (object sender, EventArgs e)
		{
			switch (SelectMode)
			{
				case ESelectType.Vertex:
					List<CSelectedVertice> changedVerts = new List<CSelectedVertice> ();

					do
					{
						bool changed = false;

						for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
							if (((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]].Flags & ESkinVertexFlags.Selected) != 0 ||
								 (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]].Flags & ESkinVertexFlags.Selected) != 0 ||
								 (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]].Flags & ESkinVertexFlags.Selected) != 0))
							{
								changedVerts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0], null));
								changedVerts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1], null));
								changedVerts.Add(new CSelectedVertice(CMDLGlobals.g_CurSkin, CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2], null));
								changed = true;
							}

						if (changed == false)
							break;
					}
					while (false);

					foreach (CSelectedVertice v in changedVerts)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v.Index].Flags |= ESkinVertexFlags.Selected;
					break;
				case ESelectType.Face:
					break;
			}

			ImageUpdated ();
		}

		private void button7_Click (object sender, EventArgs e)
		{
			List<CSelectedVertice> seld = RetrieveSelectedVertices ();

			if (seld.Count == 0)
				return;

			float centrex = 0, centrey = 0;
			for (int i = 0; i < seld.Count; i++)
			{
				centrex += CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].s;
				centrey += CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].t;

				seld[i].Data = new PointF(0, 0);
			}

			centrex /= seld.Count;
			centrey /= seld.Count;

			for (int i = 0; i < seld.Count; i++)
			{
				float s = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].s, t = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].t;

				if (checkBox2.Checked)
				{
					s = (float)(2.0 * centrex - CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].s);
					if (s < 0 || s >= 1)
						return;
				}

				if (checkBox3.Checked)
				{
					t = (float)(2.0 * centrey - CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].t);
					if (t < 0 || t >= 1)
						return;
				}

				seld[i].Data = new PointF(s, t);
			}

			for (int i = 0; i < seld.Count; i++)
			{
				CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].s = ((PointF)seld[i].Data).X;
				CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[seld[i].Index].t = ((PointF)seld[i].Data).Y;
			}

			ImageUpdated ();
		}

		private void button8_Click (object sender, EventArgs e)
		{
			int WeldTarget = 0;

			if (SelectMode != ESelectType.Vertex)
				return;

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; i++)
				if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i].Flags & ESkinVertexFlags.Selected) != 0)
				{
					WeldTarget = i;
					break;
				}

			for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
			{
				for (int i = 0; i < 3; i++)
					if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i]].Flags & ESkinVertexFlags.Selected) != 0)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i] = WeldTarget;
			}

			CMDLGlobals.g_CurMdl.DeleteUnusedSkinVerts ();

			ImageUpdated ();
		}

		private void toolStripMenuItem10_Click (object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.DeleteUnusedSkinVerts ();

			ImageUpdated ();
		}

		bool IsSkinVertUsedByUnselected (int n)
		{
			for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
			{
				if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags & ETriangleFlags.SkinSelected) == 0)
				{
					if (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0] == n ||
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1] == n ||
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2] == n)
						return true;
				}
			}

			return false;
		}

		private void button9_Click (object sender, EventArgs e)
		{
			if (SelectMode != ESelectType.Face)
				return;

			{
				bool Anything = false;
				for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; i++)
					if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[i].Flags & ETriangleFlags.SkinSelected) != 0)
						Anything = true;

				if (!Anything)
					return;
			}

			int[] ToVerts = new int[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count];
			int VertTop, t;
			CSkinVertex[] NewVerts;

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; i++)
				ToVerts[i] = -1;

			VertTop = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count;

			for (t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
			{
				if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags & ETriangleFlags.SkinSelected) != 0)
				{
					if (IsSkinVertUsedByUnselected(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]))
						if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]] == -1)
							ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]] = VertTop++;
					if (IsSkinVertUsedByUnselected(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]))
						if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]] == -1)
							ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]] = VertTop++;
					if (IsSkinVertUsedByUnselected(CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]))
						if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]] == -1)
							ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]] = VertTop++;
				}
			}

			if (VertTop == CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count)
				return;

			NewVerts = new CSkinVertex[VertTop];

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; ++i)
				NewVerts[i] = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[i];

			for (int v = 0; v < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; v++)
			{
				if (ToVerts[v] != -1)
				{
					NewVerts[ToVerts[v]] = new CSkinVertex ();
					NewVerts[ToVerts[v]].s = NewVerts[v].s;
					NewVerts[ToVerts[v]].t = NewVerts[v].t;
					NewVerts[ToVerts[v]].Flags &= ~ESkinVertexFlags.Selected;
				}
			}

			for (t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
			{
				if ((CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].Flags & ETriangleFlags.SkinSelected) != 0)
				{
					if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]] != -1)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0] = ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]];
					if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]] != -1)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1] = ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]];
					if (ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]] != -1)
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2] = ToVerts[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]];
				}
			}

			CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts = NewVerts.ToList<CSkinVertex>();
		}

		private void button10_Click (object sender, EventArgs e)
		{
			if (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count <= 1)
				return;

			if (SelectMode == ESelectType.Vertex)
			{
				bool[] del = new bool[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count];
				int v;

				for (v = 0; v < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; v++)
					del[v] = (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v].Flags & ESkinVertexFlags.Selected) != 0;

				int[] targ = new int[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count];
				int tg = 0;

				for (v = 0; v < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; v++)
				{
					if (!del[v])
					{
						targ[v] = tg;
						tg++;
					}
					else targ[v] = -1;
				}

				for (int t = 0; t < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris.Count; t++)
				{
					bool res = false;
					for (int i = 0; i < 3; i++)
						if (targ[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i]] == -1)
							res = true;

					if (res)
					{
						for (int i = 0; i < 3; i++)
							CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[i] = 0;
					}
					else
					{
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0] = targ[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[0]];
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1] = targ[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[1]];
						CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2] = targ[CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].Tris[t].SkinVerts[2]];
					}
				}

				CSkinVertex[] newskinverts = new CSkinVertex[tg];

				tg = 0;
				for (v = 0; v < CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count; v++)
				{
					if (!del[v])
					{
						newskinverts[tg] = CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts[v];
						tg++;
					}
				}

				CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts = newskinverts.ToList<CSkinVertex>();

				if (CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Count == 0)
					CMDLGlobals.g_CurMdl.Meshes[CMDLGlobals.g_CurSkin].SkinVerts.Add(new CSkinVertex());
			}

			ImageUpdated ();
		}

		private void closeToolStripMenuItem_Click (object sender, EventArgs e)
		{
			Hide ();
		}

		private void undoToolStripMenuItem_Click (object sender, EventArgs e)
		{
			UndoHandlers.SkinUndoHandler.Undo ();
			ImageUpdated ();
		}

		private void redoToolStripMenuItem_Click (object sender, EventArgs e)
		{
			UndoHandlers.SkinUndoHandler.Redo ();
			ImageUpdated ();
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}

		AddSkin AddSkinDialog = new AddSkin();
		private void addNewSkinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddSkinDialog.StartPosition = FormStartPosition.CenterParent;
			AddSkinDialog.numericUpDown1.Maximum = CMDLGlobals.g_CurMdl.Skins.Count;

			if (AddSkinDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				CSkin Skin = new CSkin(ModelEditor.FillSkins);
				Skin.MakeEmptySkin(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width, CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height);

				CMDLGlobals.g_CurMdl.Skins.InsertSkin((int)AddSkinDialog.numericUpDown1.Value, Skin);
				SetSkin((int)AddSkinDialog.numericUpDown1.Value);
			}
		}

		private void deleteCurrentSkinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_CurMdl.Skins.Count == 1)
			{
				MessageBox.Show("One skin should always be available.");
				return;
			}

			CMDLGlobals.g_CurMdl.Skins.RemoveSkin(CMDLGlobals.g_CurSkin);
			SetSkin(CMDLGlobals.g_CurSkin);
		}

		private void clearCurrentSkinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (FastPixel fp = new FastPixel(CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin, true))
				for (int x = 0; x < CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin.Width; ++x)
					for (int y = 0; y < CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin.Height; ++y)
						fp.SetPixel(x, y, Color.Black);

			ImageUpdated();
			pictureBox1.Invalidate();
		}

		private void resizeSkinToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (ResizeSkin ResizeSkinDialog = new ResizeSkin())
			{
				ResizeSkinDialog.numericUpDown1.Value =
					ResizeSkinDialog.numericUpDown6.Value = CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin.Width;
				ResizeSkinDialog.numericUpDown2.Value =
					ResizeSkinDialog.numericUpDown5.Value = CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin.Width;

				ResizeSkinDialog.numericUpDown4.Value = CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width;
				ResizeSkinDialog.numericUpDown3.Value = CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height;

				if (ResizeSkinDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					CSkin sk = CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin);

					Bitmap newBitmap = new Bitmap((int)ResizeSkinDialog.numericUpDown1.Value, (int)ResizeSkinDialog.numericUpDown2.Value);

					using (Graphics g = Graphics.FromImage(newBitmap))
						g.DrawImage(sk.Skin, new Rectangle(Point.Empty, (ResizeSkinDialog.checkBox1.Checked) ? newBitmap.Size : sk.Skin.Size));

					string oldPath = sk.Path;
					sk.SetBitmap(newBitmap);
					sk.Path = oldPath;

					ImageUpdated();
					pictureBox1.Invalidate();
				}
			}
		}

		GetPositionDialog GetPosition = new GetPositionDialog();
		private void getPositionFromModelToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (GetPosition.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				SkinVertPos SVP = GetPosition.GetSkinVertPos;

				float t = 10, l = 10, r = 100, b = 100;

				if (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width < 105)
					r = (float)(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Width - 10);
				if (CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height < b)
					b = (float)(CMDLGlobals.g_CurMdl.Skins.SizeForSkin(CMDLGlobals.g_CurSkin).Height - 10);

				CMDLGlobals.g_CurMdl.GetSkinVerticesFrom3DVertices(SVP, (int)GetPosition.numericUpDown1.Value, l, t, r, b, GetPosition.checkBox1.Checked);

				ImageUpdated();
			}
		}

		private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{

		}

		private void currentImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog ChooseSkinDlg = new SaveFileDialog())
			{
				ChooseSkinDlg.DefaultExt = "png";
				ChooseSkinDlg.Filter = "Supported image files (*.png;*.bmp;*.jpg)|*.png;*.bmp;*.jpg";
				ChooseSkinDlg.AddExtension = true;

				if (ChooseSkinDlg.ShowDialog() == DialogResult.Cancel)
					return;

				System.Drawing.Imaging.ImageFormat imgf = System.Drawing.Imaging.ImageFormat.Png;

				if (ChooseSkinDlg.FileName.EndsWith(".bmp"))
					imgf = System.Drawing.Imaging.ImageFormat.Bmp;
				else if (ChooseSkinDlg.FileName.EndsWith(".jpg"))
					imgf = System.Drawing.Imaging.ImageFormat.Jpeg;

				CMDLGlobals.g_CurMdl.Skins.GetSkinAt(CMDLGlobals.g_CurSkin).Skin.Save(ChooseSkinDlg.FileName, imgf);
			}

		}

		private void dontDrawToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void normalLinesToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void noneToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void dotsToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void ticksToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void sync3DSelectionToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{

		}
	}

	public class CSelectedVertice
	{
		public int Mesh;
		public int Index;
		public object Data;

		public CSelectedVertice(int _Mesh, int _Index, object _Data)
		{
			Mesh = _Mesh;
			Index = _Index;
			Data = _Data;
		}
	}
}
