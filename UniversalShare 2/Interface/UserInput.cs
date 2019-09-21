using System;
using System.Threading;
using System.Windows.Forms;
using UniversalShareCore.Net;
using UniversalShareCore.Operation;

namespace UniversalShare_2.Interface {
    public class UserInput : Form {
        public static bool  ChackboxChecked;
        private       Label _infoLable;

        private Button   _noButton;
        private CheckBox _remember;
        private Panel    _panel1;
        private TextBox  _descriptionBox;
        private Panel    _panel2;
        private Label    _label1;
        private Button   _yesButton;


        public UserInput() { InitializeComponent(); }
        /*
        //Console.WriteLine(us.GetConfirm( new RegInfo(null,-1,@"..\\..\\..\\test.txt", RegInfo.TYPE.SINGLE_FILE), new TypeHolder(@"C:\windows\system32\cmd.exe","-c pause",true," descript",false)));
        public bool GetConfirm(RegInfo reg, TypeHolder th) {
            ResetAndInfo( th.Description + "\nFileType: " + reg.Extension + "     ID: " + reg.Id + "\nOpenWith: " + th.OpenWith );

            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( ChackboxChecked ) ßMainPoint.S.RememberType.Add( reg.Extension, new RememberType( this._descriptionBox.Text, ret, reg.Extension ) );

            return bret;
        }*/

        public bool GetConfirm(TokenItem ti) {
            ResetAndInfo( "Key= " + string.Join( " ,", NetBase.SubArray( ti.TokenBytes, 0, 8 ) ) + "\n" + ti.Base64Key + "\n" + ti.Description );

            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( ChackboxChecked ) {
                if ( ßProgram.D.TokenList.ContainsKey( ti.Base64Key ) ) ßProgram.D.TokenList.Remove( ti.Base64Key );
                ßProgram.D.TokenList.Add( ti.Base64Key, new TokenItem( ti.TokenBytes, bret, true, this._descriptionBox.Text ) );
            }

            return bret;
        }

        private void ResetAndInfo(string info) {
            this._descriptionBox.Text = "";
            this.Text                 = Console.Title.ToUpper();
            this._infoLable.Text      = info;
            this._remember.Checked    = false;
            ChackboxChecked           = false;
        }

        public string GetString(string description, string example) {
            ResetAndInfo( description );
            this._descriptionBox.Text = example;
            ShowDialog();
            return this._descriptionBox.Text;
        }

        // ReSharper disable RedundantDelegateCreation
        private void InitializeComponent() {
            this._noButton       = new Button();
            this._yesButton      = new Button();
            this._infoLable      = new Label();
            this._remember       = new CheckBox();
            this._panel1         = new Panel();
            this._descriptionBox = new TextBox();
            this._panel2         = new Panel();
            this._label1         = new Label();
            this._panel1.SuspendLayout();
            this._panel2.SuspendLayout();
            SuspendLayout();
            // 
            // _noButton
            // 
            this._noButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this._noButton.Location                =  new System.Drawing.Point( 299, 5 );
            this._noButton.Margin                  =  new Padding( 5 );
            this._noButton.Name                    =  "_noButton";
            this._noButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._noButton.TabIndex                =  0;
            this._noButton.Text                    =  @"NO";
            this._noButton.UseVisualStyleBackColor =  true;
            this._noButton.Click                   += new EventHandler( NoButton_Click );
            // 
            // _yesButton
            // 
            this._yesButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this._yesButton.Location                =  new System.Drawing.Point( 204, 5 );
            this._yesButton.Margin                  =  new Padding( 5 );
            this._yesButton.Name                    =  "_yesButton";
            this._yesButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._yesButton.TabIndex                =  1;
            this._yesButton.Text                    =  @"YES";
            this._yesButton.UseVisualStyleBackColor =  true;
            this._yesButton.Click                   += new EventHandler( YesButton_Click );
            // 
            // _infoLable
            // 
            this._infoLable.Dock     = DockStyle.Fill;
            this._infoLable.Font     = new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this._infoLable.Location = new System.Drawing.Point( 0, 0 );
            this._infoLable.Name     = "_infoLable";
            this._infoLable.Size     = new System.Drawing.Size( 389, 87 );
            this._infoLable.TabIndex = 2;
            this._infoLable.Text     = @"Info Currently Empty";
            // 
            // _remember
            // 
            this._remember.AutoSize                =  true;
            this._remember.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this._remember.Location                =  new System.Drawing.Point( 12, 10 );
            this._remember.Name                    =  "_remember";
            this._remember.Size                    =  new System.Drawing.Size( 174, 21 );
            this._remember.TabIndex                =  3;
            this._remember.Text                    =  @"Remember for this type";
            this._remember.UseVisualStyleBackColor =  true;
            this._remember.CheckedChanged          += new EventHandler( Remember_CheckedChanged );
            // 
            // panel1
            // 
            this._panel1.Controls.Add( this._remember );
            this._panel1.Controls.Add( this._yesButton );
            this._panel1.Controls.Add( this._noButton );
            this._panel1.Dock     = DockStyle.Bottom;
            this._panel1.Location = new System.Drawing.Point( 0, 127 );
            this._panel1.Margin   = new Padding( 0 );
            this._panel1.Name     = "_panel1";
            this._panel1.Size     = new System.Drawing.Size( 389, 38 );
            this._panel1.TabIndex = 4;
            // 
            // _DescriptionBox
            // 
            this._descriptionBox.Dock     = DockStyle.Bottom;
            this._descriptionBox.Location = new System.Drawing.Point( 0, 13 );
            this._descriptionBox.Name     = "_descriptionBox";
            this._descriptionBox.Size     = new System.Drawing.Size( 389, 27 );
            this._descriptionBox.TabIndex = 4;
            // 
            // panel2
            // 
            this._panel2.Controls.Add( this._descriptionBox );
            this._panel2.Controls.Add( this._label1 );
            this._panel2.Dock     = DockStyle.Bottom;
            this._panel2.Location = new System.Drawing.Point( 0, 87 );
            this._panel2.Name     = "_panel2";
            this._panel2.Size     = new System.Drawing.Size( 389, 40 );
            this._panel2.TabIndex = 5;
            // 
            // label1
            // 
            this._label1.Dock     = DockStyle.Fill;
            this._label1.Font     = new System.Drawing.Font( "Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this._label1.Location = new System.Drawing.Point( 0, 0 );
            this._label1.Name     = "_label1";
            this._label1.Size     = new System.Drawing.Size( 389, 40 );
            this._label1.TabIndex = 5;
            this._label1.Text     = @"Give A Description:";
            // 
            // UserInput
            // 
            this.ClientSize = new System.Drawing.Size( 389, 165 );
            this.Controls.Add( this._infoLable );
            this.Controls.Add( this._panel2 );
            this.Controls.Add( this._panel1 );
            this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254 );
            this.Name = "UserInput";
            this._panel1.ResumeLayout( false );
            this._panel1.PerformLayout();
            this._panel2.ResumeLayout( false );
            this._panel2.PerformLayout();
            ResumeLayout( false );
        }

        // ReSharper restore RedundantDelegateCreation
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