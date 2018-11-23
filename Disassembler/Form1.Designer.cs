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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.open_file_button = new System.Windows.Forms.Button();
            this.save_button = new System.Windows.Forms.Button();
            this.saveas_button = new System.Windows.Forms.Button();
            this.set_offset_button = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.disassemble_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.disassemble_button);
            this.splitContainer1.Panel2.Controls.Add(this.button2);
            this.splitContainer1.Panel2.Controls.Add(this.set_offset_button);
            this.splitContainer1.Panel2.Controls.Add(this.saveas_button);
            this.splitContainer1.Panel2.Controls.Add(this.save_button);
            this.splitContainer1.Panel2.Controls.Add(this.open_file_button);
            this.splitContainer1.Size = new System.Drawing.Size(982, 453);
            this.splitContainer1.SplitterDistance = 850;
            this.splitContainer1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(850, 453);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // open_file_button
            // 
            this.open_file_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.open_file_button.Location = new System.Drawing.Point(0, 0);
            this.open_file_button.Name = "open_file_button";
            this.open_file_button.Size = new System.Drawing.Size(128, 40);
            this.open_file_button.TabIndex = 0;
            this.open_file_button.Text = "Open";
            this.open_file_button.UseVisualStyleBackColor = true;
            this.open_file_button.Click += new System.EventHandler(this.open_file_button_Click);
            // 
            // save_button
            // 
            this.save_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.save_button.Location = new System.Drawing.Point(0, 40);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(128, 40);
            this.save_button.TabIndex = 1;
            this.save_button.Text = "Save";
            this.save_button.UseVisualStyleBackColor = true;
            // 
            // saveas_button
            // 
            this.saveas_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.saveas_button.Location = new System.Drawing.Point(0, 80);
            this.saveas_button.Name = "saveas_button";
            this.saveas_button.Size = new System.Drawing.Size(128, 40);
            this.saveas_button.TabIndex = 2;
            this.saveas_button.Text = "Save As";
            this.saveas_button.UseVisualStyleBackColor = true;
            // 
            // set_offset_button
            // 
            this.set_offset_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.set_offset_button.Location = new System.Drawing.Point(0, 120);
            this.set_offset_button.Name = "set_offset_button";
            this.set_offset_button.Size = new System.Drawing.Size(128, 40);
            this.set_offset_button.TabIndex = 3;
            this.set_offset_button.Text = "Set Offset";
            this.set_offset_button.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 160);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 40);
            this.button2.TabIndex = 4;
            this.button2.Text = "Set NumBytes";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // disassemble_button
            // 
            this.disassemble_button.Dock = System.Windows.Forms.DockStyle.Top;
            this.disassemble_button.Location = new System.Drawing.Point(0, 200);
            this.disassemble_button.Name = "disassemble_button";
            this.disassemble_button.Size = new System.Drawing.Size(128, 40);
            this.disassemble_button.TabIndex = 5;
            this.disassemble_button.Text = "Disassemble";
            this.disassemble_button.UseVisualStyleBackColor = true;
            this.disassemble_button.Click += new System.EventHandler(this.disassemble_button_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 453);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "8086 Disassembler";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button set_offset_button;
        private System.Windows.Forms.Button saveas_button;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button open_file_button;
        private System.Windows.Forms.Button disassemble_button;
    }
}

