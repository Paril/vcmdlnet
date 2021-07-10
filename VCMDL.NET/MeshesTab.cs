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
    public partial class MeshesTab : UserControl
    {
		public MeshesTab()
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

		public void RefreshMeshes()
		{
			listBox1.BeginUpdate();

			CMesh mesh = (CMesh)listBox1.SelectedItem;

			listBox1.Items.Clear();

			foreach (var m in CMDLGlobals.g_CurMdl.Meshes)
			{
				int i = listBox1.Items.Add(m);

				if (mesh == m)
					listBox1.SelectedIndex = i;
			}

			listBox1.EndUpdate();
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
