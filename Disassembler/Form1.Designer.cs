namespace Disassembler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.numBytes_label = new System.Windows.Forms.Label();
            this.offset_label = new System.Windows.Forms.Label();
            this.disassemble_button = new System.Windows.Forms.Button();
            this.setlength_button = new System.Windows.Forms.Button();
            this.set_offset_button = new System.Windows.Forms.Button();
            this.saveas_button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.open_file_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.numBytes_label);
            this.splitContainer1.Panel2.Controls.Add(this.offset_label);
            this.splitContainer1.Panel2.Controls.Add(this.disassemble_button);
            this.splitContainer1.Panel2.Controls.Add(this.setlength_button);
            this.splitContainer1.Panel2.Controls.Add(this.set_offset_button);
            this.splitContainer1.Panel2.Controls.Add(this.saveas_button);
            this.splitContainer1.Panel2.Controls.Add(this.save_button);
            this.splitContainer1.Panel2.Controls.Add(this.open_file_button);
            this.splitContainer1.Size = new System.Drawing.Size(981, 453);
            this.splitContainer1.SplitterDistance = 849;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(849, 453);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(841, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(3, 2);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(835, 420);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(841, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox2.Location = new System.Drawing.Point(3, 2);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(835, 420);
            this.richTextBox2.TabIndex = 0;
            this.richTextBox2.Text = "";
            // 
            // numBytes_label
            // 
            this.numBytes_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.numBytes_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numBytes_label.Location = new System.Drawing.Point(0, 294);
            this.numBytes_label.Name = "numBytes_label";
            this.numBytes_label.Size = new System.Drawing.Size(128, 60);
            this.numBytes_label.TabIndex = 7;
            this.numBytes_label.Text = "0";
            this.numBytes_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // offset_label
            // 
            this.offset_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.offset_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offset_label.Location = new System.Drawing.Point(0, 234);
            this.offset_label.Name = "offset_label";
            this.offset_label.Size = new System.Drawing.Size(128, 60);
            this.offset_label.TabIndex = 6;
            this.offset_label.Text = "None";
            this.offset_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // disassemble_button
            // 
            this.disassemble_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.disassemble_button.Location = new System.Drawing.Point(0, 195);
            this.disassemble_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.disassemble_button.Name = "disassemble_button";
            this.disassemble_button.Size = new System.Drawing.Size(128, 39);
            this.disassemble_button.TabIndex = 5;
            this.disassemble_button.Text = "Disassemble";
            this.disassemble_button.UseVisualStyleBackColor = true;
            this.disassemble_button.Click += new System.EventHandler(this.disassemble_button_Click);
            // 
            // setlength_button
            // 
            this.setlength_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.setlength_button.Location = new System.Drawing.Point(0, 156);
            this.setlength_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.setlength_button.Name = "setlength_button";
            this.setlength_button.Size = new System.Drawing.Size(128, 39);
            this.setlength_button.TabIndex = 4;
            this.setlength_button.Text = "Set NumBytes";
            this.setlength_button.UseVisualStyleBackColor = true;
            this.setlength_button.Click += new System.EventHandler(this.setLength_button_Click);
            // 
            // set_offset_button
            // 
            this.set_offset_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.set_offset_button.Location = new System.Drawing.Point(0, 117);
            this.set_offset_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.set_offset_button.Name = "set_offset_button";
            this.set_offset_button.Size = new System.Drawing.Size(128, 39);
            this.set_offset_button.TabIndex = 3;
            this.set_offset_button.Text = "Set Offset";
            this.set_offset_button.UseVisualStyleBackColor = true;
            this.set_offset_button.Click += new System.EventHandler(this.set_offset_button_Click);
            // 
            // saveas_button
            // 
            this.saveas_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveas_button.Location = new System.Drawing.Point(0, 78);
            this.saveas_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.saveas_button.Name = "saveas_button";
            this.saveas_button.Size = new System.Drawing.Size(128, 39);
            this.saveas_button.TabIndex = 2;
            this.saveas_button.Text = "Save As";
            this.saveas_button.UseVisualStyleBackColor = true;
            this.saveas_button.Click += new System.EventHandler(this.saveas_button_Click);
            // 
            // save_button
            // 
            this.save_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.save_button.Location = new System.Drawing.Point(0, 39);
            this.save_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(128, 39);
            this.save_button.TabIndex = 1;
            this.save_button.Text = "Save";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // open_file_button
            // 
            this.open_file_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.open_file_button.Location = new System.Drawing.Point(0, 0);
            this.open_file_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.open_file_button.Name = "open_file_button";
            this.open_file_button.Size = new System.Drawing.Size(128, 39);
            this.open_file_button.TabIndex = 0;
            this.open_file_button.Text = "Open";
            this.open_file_button.UseVisualStyleBackColor = true;
            this.open_file_button.Click += new System.EventHandler(this.open_file_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 453);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "8086 Disassembler";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button setlength_button;
        private System.Windows.Forms.Button set_offset_button;
        private System.Windows.Forms.Button saveas_button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button open_file_button;
        private System.Windows.Forms.Button disassemble_button;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label numBytes_label;
        private System.Windows.Forms.Label offset_label;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}

