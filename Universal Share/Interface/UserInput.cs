using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Universal_Share.Net;
using Universal_Share.Options;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Interface {
    public class UserInput : Form {
        public static bool  ChackboxChecked;
        private       Label _infoLable;

        private Button   _noButton;
        private CheckBox _remember;
        private Panel    panel1;
        private TextBox  _DescriptionBox;
        private Panel    panel2;
        private Label    label1;
        private Button   _yesButton;
        public UserInput() { InitializeComponent(); }

        //Console.WriteLine(us.GetConfirm( new RegInfo(null,-1,@"..\\..\\..\\test.txt", RegInfo.TYPE.SINGLE_FILE), new TypeHolder(@"C:\windows\system32\cmd.exe","-c pause",true," descript",false)));
        public bool GetConfirm(RegInfo reg, TypeHolder th) {
            ResetAndInfo( th.Description + "\nFileType: " + reg.Type + "     ID: " + reg.ID + "\nOpenWith: " + th.OpenWith );

            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( ChackboxChecked ) ßMainPoint.S.RememberType.Add( reg.Type, new RememberType( this._DescriptionBox.Text, ret, reg.Type ) );

            return bret;
        }

        public bool GetConfirm(TokenItem ti) {
            ResetAndInfo( "Key= " + string.Join( " ,", NetBase.SubArray( ti.TokenBytes, 0, 8 ) ) + "\n" + ti.Base64Key +"\n"+ti.Description);

            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( ChackboxChecked ) ßMainPoint.S.ToakenList.Add( ti.Base64Key, new TokenItem( ti.TokenBytes, bret, true, this._DescriptionBox.Text ) );
            return bret;
        }

        private void ResetAndInfo(string info) {
            this._DescriptionBox.Text = "";
            this.Text = Console.Title.ToUpper();
            this._infoLable.Text   = info;
            this._remember.Checked = false;
            ChackboxChecked        = false;
        }

        private void InitializeComponent() {
            this._noButton       = new System.Windows.Forms.Button();
            this._yesButton      = new System.Windows.Forms.Button();
            this._infoLable      = new System.Windows.Forms.Label();
            this._remember       = new System.Windows.Forms.CheckBox();
            this.panel1          = new System.Windows.Forms.Panel();
            this._DescriptionBox = new System.Windows.Forms.TextBox();
            this.panel2          = new System.Windows.Forms.Panel();
            this.label1          = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _noButton
            // 
            this._noButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._noButton.Location                =  new System.Drawing.Point( 299, 5 );
            this._noButton.Margin                  =  new System.Windows.Forms.Padding( 5 );
            this._noButton.Name                    =  "_noButton";
            this._noButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._noButton.TabIndex                =  0;
            this._noButton.Text                    =  "NO";
            this._noButton.UseVisualStyleBackColor =  true;
            this._noButton.Click                   += new System.EventHandler( this.NoButton_Click );
            // 
            // _yesButton
            // 
            this._yesButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._yesButton.Location                =  new System.Drawing.Point( 204, 5 );
            this._yesButton.Margin                  =  new System.Windows.Forms.Padding( 5 );
            this._yesButton.Name                    =  "_yesButton";
            this._yesButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._yesButton.TabIndex                =  1;
            this._yesButton.Text                    =  "YES";
            this._yesButton.UseVisualStyleBackColor =  true;
            this._yesButton.Click                   += new System.EventHandler( this.YesButton_Click );
            // 
            // _infoLable
            // 
            this._infoLable.Dock     = System.Windows.Forms.DockStyle.Fill;
            this._infoLable.Font     = new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._infoLable.Location = new System.Drawing.Point( 0, 0 );
            this._infoLable.Name     = "_infoLable";
            this._infoLable.Size     = new System.Drawing.Size( 389, 87 );
            this._infoLable.TabIndex = 2;
            this._infoLable.Text     = "Info Currently Emty";
            // 
            // _remember
            // 
            this._remember.AutoSize                =  true;
            this._remember.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._remember.Location                =  new System.Drawing.Point( 12, 10 );
            this._remember.Name                    =  "_remember";
            this._remember.Size                    =  new System.Drawing.Size( 174, 21 );
            this._remember.TabIndex                =  3;
            this._remember.Text                    =  "Remember for this type";
            this._remember.UseVisualStyleBackColor =  true;
            this._remember.CheckedChanged          += new System.EventHandler( this.Remember_CheckedChanged );
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this._remember );
            this.panel1.Controls.Add( this._yesButton );
            this.panel1.Controls.Add( this._noButton );
            this.panel1.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point( 0, 127 );
            this.panel1.Margin   = new System.Windows.Forms.Padding( 0 );
            this.panel1.Name     = "panel1";
            this.panel1.Size     = new System.Drawing.Size( 389, 38 );
            this.panel1.TabIndex = 4;
            // 
            // _DescriptionBox
            // 
            this._DescriptionBox.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this._DescriptionBox.Location = new System.Drawing.Point( 0, 13 );
            this._DescriptionBox.Name     = "_DescriptionBox";
            this._DescriptionBox.Size     = new System.Drawing.Size( 389, 27 );
            this._DescriptionBox.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add( this._DescriptionBox );
            this.panel2.Controls.Add( this.label1 );
            this.panel2.Dock     = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point( 0, 87 );
            this.panel2.Name     = "panel2";
            this.panel2.Size     = new System.Drawing.Size( 389, 40 );
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Dock     = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font     = new System.Drawing.Font( "Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this.label1.Location = new System.Drawing.Point( 0, 0 );
            this.label1.Name     = "label1";
            this.label1.Size     = new System.Drawing.Size( 389, 40 );
            this.label1.TabIndex = 5;
            this.label1.Text     = "Give A Description:";
            // 
            // UserInput
            // 
            this.ClientSize = new System.Drawing.Size( 389, 165 );
            this.Controls.Add( this._infoLable );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.panel1 );
            this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this.Name = "UserInput";
            this.panel1.ResumeLayout( false );
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout( false );
            this.panel2.PerformLayout();
            this.ResumeLayout( false );
        }

        private void Remember_CheckedChanged(object sender, EventArgs e) { ChackboxChecked = this._remember.Checked; }

        private void YesButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private void NoButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.No;
            Close();
        }
    }
}