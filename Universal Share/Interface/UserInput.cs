using System;
using System.Drawing;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share;
using Universal_Share.ProgMain;

namespace Universal_Share.Interface {
    public class UserInput : Form {
        public static bool  chackboxChecked;
        private       Label InfoLable;

        private Button   NoButton;
        private CheckBox Remember;
        private Button   YesButton;
        public UserInput() { InitializeComponent(); }

        //Console.WriteLine(us.GetConfirm( new RegInfo(null,-1,@"..\\..\\..\\test.txt", RegInfo.TYPE.SINGLE_FILE), new TypeHolder(@"C:\windows\system32\cmd.exe","-c pause",true," descript",false)));
        public bool GetConfirm(RegInfo reg, TypeHolder th) {
            var ret  = ShowDialog();
            var bret = ret == DialogResult.Yes || ret == DialogResult.OK;
            if ( chackboxChecked ) ßMainPoint.S.RememberType.Add( reg.Type, ret );

            return bret;
        }

        private void InitializeComponent() {
            this.NoButton  = new Button();
            this.YesButton = new Button();
            this.InfoLable = new Label();
            this.Remember  = new CheckBox();
            SuspendLayout();
            // 
            // NoButton
            // 
            this.NoButton.Font                    =  new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.NoButton.Location                =  new Point( 292, 106 );
            this.NoButton.Name                    =  "NoButton";
            this.NoButton.Size                    =  new Size( 85, 30 );
            this.NoButton.TabIndex                =  0;
            this.NoButton.Text                    =  "NO";
            this.NoButton.UseVisualStyleBackColor =  true;
            this.NoButton.Click                   += NoButton_Click;
            // 
            // YesButton
            // 
            this.YesButton.Font                    =  new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.YesButton.Location                =  new Point( 201, 106 );
            this.YesButton.Name                    =  "YesButton";
            this.YesButton.Size                    =  new Size( 85, 30 );
            this.YesButton.TabIndex                =  1;
            this.YesButton.Text                    =  "YES";
            this.YesButton.UseVisualStyleBackColor =  true;
            this.YesButton.Click                   += YesButton_Click;
            // 
            // InfoLable
            // 
            this.InfoLable.AutoSize = true;
            this.InfoLable.Font     = new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.InfoLable.Location = new Point( 8, 9 );
            this.InfoLable.Name     = "InfoLable";
            this.InfoLable.Size     = new Size( 162, 22 );
            this.InfoLable.TabIndex = 2;
            this.InfoLable.Text     = "Info Currently Emty";
            // 
            // Remember
            // 
            this.Remember.AutoSize                =  true;
            this.Remember.Font                    =  new Font( "Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.Remember.Location                =  new Point( 12, 113 );
            this.Remember.Name                    =  "Remember";
            this.Remember.Size                    =  new Size( 174, 21 );
            this.Remember.TabIndex                =  3;
            this.Remember.Text                    =  "Remember for this type";
            this.Remember.UseVisualStyleBackColor =  true;
            this.Remember.CheckedChanged          += Remember_CheckedChanged;
            // 
            // UserInput
            // 
            this.ClientSize = new Size( 389, 148 );
            this.Controls.Add( this.Remember );
            this.Controls.Add( this.InfoLable );
            this.Controls.Add( this.YesButton );
            this.Controls.Add( this.NoButton );
            this.Font = new Font( "Microsoft Sans Serif", 13F, FontStyle.Regular, GraphicsUnit.Point, 254 );
            this.Name = "UserInput";
            ResumeLayout( false );
            PerformLayout();
        }

        private void Remember_CheckedChanged(object sender, EventArgs e) { chackboxChecked = this.Remember.Checked; }

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