using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using FastColoredTextBoxNS;

namespace VFrame_Editor
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			#region Text-Editor
			txteditor.Font = new Font(txtConf.Read("Name", "Font"), Convert.ToSingle(txtConf.Read("Size", "Font")));
			int indent = Convert.ToInt32(txtConf.Read("TabIndent", "Editor"));
			if (indent < 1)
			{
				MessageBox.Show("Cannot set indent to below 1.", "Error: Indent Below Min",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				txteditor.TabIndent = 1;
				txtConf.Write("TabIndent", "1", "Editor");
			}
			else if (indent > 10)
			{
				MessageBox.Show("Cannot set indent to above 10.", "Error: Indent Above Max",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				txteditor.TabIndent = 10;
				txtConf.Write("TabIndent", "10", "Editor");
			}
			else
			{
				txteditor.TabIndent = indent;
			}
			timer1.Enabled = true;
			trackBar1.Value = Convert.ToInt32(txtConf.Read("TabIndent", "Editor"));
			txteditor.ShowHRuler = Convert.ToBoolean(txtConf.Read("HRuler", "Editor").ToLower());
			rulerToolStripMenuItem.Checked = txteditor.ShowHRuler;
			#endregion
			#region Code-Editor
			cdeeditor.Font = new Font(cdeConf.Read("Name", "Font"), Convert.ToSingle(cdeConf.Read("Size", "Font")));
			int indent1 = Convert.ToInt32(cdeConf.Read("TabLength", "Editor"));
			if (indent1 < 1)
			{
				MessageBox.Show("Cannot set indent to below 1.", "Error: Indent Below Min",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				cdeeditor.TabLength = 1;
				cdeConf.Write("TabLength", "1", "Editor");
			}
			else if (indent > 10)
			{
				MessageBox.Show("Cannot set indent to above 10.", "Error: Indent Above Max",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
				cdeeditor.TabLength = 10;
				cdeConf.Write("TabLength", "10", "Editor");
			}
			else
			{
				txteditor.TabIndent = indent1;
			}
			timer1.Enabled = true;
			trackBar1.Value = Convert.ToInt32(cdeConf.Read("TabLength", "Editor"));
			timer2.Enabled = true;
			cdeeditor.WordWrap = Convert.ToBoolean(cdeConf.Read("WordWrap", "Editor").ToLower());
			wordWrapToolStripMenuItem.Checked = cdeeditor.WordWrap;
			comboBox1_TextChanged(this, EventArgs.Empty);
			#endregion
			comboBox1.DataSource = null;
			comboBox1.DataSource = langs;

			TopMost = Convert.ToBoolean(wndConf.Read("TopMost", "General").ToLower());
			ShowIcon = Convert.ToBoolean(wndConf.Read("ShowIcon", "General").ToLower());
			ShowInTaskbar = Convert.ToBoolean(wndConf.Read("ShowInTaskbar", "General").ToLower());
			MaximizeBox = Convert.ToBoolean(wndConf.Read("MaximizeBox", "General").ToLower());
			MinimizeBox = Convert.ToBoolean(wndConf.Read("MinimizeBox", "General").ToLower());
		}

		static string BasePath = Application.StartupPath + "\\";
		static Config txtConf = new Config(BasePath + "Config" + "\\" + "TextEditor.ini");
		static Config cdeConf = new Config(BasePath + "Config" + "\\" + "CodeEditor.ini");
		static Config wndConf = new Config(BasePath + "Config" + "\\" + "WindowSettings.ini");
		static List<string> langs = new List<string>()
		{
			"Custom", "CSharp", "VB",
			"HTML", "XML", "SQL",
			"PHP", "JS", "Lua"
		};

		#region Text-Editor

		private void open_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog
			{
				Title = "Open a file...",
				FileName = "",
				Filter = "All Files|*.*",
				FilterIndex = 0
			})
				if (ofd.ShowDialog() == DialogResult.OK)
					using (StreamReader reader = new StreamReader(ofd.FileName))
					{
						txteditor.Text = reader.ReadToEnd();
						lblFile.Text = "File: " + ofd.FileName;
						reader.Close();
					}
		}
		private void new__Click(object sender, EventArgs e)
		{
			if (txteditor.Text == "" && lblFile.Text == "File: - [ none ] -")
			{
				return;
			}
			else if (txteditor.Text == "" && lblFile.Text != "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"You have unsaved changes in this file.\n" +
					"Would you like you save them?", "Warning: Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(txteditor.Text);
									lblFile.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;

					case DialogResult.No:
						txteditor.Text = "";
						lblFile.Text = "File: - [ none ] -";
						break;
				}
			}
			else if (txteditor.Text != "" && lblFile.Text != "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"Are you sure you want to make a new file?\n" +
					"The changes won't be saved to the file.", "Warning: Discarding Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						txteditor.Text = "";
						lblFile.Text = "File: - [ none ] -";
						break;

					case DialogResult.No:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(txteditor.Text);
									lblFile.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;
				}
			}
			else if (txteditor.Text != "" && lblFile.Text == "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"You have unsaved changes in this file.\n" +
					"Would you like you save them?", "Warning: Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(txteditor.Text);
									lblFile.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;

					case DialogResult.No:
						txteditor.Text = "";
						lblFile.Text = "File: - [ none ] -";
						break;
				}
			}
		}
		private void save_Click(object sender, EventArgs e)
		{
			if (txteditor.Text == "" && lblFile.Text == "File: - [ none ] -")
			{
				using (SaveFileDialog sfd = new SaveFileDialog
				{
					Title = "Save changes...",
					FileName = "",
					Filter = "Blank File|*",
					FilterIndex = 0
				})
					if (sfd.ShowDialog() == DialogResult.OK)
						using (StreamWriter writer = new StreamWriter(sfd.FileName))
						{
							writer.Write(txteditor.Text);
							lblFile.Text = "File: " + sfd.FileName;
							writer.Close();
						}
			}
			else if (txteditor.Text == "" && lblFile.Text != "File: - [ none ] -")
			{
				using (StreamWriter writer = new StreamWriter(lblFile.Text.Substring(5)))
				{
					writer.Write(txteditor.Text);
					writer.Close();
				}
			}
			else if (txteditor.Text != "" && lblFile.Text != "File: - [ none ] -")
			{
				using (StreamWriter writer = new StreamWriter(lblFile.Text.Substring(5)))
				{
					writer.Write(txteditor.Text);
					writer.Close();
				}
			}
			else if (txteditor.Text != "" && lblFile.Text == "File: - [ none ] -")
			{
				using (SaveFileDialog sfd = new SaveFileDialog
				{
					Title = "Save changes...",
					FileName = "",
					Filter = "All Files|*.*",
					FilterIndex = 0
				})
					if (sfd.ShowDialog() == DialogResult.OK)
						using (StreamWriter writer = new StreamWriter(sfd.FileName))
						{
							writer.Write(txteditor.Text);
							lblFile.Text = "File: " + sfd.FileName;
							writer.Close();
						}
			}
		}
		private void saveAs_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog
			{
				Title = "Save file as...",
				FileName = "",
				Filter = "All Files|*.*",
				FilterIndex = 0
			})
				if (sfd.ShowDialog() == DialogResult.OK)
					using (StreamWriter writer = new StreamWriter(sfd.FileName))
					{
						writer.Write(txteditor.Text);
						lblFile.Text = "File: " + sfd.FileName;
						writer.Close();
					}
		}
		private void font_Click(object sender, EventArgs e)
		{
			using (FontDialog fd = new FontDialog
			{
				AllowScriptChange = false,
				AllowSimulations = true,
				AllowVectorFonts = false,
				AllowVerticalFonts = false,
				Color = txteditor.ForeColor,
				FixedPitchOnly = true,
				Font = new Font("Consolas", 10F),
				FontMustExist = true,
				MaxSize = 20,
				MinSize = 8,
				ShowApply = true,
				ShowColor = false,
				ShowEffects = false,
				ShowHelp = false
			})
			{
				fd.Apply += delegate (object ss, EventArgs ee)
				{
					txteditor.Font = fd.Font;
					txtConf.Write("Name", fd.Font.Name, "Font");
					txtConf.Write("Size", fd.Font.Size.ToString(), "Font");
				};
				if (fd.ShowDialog() == DialogResult.OK)
				{
					txteditor.Font = fd.Font;
					txtConf.Write("Name", fd.Font.Name, "Font");
					txtConf.Write("Size", fd.Font.Size.ToString(), "Font");
				}
			}
		}
		private void rulerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			switch (rulerToolStripMenuItem.Checked)
			{
				case true:
					txteditor.ShowHRuler = false;
					rulerToolStripMenuItem.Checked = false;
					txtConf.Write("HRuler", txteditor.ShowHRuler.ToString().ToLower(), "Editor");
					break;

				case false:
					txteditor.ShowHRuler = true;
					rulerToolStripMenuItem.Checked = true;
					txtConf.Write("HRuler", txteditor.ShowHRuler.ToString().ToLower(), "Editor");
					break;
			}
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			lblIndent.Text = "Indent: " + txteditor.TabIndent.ToString();
			lblFont.Text = "Font: " + txteditor.Font.Name + ", " + txteditor.Font.Size.ToString() + "pt";
		}

		#endregion
		#region Code-Editor

		private void openn_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog
			{
				Title = "Open a file...",
				FileName = "",
				Filter = "All Files|*.*",
				FilterIndex = 0
			})
				if (ofd.ShowDialog() == DialogResult.OK)
					using (StreamReader reader = new StreamReader(ofd.FileName))
					{
						cdeeditor.Text = reader.ReadToEnd();
						lbl_File.Text = "File: " + ofd.FileName;
						reader.Close();
					}
		}
		private void neww_Click(object sender, EventArgs e)
		{
			if (cdeeditor.Text == "" && lbl_File.Text == "File: - [ none ] -")
			{
				return;
			}
			else if (cdeeditor.Text == "" && lbl_File.Text != "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"You have unsaved changes in this file.\n" +
					"Would you like you save them?", "Warning: Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(cdeeditor.Text);
									lbl_File.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;

					case DialogResult.No:
						cdeeditor.Text = "";
						lbl_File.Text = "File: - [ none ] -";
						break;
				}
			}
			else if (cdeeditor.Text != "" && lbl_File.Text != "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"Are you sure you want to make a new file?\n" +
					"The changes won't be saved to the file.", "Warning: Discarding Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						cdeeditor.Text = "";
						lbl_File.Text = "File: - [ none ] -";
						break;

					case DialogResult.No:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(cdeeditor.Text);
									lbl_File.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;
				}
			}
			else if (cdeeditor.Text != "" && lbl_File.Text == "File: - [ none ] -")
			{
				var unsaved = MessageBox.Show(
					"You have unsaved changes in this file.\n" +
					"Would you like you save them?", "Warning: Unsaved Changes",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				switch (unsaved)
				{
					case DialogResult.Yes:
						using (SaveFileDialog sfd = new SaveFileDialog
						{
							Title = "Save changes...",
							FileName = "",
							Filter = "All Files|*.*",
							FilterIndex = 0
						})
							if (sfd.ShowDialog() == DialogResult.OK)
								using (StreamWriter writer = new StreamWriter(sfd.FileName))
								{
									writer.Write(cdeeditor.Text);
									lbl_File.Text = "File: " + sfd.FileName;
									writer.Close();
								}
						break;

					case DialogResult.No:
						cdeeditor.Text = "";
						lbl_File.Text = "File: - [ none ] -";
						break;
				}
			}
		}
		private void savee_Click(object sender, EventArgs e)
		{
			if (cdeeditor.Text == "" && lbl_File.Text == "File: - [ none ] -")
			{
				using (SaveFileDialog sfd = new SaveFileDialog
				{
					Title = "Save changes...",
					FileName = "",
					Filter = "Blank File|*",
					FilterIndex = 0
				})
					if (sfd.ShowDialog() == DialogResult.OK)
						using (StreamWriter writer = new StreamWriter(sfd.FileName))
						{
							writer.Write(cdeeditor.Text);
							lbl_File.Text = "File: " + sfd.FileName;
							writer.Close();
						}
			}
			else if (cdeeditor.Text == "" && lbl_File.Text != "File: - [ none ] -")
			{
				using (StreamWriter writer = new StreamWriter(lbl_File.Text.Substring(5)))
				{
					writer.Write(cdeeditor.Text);
					writer.Close();
				}
			}
			else if (cdeeditor.Text != "" && lbl_File.Text != "File: - [ none ] -")
			{
				using (StreamWriter writer = new StreamWriter(lbl_File.Text.Substring(5)))
				{
					writer.Write(cdeeditor.Text);
					writer.Close();
				}
			}
			else if (cdeeditor.Text != "" && lbl_File.Text == "File: - [ none ] -")
			{
				using (SaveFileDialog sfd = new SaveFileDialog
				{
					Title = "Save changes...",
					FileName = "",
					Filter = "All Files|*.*",
					FilterIndex = 0
				})
					if (sfd.ShowDialog() == DialogResult.OK)
						using (StreamWriter writer = new StreamWriter(sfd.FileName))
						{
							writer.Write(cdeeditor.Text);
							lbl_File.Text = "File: " + sfd.FileName;
							writer.Close();
						}
			}
		}
		private void saveAs__Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog
			{
				Title = "Save file as...",
				FileName = "",
				Filter = "All Files|*.*",
				FilterIndex = 0
			})
				if (sfd.ShowDialog() == DialogResult.OK)
					using (StreamWriter writer = new StreamWriter(sfd.FileName))
					{
						writer.Write(cdeeditor.Text);
						lbl_File.Text = "File: " + sfd.FileName;
						writer.Close();
					}
		}
		private void fontt_Click(object sender, EventArgs e)
		{
			using (FontDialog fd = new FontDialog
			{
				AllowScriptChange = false,
				AllowSimulations = true,
				AllowVectorFonts = false,
				AllowVerticalFonts = false,
				Color = txteditor.ForeColor,
				FixedPitchOnly = true,
				Font = new Font("Consolas", 10F),
				FontMustExist = true,
				MaxSize = 20,
				MinSize = 8,
				ShowApply = true,
				ShowColor = false,
				ShowEffects = false,
				ShowHelp = false
			})
			{
				fd.Apply += delegate (object ss, EventArgs ee)
				{
					cdeeditor.Font = fd.Font;
					cdeConf.Write("Name", fd.Font.Name, "Font");
					cdeConf.Write("Size", fd.Font.Size.ToString(), "Font");
				};
				if (fd.ShowDialog() == DialogResult.OK)
				{
					cdeeditor.Font = fd.Font;
					cdeConf.Write("Name", fd.Font.Name, "Font");
					cdeConf.Write("Size", fd.Font.Size.ToString(), "Font");
				}
			}
		}
		private void timer2_Tick(object sender, EventArgs e)
		{
			lbl_Indent.Text = "Indent: " + cdeeditor.TabLength.ToString();
			lbl_Font.Text = "Font: " + cdeeditor.Font.Name + ", " + cdeeditor.Font.Size.ToString() + "pt";
		}

		#endregion

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			txteditor.TabIndent = trackBar1.Value;
			txtConf.Write("TabIndent", txteditor.TabIndent.ToString(), "Editor");

			cdeeditor.TabLength = trackBar1.Value;
			cdeConf.Write("TabLength", cdeeditor.TabLength.ToString(), "Editor");
		}
		private void textTab_Click(object sender, EventArgs e)
		{
			pnlText.Visible = true;
			pnlCode.Visible = false;
			pnlTxtEditor.BringToFront();
		}
		private void codeTab_Click(object sender, EventArgs e)
		{
			pnlCode.Visible = true;
			pnlText.Visible = false;
			pnlCdeEditor.BringToFront();
		}
		private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var ww = wordWrapToolStripMenuItem;
			switch (ww.Checked)
			{
				case true:
					cdeeditor.WordWrap = false;
					ww.Checked = false;
					cdeConf.Write("WordWrap", cdeeditor.WordWrap.ToString().ToLower(), "Editor");
					break;

				case false:
					cdeeditor.WordWrap = true;
					ww.Checked = true;
					cdeConf.Write("WordWrap", cdeeditor.WordWrap.ToString().ToLower(), "Editor");
					break;
			}
		}
		private void comboBox1_TextChanged(object sender, EventArgs e)
		{
			switch (comboBox1.Text)
			{
				case "Custom":
					cdeeditor.Language = Language.Custom;
					break;

				case "CSharp":
					cdeeditor.Language = Language.CSharp;
					break;

				case "VB":
					cdeeditor.Language = Language.VB;
					break;

				case "HTML":
					cdeeditor.Language = Language.HTML;
					break;

				case "XML":
					cdeeditor.Language = Language.XML;
					break;

				case "SQL":
					cdeeditor.Language = Language.SQL;
					break;

				case "PHP":
					cdeeditor.Language = Language.PHP;
					break;

				case "JS":
					cdeeditor.Language = Language.JS;
					break;

				case "Lua":
					cdeeditor.Language = Language.Lua;
					break;
			}
		}
	}
}
