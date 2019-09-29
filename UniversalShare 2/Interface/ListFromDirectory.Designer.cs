using System.Windows.Forms;

namespace UniversalShare_2.Interface
{
    partial class ListFromDirectory<TK,TV>
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader = new ColumnHeader();
            this.SuspendLayout();
            // 
            // listView1
            //
            this.listView1.Columns.Add( this.columnHeader );
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(240, 200);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // ListFromDirectory
            // 
            this.Controls.Add(this.listView1);
            this.Name = "ListFromDirectory";
            this.Size = new System.Drawing.Size(240, 200);
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ListView listView1;
    }
}
