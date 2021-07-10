using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace VCMDL.NET
{
	public partial class ModelEditor : Form
	{
		CControlMouseMoveHook simpleOpenGlControl1_MouseHook, simpleOpenGlControl2_MouseHook, simpleOpenGlControl3_MouseHook, simpleOpenGlControl4_MouseHook;
		CMouseStatus MouseStatus;
		public NewCamera Cam;

		void InitMouseStatus()
		{
			MouseStatus = new CMouseStatus(toolStripStatusLabel1);

			MouseStatus.Add(Program.Ctrl_CommonToolbox.checkBox1, "Toggle to selection mode");
			MouseStatus.Add(Program.Ctrl_CommonToolbox.checkBox2, "Toggle to move mode");
		}

		public void EnableBlend()
		{
			Gl.glEnable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
		}

		public void DisableBlend()
		{
			Gl.glDisable(Gl.GL_BLEND);
			Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE);
		}

		public void Reshape3dView(ViewportControl control)
		{
			if (Cam == null)
				return;

			Gl.glViewport(0, 0, control.Width, control.Height);

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();

			// set the perspective with the appropriate aspect ratio
			float w = (float)control.Width;
			float h = (float)control.Height;
			float aspect = 1.0f * (w / h);
			Glu.gluPerspective(CalcFovY(90, w, h), aspect, 1, 99999);
			Cam.perspective((float)CalcFovY(90, w, h), aspect, 1, 99999);

			// select modelview matrix and clear it out
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
		}

		public void Reshape2dView(ViewportControl control)
		{
			Gl.glViewport(0, 0, control.Width, control.Height);

			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();

			Glu.gluOrtho2D(0, control.Width, 0, control.Height);

			// select modelview matrix and clear it out
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
		}

		public void InitCommon()
		{
			Gl.glEnable(Gl.GL_DEPTH_TEST);

			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, new float[4] { 0.0f, 0.0f, 0.0f, 1.0f });
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, new float[4] { 0.65f, 0.65f, 0.65f, 1.0f });
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, new float[4] { 1.0f, 1.0f, 1.0f, 1.0f });
			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, new float[4] { 1.0f, 1.0f, 1.0f, 1.0f });

			Gl.glEnable(Gl.GL_LIGHT0);
			Gl.glDepthFunc(Gl.GL_LESS);
			Gl.glEnable(Gl.GL_LIGHTING);

			Gl.glEnable(Gl.GL_COLOR_MATERIAL);

			Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, new float[4] { 0.3f, 0.3f, 0.3f, 1.0f });

			//  A bit of extra initialisation here, we have to enable textures.
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);			// Really Nice Perspective Calculations

			Gl.glClearColor(((float)SystemColors.ControlDark.R) / 255, ((float)SystemColors.ControlDark.G) / 255, ((float)SystemColors.ControlDark.B) / 255, 0.0f);
		}

		public void Init3dView(ViewportControl control)
		{
			control.InitializeContexts();

			InitCommon();

			Cam = new NewCamera();
			Cam.setBehavior(NewCamera.CameraBehavior.CAMERA_BEHAVIOR_ORBIT);

			Reshape3dView(control);

			simpleOpenGlControl1_MouseHook = new CControlMouseMoveHook(
			simpleOpenGlControl1,
			simpleOpenGlControl1_MouseDown,
			simpleOpenGlControl1_MouseUp,
			simpleOpenGlControl1_MouseMove,
			simpleOpenGlControl1_MouseWheel);

			control.ControlData = new COpenGlControlData();
			control.ControlData.Is3D = true;
			control.ControlData.Flags |= EControlDataFlags.ShowAxis | EControlDataFlags.ShowGrid | EControlDataFlags.Textured | EControlDataFlags.Gourad | EControlDataFlags.Lighting;
			SetShading(control.ControlData);
		}

		public void Init2DView(ViewportControl control)
		{
			control.InitializeContexts();

			InitCommon();
			Reshape2dView(control);

			control.ControlData = new COpenGlControlData();
			control.ControlData.Is3D = false;
			control.ControlData.Flags |= EControlDataFlags.ShowVerticeTicks | EControlDataFlags.ShowAxis | EControlDataFlags.ShowGrid | EControlDataFlags.Textured | EControlDataFlags.Gourad | EControlDataFlags.Lighting | EControlDataFlags.ShowBackfaces;
			SetShading(control.ControlData);
		}

		public ModelEditor()
		{
			InitializeComponent();
			Program.InitForms();

			InitGrid(CMDLGlobals.g_GridSize, CMDLGlobals.g_GridSlices);
			InitJoint(16);

			Init3dView(simpleOpenGlControl1);
			Init2DView(simpleOpenGlControl2);

			simpleOpenGlControl2_MouseHook = new CControlMouseMoveHook(
			simpleOpenGlControl2,
			simpleOpenGlControl2_MouseDown,
			simpleOpenGlControl2_MouseUp,
			simpleOpenGlControl2_MouseMove,
			simpleOpenGlControl2_MouseWheel);

			Init2DView(simpleOpenGlControl3);

			simpleOpenGlControl3_MouseHook = new CControlMouseMoveHook(
			simpleOpenGlControl3,
			simpleOpenGlControl3_MouseDown,
			simpleOpenGlControl3_MouseUp,
			simpleOpenGlControl3_MouseMove,
			simpleOpenGlControl3_MouseWheel);

			Init2DView(simpleOpenGlControl4);

			simpleOpenGlControl4_MouseHook = new CControlMouseMoveHook(
			simpleOpenGlControl4,
			simpleOpenGlControl4_MouseDown,
			simpleOpenGlControl4_MouseUp,
			simpleOpenGlControl4_MouseMove,
			simpleOpenGlControl4_MouseWheel);

			simpleOpenGlControl1.Viewport = EViewport.XYZViewport;
			simpleOpenGlControl2.Viewport = EViewport.XYViewport;
			simpleOpenGlControl3.Viewport = EViewport.ZYViewport;
			simpleOpenGlControl4.Viewport = EViewport.XZViewport;

			createVertexPos.x = simpleOpenGlControl1.Width / 2;
			createVertexPos.y = simpleOpenGlControl1.Width / 2;
			createVertexPos.z = simpleOpenGlControl1.Width / 2;
		}

		/*
		====================
		CalcFovY

		Calculates aspect based on fovX and the screen dimensions
		====================
		*/
		public static double CalcFovY(double fovX, double width, double height)
		{
			return (Math.Atan(height / (width / Math.Tan(fovX / 360.0 * Math.PI)))) * ((180.0 / Math.PI) * 2);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)WM.CLOSE)
				Program.ClosingFinal = true;

			base.WndProc(ref m);
		}

		public void SetModifyTabData()
		{
			((TableLayoutPanel)theModelTab.Controls[0]).Controls.Remove(theModelVertexTab);
			((TableLayoutPanel)theModelTab.Controls[0]).Controls.Remove(theModelFaceTab);

			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				((TableLayoutPanel)theModelTab.Controls[0]).Controls.Add(theModelVertexTab);
				((TableLayoutPanel)theModelTab.Controls[0]).SetRow(theModelVertexTab, 1);
			}
			else
			{
				((TableLayoutPanel)theModelTab.Controls[0]).Controls.Add(theModelFaceTab);
				((TableLayoutPanel)theModelTab.Controls[0]).SetRow(theModelFaceTab, 1);
			}
		}

		// Call to re-add the common toolbox to the current tab page
		public void TabPageChanged()
		{
			if (tabControl1.SelectedTab.Controls.Count == 0)
				return;

			if (tabControl1.SelectedTab != tabPage3 && tabControl1.SelectedTab != tabPage5)
			{
				((TableLayoutPanel)tabControl1.SelectedTab.Controls[0].Controls[0]).Controls.Add(Program.Ctrl_CommonToolbox);
				((TableLayoutPanel)tabControl1.SelectedTab.Controls[0].Controls[0]).SetRow(Program.Ctrl_CommonToolbox, 0);

				if (tabControl1.SelectedTab == tabPage4)
					SetModifyTabData();
			}
		}

		public static ModelVertexTab theModelVertexTab;
		public static ModelFaceTab theModelFaceTab;
		public static ModelTab theModelTab;
		public static ViewTab theViewTab;
		public static MeshesTab theMeshesTab;
		public static CreateTab theCreateTab;
		public static BoneTab theBoneTab;

		public delegate void DoForEachRenderingContextDelegate(object[] args);

		static public void DoForEachRenderingContext(DoForEachRenderingContextDelegate deleg, object[] args)
		{
			Program.Form_ModelEditor.simpleOpenGlControl1.MakeCurrent();
			deleg(args);
			Program.Form_ModelEditor.simpleOpenGlControl2.MakeCurrent();
			deleg(args);
			Program.Form_ModelEditor.simpleOpenGlControl3.MakeCurrent();
			deleg(args);
			Program.Form_ModelEditor.simpleOpenGlControl4.MakeCurrent();
			deleg(args);

			Program.Form_ModelEditor.simpleOpenGlControl1.MakeCurrent();
		}

		void ResetCamera()
		{
			Cam.lookAt(new Vector3(25, 25, -25), Vector3.Empty, new Vector3(0, 1, 0));
			Cam.zoom(-Cam.getOrbitMaxZoom(), Cam.getOrbitMinZoom(), Cam.getOrbitMaxZoom());
			Cam.zoom(NewCamera.DEFAULT_ORBIT_OFFSET_DISTANCE, Cam.getOrbitMinZoom(), Cam.getOrbitMaxZoom());

			CMDLGlobals.g_PanPosition = Vector3.Empty;
			CMDLGlobals.g_OldPanPosition = Vector3.Empty;
			CMDLGlobals.g_Zoom2DFactor = 2;
			RedrawAllViews();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			CMDLGlobals.g_Palette.Load("QUAKE2.PAL");
			CMDLGlobals.g_ModelSelectType = ESelectType.Vertex;
			CMDLGlobals.g_MainActionMode = EActionType.Select;

			tabControl1.SelectedTab = tabPage4;

			tabPage5.Controls.Add(theMeshesTab = new MeshesTab());
			tabPage4.Controls.Add(theModelTab = new ModelTab());
			tabPage2.Controls.Add(theViewTab = new ViewTab());
			tabPage3.Controls.Add(theCreateTab = new CreateTab());
			//tabPage1.Controls.Add(theBoneTab = new BoneTab());
			theModelVertexTab = new ModelVertexTab();
			theModelFaceTab = new ModelFaceTab();

			TabPageChanged();

			CModules.FindPlugins();

			ResetCamera();

			UpdateModelSelectionType();
			CommonToolbox.UpdateBoxActionType();

			KeyPreview = true;
			InitControls();

			InitMouseStatus();

			CMDLGlobals.g_CurMdl.ClearModel();
			ModelUpdated();
		}

		public void DisableLighting(COpenGlControlData cd)
		{
			Gl.glDisable(Gl.GL_LIGHTING);
		}

		public void EnableLighting(COpenGlControlData cd)
		{
			if ((cd.Flags & EControlDataFlags.Lighting) != 0)
				Gl.glEnable(Gl.GL_LIGHTING);
		}

		private void skinsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.Form_SkinEditor.Visible)
			{
				Program.Form_SkinEditor.BringToFront();
				return;
			}

			Program.Form_SkinEditor.Show();
			Program.Form_SkinEditor.ImageUpdated();
		}

		public void SetFrame(int Frame, bool Redraw = true)
		{
			if (Frame < 0 || Frame >= CMDLGlobals.g_CurMdl.Frames.Count)
				return;

			CMDLGlobals.g_CurFrame = Frame;
			label1.Text = Frame.ToString();
			label2.Text = CMDLGlobals.g_CurMdl.Frames[Frame].FrameName;

			hScrollBar1.Value = Frame;

			CMDLGlobals.g_CurMdl.CalcNormals(TCompleteModel.CALCNORMS_ALL, Frame);

			if (Redraw)
				Program.Form_ModelEditor.RedrawAllViews();
		}

		System.Timers.Timer InterpolateTimer;
		public bool Animating = false;
		public int AnimateStart = 0, AnimateEnd = 0;
		float InterpFPS = 0;
		public float InterpolatePercent = 0;

		public double CurrenTime = 0, LastTime = 0;
		public void InterpolateTick(object sender, EventArgs e)
		{
			LastTime = CurrenTime;
			CurrenTime = DateTime.Now.TimeOfDay.TotalSeconds;

			InterpolatePercent += (float)(InterpFPS * (CurrenTime - LastTime));
			if (InterpolatePercent >= 1.0f)
			{
				if (CMDLGlobals.g_CurFrame == AnimateEnd)
					SetFrame(AnimateStart, false);
				else
					SetFrame(CMDLGlobals.g_CurFrame + 1, false);

				InterpolatePercent = 0;
			}

			RedrawModelView();
		}

		public void AnimateClicked(int Start, int End, int FPS)
		{
			CurrenTime = DateTime.Now.TimeOfDay.TotalSeconds;
			if (Animating)
			{
				InterpolateTimer.Stop();
				Animating = false;
				return;
			}

			AnimateStart = Start;
			AnimateEnd = End;
			Animating = true;

			SetFrame(Start);

			InterpolateTimer = new System.Timers.Timer(10);
			InterpolateTimer.SynchronizingObject = this;
			InterpolateTimer.Elapsed += InterpolateTick;
			InterpolateTimer.Start();

			InterpFPS = FPS;
		}

		public class CAnimationDropDownItem
		{
			public string AnimationName;
			public int StartFrame, EndFrame;

			public CAnimationDropDownItem(string _AnimationName, int _StartFrame, int _EndFrame)
			{
				AnimationName = _AnimationName;
				StartFrame = _StartFrame;
				EndFrame = _EndFrame;
			}

			public override string ToString()
			{
				return AnimationName;
			}
		}

		public void FramesChanged()
		{
			theViewTab.ClearAnimations();
			int CurFrame = 0;

			while (true)
			{
				string NewName = "";
				int NewStart = 0, NewEnd = 0;

				// Find the last non-numbered character
				for (int z = CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName.Length - 1; z >= 0; --z)
				{
					if (!Char.IsDigit(CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName[z]))
					{
						NewName = CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName.Substring(0, z + 1);
						break;
					}
				}

				NewStart = CurFrame;

				for (; CurFrame < CMDLGlobals.g_CurMdl.Frames.Count; CurFrame++)
				{
					string ThisNewName = "";
					// Find the last non-numbered character
					for (int z = CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName.Length - 1; z >= 0; --z)
					{
						if (!Char.IsDigit(CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName[z]))
						{
							ThisNewName = CMDLGlobals.g_CurMdl.Frames[CurFrame].FrameName.Substring(0, z + 1);
							break;
						}
					}

					if (NewName != ThisNewName)
					{
						NewEnd = CurFrame - 1;
						break;
					}
				}

				if (CurFrame == CMDLGlobals.g_CurMdl.Frames.Count)
					NewEnd = CMDLGlobals.g_CurMdl.Frames.Count - 1;

				theViewTab.AddAnimation(new CAnimationDropDownItem(NewName, NewStart, NewEnd));

				if (CurFrame == CMDLGlobals.g_CurMdl.Frames.Count)
					break;
			}
		}

		public void ModelLoaded()
		{
			CMDLGlobals.g_CurMdl.CalcAllNormals();
			hScrollBar1.LargeChange = 5;
			SetFrame(0);
			ModelUpdated();
			FramesChanged();

			ResetCamera();

			CMDLGlobals.g_CurMdl.Fit(true);
			Program.Form_SkinEditor.ModelLoaded();
		}

		string _loadedFile = null;

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog dlg = new OpenFileDialog())
			{
				dlg.Filter = "VCMDL.NET Model Format (*.qmf)|*.qmf|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return;

				try
				{
					CMDLGlobals.g_CurMdl.LoadFromQMF(dlg.FileName, false);
					ModelLoaded();
					_loadedFile = dlg.FileName;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error occured while loading: " + ex.ToString(), "Error");
				}
			}
		}

		void SaveFile()
		{
			try
			{
				CMDLGlobals.g_CurMdl.SaveToQMF(_loadedFile);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error occured while loading: " + ex.ToString(), "Error");
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_loadedFile == null)
				saveAsToolStripMenuItem_Click(sender, e);
			else
				SaveFile();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog dlg = new SaveFileDialog())
			{
				dlg.Filter = "VCMDL.NET Model Format (*.qmf)|*.qmf|All Files (*)|*";
				dlg.RestoreDirectory = true;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return;

				_loadedFile = dlg.FileName;
				SaveFile();
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			SetFrame(CMDLGlobals.g_CurFrame + 1);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			SetFrame(CMDLGlobals.g_CurFrame - 1);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPageChanged();
		}

		private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
		{
			SetFrame(e.NewValue);
		}

		public static void DrawOriginAxis(float Size)
		{
			Gl.glLineWidth(2);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
			Gl.glBegin(Gl.GL_LINES);
			PGl.glColor(Color.Red);
			Gl.glVertex3i(0, 0, 0);
			Gl.glVertex3f(0, Size, 0);

			PGl.glColor(Color.LimeGreen);
			Gl.glVertex3i(0, 0, 0);
			Gl.glVertex3f(0, 0, Size);

			PGl.glColor(Color.Blue);
			Gl.glVertex3i(0, 0, 0);
			Gl.glVertex3f(Size, 0, 0);
			Gl.glEnd();
			Gl.glLineWidth(1);
		}

		public void DrawAxis(ViewportControl vp)
		{
			if ((vp.ControlData.Flags & EControlDataFlags.ShowAxis) != 0)
				DrawOriginAxis(25);
		}

		static float[] JointVertices;
		public static void InitJoint(int Segments)
		{
			JointVertices = new float[((Segments * 3) * 3) * 2];

			int CurVal = 0;
			PGl.glCircle(1, Segments, delegate(double x, double y) { JointVertices[CurVal++] = (float)x; JointVertices[CurVal++] = (float)y; JointVertices[CurVal++] = 0; });
			PGl.glCircle(1, Segments, delegate(double x, double y) { JointVertices[CurVal++] = (float)x; JointVertices[CurVal++] = 0; JointVertices[CurVal++] = (float)y; });
			PGl.glCircle(1, Segments, delegate(double x, double y) { JointVertices[CurVal++] = 0; JointVertices[CurVal++] = (float)x; JointVertices[CurVal++] = (float)y; });
		}

		static float[] GridVertices;
		public static void InitGrid(int Size, int Slices)
		{
			int gridSize = Size;
			int numSlices = Slices;
			int totalLength = (gridSize * numSlices);
			int startX = -(totalLength / 2);
			int endX = totalLength / 2;
			int numVerts = (numSlices + 1) * 8;

			int vertIndex = 0;
			GridVertices = new float[numVerts];

			for (int x = startX, i = 0; i <= numSlices; x += gridSize, ++i)
			{
				GridVertices[vertIndex++] = x;
				GridVertices[vertIndex++] = startX;

				GridVertices[vertIndex++] = x;
				GridVertices[vertIndex++] = endX;

				GridVertices[vertIndex++] = startX;
				GridVertices[vertIndex++] = x;

				GridVertices[vertIndex++] = endX;
				GridVertices[vertIndex++] = x;
			}
		}

		Point startMouse, endMouse;
		bool mouseSelecting = false;

		Vector3 createVertexPos = Vector3.Empty;

		public static void DrawJoint(Vector3 pos, Vector3 rotation, ref EBoneFlags Selected, float scale = 1)
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
			Gl.glPushMatrix();
			Gl.glTranslatef(pos.x, pos.y, pos.z);
			Gl.glRotatef(rotation.x, 1, 0, 0);
			Gl.glRotatef(rotation.y, 0, 1, 0);
			Gl.glRotatef(rotation.z, 0, 0, 1);

			if (scale == 1)
				DrawOriginAxis(4);

			Gl.glScalef(scale, scale, scale);

			if ((Selected & EBoneFlags.TempSelected) != 0)
			{
				PGl.glColor(Color.Purple);
				Selected &= ~EBoneFlags.TempSelected;
			}
			else if ((Selected & EBoneFlags.Selected) != 0)
				PGl.glColor(Color.LimeGreen);
			else
				PGl.glColor(Color.Blue);

			Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
			Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, JointVertices);
			Gl.glDrawArrays(Gl.GL_LINES, 0, JointVertices.Length / 3);
			Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
			Gl.glPopMatrix();
		}

		public static void DrawJointConnection(Vector3 P1, Vector3 N1, Vector3 P2)
		{
			Vector3 f, r, u;
			N1.ToVectors(out f, out r, out u);

			Vector3 topRight, topLeft, bottomLeft, bottomRight;

			topRight = P1.MultiplyAngles(8, f);
			topLeft = topRight;
			bottomLeft = topRight;
			bottomRight = topRight;

			topRight = topRight.MultiplyAngles(4, u).MultiplyAngles(4, r);
			topLeft = topLeft.MultiplyAngles(4, u).MultiplyAngles(-4, r);
			bottomLeft = bottomLeft.MultiplyAngles(-4, u).MultiplyAngles(-4, r);
			bottomRight = bottomRight.MultiplyAngles(-4, u).MultiplyAngles(4, r);

			Vector3 endPt = P2.MultiplyAngles(-8, N1);

			Gl.glBegin(Gl.GL_LINES);
			PGl.glVertex(topRight);
			PGl.glVertex(endPt);
			PGl.glVertex(topLeft);
			PGl.glVertex(endPt);
			PGl.glVertex(bottomLeft);
			PGl.glVertex(endPt);
			PGl.glVertex(bottomRight);
			PGl.glVertex(endPt);

			PGl.glVertex(topRight);
			PGl.glVertex(topLeft);
			PGl.glVertex(bottomLeft);
			PGl.glVertex(bottomRight);

			PGl.glVertex(topLeft);
			PGl.glVertex(bottomLeft);
			PGl.glVertex(topRight);
			PGl.glVertex(bottomRight);
			Gl.glEnd();
		}

		public static void DrawGrid(ViewportControl vp)
		{
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);

			if ((vp.ControlData.Flags & EControlDataFlags.ShowGrid) != 0)
			{
				Gl.glColor3ub(235, 211, 199);

				// activate and specify pointer to vertex array
				Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
				Gl.glVertexPointer(2, Gl.GL_FLOAT, 0, GridVertices);

				Gl.glDrawArrays(Gl.GL_LINES, 0, GridVertices.Length / 2);

				// deactivate vertex arrays after drawing
				Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
			}
		}

		void DrawCreateVertexPos(ViewportControl ctrl)
		{
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glViewport(0, 0, ctrl.Width, ctrl.Height);
			Gl.glOrtho(0, ctrl.Width, 0, ctrl.Height, 0, 99999);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Gl.glPushMatrix();
			DisableLighting(ctrl.ControlData);

			Gl.glBegin(Gl.GL_LINES);
			PGl.glColor(Color.BlueViolet, 127);
			switch (ctrl.Viewport)
			{
				case EViewport.XYViewport:
					Gl.glVertex2f(createVertexPos.x, 0);
					Gl.glVertex2f(createVertexPos.x, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.y);
					Gl.glVertex2f(ctrl.Width, createVertexPos.y);
					break;
				case EViewport.XZViewport:
					Gl.glVertex2f(createVertexPos.y, 0);
					Gl.glVertex2f(createVertexPos.y, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.z);
					Gl.glVertex2f(ctrl.Width, createVertexPos.z);
					break;
				case EViewport.ZYViewport:
					Gl.glVertex2f(createVertexPos.x, 0);
					Gl.glVertex2f(createVertexPos.x, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.z);
					Gl.glVertex2f(ctrl.Width, createVertexPos.z);
					break;
			}
			Gl.glEnd();

			PGl.glColor(Color.Yellow);
			Gl.glBegin(Gl.GL_POINTS);
			switch (ctrl.Viewport)
			{
				case EViewport.XYViewport:
					Gl.glVertex2f(createVertexPos.x, createVertexPos.y);
					break;
				case EViewport.XZViewport:
					Gl.glVertex2f(createVertexPos.y, createVertexPos.z);
					break;
				case EViewport.ZYViewport:
					Gl.glVertex2f(createVertexPos.x, createVertexPos.z);
					break;
			}
			Gl.glEnd();

			Gl.glPopMatrix();
		}

		void DrawCreateBonePos(ViewportControl ctrl)
		{
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glViewport(0, 0, ctrl.Width, ctrl.Height);
			Gl.glOrtho(0, ctrl.Width, 0, ctrl.Height, 0, 99999);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Gl.glPushMatrix();
			DisableLighting(ctrl.ControlData);

			Gl.glBegin(Gl.GL_LINES);
			PGl.glColor(Color.BlueViolet, 127);
			switch (ctrl.Viewport)
			{
				case EViewport.XYViewport:
					Gl.glVertex2f(createVertexPos.x, 0);
					Gl.glVertex2f(createVertexPos.x, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.y);
					Gl.glVertex2f(ctrl.Width, createVertexPos.y);
					break;
				case EViewport.XZViewport:
					Gl.glVertex2f(createVertexPos.y, 0);
					Gl.glVertex2f(createVertexPos.y, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.z);
					Gl.glVertex2f(ctrl.Width, createVertexPos.z);
					break;
				case EViewport.ZYViewport:
					Gl.glVertex2f(createVertexPos.x, 0);
					Gl.glVertex2f(createVertexPos.x, ctrl.Height);

					Gl.glVertex2f(0, createVertexPos.z);
					Gl.glVertex2f(ctrl.Width, createVertexPos.z);
					break;
			}
			Gl.glEnd();

			EBoneFlags dummy = 0;

			switch (ctrl.Viewport)
			{
				case EViewport.XYViewport:
					DrawJoint(new Vector3(createVertexPos.x, createVertexPos.y, 0), Vector3.Empty, ref dummy, 1 * CMDLGlobals.g_Zoom2DFactor);
					break;
				case EViewport.XZViewport:
					DrawJoint(new Vector3(createVertexPos.y, createVertexPos.z, 0), Vector3.Empty, ref dummy, 1 * CMDLGlobals.g_Zoom2DFactor);
					break;
				case EViewport.ZYViewport:
					DrawJoint(new Vector3(createVertexPos.x, createVertexPos.z, 0), Vector3.Empty, ref dummy, 1 * CMDLGlobals.g_Zoom2DFactor);
					break;
			}

			Gl.glPopMatrix();
		}

		void DrawMouseRectangle(ViewportControl ctrl, bool Is2D = false)
		{
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glViewport(0, 0, ctrl.Width, ctrl.Height);
			Gl.glOrtho(0, ctrl.Width, 0, ctrl.Height, 0, 99999);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Gl.glPushMatrix();
			DisableLighting(ctrl.ControlData);

			if (!Is2D || (Is2D && (CMDLGlobals.g_MainActionMode & EActionType.Selecting) != 0))
			{
				Rectangle r = RectangleFromPoints(startMouse, endMouse);
				Gl.glBegin(Gl.GL_LINES);
				PGl.glColor(Color.BlueViolet);
				Gl.glVertex2f(r.X, r.Y);
				Gl.glVertex2f(r.X + r.Width, r.Y);

				Gl.glVertex2f(r.X + r.Width, r.Y);
				Gl.glVertex2f(r.X + r.Width, r.Y + r.Height);

				Gl.glVertex2f(r.X + r.Width + 1, r.Y + r.Height);
				Gl.glVertex2f(r.X, r.Y + r.Height);

				Gl.glVertex2f(r.X, r.Y + r.Height);
				Gl.glVertex2f(r.X, r.Y);
				Gl.glEnd();
			}

			Gl.glPopMatrix();
		}

		public void DrawScene(bool is3D, ViewportControl vp)
		{
			DisableLighting(vp.ControlData);

			Gl.glPushMatrix();
			Gl.glTranslatef(CMDLGlobals.g_PlanePosition.y, CMDLGlobals.g_PlanePosition.x, CMDLGlobals.g_PlanePosition.z);
			DrawGrid(vp);
			Gl.glPopMatrix();

			Gl.glPushMatrix();
			DrawAxis(vp);
			Gl.glPopMatrix();

			Gl.glPushMatrix();
			if (is3D == false)
			{
				PGl.glColor(Color.White);
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, 0);
				if (vp.ControlData.BackImage.Skin != null)
					DrawBackImage(vp);
			}
			Gl.glPopMatrix();

			Gl.glPushMatrix();
			EnableLighting(vp.ControlData);
			CMDLGlobals.g_CurMdl.Draw(is3D, vp.ControlData);
			Gl.glPopMatrix();
		}

		private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear The Screen And The Depth Buffer
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Gl.glMultMatrixf(Cam.getProjectionMatrix().toFloatArray());

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();
			Gl.glMultMatrixf(Cam.getViewMatrix().toFloatArray());
			Gl.glRotatef(-90, 1, 0, 0);

			DrawScene(true, simpleOpenGlControl1);

			if (CMDLGlobals.g_SelectedViewport == EViewport.XYZViewport)
			{
				if (CMDLGlobals.g_MainActionMode == EActionType.BuildingFace1 || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace2)
				{
					CVerticeFrameData fd = CMDLGlobals.g_CurMdl.Meshes[BuildingFaceMesh].Verts[BuildingFaceVerts[0]].FrameData[CMDLGlobals.g_CurFrame];
					CVerticeFrameData fd2 = CMDLGlobals.g_CurMdl.Meshes[BuildingFaceMesh].Verts[BuildingFaceVerts[1]].FrameData[CMDLGlobals.g_CurFrame];

					Gl.glDisable(Gl.GL_DEPTH_TEST);
					Gl.glBegin(Gl.GL_LINES);
					PGl.glVertex(fd.Position);
					Gl.glVertex3f(RotatedVector(BuildingFaceMousePos).x, -RotatedVector(BuildingFaceMousePos).y, -RotatedVector(BuildingFaceMousePos).z);

					if (CMDLGlobals.g_MainActionMode == EActionType.BuildingFace2)
					{
						PGl.glVertex(fd2.Position);
						Gl.glVertex3f(RotatedVector(BuildingFaceMousePos).x, -RotatedVector(BuildingFaceMousePos).y, -RotatedVector(BuildingFaceMousePos).z);

						PGl.glVertex(fd.Position);
						PGl.glVertex(fd2.Position);
					}
					Gl.glEnd();
					Gl.glEnable(Gl.GL_DEPTH_TEST);
				}
				else if (mouseSelecting)
					DrawMouseRectangle(simpleOpenGlControl1);
			}
		}

		public static void FillSkins(CSkin Skin)
		{
			DoForEachRenderingContext((object[] o) => Skin.Fill(), null);
		}

		public static void FillSingleSkin(CSkin Skin)
		{
			Skin.Fill();
		}

		public static void Setup2DOrtho(Control ctl, float X, float Y)
		{
			Gl.glViewport(0, 0, ctl.Width, ctl.Height);
			Gl.glOrtho(0, ctl.Width / CMDLGlobals.g_Zoom2DFactor, 0, ctl.Height / CMDLGlobals.g_Zoom2DFactor, 0, 99999);
			Gl.glTranslatef((ctl.Width / CMDLGlobals.g_Zoom2DFactor) / 2 + X, (ctl.Height / CMDLGlobals.g_Zoom2DFactor) / 2 + Y, -(99999 / 2));
		}

		private void DrawBackImage(ViewportControl ctrl)
		{
			Gl.glPushMatrix();
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			DisableLighting(ctrl.ControlData);

			ctrl.ControlData.BackImage.Bind();

			float Left = -(ctrl.ControlData.BackImage.SkinSize.Width / 2) * ctrl.ControlData.Scale.X,
				  Right = (ctrl.ControlData.BackImage.SkinSize.Width / 2) * ctrl.ControlData.Scale.X,
				  Top = -(ctrl.ControlData.BackImage.SkinSize.Height / 2) * ctrl.ControlData.Scale.Y,
				  Bottom = (ctrl.ControlData.BackImage.SkinSize.Height / 2) * ctrl.ControlData.Scale.Y;

			Vector3[] verts = new Vector3[4];

			switch (ctrl.Viewport)
			{
				case EViewport.XYViewport:
					verts[0] = new Vector3(Top, Left, 0);
					verts[1] = new Vector3(Top, Right, 0);
					verts[2] = new Vector3(Bottom, Right, 0);
					verts[3] = new Vector3(Bottom, Left, 0);

					Gl.glTranslatef(-ctrl.ControlData.Offset.Y, -ctrl.ControlData.Offset.X, 0);
					break;
				case EViewport.XZViewport:
					verts[0] = new Vector3(Right, 0, Bottom);
					verts[1] = new Vector3(Left, 0, Bottom);
					verts[2] = new Vector3(Left, 0, Top);
					verts[3] = new Vector3(Right, 0, Top);

					Gl.glTranslatef(-ctrl.ControlData.Offset.X, 0, -ctrl.ControlData.Offset.Y);
					break;
				case EViewport.ZYViewport:
					verts[0] = new Vector3(0, Left, Bottom);
					verts[1] = new Vector3(0, Right, Bottom);
					verts[2] = new Vector3(0, Right, Top);
					verts[3] = new Vector3(0, Left, Top);

					Gl.glTranslatef(0, ctrl.ControlData.Offset.X, -ctrl.ControlData.Offset.Y);
					break;
			}

			Gl.glBegin(Gl.GL_QUADS);
			Gl.glTexCoord2f(0, 0);
			PGl.glVertex(verts[0]);
			Gl.glTexCoord2f(1, 0);
			PGl.glVertex(verts[1]);
			Gl.glTexCoord2f(1, 1);
			PGl.glVertex(verts[2]);
			Gl.glTexCoord2f(0, 1);
			PGl.glVertex(verts[3]);
			Gl.glEnd();

			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glPopMatrix();
		}

		private void Draw2DView(ViewportControl vp)
		{
			DrawScene(false, vp);

			if ((CMDLGlobals.g_MainActionMode & EActionType.Creating) != 0)
			{
				if ((CMDLGlobals.g_MainActionMode & EActionType.CreateVertex) != 0)
					DrawCreateVertexPos(vp);
				else if ((CMDLGlobals.g_MainActionMode & EActionType.CreateBone) != 0)
					DrawCreateBonePos(vp);
			}
			if (CMDLGlobals.g_SelectedViewport == vp.Viewport && mouseSelecting)
				DrawMouseRectangle(vp, true);
		}

		private void simpleOpenGlControl2_Paint(object sender, PaintEventArgs e)
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear The Screen And The Depth Buffer
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Setup2DOrtho(simpleOpenGlControl2, CMDLGlobals.g_PanPosition.y, CMDLGlobals.g_PanPosition.x);
			Gl.glRotatef(90, 1, 0, 0);
			Gl.glRotatef(-90, 0, 1, 0);
			Gl.glRotatef(-90, 1, 0, 0);

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Draw2DView(simpleOpenGlControl2);
		}

		private void simpleOpenGlControl3_Paint(object sender, PaintEventArgs e)
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear The Screen And The Depth Buffer
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Setup2DOrtho(simpleOpenGlControl3, CMDLGlobals.g_PanPosition.y, CMDLGlobals.g_PanPosition.z);
			Gl.glRotatef(-90, 0, 1, 0);
			Gl.glRotatef(-90, 1, 0, 0);

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Draw2DView(simpleOpenGlControl3);
		}

		private void simpleOpenGlControl4_Paint(object sender, PaintEventArgs e)
		{
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);	// Clear The Screen And The Depth Buffer
			Gl.glMatrixMode(Gl.GL_PROJECTION);
			Gl.glLoadIdentity();
			Setup2DOrtho(simpleOpenGlControl4, CMDLGlobals.g_PanPosition.x, CMDLGlobals.g_PanPosition.z);
			Gl.glRotatef(180, 0, 1, 0);
			Gl.glRotatef(-90, 1, 0, 0);

			Gl.glMatrixMode(Gl.GL_MODELVIEW);
			Gl.glLoadIdentity();

			Draw2DView(simpleOpenGlControl4);
		}

		public void RedrawModelView()
		{
			simpleOpenGlControl1.Draw();
		}

		public void RedrawAllViews()
		{
			RedrawModelView();
			Redraw2dViews();
		}

		public void Redraw2dViews()
		{
			simpleOpenGlControl2.Draw();
			simpleOpenGlControl3.Draw();
			simpleOpenGlControl4.Draw();
		}

		private void simpleOpenGlControl1_Load(object sender, EventArgs e)
		{
		}

		public Vector3[] StoredMouseClickPositions = new Vector3[4] { Vector3.Empty, Vector3.Empty, Vector3.Empty, Vector3.Empty };
		public Vector3[] StoredMouseClickPositionsEnd = new Vector3[4] { Vector3.Empty, Vector3.Empty, Vector3.Empty, Vector3.Empty };

		int BuildingFaceMesh;
		int[] BuildingFaceVerts = new int[3];
		Vector3 BuildingFaceMousePos = Vector3.Empty;
		bool FlipTriangleOrderStrip = false;

		public void simpleOpenGlControl1_MouseDown(object sender, HookMouseEventArgs e)
		{
			CMDLGlobals.g_SelectedViewport = EViewport.XYZViewport;

			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) != 0)
			{
				if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left)
					CMDLGlobals.g_Panning = true;
				else if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right)
					CMDLGlobals.g_Zooming = true;
			}
			else
			{
				if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left)
				{
					if (CMDLGlobals.g_MainActionMode == EActionType.BuildFace || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace1 || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace2)
					{
						PointF clPos = simpleOpenGlControl1.PointToClient(e.ClickPos);
						PointF endPos = simpleOpenGlControl1.PointToClient(e.ClickPos);

						clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
						endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

						Vector3[] PewRay = new Vector3[2];
						GetTwoRaysThing(clPos, ref PewRay[0], ref PewRay[1]);

						clPos.X -= SingleDotSize;
						clPos.Y -= SingleDotSize;
						endPos.X += SingleDotSize;
						endPos.Y += SingleDotSize;

						if (clPos.X > endPos.X && clPos.Y > endPos.Y)
						{
							float oldClX = clPos.X;
							clPos.X = endPos.X;
							endPos.X = oldClX;
						}
						else if (clPos.X < endPos.X && clPos.Y < endPos.Y)
						{
							float oldClY = clPos.Y;
							clPos.Y = endPos.Y;
							endPos.Y = oldClY;
						}
						else if (clPos.X > endPos.X && clPos.Y < endPos.Y)
						{
							PointF oldClPos = clPos;
							clPos = endPos;
							endPos = oldClPos;
						}

						List<Tuple<int, int>> SelectedVertices = FrustumSelectedVertices(ref clPos, ref endPos);

						if (SelectedVertices.Count == 1)
						{
							if (SelectedVertices[0].Item1 == BuildingFaceMesh)
							{
								BuildingFaceMousePos = PewRay[1];
								switch (CMDLGlobals.g_MainActionMode)
								{
									case EActionType.BuildFace:
										FlipTriangleOrderStrip = false;
										BuildingFaceVerts[0] = SelectedVertices[0].Item2;
										BuildingFaceMesh = SelectedVertices[0].Item1;
										CMDLGlobals.g_MainActionMode = EActionType.BuildingFace1;
										break;
									case EActionType.BuildingFace1:
										if (BuildingFaceVerts[0] != SelectedVertices[0].Item2)
										{
											BuildingFaceVerts[1] = SelectedVertices[0].Item2;
											CMDLGlobals.g_MainActionMode = EActionType.BuildingFace2;
										}
										break;
									case EActionType.BuildingFace2:
										if (BuildingFaceVerts[0] != SelectedVertices[0].Item2 &&
											BuildingFaceVerts[1] != SelectedVertices[0].Item2)
										{
											BuildingFaceVerts[2] = SelectedVertices[0].Item2;
											CMDLGlobals.g_CurMdl.CreateFace(BuildingFaceMesh, (FlipTriangleOrderStrip) ? new int[] { BuildingFaceVerts[2], BuildingFaceVerts[1], BuildingFaceVerts[0] } : BuildingFaceVerts);

											if ((ModifierKeys & Keys.Shift) != 0)
											{
												FlipTriangleOrderStrip = !FlipTriangleOrderStrip;
												BuildingFaceVerts[0] = BuildingFaceVerts[1];
												BuildingFaceVerts[1] = BuildingFaceVerts[2];
											}
											else if ((ModifierKeys & Keys.Control) != 0)
											{
												BuildingFaceVerts[1] = BuildingFaceVerts[2];
											}
											else
												CMDLGlobals.g_MainActionMode = EActionType.BuildFace;
										}
										break;
								}

								RedrawAllViews();
							}
						}
					}
					else
					{
						if (CMDLGlobals.g_MainActionMode == EActionType.Select || (ModifierKeys & Keys.Shift) != 0)
							CMDLGlobals.g_MainActionMode |= EActionType.Selecting;

						mouseSelecting = true;
					}
				}
				else if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right && (CMDLGlobals.g_MainActionMode == EActionType.BuildingFace1 || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace2))
					CMDLGlobals.g_MainActionMode = EActionType.BuildFace;
			}
		}

		public List<Tuple<int, int>> FrustumSelectedVertices(ref PointF clPos, ref PointF endPos)
		{
			List<Tuple<int, int>> SelectedVertices = new List<Tuple<int, int>>();
			Vector3[] P = new Vector3[8];
			Vector3[] Frustum = new Vector3[4];
			GetTwoRaysThing(clPos, ref P[0], ref P[1]);
			GetTwoRaysThing(new PointF(clPos.X, endPos.Y), ref P[2], ref P[3]);
			GetTwoRaysThing(endPos, ref P[4], ref P[5]);
			GetTwoRaysThing(new PointF(endPos.X, clPos.Y), ref P[6], ref P[7]);

			Frustum[0] = (P[0] - P[1]).CrossProduct(P[2] - P[3]);
			Frustum[1] = (P[2] - P[3]).CrossProduct(P[4] - P[5]);
			Frustum[2] = (P[4] - P[5]).CrossProduct(P[6] - P[7]);
			Frustum[3] = (P[6] - P[7]).CrossProduct(P[0] - P[1]);

			// Check which points are within the Frustum formed by the selected square.
			if (CMDLGlobals.g_CurMdl.Frames.Count != 0 &&
				CMDLGlobals.g_CurMdl.Meshes.Count != 0)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0, x = 0; i < mesh.Verts.Count; i++)
					{
						for (x = 0; x < 4; x++)
						{
							if (((RotatedVector(mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame].Position) - P[x * 2]).DotProduct(Frustum[x])) > 0)
								break;
						}

						if (x == 4)
							SelectedVertices.Add(Tuple.Create(m, i));
					}
				}
			}

			return SelectedVertices;
		}

		public List<int> FrustumSelectedBones(ref PointF clPos, ref PointF endPos)
		{
			List<int> SelectedBones = new List<int>();
			Vector3[] P = new Vector3[8];
			Vector3[] Frustum = new Vector3[4];
			GetTwoRaysThing(clPos, ref P[0], ref P[1]);
			GetTwoRaysThing(new PointF(clPos.X, endPos.Y), ref P[2], ref P[3]);
			GetTwoRaysThing(endPos, ref P[4], ref P[5]);
			GetTwoRaysThing(new PointF(endPos.X, clPos.Y), ref P[6], ref P[7]);

			Frustum[0] = (P[0] - P[1]).CrossProduct(P[2] - P[3]);
			Frustum[1] = (P[2] - P[3]).CrossProduct(P[4] - P[5]);
			Frustum[2] = (P[4] - P[5]).CrossProduct(P[6] - P[7]);
			Frustum[3] = (P[6] - P[7]).CrossProduct(P[0] - P[1]);

			// Check which points are within the Frustum formed by the selected square.
			if (CMDLGlobals.g_CurMdl.Frames.Count != 0 &&
				CMDLGlobals.g_CurMdl.Bones.Count != 0)
			{
				for (int i = 0, x = 0; i < CMDLGlobals.g_CurMdl.Bones.Count; i++)
				{
					for (x = 0; x < 4; x++)
					{
						if (((RotatedVector(CMDLGlobals.g_CurMdl.Bones[i].Position) - P[x * 2]).DotProduct(Frustum[x])) > 0)
							break;
					}

					if (x == 4)
						SelectedBones.Add(i);
				}
			}

			return SelectedBones;
		}

		public bool CheckSameClockDir(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 norm)
		{
			// normal of trinagle
			float testi = (((pt2.y - pt1.y) * (pt3.z - pt1.z)) - ((pt3.y - pt1.y) * (pt2.z - pt1.z)));
			float testj = (((pt2.z - pt1.z) * (pt3.x - pt1.x)) - ((pt3.z - pt1.z) * (pt2.x - pt1.x)));
			float testk = (((pt2.x - pt1.x) * (pt3.y - pt1.y)) - ((pt3.x - pt1.x) * (pt2.y - pt1.y)));

			// Dot product with triangle normal
			float dotprod = testi * norm.x + testj * norm.y + testk * norm.z;

			//answer
			if (dotprod < 0)
				return false;
			else
				return true;
		}

		//
		// Check for an intersection (HitPos) between a line(LP1,LP2) and a triangle face (TP1, TP2, TP3)
		//
		public bool CheckLineTri(Vector3 pt1, Vector3 pt2, Vector3 pt3, Vector3 linept, Vector3 vect, ref Vector3 HitPos)
		{
			// vector form triangle pt1 to pt2
			Vector3 V1 = pt2 - pt1;
			// vector form triangle pt2 to pt3
			Vector3 V2 = pt3 - pt2;
			// vector normal of triangle
			Vector3 norm = V1.CrossProduct(V2);

			if (norm.DotProduct(vect) > 0)
			{
				//Find point of intersect to triangle plane.
				//find t to intersect point
				float t = -(norm.x * (linept.x - pt1.x) + norm.y * (linept.y - pt1.y) + norm.z * (linept.z - pt1.z)) /
						(norm.x * vect.x + norm.y * vect.y + norm.z * vect.z);

				// if ds is neg line started past triangle so can't hit triangle.
				if (t < 0)
					return false;

				HitPos = linept + vect * t;

				if (CheckSameClockDir(pt1, pt2, HitPos, norm) &&
					CheckSameClockDir(pt2, pt3, HitPos, norm) &&
					CheckSameClockDir(pt3, pt1, HitPos, norm))
					return true;
			}

			return false;
		}

		public static Vector3 RotatedVector(Vector3 v)
		{
			Vector3 nV = new Vector3(v);
			float oldY = nV.y;
			nV.y = nV.z;
			nV.z = -oldY;
			return nV;
		}

		struct STriRetvals
		{
			public int Mesh;
			public int Index;
			public float Dist;
		}

		public void DoTriangleSelectionThing(PointF clPos, ETriangleFlags Add)
		{
			List<STriRetvals> TriRetVals = new List<STriRetvals>();
			Vector3[] P = new Vector3[2];
			GetTwoRaysThing(clPos, ref P[0], ref P[1]);

			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					Vector3 pt0 = mesh.Verts[mesh.Tris[i].Vertices[0]].FrameData[CMDLGlobals.g_CurFrame].Position;
					Vector3 pt1 = mesh.Verts[mesh.Tris[i].Vertices[1]].FrameData[CMDLGlobals.g_CurFrame].Position;
					Vector3 pt2 = mesh.Verts[mesh.Tris[i].Vertices[2]].FrameData[CMDLGlobals.g_CurFrame].Position;

					Vector3 HitPos = Vector3.Empty;
					if (CheckLineTri(RotatedVector(pt0), RotatedVector(pt1), RotatedVector(pt2), P[0], P[1], ref HitPos))
					{
						STriRetvals s = new STriRetvals();
						s.Mesh = m;
						s.Index = i;
						s.Dist = (HitPos - Cam.getPosition()).Length();
						TriRetVals.Add(s);
					}
				}
			}

			if (TriRetVals.Count != 0)
			{
				int HighestDistIndex = -1;
				int HighestDistMesh = -1;
				float HighestDist = 999999;

				foreach (STriRetvals v in TriRetVals)
				{
					if (v.Dist < HighestDist)
					{
						HighestDist = v.Dist;
						HighestDistIndex = v.Index;
						HighestDistMesh = v.Mesh;
					}
				}

				if (Add == ETriangleFlags.Selected && (ModifierKeys & Keys.Alt) != 0)
				{
					CMDLGlobals.g_CurMdl.Meshes[HighestDistMesh].Tris[HighestDistIndex].Flags &= ~Add;

					if (Add == ETriangleFlags.Selected)
						CMDLGlobals.g_CurMdl.Meshes[HighestDistMesh].Tris[HighestDistIndex].Flags &= ~ETriangleFlags.SkinSelected;
				}
				else
				{
					CMDLGlobals.g_CurMdl.Meshes[HighestDistMesh].Tris[HighestDistIndex].Flags |= Add;

					if (Add == ETriangleFlags.Selected)
						CMDLGlobals.g_CurMdl.Meshes[HighestDistMesh].Tris[HighestDistIndex].Flags |= ETriangleFlags.SkinSelected;
				}
			}

			RedrawModelView();
		}

		void SelectEachVertice(Tuple<int, int> meshVert)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				if ((ModifierKeys & Keys.Alt) != 0)
					CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Verts[meshVert.Item2].Flags &= ~EVerticeFlags.Selected;
				else
					CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Verts[meshVert.Item2].Flags |= EVerticeFlags.Selected;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				//if ((ModifierKeys & Keys.Alt) != 0)
				//	CMDLGlobals.g_CurMdl.Bones[Vert].Flags &= ~EBoneFlags.Selected;
				//else
				//	CMDLGlobals.g_CurMdl.Bones[Vert].Flags |= EBoneFlags.Selected;
			}
			else
			{
				for (int z = 0; z < CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris.Count; ++z)
				{
					for (int h = 0; h < 3; ++h)
					{
						if (CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Vertices[h] == meshVert.Item2)
						{
							if ((ModifierKeys & Keys.Alt) != 0)
							{
								if (CMDLGlobals.g_SyncSelections)
									CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags &= ~ETriangleFlags.SkinSelected;

								CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags &= ~ETriangleFlags.Selected;
							}
							else
							{
								if (CMDLGlobals.g_SyncSelections)
									CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags |= ETriangleFlags.SkinSelected;

								CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags |= ETriangleFlags.Selected;
							}
							break;
						}
					}
				}
			}
		}

		void TempSelectEachVertice(Tuple<int, int> meshVert)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				if ((ModifierKeys & Keys.Alt) != 0)
					CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Verts[meshVert.Item2].Flags &= ~EVerticeFlags.TempSelected;
				else
					CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Verts[meshVert.Item2].Flags |= EVerticeFlags.TempSelected;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				//if ((ModifierKeys & Keys.Alt) != 0)
				//	CMDLGlobals.g_CurMdl.Bones[Vert].Flags &= ~EBoneFlags.TempSelected;
				//else
				//	CMDLGlobals.g_CurMdl.Bones[Vert].Flags |= EBoneFlags.TempSelected;
			}
			else
			{
				for (int z = 0; z < CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris.Count; ++z)
				{
					for (int h = 0; h < 3; ++h)
					{
						if (CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Vertices[h] == meshVert.Item2)
						{
							if ((ModifierKeys & Keys.Alt) != 0)
								CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags &= ~ETriangleFlags.TempSelected;
							else
								CMDLGlobals.g_CurMdl.Meshes[meshVert.Item1].Tris[z].Flags |= ETriangleFlags.TempSelected;
							break;
						}
					}
				}
			}
		}

		public void simpleOpenGlControl1_MouseUp(object sender, HookMouseEventArgs e)
		{
			CMDLGlobals.g_Panning = CMDLGlobals.g_Zooming = false;

			if (simpleOpenGlControl1.PointToClient(e.ClickPos) == e.MouseEvent.Location && e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right)
			{
				contextMenuStrip1.Show(simpleOpenGlControl1, e.MouseEvent.Location);
				return;
			}

			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) == 0 && e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left && mouseSelecting)
			{
				mouseSelecting = false;
				CMDLGlobals.g_MainActionMode &= ~EActionType.Selecting;

				PointF clPos = simpleOpenGlControl1.PointToClient(e.ClickPos);
				PointF endPos = e.MouseEvent.Location;

				clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
				endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

				if (clPos == endPos)
				{
					if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
					{
						DoTriangleSelectionThing(clPos, ETriangleFlags.Selected);
						return;
					}

					clPos.X -= 1.8f;
					clPos.Y -= 1.8f;
					endPos.X += 1.8f;
					endPos.Y += 1.8f;
				}

				if (clPos.X > endPos.X && clPos.Y > endPos.Y)
				{
					float oldClX = clPos.X;
					clPos.X = endPos.X;
					endPos.X = oldClX;
				}
				else if (clPos.X < endPos.X && clPos.Y < endPos.Y)
				{
					float oldClY = clPos.Y;
					clPos.Y = endPos.Y;
					endPos.Y = oldClY;
				}
				else if (clPos.X > endPos.X && clPos.Y < endPos.Y)
				{
					PointF oldClPos = clPos;
					clPos = endPos;
					endPos = oldClPos;
				}

				if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex ||
					CMDLGlobals.g_ModelSelectType == ESelectType.Face)
					FrustumSelectedVertices(ref clPos, ref endPos).ForEach(SelectEachVertice);
				//else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
				//	FrustumSelectedBones(ref clPos, ref endPos).ForEach(SelectEachVertice);
			}

			RedrawAllViews();
		}

		public void GetTwoRaysThing(PointF p, ref Vector3 ClickRayP1, ref Vector3 ClickRayP2)
		{
			// This function will find 2 points in world space that are on the line into the screen defined by screen-space( ie. window-space ) point (x,y)
			double[] mvmatrix = new double[16];
			double[] projmatrix = new double[16];
			int[] viewport = new int[4];
			double dX, dY, dZ, dClickY; // glUnProject uses doubles, but I'm using floats for these 3D vectors

			Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);
			for (int i = 0; i < 16; ++i)
				mvmatrix[i] = (double)Cam.getViewMatrix().toFloatArray()[i];
			for (int i = 0; i < 16; ++i)
				projmatrix[i] = (double)Cam.getProjectionMatrix().toFloatArray()[i];
			dClickY = (double)(p.Y); // OpenGL renders with (0,0) on bottom, mouse reports with (0,0) on top

			Glu.gluUnProject((double)p.X, dClickY, 0.0, mvmatrix, projmatrix, viewport, out dX, out dY, out dZ);
			ClickRayP1 = new Vector3((float)dX, (float)dY, (float)dZ);
			Glu.gluUnProject((double)p.X, dClickY, 1.0, mvmatrix, projmatrix, viewport, out dX, out dY, out dZ);
			ClickRayP2 = new Vector3((float)dX, (float)dY, (float)dZ);
		}

		public void simpleOpenGlControl1_MouseWheel(object sender, HookMouseEventArgs e)
		{
			if (e.MouseEvent.Delta > 0)
				Cam.zoom(-25.0f, Cam.getOrbitMinZoom(), Cam.getOrbitMaxZoom());
			else
				Cam.zoom(25.0f, Cam.getOrbitMinZoom(), Cam.getOrbitMaxZoom());

			RedrawModelView();
		}

		public void simpleOpenGlControl1_MouseMove(object sender, HookMouseEventArgs e)
		{
			if (mouseSelecting)
			{
				startMouse = simpleOpenGlControl1.PointToClient(e.ClickPos);
				endMouse = e.MouseEvent.Location;

				startMouse.Y = simpleOpenGlControl1.Height - startMouse.Y;
				endMouse.Y = simpleOpenGlControl1.Height - endMouse.Y;

				if ((CMDLGlobals.g_MainActionMode & EActionType.Selecting) != 0)
				{
					PointF clPos = simpleOpenGlControl1.PointToClient(e.ClickPos);
					PointF endPos = e.MouseEvent.Location;

					clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
					endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

					if (clPos == endPos)
					{
						if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
						{
							DoTriangleSelectionThing(clPos, ETriangleFlags.Selected);
							return;
						}

						clPos.X -= 1.8f;
						clPos.Y -= 1.8f;
						endPos.X += 1.8f;
						endPos.Y += 1.8f;
					}

					if (clPos.X > endPos.X && clPos.Y > endPos.Y)
					{
						float oldClX = clPos.X;
						clPos.X = endPos.X;
						endPos.X = oldClX;
					}
					else if (clPos.X < endPos.X && clPos.Y < endPos.Y)
					{
						float oldClY = clPos.Y;
						clPos.Y = endPos.Y;
						endPos.Y = oldClY;
					}
					else if (clPos.X > endPos.X && clPos.Y < endPos.Y)
					{
						PointF oldClPos = clPos;
						clPos = endPos;
						endPos = oldClPos;
					}

					if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex ||
						CMDLGlobals.g_ModelSelectType == ESelectType.Face)
						FrustumSelectedVertices(ref clPos, ref endPos).ForEach(TempSelectEachVertice);
					//else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
					//	FrustumSelectedBones(ref clPos, ref endPos).ForEach(TempSelectEachVertice);
				}

				RedrawModelView();
				return;
			}
			else if (CMDLGlobals.g_MainActionMode == EActionType.BuildFace || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace1 || CMDLGlobals.g_MainActionMode == EActionType.BuildingFace2)
			{
				RedrawModelView();

				PointF clPos = e.MouseEvent.Location;

				clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);

				Vector3[] PewRay = new Vector3[2];
				GetTwoRaysThing(clPos, ref PewRay[0], ref PewRay[1]);

				BuildingFaceMousePos = PewRay[1];

				clPos = e.MouseEvent.Location;
				PointF endPos = e.MouseEvent.Location;

				clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
				endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

				clPos.X -= SingleDotSize;
				clPos.Y -= SingleDotSize;
				endPos.X += SingleDotSize;
				endPos.Y += SingleDotSize;

				if (clPos.X > endPos.X && clPos.Y > endPos.Y)
				{
					float oldClX = clPos.X;
					clPos.X = endPos.X;
					endPos.X = oldClX;
				}
				else if (clPos.X < endPos.X && clPos.Y < endPos.Y)
				{
					float oldClY = clPos.Y;
					clPos.Y = endPos.Y;
					endPos.Y = oldClY;
				}
				else if (clPos.X > endPos.X && clPos.Y < endPos.Y)
				{
					PointF oldClPos = clPos;
					clPos = endPos;
					endPos = oldClPos;
				}

				List<Tuple<int, int>> SelectedVertices = FrustumSelectedVertices(ref clPos, ref endPos);

				foreach (var v in SelectedVertices)
					CMDLGlobals.g_CurMdl.Meshes[v.Item1].Verts[v.Item2].Flags |= EVerticeFlags.TempSelected;
				return;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex && (CMDLGlobals.g_MainActionMode & EActionType.Pan) == 0)
			{
				PointF clPos = e.MouseEvent.Location;
				PointF endPos = e.MouseEvent.Location;

				clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
				endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

				clPos.X -= SingleDotSize;
				clPos.Y -= SingleDotSize;
				endPos.X += SingleDotSize;
				endPos.Y += SingleDotSize;

				if (clPos.X > endPos.X && clPos.Y > endPos.Y)
				{
					float oldClX = clPos.X;
					clPos.X = endPos.X;
					endPos.X = oldClX;
				}
				else if (clPos.X < endPos.X && clPos.Y < endPos.Y)
				{
					float oldClY = clPos.Y;
					clPos.Y = endPos.Y;
					endPos.Y = oldClY;
				}
				else if (clPos.X > endPos.X && clPos.Y < endPos.Y)
				{
					PointF oldClPos = clPos;
					clPos = endPos;
					endPos = oldClPos;
				}

				List<Tuple<int, int>> SelectedVertices = FrustumSelectedVertices(ref clPos, ref endPos);

				foreach (var v in SelectedVertices)
					CMDLGlobals.g_CurMdl.Meshes[v.Item1].Verts[v.Item2].Flags |= EVerticeFlags.TempSelected;

				RedrawModelView();
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Face && (CMDLGlobals.g_MainActionMode & EActionType.Pan) == 0)
			{
				PointF clPos = e.MouseEvent.Location;
				PointF endPos = e.MouseEvent.Location;

				clPos.Y = (simpleOpenGlControl1.Height - clPos.Y);
				endPos.Y = (simpleOpenGlControl1.Height - endPos.Y);

				if (clPos == endPos)
				{
					DoTriangleSelectionThing(clPos, ETriangleFlags.TempSelected);
					return;
				}
			}

			if (!CMDLGlobals.g_Panning && !CMDLGlobals.g_Zooming)
				return;

			if (CMDLGlobals.g_Panning)
				Cam.rotate(-e.MoveDelta.X, -e.MoveDelta.Y, 0);
			else if (CMDLGlobals.g_Zooming)
				Cam.zoom((float)((double)e.MoveDelta.Y), Cam.getOrbitMinZoom(), Cam.getOrbitMaxZoom());

			RedrawModelView();
		}

		public bool VectorInside(Vector3 pt, Vector3 MinBounds, Vector3 MaxBounds)
		{
			return (pt.x > MinBounds.x &&
				pt.y > MinBounds.y &&
				pt.z > MinBounds.z &&
				pt.x < MaxBounds.x &&
				pt.y < MaxBounds.y &&
				pt.z < MaxBounds.z);
		}

		public void CalculateCursorPosFromViewport(ViewportControl ctrl, EViewport viewport, PointF clPos, PointF endPos, ref PointF StartPos, ref PointF EndPos)
		{
			PointF tclPos = clPos;
			PointF tendPos = endPos;
			tclPos.X -= ((float)ctrl.Width) / 2;
			tclPos.Y -= ((float)ctrl.Height) / 2;

			tendPos.X -= ((float)ctrl.Width) / 2;
			tendPos.Y -= ((float)ctrl.Height) / 2;

			switch (viewport)
			{
				case EViewport.XYViewport:
					StartPos.X = (tclPos.X / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.y);
					StartPos.Y = (tclPos.Y / CMDLGlobals.g_Zoom2DFactor) + (CMDLGlobals.g_PanPosition.x);
					EndPos.X = (tendPos.X / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.y);
					EndPos.Y = (tendPos.Y / CMDLGlobals.g_Zoom2DFactor) + (CMDLGlobals.g_PanPosition.x);
					break;
				case EViewport.ZYViewport:
					StartPos.X = (tclPos.X / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.y);
					StartPos.Y = -(tclPos.Y / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.z);
					EndPos.X = (tendPos.X / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.y);
					EndPos.Y = -(tendPos.Y / CMDLGlobals.g_Zoom2DFactor) - (CMDLGlobals.g_PanPosition.z);
					break;
				case EViewport.XZViewport:
					StartPos.X = (-tclPos.X / CMDLGlobals.g_Zoom2DFactor) + (CMDLGlobals.g_PanPosition.x);
					StartPos.Y = (-(tclPos.Y / CMDLGlobals.g_Zoom2DFactor)) - (CMDLGlobals.g_PanPosition.z);
					EndPos.X = (-tendPos.X / CMDLGlobals.g_Zoom2DFactor) + (CMDLGlobals.g_PanPosition.x);
					EndPos.Y = (-(tendPos.Y / CMDLGlobals.g_Zoom2DFactor)) - (CMDLGlobals.g_PanPosition.z);
					break;
			}
		}

		const float SingleDotSize = 2.3f;

		public void Selection2D(ViewportControl ctrl, EViewport viewport, PointF clPos, PointF endPos)
		{
			mouseSelecting = false;

			PointF StartPos = new PointF();
			PointF EndPos = new PointF();

			CalculateCursorPosFromViewport(ctrl, viewport, clPos, endPos, ref StartPos, ref EndPos);

			Vector3 Start3D = Vector3.Empty, End3D = Vector3.Empty;
			float squareSize = SingleDotSize / CMDLGlobals.g_Zoom2DFactor;

			switch (viewport)
			{
				case EViewport.XYViewport:
					Start3D = new Vector3(StartPos.Y, StartPos.X, -999999);
					End3D = new Vector3(EndPos.Y, EndPos.X, 999999);

					if (StartPos == EndPos)
					{
						Start3D.x -= squareSize;
						Start3D.y -= squareSize;
						End3D.x += squareSize;
						End3D.y += squareSize;
					}
					break;
				case EViewport.ZYViewport:
					Start3D = new Vector3(-999999, StartPos.X, StartPos.Y);
					End3D = new Vector3(999999, EndPos.X, EndPos.Y);

					if (StartPos == EndPos)
					{
						Start3D.y -= squareSize;
						Start3D.z -= squareSize;
						End3D.y += squareSize;
						End3D.z += squareSize;
					}
					break;
				case EViewport.XZViewport:
					Start3D = new Vector3(StartPos.X, -999999, StartPos.Y);
					End3D = new Vector3(EndPos.X, 999999, EndPos.Y);

					if (StartPos == EndPos)
					{
						Start3D.x -= squareSize;
						Start3D.z -= squareSize;
						End3D.x += squareSize;
						End3D.z += squareSize;
					}
					break;
			}

			if (CMDLGlobals.g_CurMdl.Frames.Count != 0)
			{
				var SelectedVertices = new List<Tuple<int, int>>();

				if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex ||
					CMDLGlobals.g_ModelSelectType == ESelectType.Face)
				{
					for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
					{
						var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

						for (int i = 0; i < mesh.Verts.Count; ++i)
						{
							Vector3 v = mesh.Verts[i].FrameData[CMDLGlobals.g_CurFrame].Position;

							if (VectorInside(v, Start3D, End3D))
								SelectedVertices.Add(Tuple.Create(m, i));
						}
					}
				}
				else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
				{
					/*for (int i = 0; i < CMDLGlobals.g_CurMdl.Bones.Count; ++i)
					{
						Vector3 v = CMDLGlobals.g_CurMdl.Bones[i].Position;

						if (VectorInside(v, Start3D, End3D))
							SelectedVertices.Add(i);
					}*/
				}

				SelectedVertices.ForEach(SelectEachVertice);
			}

			RedrawAllViews();
		}

		public void MouseDown2D(HookMouseEventArgs e, ViewportControl ctrl, EViewport viewp)
		{
			CMDLGlobals.g_SelectedViewport = viewp;

			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) != 0)
			{
				if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left)
					CMDLGlobals.g_Panning = true;
				else if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right)
					CMDLGlobals.g_Zooming = true;
			}
			else
			{
				if (CMDLGlobals.g_MainActionMode == EActionType.Select || (ModifierKeys & Keys.Shift) != 0)
					CMDLGlobals.g_MainActionMode |= EActionType.Selecting;

				if (e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left)
					mouseSelecting = true;

				if (CMDLGlobals.g_MainActionMode == EActionType.CreateVertex ||
					CMDLGlobals.g_MainActionMode == EActionType.CreateBone)
				{
					switch (viewp)
					{
						case EViewport.XYViewport:
							createVertexPos = new Vector3(e.MouseEvent.Location.X, ctrl.Height - e.MouseEvent.Location.Y, createVertexPos.z);
							break;
						case EViewport.XZViewport:
							createVertexPos = new Vector3(createVertexPos.x, e.MouseEvent.Location.X, ctrl.Height - e.MouseEvent.Location.Y);
							break;
						case EViewport.ZYViewport:
							createVertexPos = new Vector3(e.MouseEvent.Location.X, createVertexPos.y, ctrl.Height - e.MouseEvent.Location.Y);
							break;
					}
					CMDLGlobals.g_MainActionMode |= EActionType.Creating;

					RedrawAllViews();
				}
				else if (((CMDLGlobals.g_MainActionMode & EActionType.CreateVertex) != 0 || (CMDLGlobals.g_MainActionMode & EActionType.CreateBone) != 0) &&
					(CMDLGlobals.g_MainActionMode & EActionType.Creating) != 0 &&
					e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right)
				{
					CMDLGlobals.g_MainActionMode &= ~EActionType.Creating;
					Redraw2dViews();
				}

				OldMousePos = e.MouseEvent.Location;
			}
		}

		public Rectangle RectangleFromPoints(Point pt1, Point pt2)
		{
			// find the top-leftest point
			int top = 999999, left = 999999;

			if (pt1.Y < top)
				top = pt1.Y;
			if (pt2.Y < top)
				top = pt2.Y;

			if (pt1.X < left)
				left = pt1.X;
			if (pt2.X < left)
				left = pt2.X;

			// bottom-rightest point
			int bottom = -999999, right = -999999;

			if (pt1.Y > bottom)
				bottom = pt1.Y;
			if (pt2.Y > bottom)
				bottom = pt2.Y;

			if (pt1.X > right)
				right = pt1.X;
			if (pt2.X > right)
				right = pt2.X;

			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		public void NormalizeRectangle(ref Point pt1, ref Point pt2)
		{
			Rectangle r = RectangleFromPoints(pt1, pt2);

			pt1 = new Point(r.X, r.Y);
			pt2 = new Point(r.X + r.Width, r.Y + r.Height);
		}

		public enum EPointPosition
		{
			TopLeft,
			BottomLeft,
			BottomRight,
			TopRight
		}

		public EPointPosition GetRectPosition(Point p1, Point p2)
		{
			if (p1.X < p2.X &&
				p1.Y < p2.Y)
				return EPointPosition.TopLeft;
			else if (p1.X > p2.X &&
				p1.Y < p2.Y)
				return EPointPosition.TopRight;
			else if (p1.X < p2.X &&
				p1.Y > p2.Y)
				return EPointPosition.BottomLeft;
			else
				return EPointPosition.BottomRight;
		}

		public void RotatePoints(ref Point p1, ref Point p2, bool CounterClockwise)
		{
			EPointPosition pos = GetRectPosition(p1, p2);
			EPointPosition newPos = pos + ((CounterClockwise) ? -1 : 1);

			if ((int)newPos == -1)
				newPos = EPointPosition.TopRight;
			if ((int)newPos == 4)
				newPos = EPointPosition.TopLeft;

			if (!CounterClockwise)
			{
				switch (newPos)
				{
					case EPointPosition.BottomLeft:
					case EPointPosition.TopRight:
						int oldY = p2.Y;
						p2.Y = p1.Y;
						p1.Y = oldY;
						break;
					case EPointPosition.BottomRight:
					case EPointPosition.TopLeft:
						int oldX = p2.X;
						p2.X = p1.X;
						p1.X = oldX;
						break;
				}
			}
			else
			{
				switch (newPos)
				{
					case EPointPosition.BottomRight:
					case EPointPosition.TopLeft:
						int oldY = p2.Y;
						p2.Y = p1.Y;
						p1.Y = oldY;
						break;
					case EPointPosition.BottomLeft:
					case EPointPosition.TopRight:
						int oldX = p2.X;
						p2.X = p1.X;
						p1.X = oldX;
						break;
				}
			}
		}

		Point OldMousePos = new Point();

		private void OpenGL2DMouseUp(HookMouseEventArgs e, ViewportControl ct, EViewport viewport, Point lcp1, Point lcp2)
		{
			CMDLGlobals.g_Panning = CMDLGlobals.g_Zooming = false;

			if (ct.PointToClient(e.ClickPos) == e.MouseEvent.Location && e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Right)
			{
				contextMenuStrip1.Show(ct, e.MouseEvent.Location);
				return;
			}
			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) == 0)
			{
				if ((CMDLGlobals.g_MainActionMode & EActionType.Selecting) != 0 &&
					e.MouseEvent.Button == System.Windows.Forms.MouseButtons.Left &&
					mouseSelecting)
				{
					Selection2D(ct, viewport, lcp1, lcp2);
					CMDLGlobals.g_MainActionMode &= ~EActionType.Selecting;
				}
				else if ((CMDLGlobals.g_MainActionMode & EActionType.Creating) != 0)
				{
					CMDLGlobals.g_MainActionMode &= ~EActionType.Creating;

					PointF StartPos = new PointF(), DummyPos = new PointF();
					CalculateCursorPosFromViewport(simpleOpenGlControl2, EViewport.XYViewport, new PointF(createVertexPos.x, createVertexPos.y), PointF.Empty, ref StartPos, ref DummyPos);

					PointF EndPos = new PointF();
					CalculateCursorPosFromViewport(simpleOpenGlControl4, EViewport.XZViewport, new PointF(createVertexPos.x, createVertexPos.z), PointF.Empty, ref EndPos, ref DummyPos);

					PointF YZ = new PointF();
					CalculateCursorPosFromViewport(simpleOpenGlControl4, EViewport.ZYViewport, new PointF(createVertexPos.y, createVertexPos.z), PointF.Empty, ref YZ, ref DummyPos);

					StartPos.Y += -(CMDLGlobals.g_PanPosition.x * 2);
					EndPos.Y += (CMDLGlobals.g_PanPosition.z * 2);

					if ((CMDLGlobals.g_MainActionMode & EActionType.CreateVertex) != 0)
						CMDLGlobals.g_CurMdl.CreateVertex(0, new Vector3(-StartPos.Y, StartPos.X, -EndPos.Y));
					//else if ((CMDLGlobals.g_MainActionMode & EActionType.CreateBone) != 0)
					//	CMDLGlobals.g_CurMdl.CreateBone(0, new Vector3(-StartPos.Y, StartPos.X, -EndPos.Y));

					RedrawAllViews();
				}
			}

			mouseSelecting = false;
		}

		ViewportControl ControlFromViewPort(EViewport vp)
		{
			switch (vp)
			{
				case EViewport.XYZViewport:
				default:
					return simpleOpenGlControl1;
				case EViewport.XYViewport:
					return simpleOpenGlControl2;
				case EViewport.ZYViewport:
					return simpleOpenGlControl3;
				case EViewport.XZViewport:
					return simpleOpenGlControl4;
			}
		}

		public void simpleOpenGlControl2_MouseUp(object sender, HookMouseEventArgs e)
		{
			Point lcp1 = simpleOpenGlControl2.PointToClient(e.ClickPos);
			Point lcp2 = e.MouseEvent.Location;
			NormalizeRectangle(ref lcp1, ref lcp2);
			OpenGL2DMouseUp(e, simpleOpenGlControl2, EViewport.XYViewport, lcp1, lcp2);
		}

		public void simpleOpenGlControl3_MouseUp(object sender, HookMouseEventArgs e)
		{
			Point lcp1 = simpleOpenGlControl3.PointToClient(e.ClickPos);
			Point lcp2 = e.MouseEvent.Location;
			NormalizeRectangle(ref lcp1, ref lcp2);
			RotatePoints(ref lcp1, ref lcp2, false);
			OpenGL2DMouseUp(e, simpleOpenGlControl3, EViewport.ZYViewport, lcp1, lcp2);
		}

		public void simpleOpenGlControl4_MouseUp(object sender, HookMouseEventArgs e)
		{
			Point lcp1 = simpleOpenGlControl4.PointToClient(e.ClickPos);
			Point lcp2 = e.MouseEvent.Location;
			NormalizeRectangle(ref lcp1, ref lcp2);
			Point bk = lcp1;
			lcp1 = lcp2;
			lcp2 = bk;
			OpenGL2DMouseUp(e, simpleOpenGlControl4, EViewport.XZViewport, lcp1, lcp2);
		}

		public void simpleOpenGlControl2_MouseDown(object sender, HookMouseEventArgs e)
		{
			MouseDown2D(e, simpleOpenGlControl2, EViewport.XYViewport);
		}

		public void simpleOpenGlControl3_MouseDown(object sender, HookMouseEventArgs e)
		{
			MouseDown2D(e, simpleOpenGlControl3, EViewport.ZYViewport);
		}

		public void simpleOpenGlControl4_MouseDown(object sender, HookMouseEventArgs e)
		{
			MouseDown2D(e, simpleOpenGlControl4, EViewport.XZViewport);
		}

		public void simpleOpenGlControl2_MouseMove(object sender, HookMouseEventArgs e)
		{
			MouseMove2D(e, EViewport.XYViewport,
				delegate(PointF StartPos)
				{
					label4.Text = StartPos.Y.ToString();
					label5.Text = StartPos.X.ToString();
				},
				simpleOpenGlControl2, delegate(CVerticeFrameData v, PointF Delta)
				{
					if ((CMDLGlobals.g_Axis & EAxis.Y) != 0)
						v.Position.x += Delta.Y;
					if ((CMDLGlobals.g_Axis & EAxis.X) != 0)
						v.Position.y += Delta.X;
				},
				new Vector3((float)CMDLGlobals.g_PanPosition.x + -((float)e.MoveDelta.Y / CMDLGlobals.g_Zoom2DFactor), (float)CMDLGlobals.g_PanPosition.y + ((float)e.MoveDelta.X / CMDLGlobals.g_Zoom2DFactor), (float)CMDLGlobals.g_PanPosition.z));
		}

		public void simpleOpenGlControl3_MouseMove(object sender, HookMouseEventArgs e)
		{
			MouseMove2D(e, EViewport.ZYViewport,
				delegate(PointF StartPos)
				{
					label5.Text = StartPos.X.ToString();
					label6.Text = StartPos.Y.ToString();
				},
				simpleOpenGlControl3, delegate(CVerticeFrameData v, PointF Delta)
				{
					if ((CMDLGlobals.g_Axis & EAxis.Z) != 0)
						v.Position.z += Delta.Y;
					if ((CMDLGlobals.g_Axis & EAxis.X) != 0)
						v.Position.y += Delta.X;
				},
				new Vector3(CMDLGlobals.g_PanPosition.x, (float)CMDLGlobals.g_PanPosition.y + ((float)e.MoveDelta.X / CMDLGlobals.g_Zoom2DFactor), (float)CMDLGlobals.g_PanPosition.z + -((float)e.MoveDelta.Y / CMDLGlobals.g_Zoom2DFactor)));
		}

		public void simpleOpenGlControl4_MouseMove(object sender, HookMouseEventArgs e)
		{
			MouseMove2D(e, EViewport.XZViewport,
				delegate(PointF StartPos)
				{
					label4.Text = StartPos.X.ToString();
					label6.Text = StartPos.Y.ToString();
				},
				simpleOpenGlControl4,
				delegate(CVerticeFrameData v, PointF Delta)
				{
					if ((CMDLGlobals.g_Axis & EAxis.Y) != 0)
						v.Position.x += Delta.X;
					if ((CMDLGlobals.g_Axis & EAxis.Z) != 0)
						v.Position.z += Delta.Y;
				},
				new Vector3((float)CMDLGlobals.g_PanPosition.x + ((float)e.MoveDelta.X / CMDLGlobals.g_Zoom2DFactor), CMDLGlobals.g_PanPosition.y, (float)CMDLGlobals.g_PanPosition.z + -((float)e.MoveDelta.Y / CMDLGlobals.g_Zoom2DFactor)));
		}

		public void ModifyZoom(float NewZoom)
		{
			CMDLGlobals.g_Zoom2DFactor = NewZoom;
		}

		public void DoTickZoom(HookMouseEventArgs e)
		{
			ModifyZoom((e.MouseEvent.Delta > 0) ? CMDLGlobals.g_Zoom2DFactor + (0.1f * CMDLGlobals.g_Zoom2DFactor) : CMDLGlobals.g_Zoom2DFactor - (0.1f * CMDLGlobals.g_Zoom2DFactor));
			Redraw2dViews();
		}

		public void simpleOpenGlControl2_MouseWheel(object sender, HookMouseEventArgs e)
		{
			DoTickZoom(e);
		}

		public void simpleOpenGlControl3_MouseWheel(object sender, HookMouseEventArgs e)
		{
			DoTickZoom(e);
		}

		public void simpleOpenGlControl4_MouseWheel(object sender, HookMouseEventArgs e)
		{
			DoTickZoom(e);
		}

		public void SetPanPosition(Vector3 v)
		{
			CMDLGlobals.g_OldPanPosition = CMDLGlobals.g_PanPosition;
			CMDLGlobals.g_PanPosition = v;

			Vector3 delta = (CMDLGlobals.g_OldPanPosition - CMDLGlobals.g_PanPosition);
			Vector3 eye = Cam.getPosition(), target = Cam.getTarget();

			delta.x = -(CMDLGlobals.g_OldPanPosition - CMDLGlobals.g_PanPosition).x;
			delta.y = (CMDLGlobals.g_OldPanPosition - CMDLGlobals.g_PanPosition).z;
			delta.z = -(CMDLGlobals.g_OldPanPosition - CMDLGlobals.g_PanPosition).y;

			eye += delta;
			target += delta;

			Cam.lookAt(new Vector3(eye.x, eye.y, eye.z), new Vector3(target.x, target.y, target.z), new Vector3(0, 1, 0));
		}

		public static bool SelectedVertsContains(List<CSelectedVertice> List, CSelectedVertice Vert)
		{
			for (int i = 0; i < List.Count; ++i)
			{
				if (List[i].Index == Vert.Index && List[i].Data == Vert.Data)
					return true;
			}
			return false;
		}

		public static List<CSelectedVertice> RetrieveSelectedVertices()
		{
			List<CSelectedVertice> verts = new List<CSelectedVertice>();

			switch (CMDLGlobals.g_ModelSelectType)
			{
				case ESelectType.Vertex:
					for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
					{
						var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

						for (int i = 0; i < mesh.Verts.Count; ++i)
						{
							CVertice Vert = mesh.Verts[i];

							if ((Vert.Flags & EVerticeFlags.Selected) != 0)
								verts.Add(new CSelectedVertice(m, i, null));
						}
					}
					break;
				case ESelectType.Face:
					for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
					{
						var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

						for (int i = 0; i < mesh.Tris.Count; i++)
						{
							if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
							{
								for (int z = 0; z < 3; ++z)
								{
									CSelectedVertice v = new CSelectedVertice(m, mesh.Tris[i].Vertices[z], null);

									if (!SelectedVertsContains(verts, v))
										verts.Add(v);
								}
							}
						}
					}
					break;
			}

			return verts;
		}

		public delegate void ModifyVerticeFunc(CVertice Vertice, int Frame, bool Selected);

		public static void ModifyVertices(ModifyVerticeFunc func)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
						func(mesh.Verts[i], CMDLGlobals.g_CurFrame, (mesh.Verts[i].Flags & EVerticeFlags.Selected) != 0);
				}
			}
			else
			{
				Dictionary<int, bool> Called = new Dictionary<int, bool>();

				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						for (int z = 0; z < 3; ++z)
						{
							if (!Called.ContainsKey(mesh.Tris[i].Vertices[z]))
							{
								Called[mesh.Tris[i].Vertices[z]] = false;
								func(mesh.Verts[mesh.Tris[i].Vertices[z]], CMDLGlobals.g_CurFrame, (mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0);

								if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
									Called[mesh.Tris[i].Vertices[z]] = true;
							}
							else if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0 && Called[mesh.Tris[i].Vertices[z]] == false)
							{
								Called[mesh.Tris[i].Vertices[z]] = true;
								func(mesh.Verts[mesh.Tris[i].Vertices[z]], CMDLGlobals.g_CurFrame, (mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0);
							}
						}
					}
				}
			}
		}

		public delegate void ModifyVerticeLocation(CVerticeFrameData v, PointF Delta);
		public delegate void SetLabelText(PointF StartPos);

		private void MouseMove2D(HookMouseEventArgs e, EViewport vp, SetLabelText slt, ViewportControl ctrl, ModifyVerticeLocation del, Vector3 PanPos)
		{
			PointF StartPos = new PointF();
			PointF EndPos = new PointF();

			CalculateCursorPosFromViewport(ctrl, vp, e.MouseEvent.Location, OldMousePos, ref StartPos, ref EndPos);

			slt(StartPos);

			if (mouseSelecting)
			{
				PointF Delta = StartPos;
				Delta.X -= EndPos.X;
				Delta.Y -= EndPos.Y;

				startMouse = ctrl.PointToClient(e.ClickPos);
				endMouse = e.MouseEvent.Location;

				startMouse.Y = ctrl.Height - startMouse.Y;
				endMouse.Y = ctrl.Height - endMouse.Y;

				if ((CMDLGlobals.g_MainActionMode & EActionType.Selecting) != 0)
				{
					ctrl.Invalidate();
				}
				else if (CMDLGlobals.g_MainActionMode == EActionType.Move)
				{
					ModifyVertices(delegate(CVertice Vert, int Frame, bool Selected)
					{
						if (Selected)
							del(Vert.FrameData[Frame], Delta);
					});

					RedrawAllViews();
				}
				else if (CMDLGlobals.g_MainActionMode == EActionType.Rotate)
				{
					if (CMDLGlobals.g_ModelSelectType != ESelectType.Bone)
					{
						PointF WatPos = new PointF();
						CalculateCursorPosFromViewport(ctrl, vp, ctrl.PointToClient(e.ClickPos), ctrl.PointToClient(e.ClickPos), ref WatPos, ref WatPos);

						Vector3 centerSelection = CMDLGlobals.g_CurMdl.GetSelectionCentre(CMDLGlobals.g_ModelSelectType);

						var ang = 2.0 * (EndPos.Y - StartPos.Y) / (double)ctrl.Height;
						var cosa = Math.Cos(ang);
						var sina = Math.Sin(ang);


						switch (vp)
						{
							case EViewport.XYViewport:
								ModifyVertices(delegate(CVertice Vert, int Frame, bool Selected)
								{
									if (Selected)
									{
										CVerticeFrameData fd = Vert.FrameData[Frame];

										if ((ModifierKeys & Keys.Control) != 0)
										{
											fd.Position = new Vector3((float)((fd.Position.x - centerSelection.x) * cosa + (fd.Position.y - centerSelection.y) * sina) + centerSelection.x,
																		(float)((fd.Position.y - centerSelection.y) * cosa - (fd.Position.x - centerSelection.x) * sina) + centerSelection.y,
																		fd.Position.z);
										}
										else
										{
											fd.Position = new Vector3((float)((fd.Position.x - WatPos.Y) * cosa + (fd.Position.y - WatPos.X) * sina) + WatPos.Y,
																		(float)((fd.Position.y - WatPos.X) * cosa - (fd.Position.x - WatPos.Y) * sina) + WatPos.X,
																		fd.Position.z);
										}
									}
								});
								break;
							case EViewport.XZViewport:
								ModifyVertices(delegate(CVertice Vert, int Frame, bool Selected)
								{
									if (Selected)
									{
										CVerticeFrameData fd = Vert.FrameData[Frame];

										if ((ModifierKeys & Keys.Control) != 0)
										{
											fd.Position = new Vector3((float)((fd.Position.x - centerSelection.x) * cosa + (fd.Position.z - centerSelection.z) * sina) + centerSelection.x,
																		fd.Position.y,
																		(float)((fd.Position.z - centerSelection.z) * cosa - (fd.Position.x - centerSelection.x) * sina) + centerSelection.z);
										}
										else
										{
											fd.Position = new Vector3((float)((fd.Position.x - WatPos.X) * cosa + (fd.Position.z - WatPos.Y) * sina) + WatPos.X,
																		fd.Position.y,
																		(float)((fd.Position.z - WatPos.Y) * cosa - (fd.Position.x - WatPos.X) * sina) + WatPos.Y);
										}
									}
								});
								break;
							case EViewport.ZYViewport:
								ModifyVertices(delegate(CVertice Vert, int Frame, bool Selected)
								{
									if (Selected)
									{
										CVerticeFrameData fd = Vert.FrameData[Frame];

										if ((ModifierKeys & Keys.Control) != 0)
										{
											fd.Position = new Vector3(fd.Position.x,
																		(float)((fd.Position.y - centerSelection.y) * cosa - (fd.Position.z - centerSelection.z) * sina) + centerSelection.y,
																		(float)((fd.Position.z - centerSelection.z) * cosa + (fd.Position.y - centerSelection.y) * sina) + centerSelection.z);
										}
										else
										{
											fd.Position = new Vector3(fd.Position.x,
																		(float)((fd.Position.y - WatPos.X) * cosa - (fd.Position.z - WatPos.Y) * sina) + WatPos.X,
																		(float)((fd.Position.z - WatPos.Y) * cosa + (fd.Position.y - WatPos.X) * sina) + WatPos.Y);
										}
									}
								});
								break;
						}
					}
					else
					{
						foreach (CBone bone in CMDLGlobals.g_CurMdl.Bones)
						{
							if ((bone.Flags & EBoneFlags.Selected) != 0)
							{
								switch (vp)
								{
									case EViewport.XYViewport:
										bone.Angles.z -= e.MoveDelta.Y;
										if (bone.Angles.z > 360)
											bone.Angles.z -= 360;
										break;
									case EViewport.XZViewport:
										bone.Angles.y -= e.MoveDelta.Y;
										if (bone.Angles.y > 360)
											bone.Angles.y -= 360;
										break;
									case EViewport.ZYViewport:
										bone.Angles.x -= e.MoveDelta.Y;
										if (bone.Angles.x > 360)
											bone.Angles.x -= 360;
										break;
								}
							}
						}
					}

					RedrawAllViews();
				}
				else if (CMDLGlobals.g_MainActionMode == EActionType.Scale)
				{
					PointF WatPos = new PointF();
					CalculateCursorPosFromViewport(ctrl, vp, ctrl.PointToClient(e.ClickPos), ctrl.PointToClient(e.ClickPos), ref WatPos, ref WatPos);

					float fact = (float)(5 * (EndPos.Y - StartPos.Y) / (double)ctrl.Height) + 1;
					if (fact < 0)
						fact = 1 / (-fact);

					Vector3 center = CMDLGlobals.g_CurMdl.GetSelectionCentre(CMDLGlobals.g_ModelSelectType);

					ModifyVertices(delegate(CVertice Vert, int Frame, bool Selected)
					{
						if (Selected)
						{
							CVerticeFrameData fd = Vert.FrameData[Frame];

							Vector3 g_down = Vector3.Empty;

							switch (vp)
							{
								case EViewport.XYViewport:
								default:
									if ((ModifierKeys & Keys.Control) != 0)
										g_down = center;
									else
										g_down.Set(WatPos.Y, WatPos.X, center.z);
									break;
								case EViewport.XZViewport:
									if ((ModifierKeys & Keys.Control) != 0)
										g_down = center;
									else
										g_down.Set(WatPos.X, center.y, WatPos.Y);
									break;
								case EViewport.ZYViewport:
									if ((ModifierKeys & Keys.Control) != 0)
										g_down = center;
									else
										g_down.Set(center.x, WatPos.X, WatPos.Y);
									break;
							}

							Vector3 n = ((fd.Position - g_down) * fact) + g_down;

							if ((CMDLGlobals.g_Axis & EAxis.X) != 0)
								fd.Position.y = n.y;
							if ((CMDLGlobals.g_Axis & EAxis.Y) != 0)
								fd.Position.x = n.x;
							if ((CMDLGlobals.g_Axis & EAxis.Z) != 0)
								fd.Position.z = n.z;
						}
					});

					RedrawAllViews();
				}
				else if (((CMDLGlobals.g_MainActionMode & EActionType.CreateVertex) != 0 || (CMDLGlobals.g_MainActionMode & EActionType.CreateBone) != 0) &&
						(CMDLGlobals.g_MainActionMode & EActionType.Creating) != 0)
				{
					switch (vp)
					{
						case EViewport.XYViewport:
							createVertexPos = new Vector3(e.MouseEvent.Location.X, ctrl.Height - e.MouseEvent.Location.Y, createVertexPos.z);
							break;
						case EViewport.XZViewport:
							createVertexPos = new Vector3(createVertexPos.x, e.MouseEvent.Location.X, ctrl.Height - e.MouseEvent.Location.Y);
							break;
						case EViewport.ZYViewport:
							createVertexPos = new Vector3(e.MouseEvent.Location.X, createVertexPos.y, ctrl.Height - e.MouseEvent.Location.Y);
							break;
					}
					RedrawAllViews();
				}

				OldMousePos = e.MouseEvent.Location;
				return;
			}

			if (!CMDLGlobals.g_Panning && !CMDLGlobals.g_Zooming)
				return;

			if (CMDLGlobals.g_Panning)
				SetPanPosition(PanPos);
			else if (CMDLGlobals.g_Zooming)
				ModifyZoom(CMDLGlobals.g_Zoom2DFactor - ((float)e.MoveDelta.Y / 100) * CMDLGlobals.g_Zoom2DFactor);

			RedrawAllViews();
		}

		bool SkipChanged = false;

		public void SetCursor(Cursor c, bool Skip3D = false, bool Skip2D = false)
		{
			if (!Skip3D)
				simpleOpenGlControl1.Cursor = c;

			if (!Skip2D)
				simpleOpenGlControl2.Cursor =
				simpleOpenGlControl3.Cursor =
				simpleOpenGlControl4.Cursor = c;
		}

		public void SetCursors()
		{
			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) != 0)
				SetCursor(Program.Resources.CursorPan);
			else
			{
				SetCursor(Program.Resources.CursorSelect, false, true);

				switch (CMDLGlobals.g_MainActionMode)
				{
					case EActionType.Select:
						SetCursor(Program.Resources.CursorSelect, true, false);
						break;
					case EActionType.Rotate:
						SetCursor(Program.Resources.CursorRotate, true, false);
						break;
					case EActionType.Scale:
						SetCursor(Program.Resources.CursorScale, true, false);
						break;
					case EActionType.Move:
						SetCursor(Program.Resources.CursorMove, true, false);
						break;
					case EActionType.CreateVertex:
					case EActionType.CreateBone:
						SetCursor(Program.Resources.CursorBuildVert, true, false);
						break;
					case EActionType.BuildFace:
					case EActionType.BuildingFace1:
					case EActionType.BuildingFace2:
						SetCursor(Program.Resources.CursorBuildFace, false, false);
						break;
				}
			}
		}

		public void UpdateModelSelectionType()
		{
			SkipChanged = true;

			if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
			{
				button5.Checked = true;
				button6.Checked = false;
				button7.Checked = false;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
			{
				button5.Checked = false;
				button6.Checked = true;
				button7.Checked = false;
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				button5.Checked = false;
				button6.Checked = false;
				button7.Checked = true;
			}

			SkipChanged = false;
			SetCursors();

			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) != 0)
				button9.Checked = true;
			else
				button9.Checked = false;

			SetModifyTabData();
			RedrawAllViews();
		}

		private void button5_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_ModelSelectType = ESelectType.Vertex;
			UpdateModelSelectionType();
		}

		private void button6_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_ModelSelectType = ESelectType.Face;
			UpdateModelSelectionType();
		}

		private void button7_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_ModelSelectType = ESelectType.Bone;
			UpdateModelSelectionType();
		}

		private void button9_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			if (button9.Checked)
				CMDLGlobals.g_MainActionMode |= EActionType.Pan;
			else
				CMDLGlobals.g_MainActionMode &= ~EActionType.Pan;

			UpdateModelSelectionType();
			CommonToolbox.UpdateBoxActionType();
		}

		// Updates any visual counts and whatnot
		public void ModelUpdated()
		{
			int totalVerts = 0, totalTris = 0;

			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				totalVerts += CMDLGlobals.g_CurMdl.Meshes[m].Verts.Count;
				totalTris += CMDLGlobals.g_CurMdl.Meshes[m].Tris.Count;
			}

			label7.Text = (CMDLGlobals.g_CurMdl.Frames.Count != 0) ? totalVerts.ToString() : "0";
			label9.Text = totalTris.ToString();
			hScrollBar1.Maximum = CMDLGlobals.g_CurMdl.Frames.Count + 3;

			label1.Text = CMDLGlobals.g_CurFrame.ToString();
			label2.Text = CMDLGlobals.g_CurMdl.Frames[CMDLGlobals.g_CurFrame].FrameName;

			hScrollBar1.Value = CMDLGlobals.g_CurFrame;

			theMeshesTab.RefreshMeshes();
		}

		public void SetShading(COpenGlControlData cd)
		{
			if ((cd.Flags & EControlDataFlags.Gourad) != 0)
				Gl.glShadeModel(Gl.GL_SMOOTH);
			else
				Gl.glShadeModel(Gl.GL_FLAT);
		}

		private void generateHeaderFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_CurMdl.Frames.Count == 0)
			{
				MessageBox.Show("No frames to export to a header file!");
				return;
			}

			using (SaveFileDialog svd = new SaveFileDialog())
			{
				svd.Filter = "C/C++ Header Files (*.h)|*.h|All Files (*)|*";
				svd.RestoreDirectory = true;
				svd.AddExtension = true;
				svd.DefaultExt = "h";

				if (svd.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
					return;

				using (System.IO.FileStream file = System.IO.File.Open(svd.FileName, System.IO.FileMode.Create))
				{
					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(file))
					{
						writer.WriteLine("// Generated by VCMDL.NET");
						writer.WriteLine();
						writer.WriteLine("enum");
						writer.WriteLine("{");

						for (int i = 0; i < CMDLGlobals.g_CurMdl.Frames.Count; ++i)
						{
							writer.WriteLine("\tFRAME_" + CMDLGlobals.g_CurMdl.Frames[i].FrameName + ",");
						}

						writer.WriteLine("};");
						writer.WriteLine();
						writer.WriteLine("const float MODEL_SCALE = 1.000000f;");
					}
				}
			}
		}

		private void memoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RedrawModelView();
		}

		bool Splitter2Moving = false, Splitter3Moving = false;

		private void splitContainer3_SplitterMoving(object sender, SplitterCancelEventArgs e)
		{
			splitContainer2.SplitterDistance = splitContainer3.SplitterDistance;
			Splitter3Moving = true;
		}

		private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
		{
			if (Splitter3Moving)
				splitContainer2.SplitterDistance = splitContainer3.SplitterDistance;
		}

		private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
		{
			if (Splitter2Moving)
				splitContainer3.SplitterDistance = splitContainer2.SplitterDistance;
		}

		private void splitContainer2_SplitterMoving(object sender, SplitterCancelEventArgs e)
		{
			splitContainer3.SplitterDistance = splitContainer2.SplitterDistance;
			Splitter2Moving = true;
		}

		private void pictureBox1_SizeChanged(object sender, EventArgs e)
		{
		}

		private void ModelEditor_Paint(object sender, PaintEventArgs e)
		{
		}

		private void ModelEditor_SizeChanged(object sender, EventArgs e)
		{
			Splitter2Moving = false;
			Splitter3Moving = false;
		}

		private void resetCameraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetCamera();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						mesh.Tris[i].Flags |= ETriangleFlags.Selected;

						if (CMDLGlobals.g_SyncSelections)
							mesh.Tris[i].Flags |= ETriangleFlags.SkinSelected;
					}
				}
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				foreach (CBone b in CMDLGlobals.g_CurMdl.Bones)
					b.Flags |= EBoneFlags.Selected;
			}
			else
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
						mesh.Verts[i].Flags |= EVerticeFlags.Selected;
				}
			}

			RedrawAllViews();
		}

		private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						mesh.Tris[i].Flags &= ~ETriangleFlags.Selected;

						if (CMDLGlobals.g_SyncSelections)
							mesh.Tris[i].Flags &= ~ETriangleFlags.SkinSelected;
					}
				}
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				//foreach (CBone bone in CMDLGlobals.g_CurMdl.Bones)
				//	bone.Flags &= ~EBoneFlags.Selected;
			}
			else
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
						mesh.Verts[i].Flags &= ~EVerticeFlags.Selected;
				}
			}

			RedrawAllViews();
		}

		private void selectInverseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
						{
							mesh.Tris[i].Flags &= ~ETriangleFlags.Selected;

							if (CMDLGlobals.g_SyncSelections)
								mesh.Tris[i].Flags &= ~ETriangleFlags.SkinSelected;
						}
						else
						{
							mesh.Tris[i].Flags |= ETriangleFlags.Selected;

							if (CMDLGlobals.g_SyncSelections)
								mesh.Tris[i].Flags |= ETriangleFlags.SkinSelected;
						}
					}
				}
			}
			else if (CMDLGlobals.g_ModelSelectType == ESelectType.Bone)
			{
				foreach (CBone b in CMDLGlobals.g_CurMdl.Bones)
					b.Flags ^= EBoneFlags.Selected;
			}
			else
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
						mesh.Verts[i].Flags ^= EVerticeFlags.Selected;
				}
			}

			RedrawAllViews();
		}

		int DictContainsVert(Dictionary<int, List<Tuple<int, int>>> VertDict, Tuple<int, int> i)
		{
			int Key = 0;

			// See if the dictionary already contains this vertice somewhere
			foreach (var lv in VertDict.Values)
			{
				foreach (var iv in lv)
					if (iv.Item1 == i.Item1 &&
						iv.Item2 == i.Item2)
						return Key;

				Key++;
			}

			return -1;
		}

		void RecursiveCheck(int Mesh, int VerticeIndex, Dictionary<int, List<Tuple<int, int>>> VertDict, int CurIndex)
		{
			var mesh = CMDLGlobals.g_CurMdl.Meshes[Mesh];

			for (int t = 0; t < mesh.Tris.Count; ++t)
			{
				for (int z = 0; z < 3; ++z)
				{
					if (mesh.Tris[t].Vertices[z] == VerticeIndex)
					{
						for (int y = 0; y < 3; ++y)
						{
							if (y == z || DictContainsVert(VertDict, Tuple.Create(mesh.Tris[t].Vertices[y], Mesh)) != -1)
								continue;

							VertDict[CurIndex].Add(Tuple.Create(mesh.Tris[t].Vertices[y], Mesh));
							RecursiveCheck(Mesh, mesh.Tris[t].Vertices[y], VertDict, CurIndex);
						}
						break;
					}
				}
			}
		}

		private void selectConnectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var VertDict = new Dictionary<int, List<Tuple<int, int>>>();
			int CurIndex = -1;

			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

				for (int i = 0; i < mesh.Verts.Count; ++i)
				{
					if (DictContainsVert(VertDict, Tuple.Create(m, i)) != -1)
						continue;

					CurIndex++;
					VertDict[CurIndex] = new List<Tuple<int, int>>();

					RecursiveCheck(m, i, VertDict, CurIndex);
				}
			}

			List<CSelectedVertice> v = RetrieveSelectedVertices();

			for (int i = 0; i < v.Count; ++i)
			{
				int Group = DictContainsVert(VertDict, Tuple.Create(v[i].Mesh, v[i].Index));

				for (int t = 0; t < VertDict[Group].Count; ++t)
				{
					int Mesh = VertDict[Group][t].Item1;
					int Vert = VertDict[Group][t].Item2;

					var mesh = CMDLGlobals.g_CurMdl.Meshes[Mesh];

					if (CMDLGlobals.g_ModelSelectType == ESelectType.Vertex)
					{
						for (int f = 0; f < CMDLGlobals.g_CurMdl.Frames.Count; ++f)
							mesh.Verts[Vert].Flags |= EVerticeFlags.Selected;
					}
					else
					{
						for (int f = 0; f < mesh.Tris.Count; ++f)
						{
							for (int z = 0; z < 3; ++z)
							{
								if (mesh.Tris[f].Vertices[z] == Vert)
								{
									mesh.Tris[f].Flags |= ETriangleFlags.Selected;

									if (CMDLGlobals.g_SyncSelections)
										mesh.Tris[f].Flags |= ETriangleFlags.SkinSelected;
									break;
								}
							}
						}
					}
				}
			}

			RedrawAllViews();
		}

		void configureToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			using (BasePath bp = new BasePath())
			{
				bp.StartPosition = FormStartPosition.CenterParent;
				bp.ShowDialog();
			}
		}

		private void selectTouchingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Get all faces that have the selected vertices connected to them
			List<CTriangle> Triangles = new List<CTriangle>();
			List<CSelectedVertice> Verts = RetrieveSelectedVertices();

			foreach (CSelectedVertice v in Verts)
			{
				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Tris.Count; ++i)
					{
						for (int z = 0; z < 3; ++z)
						{
							if (mesh.Tris[i].Vertices[z] == v.Index &&
								!Triangles.Contains(mesh.Tris[i]))
								Triangles.Add(mesh.Tris[i]);
						}
					}
				}
			}

			foreach (CTriangle t in Triangles)
			{
				if (CMDLGlobals.g_ModelSelectType == ESelectType.Face)
				{
					t.Flags |= ETriangleFlags.Selected;

					if (CMDLGlobals.g_SyncSelections)
						t.Flags |= ETriangleFlags.SkinSelected;
				}
				else
				{
					for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
					{
						var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

						for (int z = 0; z < 3; ++z)
							mesh.Verts[t.Vertices[z]].Flags |= EVerticeFlags.Selected;
					}
				}
			}

			RedrawAllViews();
		}

		private void ModelEditor_KeyPress(object sender, KeyPressEventArgs e)
		{

		}

		private void gotoFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (GotoFrame gf = new GotoFrame())
			{
				gf.StartPosition = FormStartPosition.CenterParent;
				if (gf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					Program.Form_ModelEditor.SetFrame(gf.FrameNo);
					Program.Form_ModelEditor.RedrawAllViews();
				}
			}
		}

		private void deleteFramesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (DeleteFrames gf = new DeleteFrames())
			{
				gf.StartPosition = FormStartPosition.CenterParent;
				gf.ShowDialog();
			}
		}

		private void label2_Click(object sender, EventArgs e)
		{
			label2.Visible = false;
			textBox1.Visible = true;

			textBox1.Text = label2.Text;
			textBox1.Focus();
		}

		private void textBox1_Leave(object sender, EventArgs e)
		{
			textBox1.Visible = false;
			label2.Visible = true;

			CMDLGlobals.g_CurMdl.Frames[CMDLGlobals.g_CurFrame].FrameName = textBox1.Text;
			label2.Text = textBox1.Text;
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				textBox1_Leave(null, null);
			}
		}

		private void toolStripMenuItem17_Click(object sender, EventArgs e)
		{
			using (ChangeGrid cg = new ChangeGrid())
			{
				cg.SetValues(CMDLGlobals.g_GridSize, CMDLGlobals.g_GridSlices);
				cg.StartPosition = FormStartPosition.CenterParent;
				cg.ShowDialog();
			}
		}

		public void groundPlanePositionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (GroundPlane gp = new GroundPlane())
			{
				gp.StartPosition = FormStartPosition.CenterParent;
				gp.ShowDialog();

				RedrawAllViews();
			}
		}

		ViewportControl ToolStripViewPortControl = null;

		private void noneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			gldata.Flags &= ~(EControlDataFlags.NormalsAll | EControlDataFlags.NormalsSelected);

			ctrl.Invalidate();
		}

		private void selectedFacesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			gldata.Flags &= ~EControlDataFlags.NormalsAll | EControlDataFlags.NormalsSelected;
			gldata.Flags |= EControlDataFlags.NormalsSelected;

			ctrl.Invalidate();
		}

		private void allToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			gldata.Flags &= ~EControlDataFlags.NormalsAll | EControlDataFlags.NormalsSelected;
			gldata.Flags |= EControlDataFlags.NormalsAll;

			ctrl.Invalidate();
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			ViewportControl ctrl = (ViewportControl)((ContextMenuStrip)sender).SourceControl;
			COpenGlControlData gldata = ctrl.ControlData;

			ToolStripViewPortControl = ctrl;

			if (gldata.Is3D)
				contextMenuStrip1.Items.Remove(setBackgroundImageToolStripMenuItem);
			else
				contextMenuStrip1.Items.Add(setBackgroundImageToolStripMenuItem);

			showOriginToolStripMenuItem1.Checked = (gldata.Flags & EControlDataFlags.ShowAxis) != 0;
			showGridToolStripMenuItem1.Checked = (gldata.Flags & EControlDataFlags.ShowGrid) != 0;
			showVerticeTicksToolStripMenuItem.Checked = (gldata.Flags & EControlDataFlags.ShowVerticeTicks) != 0;
			lightingToolStripMenuItem.Checked = (gldata.Flags & EControlDataFlags.Lighting) != 0;
			shadingToolStripMenuItem.Checked = (gldata.Flags & EControlDataFlags.Gourad) != 0;
			wireframeToolStripMenuItem.Checked = (gldata.Flags & EControlDataFlags.WireFrame) != 0;
			texturedToolStripMenuItem.Checked = (gldata.Flags & EControlDataFlags.Textured) != 0;
			toolStripMenuItem11.Checked = (gldata.Flags & EControlDataFlags.ShowBackfaces) != 0;

			noneToolStripMenuItem.Checked =
			selectedFacesToolStripMenuItem.Checked =
			allToolStripMenuItem.Checked = false;

			if ((gldata.Flags & EControlDataFlags.NormalsAll) != 0)
				allToolStripMenuItem.Checked = true;
			else if ((gldata.Flags & EControlDataFlags.NormalsSelected) != 0)
				selectedFacesToolStripMenuItem.Checked = true;
			else
				noneToolStripMenuItem.Checked = true;
		}

		private void showOriginToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.ShowAxis) != 0)
				gldata.Flags &= ~EControlDataFlags.ShowAxis;
			else
				gldata.Flags |= EControlDataFlags.ShowAxis;

			ctrl.Invalidate();
		}

		private void showGridToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.ShowGrid) != 0)
				gldata.Flags &= ~EControlDataFlags.ShowGrid;
			else
				gldata.Flags |= EControlDataFlags.ShowGrid;

			ctrl.Invalidate();
		}

		private void showVerticeTicksToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.ShowVerticeTicks) != 0)
				gldata.Flags &= ~EControlDataFlags.ShowVerticeTicks;
			else
				gldata.Flags |= EControlDataFlags.ShowVerticeTicks;

			ctrl.Invalidate();
		}

		private void lightingToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;
			ctrl.MakeCurrent();

			if ((gldata.Flags & EControlDataFlags.Lighting) != 0)
				gldata.Flags &= ~EControlDataFlags.Lighting;
			else
				gldata.Flags |= EControlDataFlags.Lighting;

			if ((gldata.Flags & EControlDataFlags.Lighting) != 0)
				EnableLighting(gldata);
			else
				DisableLighting(gldata);

			ctrl.Invalidate();
		}

		private void shadingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;
			ctrl.MakeCurrent();

			if ((gldata.Flags & EControlDataFlags.Gourad) != 0)
				gldata.Flags &= ~EControlDataFlags.Gourad;
			else
				gldata.Flags |= EControlDataFlags.Gourad;

			SetShading(gldata);
			ctrl.Invalidate();
		}

		private void wireframeToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.WireFrame) != 0)
				gldata.Flags &= ~EControlDataFlags.WireFrame;
			else
				gldata.Flags |= EControlDataFlags.WireFrame;

			gldata.Flags &= ~EControlDataFlags.Textured;

			ctrl.Invalidate();
		}

		private void texturedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.Textured) != 0)
				gldata.Flags &= ~EControlDataFlags.Textured;
			else
				gldata.Flags |= EControlDataFlags.Textured;

			gldata.Flags &= ~EControlDataFlags.WireFrame;

			ctrl.Invalidate();
		}

		private void toolStripMenuItem11_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			if ((gldata.Flags & EControlDataFlags.ShowBackfaces) != 0)
				gldata.Flags &= ~EControlDataFlags.ShowBackfaces;
			else
				gldata.Flags |= EControlDataFlags.ShowBackfaces;

			ctrl.Invalidate();
		}

		private void setBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ViewportControl ctrl = ToolStripViewPortControl;
			COpenGlControlData gldata = ctrl.ControlData;

			using (BackgroundImage bi = new BackgroundImage())
			{
				bi.SetupData(gldata.BackImage.Path, gldata.BackImage.Skin, gldata.Offset, gldata.Scale);
				bi.StartPosition = FormStartPosition.CenterParent;

				if (bi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					ctrl.MakeCurrent();

					gldata.BackImage.SetBitmap(bi.LoadedBitmap);

					if (gldata.BackImage.Skin != null)
						gldata.BackImage.Path = bi.LoadedBitmapFileName;
					else
						gldata.BackImage.Path = "";

					gldata.Offset = bi.Offset;
					gldata.Scale = bi.FScale;

					ctrl.Invalidate();
				}
			}
		}

		private void syncSkinSelectionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_SyncSelections = !CMDLGlobals.g_SyncSelections;

			syncSkinSelectionToolStripMenuItem.Checked = CMDLGlobals.g_SyncSelections;

			for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
			{
				var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

				for (int i = 0; i < mesh.Tris.Count; ++i)
				{
					if ((mesh.Tris[i].Flags & ETriangleFlags.Selected) != 0)
						mesh.Tris[i].Flags |= (ETriangleFlags.Selected | ETriangleFlags.SkinSelected);
					else if ((mesh.Tris[i].Flags & ETriangleFlags.SkinSelected) != 0)
						mesh.Tris[i].Flags &= ~ETriangleFlags.SkinSelected;
				}
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.Fit(true);
			RedrawAllViews();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.Fit(false);
			RedrawAllViews();
		}

		private void deleteCurrentFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (CMDLGlobals.g_CurMdl.Frames.Count == 1)
				return;
			else if (MessageBox.Show("Are you sure?", "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
				return;

			CMDLGlobals.g_CurMdl.DeleteFrame(CMDLGlobals.g_CurFrame);
			RedrawAllViews();
			ModelUpdated();
		}

		AddNewFrame gf = new AddNewFrame();
		private void addNewFrameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			gf.StartPosition = FormStartPosition.CenterParent;
			gf.Setup();
			gf.numericUpDown1.Value = CMDLGlobals.g_CurFrame + 1;
			gf.textBox1.Text = "Frame " + gf.numericUpDown1.Value.ToString();
			int oldFramesCount = CMDLGlobals.g_CurMdl.Frames.Count;
			if (gf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				CFrame CopiedFrame = new CFrame();
				int CopiedFrameNum = (int)((gf.numericUpDown1.Value == 0) ? 0 : gf.numericUpDown1.Value - 1);

				CopiedFrame.FrameName = gf.textBox1.Text;

				for (int i = 0; i < gf.numericUpDown2.Value; ++i)
				{
					if ((int)gf.numericUpDown1.Value == oldFramesCount)
						CMDLGlobals.g_CurMdl.Frames.Add(CopiedFrame);
					else
						CMDLGlobals.g_CurMdl.Frames.Insert((int)gf.numericUpDown1.Value, CopiedFrame);
				}

				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
					{
						CVerticeFrameData fd = new CVerticeFrameData();

						fd.Position = mesh.Verts[i].FrameData[CopiedFrameNum].Position;
						fd.Normal = mesh.Verts[i].FrameData[CopiedFrameNum].Normal;

						for (int z = 0; z < gf.numericUpDown2.Value; ++z)
						{
							if ((int)gf.numericUpDown1.Value == oldFramesCount)
								mesh.Verts[i].FrameData.Add(fd);
							else
								mesh.Verts[i].FrameData.Insert((int)gf.numericUpDown1.Value, fd);
						}
					}
				}

				ModelUpdated();
			}
		}

		private void moveFramesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MoveFrames mpc = new MoveFrames();
			mpc.StartPosition = FormStartPosition.CenterParent;

			if (mpc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				List<int> RemoveFrames = DeleteFrames.GetFramesFromDataGridView(MoveFrames.framesToMove);
				int InsertFrameAt = DeleteFrames.GetFramesFromDataGridView(MoveFrames.placeToInsert)[0] + 1;
				int FramesCount = CMDLGlobals.g_CurMdl.Frames.Count;
				List<CFrame> RemovedFrames = new List<CFrame>();

				for (int i = RemoveFrames.Count - 1; i >= 0; i--)
				{
					RemovedFrames.Add(CMDLGlobals.g_CurMdl.Frames[RemoveFrames[i]]);
					CMDLGlobals.g_CurMdl.Frames.RemoveAt(RemoveFrames[i]);
				}

				RemovedFrames = RemovedFrames.Invert();

				if (InsertFrameAt >= FramesCount - 1)
					CMDLGlobals.g_CurMdl.Frames.AddRange(RemovedFrames);
				else
					CMDLGlobals.g_CurMdl.Frames.InsertRange(InsertFrameAt, RemovedFrames);

				for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
				{
					var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

					for (int i = 0; i < mesh.Verts.Count; ++i)
					{
						List<CVerticeFrameData> RemovedFrameData = new List<CVerticeFrameData>();

						for (int z = RemoveFrames.Count - 1; z >= 0; z--)
						{
							RemovedFrameData.Add(mesh.Verts[i].FrameData[RemoveFrames[z]]);
							mesh.Verts[i].FrameData.RemoveAt(RemoveFrames[z]);
						}

						RemovedFrameData = RemovedFrameData.Invert();

						if (InsertFrameAt >= FramesCount - 1)
							mesh.Verts[i].FrameData.AddRange(RemovedFrameData);
						else
							mesh.Verts[i].FrameData.InsertRange(InsertFrameAt, RemovedFrameData);
					}
				}

				ModelUpdated();
			}
		}

		private void simpleOpenGlControl2_Load(object sender, EventArgs e)
		{

		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.ClearModel();
			CMDLGlobals.g_CurFrame = 0;
			CMDLGlobals.g_CurSkin = 0;
			ModelUpdated();
			_loadedFile = null;
		}

		private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void toolStripMenuItem12_Click(object sender, EventArgs e)
		{
			InterpFrames mpc = new InterpFrames();
			mpc.StartPosition = FormStartPosition.CenterParent;

			if (mpc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				List<int> framesToInterpolate = DeleteFrames.GetFramesFromDataGridView(InterpFrames.framesToMove);
				int framesToAdd = InterpFrames.numFramesToAdd;
				int FramesCount = CMDLGlobals.g_CurMdl.Frames.Count;

				framesToInterpolate.Sort();

				List<List<int>> groupedFrames = new List<List<int>>();
				int curGroupStart = -1, curGroupEnd = -1;

				for (int i = 0; i < framesToInterpolate.Count + 1; ++i)
				{
					if (curGroupStart == -1)
						curGroupStart = curGroupEnd = i;
					else
					{
						if (i != framesToInterpolate.Count && framesToInterpolate[i] - framesToInterpolate[curGroupEnd] == 1)
							curGroupEnd++;
						else
						{
							if (curGroupStart != curGroupEnd)
							{
								List<int> b = new List<int>();

								for (int z = curGroupEnd; z >= curGroupStart; --z)
									b.Add(framesToInterpolate[z]);

								groupedFrames.Add(b);
							}

							curGroupStart = -1;
							i--;
						}
					}
				}

				foreach (List<int> grp in groupedFrames)
				{
					for (int i = 1; i < grp.Count; ++i)
					{
						for (int f = framesToAdd - 1; f >= 0; --f)
						{
							CFrame fr = new CFrame();
							fr.FrameName = CMDLGlobals.g_CurMdl.Frames[grp[i]].FrameName + "_" + (f + 1).ToString();
							CMDLGlobals.g_CurMdl.Frames.Insert(grp[i] + 1, fr);
						}
					}
				}

				foreach (List<int> grp in groupedFrames)
				{
					for (int m = 0; m < CMDLGlobals.g_CurMdl.Meshes.Count; ++m)
					{
						var mesh = CMDLGlobals.g_CurMdl.Meshes[m];

						foreach (CVertice v in mesh.Verts)
						{
							for (int i = 1; i < grp.Count; ++i)
							{
								float frac = (1.0f / (framesToAdd + 1));
								List<CVerticeFrameData> fds = new List<CVerticeFrameData>();

								for (int f = framesToAdd - 1; f >= 0; --f)
								{
									int f1 = grp[i];
									int f2 = grp[i - 1];

									CVerticeFrameData fd = new CVerticeFrameData();
									fd.Position = TCompleteModel.CalculateInterpolation(v.FrameData[f1].Position, v.FrameData[f2].Position, frac * (f + 1));
									//v.FrameData.Insert(grp[i] + 1, fd);
									fds.Insert(0, fd);
								}

								v.FrameData.Insert(grp[i] + 1, fds);
							}
						}
					}
				}

				ModelUpdated();
			}
		}

		private void saveCameraAnimationToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}
	}

	public class HookMouseEventArgs
	{
		public Point MoveDelta, OldPosition, ClickPos;
		public MouseEventArgs MouseEvent;

		public HookMouseEventArgs(MouseEventArgs e)
		{
			MouseEvent = e;
		}

		public HookMouseEventArgs(MouseEventArgs e, Point p, Point op, Point cp)
		{
			MouseEvent = e;
			MoveDelta = p;
			OldPosition = op;
			ClickPos = cp;
		}
	}

	delegate void MouseHookEventHandler(Object sender, HookMouseEventArgs e);
	class CControlMouseMoveHook
	{
		private Control linkedCtrl;
		private Point oldPos, clickPos;
		private MouseHookEventHandler MouseDown, MouseUp, MouseMove, MouseWheel;

		public CControlMouseMoveHook(Control _linkedCtrl, MouseHookEventHandler _MouseDown, MouseHookEventHandler _MouseUp, MouseHookEventHandler _MouseMove, MouseHookEventHandler _MouseWheel)
		{
			linkedCtrl = _linkedCtrl;

			linkedCtrl.MouseDown += MouseDownHandler;
			linkedCtrl.MouseUp += MouseUpHandler;
			linkedCtrl.MouseMove += MouseMoveHandler;
			linkedCtrl.MouseWheel += MouseWheelHandler;

			MouseDown = _MouseDown;
			MouseUp = _MouseUp;
			MouseMove = _MouseMove;
			MouseWheel = _MouseWheel;
		}

		void MouseDownHandler(Object sender, MouseEventArgs e)
		{
			oldPos = Cursor.Position;
			clickPos = Cursor.Position;

			if (MouseDown != null)
			{
				HookMouseEventArgs hm = new HookMouseEventArgs(e, Point.Empty, oldPos, clickPos);
				MouseDown(sender, hm);
				oldPos = hm.OldPosition;
			}
		}

		void MouseUpHandler(Object sender, MouseEventArgs e)
		{
			oldPos = new Point(0, 0);

			if (MouseUp != null)
			{
				HookMouseEventArgs hm = new HookMouseEventArgs(e, Point.Empty, oldPos, clickPos);
				MouseUp(sender, hm);
				oldPos = hm.OldPosition;
			}
		}

		void MouseMoveHandler(Object sender, MouseEventArgs e)
		{
			HookMouseEventArgs ne = new HookMouseEventArgs(e, Point.Empty, Point.Empty, clickPos);
			ne.MoveDelta = new Point(Cursor.Position.X - oldPos.X, Cursor.Position.Y - oldPos.Y);
			oldPos = Cursor.Position;
			ne.OldPosition = oldPos;

			if (MouseMove != null)
			{
				MouseMove(sender, ne);
				oldPos = ne.OldPosition;
			}
		}

		void MouseWheelHandler(Object sender, MouseEventArgs e)
		{
			HookMouseEventArgs ne = new HookMouseEventArgs(e, Point.Empty, Point.Empty, clickPos);
			ne.MoveDelta = new Point(Cursor.Position.X - oldPos.X, Cursor.Position.Y - oldPos.Y);
			oldPos = Cursor.Position;
			ne.OldPosition = oldPos;

			if (MouseWheel != null)
			{
				MouseWheel(sender, ne);
				oldPos = ne.OldPosition;
			}
		}
	}

	public enum EControlDataFlags
	{
		WireFrame = 1,
		Gourad = 2,
		Lighting = 4,
		Textured = 8,
		ShowBackfaces = 16,
		ShowVerticeTicks = 32,
		ShowGrid = 64,
		ShowAxis = 128,
		NormalsSelected = 256,
		NormalsAll = 512,
	}

	public class COpenGlControlData
	{
		public bool Is3D = false;
		public EControlDataFlags Flags = 0;
		public VCMDL.NET.CSkin BackImage = new VCMDL.NET.CSkin(VCMDL.NET.ModelEditor.FillSingleSkin);
		public PointF Offset = new PointF(), Scale = new PointF(1.0f, 1.0f);
	}

	public class ViewportControl : SimpleOpenGlControl
	{
		public COpenGlControlData ControlData;
		public VCMDL.NET.EViewport Viewport;

		public ViewportControl()
		{
			SizeChanged += ViewportControlSizeChanged;
		}

		void ViewportControlSizeChanged(object sender, EventArgs e)
		{
			if (ControlData == null)
				return;

			if (ControlData.Is3D)
				Program.Form_ModelEditor.Reshape3dView((ViewportControl)sender);
			else
				Program.Form_ModelEditor.Reshape2dView((ViewportControl)sender);
		}
	}

	public delegate string GetMouseStatusDelegate(CMouseStatusIndex m);

	public class CMouseStatusIndex
	{
		Control ctrl;
		GetMouseStatusDelegate cb;
		CMouseStatus ms;

		public CMouseStatusIndex(CMouseStatus _ms, Control _ctrl, GetMouseStatusDelegate _cb)
		{
			Init(_ms, _ctrl, _cb);
		}

		public CMouseStatusIndex(CMouseStatus _ms, Control _ctrl, string _str)
		{
			Init(_ms, _ctrl, delegate(CMouseStatusIndex i) { return _str; });
		}

		void Init(CMouseStatus _ms, Control _ctrl, GetMouseStatusDelegate _cb)
		{
			cb = _cb;
			ctrl = _ctrl;
			ms = _ms;

			ctrl.MouseEnter += new EventHandler(ctrl_MouseEnter);
			ctrl.MouseHover += new EventHandler(ctrl_MouseHover);
			ctrl.MouseLeave += new EventHandler(ctrl_MouseLeave);
		}

		public static CMouseStatusIndex _lastStatusIndex = null;

		void ctrl_MouseLeave(object sender, EventArgs e)
		{
			_lastStatusIndex = null;
			ms.SetLabel("");
		}

		void ctrl_MouseEnter(object sender, EventArgs e)
		{
			ms.SetLabel(cb(this));
		}

		void ctrl_MouseHover(object sender, EventArgs e)
		{
			if (_lastStatusIndex != null || _lastStatusIndex != this)
				return;

			ms.SetLabel(cb(this));
		}
	}

	public class CMouseStatus
	{
		List<CMouseStatusIndex> Status = new List<CMouseStatusIndex>();
		ToolStripStatusLabel lb;

		public CMouseStatus(ToolStripStatusLabel _lb)
		{
			lb = _lb;
			lb.Text = "";
		}

		public void Add(Control _ctrl, GetMouseStatusDelegate _cb)
		{
			Status.Add(new CMouseStatusIndex(this, _ctrl, _cb));
		}

		public void Add(Control _ctrl, string _str)
		{
			Status.Add(new CMouseStatusIndex(this, _ctrl, _str));
		}

		public void SetLabel(string tx)
		{
			lb.Text = tx;
		}
	}
}

namespace Tao.OpenGl
{
	public class PGl
	{
		public static void glColor(Color c, byte alpha = 255)
		{
			Gl.glColor4ub(c.R, c.G, c.B, alpha);
		}

		public static void glVertex(VCMDL.NET.Vector3 v)
		{
			Gl.glVertex3f(v.x, v.y, v.z);
		}

		public delegate void DrawCircleVertices(double x, double y);

		public static void glCircle(double radius, int segments, DrawCircleVertices func)
		{
			for (int i = 0; i < segments; ++i)
			{
				double angle = i * 2 * Math.PI / segments;
				func((Math.Cos(angle) * radius), (Math.Sin(angle) * radius));

				angle = (i + 1) * 2 * Math.PI / segments;
				func((Math.Cos(angle) * radius), (Math.Sin(angle) * radius));
			}
		}
	}
};