namespace Universal_Share.Interface
{
    partial class MainFormP
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
            this.components = new System.ComponentModel.Container();
            this._idStreamMap = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Stop = new System.Windows.Forms.Button();
            this.ForceUpdateAll = new System.Windows.Forms.Button();
            this.B_StartServer = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this._TokenList = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter1 = new System.Windows.Forms.Splitter();
            this._RememberList = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter2 = new System.Windows.Forms.Splitter();
            this._RegList = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.b_SendFile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _idStreamMap
            // 
            this._idStreamMap.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this._idStreamMap.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this._idStreamMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this._idStreamMap.HideSelection = false;
            this._idStreamMap.HoverSelection = true;
            this._idStreamMap.Location = new System.Drawing.Point(0, 0);
            this._idStreamMap.Name = "_idStreamMap";
            this._idStreamMap.Size = new System.Drawing.Size(908, 546);
            this._idStreamMap.TabIndex = 0;
            this._idStreamMap.UseCompatibleStateImageBehavior = false;
            this._idStreamMap.View = System.Windows.Forms.View.Details;
            this._idStreamMap.DoubleClick += new System.EventHandler(this._idStreamMap_DoubleClick);
            this._idStreamMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this._idStreamMap_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 89;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.b_SendFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(194, 858);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Stop);
            this.panel2.Controls.Add(this.ForceUpdateAll);
            this.panel2.Controls.Add(this.B_StartServer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1102, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 858);
            this.panel2.TabIndex = 0;
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(6, 41);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(182, 23);
            this.Stop.TabIndex = 1;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.Stop_Click);
            // 
            // ForceUpdateAll
            // 
            this.ForceUpdateAll.Location = new System.Drawing.Point(6, 800);
            this.ForceUpdateAll.Name = "ForceUpdateAll";
            this.ForceUpdateAll.Size = new System.Drawing.Size(82, 23);
            this.ForceUpdateAll.TabIndex = 0;
            this.ForceUpdateAll.Text = "Update All";
            this.ForceUpdateAll.UseVisualStyleBackColor = true;
            this.ForceUpdateAll.Click += new System.EventHandler(this.ForceUpdateAll_Click);
            // 
            // B_StartServer
            // 
            this.B_StartServer.Location = new System.Drawing.Point(6, 12);
            this.B_StartServer.Name = "B_StartServer";
            this.B_StartServer.Size = new System.Drawing.Size(182, 23);
            this.B_StartServer.TabIndex = 0;
            this.B_StartServer.Text = "Start";
            this.B_StartServer.UseVisualStyleBackColor = true;
            this.B_StartServer.Click += new System.EventHandler(this.B_StartServer_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 858);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1302, 28);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this._idStreamMap);
            this.panel4.Controls.Add(this.splitter4);
            this.panel4.Controls.Add(this._TokenList);
            this.panel4.Controls.Add(this.splitter1);
            this.panel4.Controls.Add(this._RememberList);
            this.panel4.Controls.Add(this.splitter2);
            this.panel4.Controls.Add(this._RegList);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(194, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(908, 858);
            this.panel4.TabIndex = 3;
            // 
            // splitter4
            // 
            this.splitter4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(0, 546);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(908, 5);
            this.splitter4.TabIndex = 7;
            this.splitter4.TabStop = false;
            // 
            // _TokenList
            // 
            this._TokenList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4});
            this._TokenList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._TokenList.HideSelection = false;
            this._TokenList.Location = new System.Drawing.Point(0, 551);
            this._TokenList.Name = "_TokenList";
            this._TokenList.Size = new System.Drawing.Size(908, 108);
            this._TokenList.TabIndex = 3;
            this._TokenList.UseCompatibleStateImageBehavior = false;
            this._TokenList.View = System.Windows.Forms.View.Details;
            this._TokenList.DoubleClick += new System.EventHandler(this._TokenList_DoubleClick);
            this._TokenList.MouseClick += new System.Windows.Forms.MouseEventHandler(this._TokenList_MouseClick);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Width = 591;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 659);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(908, 5);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // _RememberList
            // 
            this._RememberList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this._RememberList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._RememberList.HideSelection = false;
            this._RememberList.Location = new System.Drawing.Point(0, 664);
            this._RememberList.Name = "_RememberList";
            this._RememberList.Size = new System.Drawing.Size(908, 95);
            this._RememberList.TabIndex = 1;
            this._RememberList.UseCompatibleStateImageBehavior = false;
            this._RememberList.View = System.Windows.Forms.View.Details;
            this._RememberList.DoubleClick += new System.EventHandler(this._RememberList_DoubleClick);
            this._RememberList.MouseClick += new System.Windows.Forms.MouseEventHandler(this._RememberList_MouseClick);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 89;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 759);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(908, 5);
            this.splitter2.TabIndex = 5;
            this.splitter2.TabStop = false;
            // 
            // _RegList
            // 
            this._RegList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
            this._RegList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._RegList.HideSelection = false;
            this._RegList.Location = new System.Drawing.Point(0, 764);
            this._RegList.Name = "_RegList";
            this._RegList.Size = new System.Drawing.Size(908, 94);
            this._RegList.TabIndex = 2;
            this._RegList.UseCompatibleStateImageBehavior = false;
            this._RegList.View = System.Windows.Forms.View.Details;
            this._RegList.DoubleClick += new System.EventHandler(this._RegList_DoubleClick);
            this._RegList.MouseClick += new System.Windows.Forms.MouseEventHandler(this._RegList_MouseClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Width = 89;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleateToolStripMenuItem,
            this.newToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(114, 70);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.EditToolStripMenuItem_Click);
            // 
            // deleateToolStripMenuItem
            // 
            this.deleateToolStripMenuItem.Name = "deleateToolStripMenuItem";
            this.deleateToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.deleateToolStripMenuItem.Text = "Deleate";
            this.deleateToolStripMenuItem.Click += new System.EventHandler(this.DeleteToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // b_SendFile
            // 
            this.b_SendFile.Location = new System.Drawing.Point(6, 12);
            this.b_SendFile.Name = "b_SendFile";
            this.b_SendFile.Size = new System.Drawing.Size(182, 23);
            this.b_SendFile.TabIndex = 2;
            this.b_SendFile.Text = "SendFile";
            this.b_SendFile.UseVisualStyleBackColor = true;
            this.b_SendFile.Click += new System.EventHandler(this.B_SendFile_Click);
            // 
            // MainFormP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 886);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Name = "MainFormP";
            this.Text = "MainFormP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ColsingHandler);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView _idStreamMap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button B_StartServer;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListView _RegList;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView _RememberList;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView _TokenList;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Splitter splitter4;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button ForceUpdateAll;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button b_SendFile;
    }
}