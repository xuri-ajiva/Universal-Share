using System;
using System.Drawing;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share.ProgMain;

namespace Universal_Share.Interface {
    public class UserInput : Form {
        public static bool  ChackboxChecked;
        private       Label _infoLable;

        private Button   _noButton;
        private CheckBox _remember;
        private Button   _yesButton;
        public UserInput() { InitializeComponent(); }

        //Console.WriteLine(us.GetConfirm( new RegInfo(null,-1,@"..\\..\\..\\test.txt", RegInfo.TYPE.SINGLE_FILE), new TypeHolder(@"C:\windows\system32\cmd.exe","-c pause",true," descript",false)));
        public bool GetConfirm(RegInfo reg, TypeHolder th) {
            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( ChackboxChecked ) ßMainPoint.S.RememberType.Add( reg.Type, ret );

            return bret;
        }

        private void InitializeComponent() {
            this._noButton  = new Button();
            this._yesButton = new Button();
            this._infoLable = new Label();
            this._remember  = new CheckBox();
            SuspendLayout();
            // 
            // NoButton
            // 
            this._noButton.Font                    =  new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this._noButton.Location                =  new Point( 292, 106 );
            this._noButton.Name                    =  "_noButton";
            this._noButton.Size                    =  new Size( 85, 30 );
            this._noButton.TabIndex                =  0;
            this._noButton.Text                    =  "NO";
            this._noButton.UseVisualStyleBackColor =  true;
            this._noButton.Click                   += NoButton_Click;
            // 
            // YesButton
            // 
            this._yesButton.Font                    =  new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this._yesButton.Location                =  new Point( 201, 106 );
            this._yesButton.Name                    =  "_yesButton";
            this._yesButton.Size                    =  new Size( 85, 30 );
            this._yesButton.TabIndex                =  1;
            this._yesButton.Text                    =  "YES";
            this._yesButton.UseVisualStyleBackColor =  true;
            this._yesButton.Click                   += YesButton_Click;
            // 
            // InfoLable
            // 
            this._infoLable.AutoSize = true;
            this._infoLable.Font     = new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this._infoLable.Location = new Point( 8, 9 );
            this._infoLable.Name     = "_infoLable";
            this._infoLable.Size     = new Size( 162, 22 );
            this._infoLable.TabIndex = 2;
            this._infoLable.Text     = "Info Currently Emty";
            // 
            // Remember
            // 
            this._remember.AutoSize                =  true;
            this._remember.Font                    =  new Font( "Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this._remember.Location                =  new Point( 12, 113 );
            this._remember.Name                    =  "_remember";
            this._remember.Size                    =  new Size( 174, 21 );
            this._remember.TabIndex                =  3;
            this._remember.Text                    =  "Remember for this type";
            this._remember.UseVisualStyleBackColor =  true;
            this._remember.CheckedChanged          += Remember_CheckedChanged;
            // 
            // UserInput
            // 
            this.ClientSize = new Size( 389, 148 );
            this.Controls.Add( this._remember );
            this.Controls.Add( this._infoLable );
            this.Controls.Add( this._yesButton );
            this.Controls.Add( this._noButton );
            this.Font = new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.Name = "UserInput";
            ResumeLayout( false );
            PerformLayout();
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