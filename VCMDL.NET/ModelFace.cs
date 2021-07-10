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
    public partial class ModelFaceTab : UserControl
    {
		public ModelFaceTab()
        {
            InitializeComponent();
        }

		private void button1_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.FlipNormals();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.UnifyNormals();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.Subdivide();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.TurnEdge();
			CMDLGlobals.g_CurMdl.CalcAllNormals();
			Program.Form_ModelEditor.RedrawAllViews();
		}

		ExtrudeForm theExtrudeForm = new ExtrudeForm();

		private void button5_Click(object sender, EventArgs e)
		{
			theExtrudeForm.StartPosition = FormStartPosition.CenterParent;
			theExtrudeForm.ShowDialog();
		}

		static public int[] GetQuadVerts (int[] tri1, int[] tri2)
		{
			int[] verts = new int[4] {-1, -1, -1, -1};

			for (int i = 0; i < 3; ++i)
			{
				for (int z = 0; z < 3; ++z)
				{
					if (tri1[i] == tri2[z])
					{
						if (verts[0] == -1 && verts[0] != tri1[i])
							verts[0] = tri1[i];
						else if (verts[1] != tri1[i])
						{
							verts[1] = tri1[1];
							break;
						}
					}
				}
			}

			for (int i = 0; i < 3; ++i)
			{
				if (tri1[i] != verts[0] && tri1[i] != verts[1])
				{
					verts[2] = tri1[i];
					break;
				}
			}

			for (int i = 0; i < 3; ++i)
			{
				if (tri2[i] != verts[0] && tri2[i] != verts[1])
				{
					verts[3] = tri2[i];
					break;
				}
			}

			return verts;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			CMDLGlobals.g_CurMdl.Detach();
			Program.Form_ModelEditor.ModelUpdated();
		}
    }
}
