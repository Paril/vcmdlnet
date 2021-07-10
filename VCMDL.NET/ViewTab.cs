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
    public partial class ViewTab : UserControl
    {
        public ViewTab()
        {
            InitializeComponent();
        }

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			ModelEditor.CAnimationDropDownItem Item = (ModelEditor.CAnimationDropDownItem)comboBox1.SelectedItem;

			numericUpDown2.Value = Item.StartFrame;
			numericUpDown3.Value = Item.EndFrame;
		}

		public void AddAnimation(ModelEditor.CAnimationDropDownItem Item)
		{
			comboBox1.Items.Add(Item);
		}

		public void ClearAnimations()
		{
			comboBox1.Items.Clear();
		}

		public bool IsInterpolating()
		{
			return checkBox2.Checked;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Program.Form_ModelEditor.AnimateClicked((int)numericUpDown2.Value, (int)numericUpDown3.Value, (int)numericUpDown1.Value);
		}
    }
}
