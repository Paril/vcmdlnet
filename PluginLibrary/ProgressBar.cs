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
	public partial class ProgressBar : Form
	{
		public ProgressBar()
		{
			InitializeComponent();
			Reset(100);
			TopMost = true;
		}

		public void Reset(int maxVal)
		{
			progressBar1.Value = 0;
			progressBar1.Maximum = maxVal;
		}

		public void Tick()
		{
			progressBar1.Value++;
		}

		public void Done()
		{
			Reset(100);
			Close();
		}

		public class ProgressBarBackgroundWorker : BackgroundWorker
		{
			ProgressBar Bar;

			public ProgressBarBackgroundWorker()
				: base()
			{
				Bar = new ProgressBar();
				Bar.Show();
				WorkerReportsProgress = true;
				ProgressChanged += PBBW_ReportProgress;
			}

			void PBBW_ReportProgress(object sender, ProgressChangedEventArgs e)
			{
				if ((int)e.UserState == 0)
					Bar.Reset(e.ProgressPercentage);
				else
					Bar.Tick();
			}

			public void Tick()
			{
				ReportProgress(0, (object)1);
			}

			public void Reset(int maxVal)
			{
				ReportProgress(maxVal, (object)0);
			}

			public void Done()
			{
				Bar.Done();
			}
		}

		public static ProgressBarBackgroundWorker OpenProgressBar(int maxVal)
		{
			ProgressBarBackgroundWorker work = new ProgressBarBackgroundWorker();
			work.Reset(maxVal);
			return work;
		}
	}
}
