using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
	public partial class MoveFrames : VCMDL.NET.MultiPageControl
	{
		public MoveFrames()
		{
			InitializeComponent();
		}

		static List<Control> Page1,
							 Page2;

		public static DataGridView framesToMove,
							placeToInsert;

		private void MoveFrames_Load(object sender, EventArgs e)
		{
			NumPages = 2;

			if (Page1 == null)
			{
				Page1 = new List<Control>();
				Page2 = new List<Control>();

				Label lb1 = new Label();
				lb1.TextAlign = ContentAlignment.MiddleCenter;
				lb1.Text = "Move frames...";
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
				lb2.Text = "Insert frames at...";
				lb2.Location = new Point(splitContainer1.Panel1.Size.Width / 2 - lb2.Width / 2, 12);

				Page2.Add(lb2);

				// 
				// dataGridView1
				// 
				Column1 = new DataGridViewTextBoxColumn();
				Column2 = new DataGridViewTextBoxColumn();
				dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
				dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
				dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
				dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();

				placeToInsert = new DataGridView();

				placeToInsert.AllowUserToAddRows = false;
				placeToInsert.AllowUserToDeleteRows = false;
				placeToInsert.AllowUserToResizeColumns = false;
				placeToInsert.AllowUserToResizeRows = false;
				dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
				dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
				dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
				dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
				placeToInsert.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
				placeToInsert.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
				placeToInsert.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            Column1,
            Column2});
				dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
				dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
				dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
				dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
				placeToInsert.DefaultCellStyle = dataGridViewCellStyle2;
				placeToInsert.MultiSelect = false;
				placeToInsert.Name = "dataGridView1";
				placeToInsert.ReadOnly = true;
				dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
				dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
				dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
				dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
				dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
				dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
				dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
				placeToInsert.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
				placeToInsert.RowHeadersVisible = false;
				placeToInsert.RowHeadersWidth = 8;
				dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
				placeToInsert.RowsDefaultCellStyle = dataGridViewCellStyle4;
				placeToInsert.Size = new System.Drawing.Size(225, 230);
				placeToInsert.Location = new Point(splitContainer1.Panel1.Size.Width / 2 - placeToInsert.Width / 2, 36);
				placeToInsert.TabIndex = 0;
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

				Page2.Add(placeToInsert);
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

		public void MoveFrames_PageChanged(object sender, MultiPageEventArgs e)
		{
			if (CurPage == 1)
			{
				placeToInsert.Rows.Clear();

				List<int> frames = DeleteFrames.GetFramesFromDataGridView(framesToMove);

				placeToInsert.Rows.Add(new object[] { "[Beginning]", -1 });
				placeToInsert.Rows[0].Height = 18;

				for (int i = 0, f = 0; i < CMDLGlobals.g_CurMdl.Frames.Count; ++i)
				{
					if (frames.Contains(i))
						continue;

					placeToInsert.Rows.Add(new object[] { CMDLGlobals.g_CurMdl.Frames[i].FrameName, f });
					placeToInsert.Rows[f].Height = 18;
					f++;
				}
			}
		}
	}
}
