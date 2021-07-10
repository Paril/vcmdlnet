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
	public partial class GetPositionDialog : Form
	{
		public SkinVertPos GetSkinVertPos
		{
			get
			{
				SkinVertPos SVP;

				if (radioButton1.Checked)
					SVP = SkinVertPos.Front;
				else if (radioButton2.Checked)
					SVP = SkinVertPos.Back;
				else if (radioButton3.Checked)
					SVP = SkinVertPos.Right;
				else if (radioButton4.Checked)
					SVP = SkinVertPos.Left;
				else if (radioButton5.Checked)
					SVP = SkinVertPos.Top;
				else
					SVP = SkinVertPos.Bottom;

				return SVP;
			}
		}

		public GetPositionDialog()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}
	}
}
