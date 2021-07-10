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
	public partial class ExtrudeForm : Form
	{
		public ExtrudeForm()
		{
			InitializeComponent();
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked == false)
				textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = true;
			else
				textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = false;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.ExtrudeSelected(float.Parse(textBox1.Text), checkBox1.Checked, new Vector3(float.Parse(textBox2.Text), float.Parse(textBox3.Text), float.Parse(textBox4.Text)));
			Program.Form_ModelEditor.RedrawAllViews();

			Close();
		}
	}
}
