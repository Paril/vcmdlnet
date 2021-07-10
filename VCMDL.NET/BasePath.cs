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
	public partial class BasePath : Form
	{
		public BasePath()
		{
			InitializeComponent();
		}

		private void BasePath_Load(object sender, EventArgs e)
		{
			textBox1.Text = CMDLGlobals.QuakeDataDir;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BasePath_FormClosing(object sender, FormClosingEventArgs e)
		{
			CMDLGlobals.QuakeDataDir = textBox1.Text;

			if (CMDLGlobals.QuakeDataDir.EndsWith("\\"))
				CMDLGlobals.QuakeDataDir += '\\';

			CMDLGlobals.QuakeDataDir = CMDLGlobals.QuakeDataDir.Replace('/', '\\');
		}
	}
}
