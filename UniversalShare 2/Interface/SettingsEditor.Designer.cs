/*
namespace Universal_Share.Interface
{
    partial class SettingsEditor
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.F_port = new System.Windows.Forms.NumericUpDown();
            this.b_OK = new System.Windows.Forms.Button();
            this.F_replaceString = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.F_bufferSize = new System.Windows.Forms.Label();
            this.b_Save = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.F_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(135, 523);
            this.panel1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 88);
            this.label3.Margin = new System.Windows.Forms.Padding(10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "BufferSize";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "ReplaceString";
            // 
            // F_port
            // 
            this.F_port.Location = new System.Drawing.Point(161, 8);
            this.F_port.Name = "F_port";
            this.F_port.Size = new System.Drawing.Size(191, 26);
            this.F_port.TabIndex = 1;
            this.F_port.ValueChanged += new System.EventHandler(this.F_port_ValueChanged);
            // 
            // b_OK
            // 
            this.b_OK.Location = new System.Drawing.Point(270, 483);
            this.b_OK.Name = "b_OK";
            this.b_OK.Size = new System.Drawing.Size(98, 28);
            this.b_OK.TabIndex = 2;
            this.b_OK.Text = "OK";
            this.b_OK.UseVisualStyleBackColor = true;
            this.b_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // F_replaceString
            // 
            this.F_replaceString.Location = new System.Drawing.Point(161, 45);
            this.F_replaceString.Name = "F_replaceString";
            this.F_replaceString.Size = new System.Drawing.Size(191, 26);
            this.F_replaceString.TabIndex = 2;
            this.F_replaceString.TextChanged += new System.EventHandler(this.F_replaceString_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "2^";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(188, 86);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(93, 26);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.NumericUpDown1_ValueChanged);
            // 
            // F_bufferSize
            // 
            this.F_bufferSize.AutoSize = true;
            this.F_bufferSize.Location = new System.Drawing.Point(287, 88);
            this.F_bufferSize.Name = "F_bufferSize";
            this.F_bufferSize.Size = new System.Drawing.Size(19, 20);
            this.F_bufferSize.TabIndex = 5;
            this.F_bufferSize.Text = "--";
            this.F_bufferSize.TextChanged += new System.EventHandler(this.F_bufferSize_TextChanged);
            // 
            // b_Save
            // 
            this.b_Save.Location = new System.Drawing.Point(161, 483);
            this.b_Save.Name = "b_Save";
            this.b_Save.Size = new System.Drawing.Size(98, 28);
            this.b_Save.TabIndex = 6;
            this.b_Save.Text = "Save";
            this.b_Save.UseVisualStyleBackColor = true;
            this.b_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // SettingsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 523);
            this.Controls.Add(this.b_Save);
            this.Controls.Add(this.F_bufferSize);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.F_replaceString);
            this.Controls.Add(this.b_OK);
            this.Controls.Add(this.F_port);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(254)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingsEditor";
            this.Text = "SettingsEditor";
            this.Load += new System.EventHandler(this.SettingsEditor_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.F_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown F_port;
        private System.Windows.Forms.Button b_OK;
        private System.Windows.Forms.TextBox F_replaceString;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label F_bufferSize;
        private System.Windows.Forms.Button b_Save;
    }
}*/