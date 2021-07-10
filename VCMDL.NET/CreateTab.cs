using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VCMDL.NET
{
    public partial class CreateTab : UserControl
    {
        public CreateTab()
        {
            InitializeComponent();
        }

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (Program.Ctrl_CommonToolbox.SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.CreateVertex;
			CommonToolbox.UpdateBoxActionType();
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (Program.Ctrl_CommonToolbox.SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.BuildFace;
			CommonToolbox.UpdateBoxActionType();
		}
    }
}
