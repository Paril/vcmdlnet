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
    public partial class ModelTab : UserControl
    {
        public ModelTab()
        {
            InitializeComponent();
        }

		private void button2_Click(object sender, EventArgs e)
		{
			Vector3 centre = CMDLGlobals.g_CurMdl.GetSelectionCentre(CMDLGlobals.g_ModelSelectType);
			List<CSelectedVertice> Selected = ModelEditor.RetrieveSelectedVertices();

			switch (CMDLGlobals.g_ModelSelectType)
			{
			case ESelectType.Vertex:
				foreach (CSelectedVertice sel in Selected)
				{
					CVerticeFrameData fd = CMDLGlobals.g_CurMdl.Meshes[sel.Mesh].Verts[sel.Index].FrameData[CMDLGlobals.g_CurFrame];

					if ((CMDLGlobals.g_Axis & EAxis.Z) != 0)
						fd.Position.x = -fd.Position.x + centre.x + centre.x;
					if ((CMDLGlobals.g_Axis & EAxis.X) != 0)
						fd.Position.y = -fd.Position.y + centre.y + centre.y;
					if ((CMDLGlobals.g_Axis & EAxis.Y) != 0)
						fd.Position.z = -fd.Position.z + centre.z + centre.z;
				}
				break;
			case ESelectType.Face:
				foreach (CSelectedVertice sel in Selected)
				{
					CVerticeFrameData fd = CMDLGlobals.g_CurMdl.Meshes[sel.Mesh].Verts[sel.Index].FrameData[CMDLGlobals.g_CurFrame];
					if ((CMDLGlobals.g_Axis & EAxis.Z) != 0)
						fd.Position.x = -fd.Position.x + centre.x + centre.x;
					if ((CMDLGlobals.g_Axis & EAxis.X) != 0)
						fd.Position.y = -fd.Position.y + centre.y + centre.y;
					if ((CMDLGlobals.g_Axis & EAxis.Y) != 0)
						fd.Position.z = -fd.Position.z + centre.z + centre.z;
				}
				break;
			}

			CMDLGlobals.g_CurMdl.CalcAllNormals();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.DeleteSelected();
			Program.Form_ModelEditor.RedrawAllViews();
		}
    }
}
