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
	public partial class GotoFrame : Form
	{
		public GotoFrame()
		{
			InitializeComponent();
		}

		bool addFirst = false;
		public GotoFrame(bool p)
		{
			addFirst = p;

			InitializeComponent();
		}

		private void GotoFrame_Load(object sender, EventArgs e)
		{
			if (addFirst)
			{
				dataGridView1.Rows.Add(new object[] { "[Beginning]", -1 });
				dataGridView1.Rows[0].Height = 18;
			}

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Frames.Count; ++i)
			{
				dataGridView1.Rows.Add(new object[] { CMDLGlobals.g_CurMdl.Frames[i].FrameName, i });
				dataGridView1.Rows[i].Height = 18;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			Close();
		}

		public int FrameNo;
		private void button1_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedCells[0].ColumnIndex == 0)
				FrameNo = (int)dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[1].Value;
			else
				FrameNo = (int)dataGridView1.SelectedCells[0].Value;

			FrameNo++;

			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			button1_Click(null, null);
		}
	}
}
