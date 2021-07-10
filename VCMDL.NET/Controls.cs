using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public class CKeyControls
	{
		public delegate void ControlCallback();

		public class CKeyControl
		{
			public struct CKey
			{
				public static CKey Empty = new CKey(0, 0);

				public Keys Key,
												 Modifiers;

				public CKey(Keys _Key, Keys _Modifiers = 0)
				{
					Key = _Key;
					Modifiers = _Modifiers;
				}

				public bool IsKey(Keys _Key, Keys _Modifiers)
				{
					return (Key == _Key && Modifiers == _Modifiers);
				}

				public static bool operator==(CKey Left, CKey Right)
				{
					return Left.IsKey(Right.Key, Right.Modifiers);
				}

				public static bool operator!=(CKey Left, CKey Right)
				{
					return !(Left == Right);
				}

				public override bool Equals(object obj)
				{
					return base.Equals(obj);
				}

				public override int GetHashCode()
				{
					return base.GetHashCode();
				}
			}

			public static CKeyControl CurrentControl = null;
			public int ID; // The identifier
			public string IniID; // id for ini file
			public ToolStripMenuItem Control; // control that changes text (can be null)
			public string Description; // description
			public CKey Key; // current key
			public CKey DefaultKey; // default key
			public ControlCallback CallbackFunction; // the function

			public void Callback()
			{
				CurrentControl = this;
				CallbackFunction();
				CurrentControl = null;
			}

			public CKeyControl(int _ID, string _IniID, ToolStripMenuItem _Control, string _Description, CKey _Key, CKey _DefaultKey, ControlCallback _Callback)
			{
				ID = _ID;
				IniID = _IniID;
				Control = _Control;
				Description = _Description;
				Key = _Key;
				DefaultKey = _DefaultKey;
				CallbackFunction = _Callback;
			}
		}

		public class CKeyControlList
		{
			Dictionary<int, CKeyControl> Controls = new Dictionary<int,CKeyControl>();
			Dictionary<CKeyControl.CKey, CKeyControl> ControlsKeySorted = new Dictionary<CKeyControl.CKey, CKeyControl>();
			Dictionary<string, CKeyControl> ControlsIniSorted = new Dictionary<string, CKeyControl>();

			public void AddControl(CKeyControl ctrl)
			{
				Controls.Add(ctrl.ID, ctrl);
			}

			public void AddControl<TEnum>(TEnum ID, ToolStripMenuItem Control, string Description, ControlCallback Callback, CKeyControl.CKey Key)
			{
				AddControl(new CKeyControl((int)(object)ID, Enum.GetName(typeof(TEnum), ID), Control, Description, Key, Key, Callback));
			}

			public void AddControl(int ID, string IniID, ToolStripMenuItem Control, string Description, ControlCallback Callback, CKeyControl.CKey Key)
			{
				AddControl(new CKeyControl(ID, IniID, Control, Description, Key, Key, Callback));
			}

			public CKeyControl GetControl(int ID)
			{
				return Controls[ID];
			}

			public CKeyControl GetControl(string IniID)
			{
				if (!ControlsIniSorted.ContainsKey(IniID))
					return null;

				return ControlsIniSorted[IniID];
			}

			public CKeyControl GetControl(CKeyControl.CKey Key)
			{
				if (!ControlsKeySorted.ContainsKey(Key))
					return null;

				return ControlsKeySorted[Key];
			}

			static public string KeycodeToChar(Keys keyCode)
			{
				Keys key = keyCode;

				switch (key)
				{
				case Keys.Add:
					return "+";
				case Keys.Decimal:
					return ".";
				case Keys.Divide:
					return "/";
				case Keys.Multiply:
					return "*";
				case Keys.OemBackslash:
					return "\\";
				case Keys.OemCloseBrackets:
					return "]";
				case Keys.OemMinus:
					return "-";
				case Keys.OemOpenBrackets:
					return "[";
				case Keys.OemPeriod:
					return ".";
				case Keys.OemPipe:
					return "|";
				case Keys.OemQuestion:
					return "/";
				case Keys.OemQuotes:
					return "\"";
				case Keys.OemSemicolon:
					return ";";
				case Keys.Oemcomma:
					return ",";
				case Keys.Oemplus:
					return "+";
				case Keys.Oemtilde:
					return "`";
				case Keys.Separator:
					return "-";
				case Keys.Subtract:
					return "-";
				case Keys.D0:
					return "0";
				case Keys.D1:
					return "1";
				case Keys.D2:
					return "2";
				case Keys.D3:
					return "3";
				case Keys.D4:
					return "4";
				case Keys.D5:
					return "5";
				case Keys.D6:
					return "6";
				case Keys.D7:
					return "7";
				case Keys.D8:
					return "8";
				case Keys.D9:
					return "9";
				case Keys.Space:
					return " ";
				default:
					return key.ToString();
				}
			}

			public void Update()
			{
				foreach (KeyValuePair<int, CKeyControl> ctrl in Controls)
				{
					if (ctrl.Value.Control == null)
						continue;

					foreach (KeyValuePair<int, CKeyControl> ctrl2 in Controls)
					{
						if (ctrl2.Value.Control == null)
							continue;

						if ((ctrl.Value != ctrl2.Value) && ctrl.Value.Control == ctrl2.Value.Control)
							throw new Exception("Two controls shared between two controls!");
					}
				}

				ControlsKeySorted.Clear();
				ControlsIniSorted.Clear();

				foreach (KeyValuePair<int, CKeyControl> ctrl in Controls)
				{
					if (ctrl.Value.Key != CKeyControl.CKey.Empty)
						ControlsKeySorted.Add(ctrl.Value.Key, ctrl.Value);
					ControlsIniSorted.Add(ctrl.Value.IniID, ctrl.Value);

					if (ctrl.Value.Control != null)
					{
						ctrl.Value.Control.ShowShortcutKeys = true;
						ctrl.Value.Control.ShortcutKeyDisplayString = "";
						if (ctrl.Value.Key != CKeyControl.CKey.Empty)
						{
							string ModifierString = "";

							if ((ctrl.Value.Key.Modifiers & Keys.Control) != 0)
								ModifierString += "Ctrl+";
							if ((ctrl.Value.Key.Modifiers & Keys.Alt) != 0)
								ModifierString += "Alt+";
							if ((ctrl.Value.Key.Modifiers & Keys.Shift) != 0)
								ModifierString += "Shift+";

							ctrl.Value.Control.ShortcutKeyDisplayString = ModifierString + KeycodeToChar(ctrl.Value.Key.Key);
						}
					}
				}
			}

			public void WriteToSection(StreamWriter sw, string section)
			{
				sw.WriteLine("[" + section + "]");
				
				foreach (var ctrl in Controls)
				{
					bool Ctrl = (ctrl.Value.Key.Modifiers & Keys.Control) != 0;
					bool Shift = (ctrl.Value.Key.Modifiers & Keys.Shift) != 0;
					bool Alt = (ctrl.Value.Key.Modifiers & Keys.Alt) != 0;

					string Modifiers = " | ";
					if (!Ctrl && !Shift && !Alt)
						Modifiers = "";
					else
					{
						if (Ctrl)
							Modifiers += "C";
						if (Shift)
							Modifiers += "S";
						if (Alt)
							Modifiers += "A";
					}

					sw.WriteLine(ctrl.Value.IniID + "=" + Enum.GetName(typeof(Keys), (object)ctrl.Value.Key.Key) + Modifiers);
				}
			}
		}
	}

	public partial class ModelEditor : Form
	{
		public CKeyControls.CKeyControlList ControlList = new CKeyControls.CKeyControlList();

		public enum EControlIDs
		{
			SelectAll,
			SelectNone,
			SelectInverse,
			SelectConnected,
			SelectTouching,
			TogglePan,
			ToggleX,
			ToggleY,
			ToggleZ,
			ShowVerticeTicks,
			ShowGrid,
			ShowOrigin,
			Undo,
			Redo,
			Copy,
			Paste,
			PasteToRange,
			GotoFrame,
			AddNewFrame,
			DeleteCurrentFrame,
			DeleteFrames,
			MoveFrames,
			Weld,
			New,
			Open,
			Save,
			SaveAs,
			Merge,

			Max
		}

		public void Do_SelectAll()
		{
			selectAllToolStripMenuItem_Click(null, null);
		}

		public void Do_SelectNone()
		{
			selectNoneToolStripMenuItem_Click(null, null);
		}

		public void Do_SelectInverse()
		{
			selectInverseToolStripMenuItem_Click(null, null);
		}

		public void Do_SelectConnected()
		{
			selectConnectedToolStripMenuItem_Click(null, null);
		}

		public void Do_SelectTouching()
		{
			selectTouchingToolStripMenuItem_Click(null, null);
		}

		public void Do_TogglePan()
		{
			button9.Checked = !button9.Checked;
		}

		public void Do_ToggleX()
		{
			Program.Ctrl_CommonToolbox.checkBox5.Checked = !Program.Ctrl_CommonToolbox.checkBox5.Checked;
		}

		public void Do_ToggleY()
		{
			Program.Ctrl_CommonToolbox.checkBox6.Checked = !Program.Ctrl_CommonToolbox.checkBox6.Checked;
		}

		public void Do_ToggleZ()
		{
			Program.Ctrl_CommonToolbox.checkBox7.Checked = !Program.Ctrl_CommonToolbox.checkBox7.Checked;
		}

		public void Do_ShowVerticeTicks()
		{
			ViewportControl vc = ControlFromViewPort(CMDLGlobals.g_SelectedViewport);

			vc.ControlData.Flags ^= EControlDataFlags.ShowVerticeTicks;
			vc.Invalidate();
		}

		public void Do_ShowGrid()
		{
			ViewportControl vc = ControlFromViewPort(CMDLGlobals.g_SelectedViewport);

			vc.ControlData.Flags ^= EControlDataFlags.ShowGrid;
			vc.Invalidate();
		}

		public void Do_ShowOrigin()
		{
			ViewportControl vc = ControlFromViewPort(CMDLGlobals.g_SelectedViewport);

			vc.ControlData.Flags ^= EControlDataFlags.ShowAxis;
			vc.Invalidate();
		}

		public void Do_Undo()
		{
		}

		public void Do_Redo()
		{
		}

		public void Do_Copy()
		{
		}

		public void Do_Paste()
		{
		}

		public void Do_PasteToRange()
		{
		}

		public void Do_GotoFrame()
		{
			gotoFrameToolStripMenuItem_Click(null, null);
		}

		public void Do_AddNewFrame()
		{
			addNewFrameToolStripMenuItem_Click(null, null);
		}

		public void Do_DeleteCurrentFrame()
		{
			deleteCurrentFrameToolStripMenuItem_Click(null, null);
		}

		public void Do_DeleteFrames()
		{
			deleteFramesToolStripMenuItem_Click(null, null);
		}

		public void Do_MoveFrames()
		{
			moveFramesToolStripMenuItem_Click(null, null);
		}

		public void Do_Weld()
		{
			theModelVertexTab.button1_Click(null, null);
		}

		public void Do_Plugin()
		{
			int moduleIndex = CKeyControls.CKeyControl.CurrentControl.ID - ((int)EControlIDs.Max);
			int modI = 0;

			foreach (var m in CModules.Modules)
			{
				foreach (var mod in m.Plugins)
				{
					modI++;
					if (modI == moduleIndex)
					{
						mod.Execute(CMDLGlobals.g_CurMdl);
						return;
					}
				}
			}
		}

		public void Do_New()
		{
			newToolStripMenuItem_Click(null, null);
		}

		public void Do_Open()
		{
			openToolStripMenuItem_Click(null, null);
		}

		public void Do_Save()
		{
			saveToolStripMenuItem_Click(null, null);
		}

		public void Do_SaveAs()
		{
			saveAsToolStripMenuItem_Click(null, null);
		}

		public void Do_Merge()
		{
			mergeToolStripMenuItem_Click(null, null);
		}

		public void InitControls()
		{
			ControlList.AddControl<EControlIDs>(
				EControlIDs.SelectAll,
				selectAllToolStripMenuItem,
				"Select all vertices or faces",
				Do_SelectAll,
				new CKeyControls.CKeyControl.CKey(Keys.A)
				);

			ControlList.AddControl<EControlIDs>(
				EControlIDs.SelectNone,
				selectNoneToolStripMenuItem,
				"Unselects all vertices or faces",
				Do_SelectNone,
				new CKeyControls.CKeyControl.CKey(Keys.OemQuestion)
				);

			ControlList.AddControl<EControlIDs>(
				EControlIDs.SelectInverse,
				selectInverseToolStripMenuItem,
				"Inverts selection of vertices or faces",
				Do_SelectInverse,
				new CKeyControls.CKeyControl.CKey(Keys.I)
				);

			ControlList.AddControl<EControlIDs>(
				EControlIDs.SelectConnected,
				selectConnectedToolStripMenuItem,
				"Selects all vertices/faces connected to this one",
				Do_SelectConnected,
				new CKeyControls.CKeyControl.CKey(Keys.OemCloseBrackets)
				);

			ControlList.AddControl<EControlIDs>(
				EControlIDs.SelectTouching,
				selectTouchingToolStripMenuItem,
				"Selects all vertices/faces touching this one",
				Do_SelectTouching,
				new CKeyControls.CKeyControl.CKey(Keys.OemOpenBrackets)
				);

			ControlList.AddControl<EControlIDs>(
				EControlIDs.TogglePan,
				null,
				"Toggles pan mode",
				Do_TogglePan,
				new CKeyControls.CKeyControl.CKey(Keys.P)
				);

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ToggleX,
			   null,
			   "Toggles x axis",
			   Do_ToggleX,
			   new CKeyControls.CKeyControl.CKey(Keys.X)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ToggleY,
			   null,
			   "Toggles y axis",
			   Do_ToggleY,
			   new CKeyControls.CKeyControl.CKey(Keys.Y)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ToggleZ,
			   null,
			   "Toggles z axis",
			   Do_ToggleZ,
			   new CKeyControls.CKeyControl.CKey(Keys.Z)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ShowVerticeTicks,
			   showVerticeTicksToolStripMenuItem,
			   "Toggle showing vertice ticks on selected viewport",
			   Do_ShowVerticeTicks,
			   new CKeyControls.CKeyControl.CKey(Keys.T)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ShowOrigin,
			   showOriginToolStripMenuItem1,
			   "Toggle showing the origin axis on selected viewport",
			   Do_ShowOrigin,
			   new CKeyControls.CKeyControl.CKey(Keys.O)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.ShowGrid,
			   showGridToolStripMenuItem1,
			   "Toggle showing the grid on selected viewport",
			   Do_ShowGrid,
			   new CKeyControls.CKeyControl.CKey(Keys.G)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Undo,
			   undoToolStripMenuItem,
			   "Undo",
			   Do_Undo,
			   new CKeyControls.CKeyControl.CKey(Keys.Z, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Redo,
			   redoToolStripMenuItem,
			   "Redo",
			   Do_Redo,
			   new CKeyControls.CKeyControl.CKey(Keys.Y, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Copy,
			   copySelectedToolStripMenuItem,
			   "Copy selected",
			   Do_Copy,
			   new CKeyControls.CKeyControl.CKey(Keys.C, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Paste,
			   pasteToolStripMenuItem,
			   "Paste",
			   Do_Paste,
			   new CKeyControls.CKeyControl.CKey(Keys.P, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.PasteToRange,
			   pasteToRangeToolStripMenuItem,
			   "Paste to a range of frames",
			   Do_PasteToRange,
			   new CKeyControls.CKeyControl.CKey(Keys.P, Keys.Control|Keys.Shift)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.GotoFrame,
			   gotoFrameToolStripMenuItem,
			   "Go to a specific frame",
			   Do_GotoFrame,
			   new CKeyControls.CKeyControl.CKey(Keys.G, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.AddNewFrame,
			   addNewFrameToolStripMenuItem,
			   "Add new frames",
			   Do_AddNewFrame,
			   new CKeyControls.CKeyControl.CKey(Keys.I, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.DeleteCurrentFrame,
			   deleteCurrentFrameToolStripMenuItem,
			   "Delete current frame",
			   Do_DeleteCurrentFrame,
			   new CKeyControls.CKeyControl.CKey(Keys.D, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.DeleteFrames,
			   deleteFramesToolStripMenuItem,
			   "Delete frames",
			   Do_DeleteFrames,
			   new CKeyControls.CKeyControl.CKey(Keys.D, Keys.Control|Keys.Shift)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.MoveFrames,
			   moveFramesToolStripMenuItem,
			   "Move frames",
			   Do_DeleteFrames,
			   new CKeyControls.CKeyControl.CKey(Keys.M, Keys.Control|Keys.Shift)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Weld,
			   null,
			   "Weld vertices",
			   Do_Weld,
			   new CKeyControls.CKeyControl.CKey(Keys.W)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.New,
			   newToolStripMenuItem,
			   "New",
			   Do_New,
			   new CKeyControls.CKeyControl.CKey(Keys.N, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Open,
			   openToolStripMenuItem,
			   "Open",
			   Do_Open,
			   new CKeyControls.CKeyControl.CKey(Keys.O, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Save,
			   saveToolStripMenuItem,
			   "Save",
			   Do_Save,
			   new CKeyControls.CKeyControl.CKey(Keys.S, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.SaveAs,
			   saveAsToolStripMenuItem,
			   "Save As",
			   Do_SaveAs,
			   new CKeyControls.CKeyControl.CKey(Keys.A, Keys.Control)
			   );

			ControlList.AddControl<EControlIDs>(
			   EControlIDs.Merge,
			   mergeToolStripMenuItem,
			   "Merge",
			   Do_Merge,
			   new CKeyControls.CKeyControl.CKey(Keys.M, Keys.Control)
			   );

			int modIndex = 0;
			foreach (var mod in CModules.Modules)
			{
				foreach (var plugin in mod.Plugins)
				{
					ControlList.AddControl(
						((int)EControlIDs.Max) + ++modIndex,
						plugin.Plugin.ToString().Substring(plugin.Plugin.ToString().IndexOf('+')+1),
						plugin.MenuItem,
						plugin.MenuItem.Text,
						Do_Plugin,
						CKeyControls.CKeyControl.CKey.Empty);
				}
			}

			ControlList.Update();
			LoadControls();
			ControlList.Update();
		}

		public void LoadControls()
		{
			IniFile ini = new IniFile("editor_controls.ini");

			if (!ini.Valid())
			{
				using (FileStream fs = File.Open("editor_controls.ini", FileMode.Create))
				using (StreamWriter sw = new StreamWriter(fs))
					ControlList.WriteToSection(sw, "Editor Controls");
				return;
			}
			else
			{
				foreach (var ctrlkvp in ini.GetSection("Editor Controls"))
				{
					var ctrl = ControlList.GetControl(ctrlkvp.Key);

					if (ctrl != null)
					{
						string keyV = ctrlkvp.Value;
						string modifiers = "";

						if (keyV.Contains(" | "))
							modifiers = keyV.Substring(keyV.IndexOf(" | ") + 3);

						Keys realKey = (Keys)Enum.Parse(typeof(Keys), (modifiers.Length != 0) ? keyV.Substring(0, keyV.IndexOf(" | ")) : keyV);
						Keys modifierKeys = 0;

						if (modifiers.Contains('S'))
							modifierKeys |= Keys.Shift;
						if (modifiers.Contains('A'))
							modifierKeys |= Keys.Alt;
						if (modifiers.Contains('C'))
							modifierKeys |= Keys.Control;

						CKeyControls.CKeyControl.CKey Key = new CKeyControls.CKeyControl.CKey(realKey, modifierKeys);

						ctrl.Key = Key;
					}
				}
			}
		}

		protected override bool ProcessKeyPreview(ref System.Windows.Forms.Message m)
		{
			if (textBox1.Focused)
				return base.ProcessKeyPreview(ref m);

			if (m.Msg == (int)WM.KEYDOWN)
			{
				CKeyControls.CKeyControl ctrl = ControlList.GetControl(new CKeyControls.CKeyControl.CKey((System.Windows.Forms.Keys)m.WParam, ModifierKeys));

				if (ctrl != null)
				{
					ctrl.Callback();
					CMouseStatusIndex._lastStatusIndex = null;
				}
			}

			return base.ProcessKeyPreview(ref m);
		}
	}
}
