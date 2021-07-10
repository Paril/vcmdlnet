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
	public partial class BackgroundImage : Form
	{
		public Bitmap LoadedBitmap = null;
		public PointF Offset = new PointF(), FScale = new PointF(1.0f, 1.0f);
		public string LoadedBitmapFileName = "";

		public BackgroundImage()
		{
			InitializeComponent();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ChooseSkinDlg = new OpenFileDialog())
			{
				ChooseSkinDlg.DefaultExt = "png";
				ChooseSkinDlg.Filter = "Supported image files (*.pcx;*.png;*.bmp;*.jpg;*.tga)|*.pcx;*.png;*.bmp;*.jpg;*.tga";

				if (ChooseSkinDlg.ShowDialog() == DialogResult.Cancel)
					return;

				textBox1.Text = ChooseSkinDlg.FileName;
				LoadedBitmap = SkinEditor.LoadTexture(ChooseSkinDlg.FileName);
				LoadedBitmapFileName = ChooseSkinDlg.FileName;
			}
		}

		public void SetupData(string p, Bitmap bitmap, PointF pointF, PointF pointF_2)
		{
			LoadedBitmapFileName = p;
			LoadedBitmap = bitmap;
			Offset = pointF;
			FScale = pointF_2;

			textBox1.Text = LoadedBitmapFileName;
			textBox2.Text = Offset.X.ToString();
			textBox3.Text = Offset.Y.ToString();
			textBox4.Text = FScale.X.ToString();
			textBox5.Text = FScale.Y.ToString();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;

			Offset = new PointF(float.Parse(textBox2.Text), float.Parse(textBox3.Text));
			FScale = new PointF(float.Parse(textBox4.Text), float.Parse(textBox5.Text));

			Close();
		}
	}
}
