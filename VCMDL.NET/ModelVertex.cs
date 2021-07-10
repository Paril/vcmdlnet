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
    public partial class ModelVertexTab : UserControl
    {
		public ModelVertexTab()
        {
            InitializeComponent();
        }

		public void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.WeldSelected();
		}
    }
}
