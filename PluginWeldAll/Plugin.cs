using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace VCMDL.NET
{
	public class Plugin
	{
		public class CWeldAllPlugin : CPlugin
		{
			public CWeldAllPlugin()
				: base(EPluginType.PLUGIN_TOOLS)
			{
				Name = "Weld All";
			}

			public static void FixDictionaryEntries(CPluginModel Model, int Modified, Dictionary<int, List<int>> Redirection)
			{
				foreach (int Key in Redirection.Keys)
				{
					for (int i = 0; i < Redirection[Key].Count; ++i)
					{
						if (Redirection[Key][i] > Modified)
							Redirection[Key][i]--;
					}
				}

				for (int z = 0; z < Model.GetTriangleCount(); ++z)
				{
					CPluginTriangle Tri = Model.GetTriangleAt(z);

					for (int t = 0; t < 3; ++t)
					{
						if (Tri.Vertices[t] > Modified)
							Tri.Vertices[t]--;
					}
				}
			}

			public static void DoWeldAll(CPluginModel Model, float Sensitivity)
			{
				Dictionary<int, List<int>> Redirection = new Dictionary<int, List<int>>();
				List<int> SkipCheck = new List<int>();

				for (int i = 0; i < Model.GetVerticeCount(); ++i)
				{
					if (SkipCheck.Contains(i))
						continue;

					List<int> Match = new List<int>();
					for (int z = 0; z < Model.GetVerticeCount(); ++z)
					{
						if (i == z)
							continue;

						if ((Model.GetVerticeAt(i).GetFrameDataAt(0).Vector - Model.GetVerticeAt(z).GetFrameDataAt(0).Vector).Length() < Sensitivity)
						{
							SkipCheck.Add(z);
							Match.Add(z);
						}
					}

					if (Match.Count != 0)
						Redirection.Add(i, Match);
				}

				foreach (int Key in Redirection.Keys)
				{
					for (int i = 0; i < Redirection[Key].Count; ++i)
					{
						for (int z = 0; z < Model.GetTriangleCount(); ++z)
						{
							CPluginTriangle Tri = Model.GetTriangleAt(z);
							
							for (int t = 0; t < 3; ++t)
							{
								if (Tri.Vertices[t] == Redirection[Key][i])
									Tri.Vertices[t] = Key;
							}
						}
					}
				}

				foreach (int Key in Redirection.Keys)
				{
					for (int i = 0; i < Redirection[Key].Count; ++i)
					{
						Model.RemoveVerticeAt(Redirection[Key][i]);
						FixDictionaryEntries(Model, Redirection[Key][i], Redirection);
					}
				}
			}

			class SensitivityInputForm : Form
			{
				TextBox InputBox;
				Button OKButton, CancButton;

				public SensitivityInputForm()
				{
					StartPosition = FormStartPosition.CenterParent;
					FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
					Size = new System.Drawing.Size(200, 125);

					Label desc = new Label();
					desc.Text = "Enter weld vertex\nsensitivity.";
					desc.AutoSize = true;
					desc.TextAlign = ContentAlignment.MiddleCenter;
					desc.Location = new Point(50, 10);
					Controls.Add(desc);

					InputBox = new TextBox();
					InputBox.Location = new Point(25, 45);
					InputBox.Size = new Size(145, InputBox.Height);
					InputBox.Text = "0.1";
					Controls.Add(InputBox);

					OKButton = new Button();
					OKButton.Text = "OK";
					OKButton.Location = new Point(20, 75);
					OKButton.Click += new EventHandler(OKButton_Click);
					Controls.Add(OKButton);

					CancButton = new Button();
					CancButton.Text = "Cancel";
					CancButton.Location = new Point(100, 75);
					CancButton.Click += new EventHandler(CancelButton_Click);
					Controls.Add(CancButton);
				}

				void CancelButton_Click(object sender, EventArgs e)
				{
					DialogResult = System.Windows.Forms.DialogResult.Cancel;
				}

				void OKButton_Click(object sender, EventArgs e)
				{
					DialogResult = System.Windows.Forms.DialogResult.OK;
				}

				public static DialogResult Open(ref string inputresult)
				{
					SensitivityInputForm Form = new SensitivityInputForm();
					DialogResult res = Form.ShowDialog();
					inputresult = Form.InputBox.Text;
					return res;
				}
			}

			public override bool Execute(CPluginModel Model)
			{
				string res = "";
				if (SensitivityInputForm.Open(ref res) == DialogResult.Cancel)
					return false;

				float Sensitivity;
				if (!float.TryParse(res, out Sensitivity))
				{
					MessageBox.Show("Invalid number input!");
					return false;
				}

				DoWeldAll(Model, Sensitivity);
				return true;
			}

			public static CPlugin CreatePlugin()
			{
				return new CWeldAllPlugin();
			}
		}
	}
}