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
	public partial class AddNewFrame : Form
	{
		public AddNewFrame()
		{
			InitializeComponent();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			using (GotoFrame gf = new GotoFrame(true))
			{
				gf.StartPosition = FormStartPosition.CenterParent;
				if (gf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					numericUpDown1.Value = gf.FrameNo;
			}
		}

		public void Setup()
		{
			numericUpDown1.Maximum = CMDLGlobals.g_CurMdl.Frames.Count + 3;
		}

		private void AddNewFrame_Load(object sender, EventArgs e)
		{
			numericUpDown2.Minimum = 1;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}
	}
}
