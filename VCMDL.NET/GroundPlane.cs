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
	public partial class GroundPlane : Form
	{
		public GroundPlane()
		{
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void GroundPlane_Load(object sender, EventArgs e)
		{
			textBox1.Text = CMDLGlobals.g_PlanePosition.x.ToString();
			textBox2.Text = CMDLGlobals.g_PlanePosition.y.ToString();
			textBox3.Text = CMDLGlobals.g_PlanePosition.z.ToString();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_PlanePosition.x = float.Parse(textBox1.Text);
			CMDLGlobals.g_PlanePosition.y = float.Parse(textBox2.Text);
			CMDLGlobals.g_PlanePosition.z = float.Parse(textBox3.Text);

			Close();
		}
	}
}
