using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Disassembler.Core;

namespace Disassembler
{
    public partial class Form1 : Form
    {
        private string file_path = "add2.exe"; //Dummy path
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                file_path = openFileDialog.FileName;
                //Debug.WriteLine(file_path);
                richTextBox1.Text = Controller.GetInstance().Init(file_path);

            }
            else if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {

            }
            else
            {

            }
        }

        private void disassemble_button_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            output = Controller.GetInstance().Disassemble(offset, num_bytes);
            richTextBox2.AppendText("\t.mode small" + Environment.NewLine + "\t.code" + Environment.NewLine);
            foreach (string str in output)
            {
                richTextBox2.AppendText(str);
            }
            richTextBox2.AppendText("\tend");
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
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error");
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
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error");
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
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the CS length in either HEX or DEC format.", "Set num_bytes", "");
            if (input.Length > 2)
            {
                if (TestInput(input.ToLower()))
                {
                    if (input.Substring(0, 2).Equals("0x"))
                    {
                        num_bytes = Convert.ToInt32(input.ToUpper(), 16);
                        numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                    }
                    else
                    {
                        num_bytes = Convert.ToInt32(input.ToUpper(), 10);
                        numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error");
                    num_bytes = 0;
                    numBytes_label.Text = "Length:" + Environment.NewLine + "0";
                }
            }
            else if (input.Length > 0 && input.Length < 3)
            {
                if (TestInput(input.ToLower()))
                {
                    num_bytes = Convert.ToInt32(input.ToUpper(), 10);
                    numBytes_label.Text = "Length:" + Environment.NewLine + num_bytes.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid input detected, please enter a prefixed HEX or DEC string", "Number format error");
                    num_bytes = 0;
                    numBytes_label.Text = "Length:" + Environment.NewLine + "0";
                }
            }
            else
            {
                num_bytes = 0;
                numBytes_label.Text = "Length:" + Environment.NewLine + "0";
            }
        }
    }
}
