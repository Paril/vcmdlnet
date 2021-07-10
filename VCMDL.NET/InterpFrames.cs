using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public partial class InterpFrames : VCMDL.NET.MultiPageControl
	{
		public InterpFrames()
		{
			InitializeComponent();
		}

		static List<Control> Page1,
							 Page2;

		public static DataGridView framesToMove;

		private void MoveFrames_Load(object sender, EventArgs e)
		{
			NumPages = 2;

			if (Page1 == null)
			{
				Page1 = new List<Control>();
				Page2 = new List<Control>();

				Label lb1 = new Label();
				lb1.TextAlign = ContentAlignment.MiddleCenter;
				lb1.Text = "Select frames to interpolate";
				lb1.Width = 150;
				lb1.Location = new Point(splitContainer1.Panel1.Size.Width / 2 - lb1.Width / 2, 12);

				Page1.Add(lb1);

				framesToMove = new DataGridView();

				// 
				// dataGridView1
				// 
				System.Windows.Forms.DataGridViewTextBoxColumn Column1 = new DataGridViewTextBoxColumn();
				System.Windows.Forms.DataGridViewTextBoxColumn Column2 = new DataGridViewTextBoxColumn();
				System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
				System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
				System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
				System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();

				framesToMove.AllowUserToAddRows = false;
				framesToMove.AllowUserToDeleteRows = false;
				framesToMove.AllowUserToResizeColumns = false;
				framesToMove.AllowUserToResizeRows = false;
				dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
				dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
				dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
				dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
				framesToMove.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
				framesToMove.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
				framesToMove.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            Column1,
            Column2});
				dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
				dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
				dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
				dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
				framesToMove.DefaultCellStyle = dataGridViewCellStyle2;
				framesToMove.MultiSelect = true;
				framesToMove.Name = "dataGridView1";
				framesToMove.ReadOnly = true;
				dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
				dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
				dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
				dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
				framesToMove.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
				framesToMove.RowHeadersVisible = false;
				framesToMove.RowHeadersWidth = 8;
				dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
				framesToMove.RowsDefaultCellStyle = dataGridViewCellStyle4;
				framesToMove.Size = new System.Drawing.Size(225, 230);
				framesToMove.Location = new Point(splitContainer1.Panel1.Size.Width / 2 - framesToMove.Width / 2, 36);
				framesToMove.TabIndex = 0;
//				framesToMove.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
				// 
				// Column1
				// 
				Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
				Column1.HeaderText = "Frame Name";
				Column1.Name = "Column1";
				Column1.ReadOnly = true;
				// 
				// Column2
				// 
				Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
				Column2.HeaderText = "#";
				Column2.Name = "Column2";
				Column2.ReadOnly = true;

				Page1.Add(framesToMove);

				Label lb2 = new Label();
				lb2.TextAlign = ContentAlignment.MiddleCenter;
				lb2.Text = "Data";
				lb2.Width = 150;
				lb2.Location = new Point(splitContainer1.Panel1.Size.Width / 2 - lb2.Width / 2, 12);

				Page2.Add(lb2);

				Label lb3 = new Label();
				lb3.TextAlign = ContentAlignment.MiddleCenter;
				lb3.Text = "Amount of frames\nto Interpolate:";
				lb3.AutoSize = true;
				lb3.Location = new Point(24, 48);

				Page2.Add(lb3);

				nud1 = new NumericUpDown();
				nud1.Minimum = 1;
				nud1.Maximum = 256;
				nud1.Value = 1;
				nud1.Width = 52;
				nud1.Location = new Point(186, 52);

				Page2.Add(nud1);
			}

			framesToMove.Rows.Clear();

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Frames.Count; ++i)
			{
				framesToMove.Rows.Add(new object[] { CMDLGlobals.g_CurMdl.Frames[i].FrameName, i });
				framesToMove.Rows[i].Height = 18;
			}

			PageControls[0] = Page1;
			PageControls[1] = Page2;

			splitContainer1.Panel1.Controls.Clear();
			if (PageControls[CurPage] != null)
				splitContainer1.Panel1.Controls.AddRange(PageControls[CurPage].ToArray());
		
			button2.Focus();

			PageChanged += new MultiPageChangedEventHandler(MoveFrames_PageChanged);
		}

		public static NumericUpDown nud1;
		static public int numFramesToAdd = 0;
		public void MoveFrames_PageChanged(object sender, MultiPageEventArgs e)
		{
			numFramesToAdd = (int)nud1.Value;
		}
	}
}
