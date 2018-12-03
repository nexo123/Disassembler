using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Disassembler.Core;
using Disassembler.Utils;

namespace Disassembler
{
    public partial class Form1 : Form
    {
        private int offset = -1;
        private int num_bytes = 0;
        private List<string> output;

        public Form1()
        {
            InitializeComponent();
        }

        private void open_file_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open source file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (output != null && output.Count > 0)
                {
                    if (MessageBox.Show("All unsaved changes will be lost. Are you sure to continue?", "Confirm opening new file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        open_file(openFileDialog.FileName);
                    }
                }
                else
                {
                    open_file(openFileDialog.FileName);
                }
            }
        }

        private void open_file(string path)
        {
            if (FileManager.GetInstance().Open(path))
            {
                cleanup();
                richTextBox1.Text = FileManager.GetInstance().FileToHex();
                tabControl1.SelectedIndex = 0;
                string[] parts = path.Split('\\');
                string file_name = parts[parts.Length - 1];
                tabPage1.Text = get_file_name(path);
                tabPage2.Text = "Output";
            }
        }

        private string get_file_name(string path)
        {
            string[] parts = path.Split('\\');
            string file_name = parts[parts.Length - 1];
            return file_name;
        }

        private void cleanup()
        {
            output = null;
            richTextBox1.Clear();
            richTextBox2.Clear();
            offset = -1;
            num_bytes = 0;
        }

        private void disassemble_button_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            output = Controller.GetInstance().Disassemble(offset, num_bytes);
            if (output != null)
            {
                richTextBox2.AppendText("\t.mode small" + Environment.NewLine + "\t.code" + Environment.NewLine);
                foreach (string str in output)
                {
                    richTextBox2.AppendText(str);
                }
                richTextBox2.AppendText("\tend");
                tabControl1.SelectedIndex = 1;
            }
        }

        private void set_offset_button_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the offset in either HEX or DEC format.", "Set offset", "");
            if (input.Length > 2)
            {
                if (TestInput(input.ToLower()))
                {
                    if (input.Substring(0, 2).Equals("0x"))
                    {
                        offset = Convert.ToInt32(input.ToUpper(), 16);
                        offset_label.Text = "Offset:" + Environment.NewLine + offset.ToString();
                    }
                    else
                    {
                        offset = Convert.ToInt32(input.ToUpper(), 10);
                        offset_label.Text = "Offset:" + Environment.NewLine + offset.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    offset = -1;
                    offset_label.Text = "Offset:" + Environment.NewLine + "None";
                }
            }
            else if (input.Length > 0 && input.Length < 3)
            {
                if (TestInput(input.ToLower()))
                {
                    offset = Convert.ToInt32(input.ToUpper(), 10);
                    offset_label.Text = "Offset:" + Environment.NewLine + offset.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    offset = 0;
                    offset_label.Text = "Offset:" + Environment.NewLine + "0";
                }
            }
            else
            {
                offset = -1;
                offset_label.Text = "Offset:" + Environment.NewLine + "None";
            }
        }

        /// <summary>
        /// Simple method to validate inputbox input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns></returns>
        private bool TestInput(string input)
        {
            if (input.Length > 2)
            {
                if (input.Substring(0, 2).Equals("0x"))
                {
                    //Validate HEX input
                    string number = input.Substring(2, input.Length - 2);
                    foreach (char c in number)
                    {
                        if ((c < '0' || c > '9') && (c < 'a' || c > 'f'))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    //Validate DEC input
                    foreach (char c in input)
                    {
                        if (c < '0' || c > '9')
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            else
            {
                //Validate DEC input
                foreach (char c in input)
                {
                    if (c < '0' || c > '9')
                    {
                        return false;
                    }
                }
                return true;
            }
        }


        private void setLength_button_Click(object sender, EventArgs e)
        {
            //show the input dialogbox
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the CS length in either HEX or DEC format.", "Set num_bytes", "");
            //validate the input
            if (input.Length > 2) //if length is more than 2, it can be either hex or dec number
            {
                if (TestInput(input.ToLower())) //test if numerical characters and letters A-F
                {
                    if (input.Substring(0, 2).Equals("0x")) //test if hex prefix
                    {
                        num_bytes = Convert.ToInt32(input.ToUpper(), 16);
                        numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                    }
                    else //else treat it as decimal
                    {
                        num_bytes = Convert.ToInt32(input.ToUpper(), 10);
                        numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                    }
                }
                else
                {
                    //handle errors if text validation did not pass
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    num_bytes = 0;
                    numBytes_label.Text = "Length:" + Environment.NewLine + "0";
                }
            }
            else if (input.Length > 0 && input.Length <= 2) //it can only be decimal 
            {
                //same as above
                if (TestInput(input.ToLower()))
                {
                    num_bytes = Convert.ToInt32(input.ToUpper(), 10);
                    numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    num_bytes = 0;
                    numBytes_label.Text = "Length:" + Environment.NewLine + "0";
                }
            }
            else //if no input reset it to 0
            {
                num_bytes = 0;
                numBytes_label.Text = "Length:" + Environment.NewLine + "0";
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            //save file with no argument = will default to "output.asm"
            save_file("");
        }

        private void saveas_button_Click(object sender, EventArgs e)
        {
            //show save file dialog to let the user choose destination folder and file
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Assembly file|*.asm|Text file|*.txt",
                Title = "Save the output"
            };
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //testif the user choose YES.
                save_file(saveFileDialog.FileName);         
            }
        }

        /// <summary>
        /// Save the output to disk.
        /// </summary>
        /// <param name="path">Path to the output file. Default "output.asm".</param>
        private void save_file(string path)
        {
            if (output == null || output.Count == 0)
            {
                MessageBox.Show("Saving file failed! There is nothing to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(".model small" + Environment.NewLine + ".code" + Environment.NewLine);
            foreach (string str in output)
            {
                sb.Append(str.Replace("\t", " ").Remove(0, 1));
            }
            sb.Append("end");
            if (FileManager.GetInstance().WriteFile(path, sb.ToString()))
            {
                tabPage2.Text = path.Length == 0 ? "output.asm" : get_file_name(path);
                MessageBox.Show("File saved!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            { 
                MessageBox.Show("Saving file failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
