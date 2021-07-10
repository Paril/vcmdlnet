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
	public partial class ResizeSkin : Form
	{
		public ResizeSkin()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
		}
	}
}
