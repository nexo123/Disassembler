using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Disassembler.Core;

namespace Disassembler
{
    public partial class Form1 : Form
    {
        private string file_path = "add2.exe"; //Dummy path
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
            Controller.GetInstance().Disassemble(-1, 0);
        }
    }
}
