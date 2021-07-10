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
	public partial class DeleteFrames : Form
	{
		public DeleteFrames()
		{
			InitializeComponent();
		}

		private void GotoFrame_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < CMDLGlobals.g_CurMdl.Frames.Count; ++i)
			{
				dataGridView1.Rows.Add(new object[] { CMDLGlobals.g_CurMdl.Frames[i].FrameName, i });
				dataGridView1.Rows[i].Height = 18;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Close();
		}

		public static List<int> GetFramesFromDataGridView(DataGridView dgv)
		{
			List<int> FrameIndices = new List<int>();

			for (int i = 0; i < dgv.SelectedCells.Count; ++i)
			{
				int FrameNo;

				if (dgv.SelectedCells[i].ColumnIndex == 0)
					FrameNo = (int)dgv.Rows[dgv.SelectedCells[i].RowIndex].Cells[1].Value;
				else
					FrameNo = (int)dgv.SelectedCells[i].Value;

				if (!FrameIndices.Contains(FrameNo))
					FrameIndices.Add(FrameNo);
			}

			FrameIndices.Sort();

			return FrameIndices;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			List<int> FrameIndices = GetFramesFromDataGridView(dataGridView1);
		
			if (FrameIndices.Count == CMDLGlobals.g_CurMdl.Frames.Count)
			{
				FrameIndices.Remove(0);
				MessageBox.Show("All frames have been selected; there must be at least one frame at all times. The first frame will be spared.");
			}

			if (FrameIndices.Count != 0)
				CMDLGlobals.g_CurMdl.DeleteFrames(FrameIndices);

			Program.Form_ModelEditor.ModelUpdated();
			Close();
		}
	}
}
