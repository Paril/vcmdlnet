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
	public partial class ChangeGrid : Form
	{
		public ChangeGrid()
		{
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		public void SetValues(int Size, int Slices)
		{
			numericUpDown1.Value = Size;
			numericUpDown2.Value = Slices;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_GridSize = (int)numericUpDown1.Value;
			CMDLGlobals.g_GridSlices = (int)numericUpDown2.Value;

			Close();

			ModelEditor.InitGrid(CMDLGlobals.g_GridSize, CMDLGlobals.g_GridSlices);
			Program.Form_ModelEditor.RedrawAllViews();
		}
	}
}
