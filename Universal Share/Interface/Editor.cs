using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Universal_Share.Options;

namespace Universal_Share.Interface {
    public class Editor : Form {
        private void resetAcc() {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.label1.Text   = "";
            this.label2.Text   = "";
            this.label3.Text   = "";
            this.label4.Text   = "";
            this.label5.Text   = "";
            this.label6.Text   = "";
            this.label7.Text   = "";
            this.comboBox1.Items.Clear();
        }

        public (TypeHolder, RegInfo.TYPE) CreateNewTypeHolder(object[] range) {
            resetAcc();
            this.comboBox1.Items.AddRange( range );

            this.label1.Text = "OpenWith:";
            this.label2.Text = "ArgumentsBeforePathToFile:";
            this.label3.Text = "ArgumentsAfterPathToFile:";
            this.label4.Text = "UserConfirm: (0 or 1)";
            this.label5.Text = "Description:";
            this.label6.Text = "CloseFileStream: (0 or 1)";

            var ret = ShowDialog();

            if ( Enum.TryParse<RegInfo.TYPE>( this.comboBox1.Text, out var eum ) )
                if ( ret == DialogResult.Yes )
                    return ( new TypeHolder( this.textBox1.Text, this.textBox2.Text, this.textBox3.Text, this.textBox4.Text == "1", this.textBox5.Text, this.textBox6.Text == "1" ), eum );
            throw new NotSupportedException();
        }

        public (RememberType, RegInfo.TYPE) CreateNewRememberType(object[] range) {
            resetAcc();
            this.comboBox1.Items.AddRange( range );

            this.label1.Text = "DialogResult: (1 or 0)";
            this.label2.Text = "Description:";

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( Enum.TryParse<RegInfo.TYPE>( this.comboBox1.Text, out var enu ) )
                if ( ret == DialogResult.Yes )
                    return ( new RememberType( this.textBox2.Text, ( this.textBox2.Text == "1" ) ? DialogResult.Yes : DialogResult.No, enu ), enu );
            throw new NotSupportedException();
        }

        public Editor() { InitializeComponent(); }

        private void _yesButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private void _noButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.No;
            Close();
        }

        private Panel    panel1;
        private Panel    panel2;
        private Button   _yesButton;
        private Panel    mArea;
        private Panel    panel8;
        private TextBox  textBox6;
        private Label    label6;
        private Panel    panel7;
        private TextBox  textBox5;
        private Label    label5;
        private Panel    panel6;
        private TextBox  textBox4;
        private Label    label4;
        private Panel    panel4;
        private TextBox  textBox3;
        private Label    label3;
        private Panel    panel3;
        private TextBox  textBox2;
        private Label    label2;
        private Panel    panel5;
        private TextBox  textBox1;
        private Label    label1;
        private Panel    panel9;
        private Label    label7;
        private ComboBox comboBox1;
        private Button   _noButton;

        private void InitializeComponent() {
            this.panel1     = new Panel();
            this.mArea      = new Panel();
            this.panel8     = new Panel();
            this.textBox6   = new TextBox();
            this.label6     = new Label();
            this.panel7     = new Panel();
            this.textBox5   = new TextBox();
            this.label5     = new Label();
            this.panel6     = new Panel();
            this.textBox4   = new TextBox();
            this.label4     = new Label();
            this.panel4     = new Panel();
            this.textBox3   = new TextBox();
            this.label3     = new Label();
            this.panel3     = new Panel();
            this.textBox2   = new TextBox();
            this.label2     = new Label();
            this.panel5     = new Panel();
            this.textBox1   = new TextBox();
            this.label1     = new Label();
            this.panel9     = new Panel();
            this.comboBox1  = new ComboBox();
            this.label7     = new Label();
            this.panel2     = new Panel();
            this._yesButton = new Button();
            this._noButton  = new Button();
            this.panel1.SuspendLayout();
            this.mArea.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add( this.mArea );
            this.panel1.Dock     = DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point( 0, 0 );
            this.panel1.Name     = "panel1";
            this.panel1.Size     = new System.Drawing.Size( 543, 277 );
            this.panel1.TabIndex = 0;
            // 
            // mArea
            // 
            this.mArea.Controls.Add( this.panel8 );
            this.mArea.Controls.Add( this.panel7 );
            this.mArea.Controls.Add( this.panel6 );
            this.mArea.Controls.Add( this.panel4 );
            this.mArea.Controls.Add( this.panel3 );
            this.mArea.Controls.Add( this.panel5 );
            this.mArea.Controls.Add( this.panel9 );
            this.mArea.Location = new System.Drawing.Point( 12, 12 );
            this.mArea.Name     = "mArea";
            this.mArea.Size     = new System.Drawing.Size( 519, 207 );
            this.mArea.TabIndex = 4;
            // 
            // panel8
            // 
            this.panel8.Controls.Add( this.textBox6 );
            this.panel8.Controls.Add( this.label6 );
            this.panel8.Dock     = DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point( 0, 166 );
            this.panel8.Name     = "panel8";
            this.panel8.Size     = new System.Drawing.Size( 519, 27 );
            this.panel8.TabIndex = 3;
            // 
            // textBox6
            // 
            this.textBox6.Dock     = DockStyle.Fill;
            this.textBox6.Location = new System.Drawing.Point( 249, 0 );
            this.textBox6.Name     = "textBox6";
            this.textBox6.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox6.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Dock     = DockStyle.Left;
            this.label6.Location = new System.Drawing.Point( 0, 0 );
            this.label6.Name     = "label6";
            this.label6.Size     = new System.Drawing.Size( 249, 27 );
            this.label6.TabIndex = 1;
            this.label6.Text     = "label6";
            // 
            // panel7
            // 
            this.panel7.Controls.Add( this.textBox5 );
            this.panel7.Controls.Add( this.label5 );
            this.panel7.Dock     = DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point( 0, 139 );
            this.panel7.Name     = "panel7";
            this.panel7.Size     = new System.Drawing.Size( 519, 27 );
            this.panel7.TabIndex = 3;
            // 
            // textBox5
            // 
            this.textBox5.Dock     = DockStyle.Fill;
            this.textBox5.Location = new System.Drawing.Point( 249, 0 );
            this.textBox5.Name     = "textBox5";
            this.textBox5.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox5.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Dock     = DockStyle.Left;
            this.label5.Location = new System.Drawing.Point( 0, 0 );
            this.label5.Name     = "label5";
            this.label5.Size     = new System.Drawing.Size( 249, 27 );
            this.label5.TabIndex = 1;
            this.label5.Text     = "label5";
            // 
            // panel6
            // 
            this.panel6.Controls.Add( this.textBox4 );
            this.panel6.Controls.Add( this.label4 );
            this.panel6.Dock     = DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point( 0, 112 );
            this.panel6.Name     = "panel6";
            this.panel6.Size     = new System.Drawing.Size( 519, 27 );
            this.panel6.TabIndex = 3;
            // 
            // textBox4
            // 
            this.textBox4.Dock     = DockStyle.Fill;
            this.textBox4.Location = new System.Drawing.Point( 249, 0 );
            this.textBox4.Name     = "textBox4";
            this.textBox4.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox4.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Dock     = DockStyle.Left;
            this.label4.Location = new System.Drawing.Point( 0, 0 );
            this.label4.Name     = "label4";
            this.label4.Size     = new System.Drawing.Size( 249, 27 );
            this.label4.TabIndex = 1;
            this.label4.Text     = "label4";
            // 
            // panel4
            // 
            this.panel4.Controls.Add( this.textBox3 );
            this.panel4.Controls.Add( this.label3 );
            this.panel4.Dock     = DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point( 0, 85 );
            this.panel4.Name     = "panel4";
            this.panel4.Size     = new System.Drawing.Size( 519, 27 );
            this.panel4.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Dock     = DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point( 249, 0 );
            this.textBox3.Name     = "textBox3";
            this.textBox3.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Dock     = DockStyle.Left;
            this.label3.Location = new System.Drawing.Point( 0, 0 );
            this.label3.Name     = "label3";
            this.label3.Size     = new System.Drawing.Size( 249, 27 );
            this.label3.TabIndex = 1;
            this.label3.Text     = "label3";
            // 
            // panel3
            // 
            this.panel3.Controls.Add( this.textBox2 );
            this.panel3.Controls.Add( this.label2 );
            this.panel3.Dock     = DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point( 0, 58 );
            this.panel3.Name     = "panel3";
            this.panel3.Size     = new System.Drawing.Size( 519, 27 );
            this.panel3.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Dock     = DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point( 249, 0 );
            this.textBox2.Name     = "textBox2";
            this.textBox2.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Dock     = DockStyle.Left;
            this.label2.Location = new System.Drawing.Point( 0, 0 );
            this.label2.Name     = "label2";
            this.label2.Size     = new System.Drawing.Size( 249, 27 );
            this.label2.TabIndex = 1;
            this.label2.Text     = "label2";
            // 
            // panel5
            // 
            this.panel5.Controls.Add( this.textBox1 );
            this.panel5.Controls.Add( this.label1 );
            this.panel5.Dock     = DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point( 0, 31 );
            this.panel5.Name     = "panel5";
            this.panel5.Size     = new System.Drawing.Size( 519, 27 );
            this.panel5.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Dock     = DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point( 249, 0 );
            this.textBox1.Name     = "textBox1";
            this.textBox1.Size     = new System.Drawing.Size( 270, 27 );
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock     = DockStyle.Left;
            this.label1.Location = new System.Drawing.Point( 0, 0 );
            this.label1.Name     = "label1";
            this.label1.Size     = new System.Drawing.Size( 249, 27 );
            this.label1.TabIndex = 1;
            this.label1.Text     = "label1";
            // 
            // panel9
            // 
            this.panel9.Controls.Add( this.comboBox1 );
            this.panel9.Controls.Add( this.label7 );
            this.panel9.Dock     = DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point( 0, 0 );
            this.panel9.Name     = "panel9";
            this.panel9.Size     = new System.Drawing.Size( 519, 31 );
            this.panel9.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock              = DockStyle.Fill;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location          = new System.Drawing.Point( 249, 0 );
            this.comboBox1.Name              = "comboBox1";
            this.comboBox1.Size              = new System.Drawing.Size( 270, 28 );
            this.comboBox1.TabIndex          = 1;
            // 
            // label7
            // 
            this.label7.Dock     = DockStyle.Left;
            this.label7.Location = new System.Drawing.Point( 0, 0 );
            this.label7.Name     = "label7";
            this.label7.Size     = new System.Drawing.Size( 249, 31 );
            this.label7.TabIndex = 2;
            this.label7.Text     = "label7";
            // 
            // panel2
            // 
            this.panel2.Controls.Add( this._yesButton );
            this.panel2.Controls.Add( this._noButton );
            this.panel2.Dock     = DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point( 0, 232 );
            this.panel2.Name     = "panel2";
            this.panel2.Size     = new System.Drawing.Size( 543, 45 );
            this.panel2.TabIndex = 0;
            // 
            // _yesButton
            // 
            this._yesButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._yesButton.Location                =  new System.Drawing.Point( 14, 8 );
            this._yesButton.Margin                  =  new Padding( 5 );
            this._yesButton.Name                    =  "_yesButton";
            this._yesButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._yesButton.TabIndex                =  3;
            this._yesButton.Text                    =  "YES";
            this._yesButton.UseVisualStyleBackColor =  true;
            this._yesButton.Click                   += new EventHandler( _yesButton_Click );
            // 
            // _noButton
            // 
            this._noButton.Font                    =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this._noButton.Location                =  new System.Drawing.Point( 109, 8 );
            this._noButton.Margin                  =  new Padding( 5 );
            this._noButton.Name                    =  "_noButton";
            this._noButton.Size                    =  new System.Drawing.Size( 85, 30 );
            this._noButton.TabIndex                =  2;
            this._noButton.Text                    =  "NO";
            this._noButton.UseVisualStyleBackColor =  true;
            this._noButton.Click                   += new EventHandler( _noButton_Click );
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size( 543, 277 );
            this.Controls.Add( this.panel2 );
            this.Controls.Add( this.panel1 );
            this.Font            =  new System.Drawing.Font( "Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte) ( 254 ) ) );
            this.FormBorderStyle =  FormBorderStyle.FixedToolWindow;
            this.Name            =  "Editor";
            this.FormClosing     += new FormClosingEventHandler( Editor_FormClosing );
            this.panel1.ResumeLayout( false );
            this.mArea.ResumeLayout( false );
            this.panel8.ResumeLayout( false );
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout( false );
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout( false );
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout( false );
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout( false );
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout( false );
            this.panel5.PerformLayout();
            this.panel9.ResumeLayout( false );
            this.panel2.ResumeLayout( false );
            ResumeLayout( false );
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e) {
            if ( this.DialogResult == DialogResult.Cancel ) e.Cancel = true;
        }

        public (RememberType, RegInfo.TYPE) EditRememberType(object[] allTypes, RememberType baseType) {
            resetAcc();
            this.comboBox1.Items.AddRange( allTypes );

            this.label1.Text   = "DialogResult: (1 or 0)";
            this.textBox1.Text = baseType.IsOkOrYes() ? "1" : "0";
            this.label2.Text   = "Description:";
            this.textBox2.Text = baseType.Description;

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( Enum.TryParse<RegInfo.TYPE>( this.comboBox1.Text, out var enu ) )
                if ( ret == DialogResult.Yes )
                    return ( new RememberType( this.textBox2.Text, ( this.textBox1.Text == "1" ) ? DialogResult.Yes : DialogResult.No, enu ), enu );
                else {
                    return ( baseType, baseType.Type );
                }
            throw new NotSupportedException();
        }

        public (TypeHolder, RegInfo.TYPE) EditTypeHolder(object[] allTypes, TypeHolder baseTypeHolder) {
            resetAcc();
            this.comboBox1.Items.AddRange( allTypes );

            this.label1.Text   = "OpenWith:";
            this.textBox1.Text = baseTypeHolder.OpenWith;
            this.label2.Text   = "ArgumentsBeforePathToFile:";
            this.textBox2.Text = baseTypeHolder.ArgumentsBeforePathToFile;
            this.label3.Text   = "ArgumentsAfterPathToFile:";
            this.textBox3.Text = baseTypeHolder.ArgumentsAfterPathToFile;
            this.label4.Text   = "UserConfirm: (0 or 1)";
            this.textBox4.Text = baseTypeHolder.UserConfirm ? "0" : "1";
            this.label5.Text   = "Description:";
            this.textBox5.Text = baseTypeHolder.Description;
            this.label6.Text   = "CloseFileStream: (0 or 1)";
            this.textBox6.Text = baseTypeHolder.CloseFileStream ? "0" : "1";

            var ret = ShowDialog();

            if ( Enum.TryParse<RegInfo.TYPE>( this.comboBox1.Text, out var eum ) )
                if ( ret == DialogResult.Yes )
                    return ( new TypeHolder( this.textBox1.Text, this.textBox2.Text, this.textBox3.Text, this.textBox4.Text == "1", this.textBox5.Text, this.textBox6.Text == "1" ), eum );
                else
                    return ( baseTypeHolder, eum );
            throw new NotSupportedException();
        }
    }
}