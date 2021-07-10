using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
    public partial class CommonToolbox : UserControl
    {
        public CommonToolbox()
        {
            InitializeComponent();
        }

		public bool SkipChanged = false;

		public static void UpdateBoxActionType()
		{
			Program.Ctrl_CommonToolbox.SkipChanged = true;

			Program.Ctrl_CommonToolbox.checkBox1.Checked = false;
			Program.Ctrl_CommonToolbox.checkBox2.Checked = false;
			Program.Ctrl_CommonToolbox.checkBox3.Checked = false;
			Program.Ctrl_CommonToolbox.checkBox4.Checked = false;

			ModelEditor.theCreateTab.checkBox1.Checked = false;
			ModelEditor.theCreateTab.checkBox2.Checked = false;
			//ModelEditor.theBoneTab.checkBox1.Checked = false;

			if ((CMDLGlobals.g_MainActionMode & EActionType.Pan) == 0)
			{
				switch (CMDLGlobals.g_MainActionMode)
				{
				case EActionType.Select:
					Program.Ctrl_CommonToolbox.checkBox1.Checked = true;
					break;
				case EActionType.Move:
					Program.Ctrl_CommonToolbox.checkBox2.Checked = true;
					break;
				case EActionType.Rotate:
					Program.Ctrl_CommonToolbox.checkBox3.Checked = true;
					break;
				case EActionType.Scale:
					Program.Ctrl_CommonToolbox.checkBox4.Checked = true;
					break;
				case EActionType.CreateVertex:
					ModelEditor.theCreateTab.checkBox1.Checked = true;
					break;
				case EActionType.BuildFace:
				case EActionType.BuildingFace1:
				case EActionType.BuildingFace2:
					ModelEditor.theCreateTab.checkBox2.Checked = true;
					break;
				case EActionType.CreateBone:
					//ModelEditor.theBoneTab.checkBox1.Checked = true;
					break;
				}
			}

			Program.Ctrl_CommonToolbox.SkipChanged = false;
			Program.Form_ModelEditor.UpdateModelSelectionType();
			Program.Form_ModelEditor.SetCursors();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.Select;
			UpdateBoxActionType();
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.Move;
			UpdateBoxActionType();
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.Rotate;
			UpdateBoxActionType();
		}

		private void checkBox4_CheckedChanged(object sender, EventArgs e)
		{
			if (SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.Scale;
			UpdateBoxActionType();
		}

		private void checkBox5_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox5.Checked)
				CMDLGlobals.g_Axis |= EAxis.X;
			else
				CMDLGlobals.g_Axis &= ~EAxis.X;
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox6.Checked)
				CMDLGlobals.g_Axis |= EAxis.Y;
			else
				CMDLGlobals.g_Axis &= ~EAxis.Y;
		}

		private void checkBox7_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox7.Checked)
				CMDLGlobals.g_Axis |= EAxis.Z;
			else
				CMDLGlobals.g_Axis &= ~EAxis.Z;
		}
    }
}
