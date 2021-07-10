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
    public partial class BoneTab : UserControl
    {
		public BoneTab()
        {
            InitializeComponent();
        }

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (Program.Ctrl_CommonToolbox.SkipChanged)
				return;

			CMDLGlobals.g_MainActionMode = EActionType.CreateBone;
			CommonToolbox.UpdateBoxActionType();
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			int b = -1;

			for (int i = 0; i < CMDLGlobals.g_CurMdl.Bones.Count; ++i)
			{
				CBone bone = CMDLGlobals.g_CurMdl.Bones[i];
				if ((bone.Flags & EBoneFlags.Selected) != 0)
				{
					if (b != -1)
						return;

					b = i;
				}
			}

			/*foreach (CVertice v in CMDLGlobals.g_CurMdl.Verts)
			{
				if ((v.Flags & EVerticeFlags.Selected) != 0)
				{
					v.Bone = b;
					v.BoneBasePosition = v.FrameData[CMDLGlobals.g_CurFrame].Position;
					v.BoneBaseAngles = CMDLGlobals.g_CurMdl.Bones[b].Angles;
				}
			}*/
		}
    }
}
