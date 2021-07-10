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
	public partial class MultiPageControl : Form
	{
		int _NumPages = 1;
		public int NumPages
		{
			get
			{
				return _NumPages;
			}

			set
			{
				if (_NumPages != value)
					_PageControls = new List<Control>[value];

				_NumPages = value;
			}
		}

		int _CurPage = 0;
		public int CurPage
		{
			get
			{
				return _CurPage;
			}

			set
			{
				if (value > _NumPages - 1)
					value = _NumPages - 1;
				else if (value < 0)
					value = 0;

				if (_CurPage != value)
				{
					bool Next = (_CurPage > value);
					_CurPage = value;
					_PageChanged(Next);
				}
			}
		}

		List<Control>[] _PageControls = new List<Control>[1];
		public List<Control>[] PageControls
		{
			get
			{
				return _PageControls;
			}

			set
			{
				_PageControls = value;
			}
		}

		public class MultiPageEventArgs : EventArgs
		{
			bool _Next;
			public bool Next
			{
				get
				{
					return _Next;
				}

				set
				{
					_Next = value;
				}
			}
		}

		public delegate void MultiPageChangedEventHandler(object sender, MultiPageEventArgs e);

		public event MultiPageChangedEventHandler PageChanged;

		protected virtual void OnPageChanged(MultiPageEventArgs e)
		{
			if (PageChanged != null)
				PageChanged(this, e);
		}

		void _PageChanged(bool IsNext)
		{
			splitContainer1.Panel1.Controls.Clear();
			if (_PageControls[_CurPage] != null)
				splitContainer1.Panel1.Controls.AddRange(_PageControls[_CurPage].ToArray());

			button2.Text = "Next >";

			if (CurPage == 0)
				button1.Enabled = false;
			else
			{
				button1.Enabled = true;

				if (CurPage == NumPages - 1)
					button2.Text = "Done";
			}

			MultiPageEventArgs args = new MultiPageEventArgs();
			args.Next = IsNext;
			OnPageChanged(args);
		}

		public MultiPageControl()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			CurPage--;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (CurPage == NumPages - 1)
			{
				DialogResult = System.Windows.Forms.DialogResult.OK;

				CurPage++;
				OnPageChanged(new MultiPageEventArgs());

				Close();
			}
			else
				CurPage++;
		}
	}
}
