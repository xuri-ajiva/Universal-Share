using System;
using System.Diagnostics;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share.Security;

namespace Universal_Share.Interface {
    public class Editor : Form {
        [DebuggerStepThrough]
        private void ResetAcc() {
            this._textBox1.Text = "";
            this._textBox2.Text = "";
            this._textBox3.Text = "";
            this._textBox4.Text = "";
            this._textBox5.Text = "";
            this._textBox6.Text = "";
            this._label1.Text   = "";
            this._label2.Text   = "";
            this._label3.Text   = "";
            this._label4.Text   = "";
            this._label5.Text   = "";
            this._label6.Text   = "";
            this._label7.Text   = "";
            try {
                this._comboBox1.DataSource = null;
                this._comboBox1.Items.Clear();
            } catch {
                // ignored
            }
        }

        public (TypeHolder, RegInfo.Type) CreateNewTypeHolder() {
            ResetAcc();
            this._comboBox1.DataSource = Enum.GetValues( typeof(RegInfo.Type) );

            this._label1.Text = Resources.Editor_CreateNewTypeHolder_OpenWith_;
            this._label2.Text = Resources.Editor_CreateNewTypeHolder_ArgumentsBeforePathToFile_;
            this._label3.Text = Resources.Editor_CreateNewTypeHolder_ArgumentsAfterPathToFile_;
            this._label4.Text = Resources.Editor_CreateNewTypeHolder_UserConfirm_ +  Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._label5.Text = Resources.Editor_CreateNewTypeHolder_Description_;
            this._label6.Text = Resources.Editor_CreateNewTypeHolder_CloseFileStream_+Resources.Editor_CreateNewTypeHolder___0_or_1_;

            var ret = ShowDialog();

            if ( Enum.TryParse<RegInfo.Type>( this._comboBox1.Text, out var eum ) )
                if ( ret == DialogResult.Yes )
                    return ( new TypeHolder( this._textBox1.Text, this._textBox2.Text, this._textBox3.Text, this._textBox4.Text == Resources.Editor_CreateNewTypeHolder__True, this._textBox5.Text, this._textBox6.Text == Resources.Editor_CreateNewTypeHolder__True ), eum );
            throw new NotSupportedException();
        }

        public (RememberType, RegInfo.Type) CreateNewRememberType() {
            ResetAcc();
            this._comboBox1.DataSource = Enum.GetValues( typeof(RegInfo.Type) );

            this._label1.Text = Resources.Editor_CreateNewRememberType_DialogResult_+Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._label2.Text = Resources.Editor_CreateNewTypeHolder_Description_;

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( Enum.TryParse<RegInfo.Type>( this._comboBox1.Text, out var enu ) )
                if ( ret == DialogResult.Yes )
                    return ( new RememberType( this._textBox2.Text, ( this._textBox2.Text == Resources.Editor_CreateNewTypeHolder__True ) ? DialogResult.Yes : DialogResult.No, enu ), enu );
            throw new NotSupportedException();
        }

        public (TokenItem, string) CreateNewTokenItem() {
            ResetAcc();
            //this.comboBox1.DataSource = Enum.GetValues( typeof(RegInfo.TYPE) );

            this._label1.Text = Resources.Editor_CreateNewTokenItem_Base64Token_;
            this._label2.Text = Resources.Editor_CreateNewTokenItem_Trusted_+Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._label3.Text = Resources.Editor_CreateNewTokenItem_Remember_+Resources.Editor_CreateNewTypeHolder___0_or_1_;

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( ret == DialogResult.Yes ) {
                var token = new TokenItem( new Byte[64], ( this._textBox2.Text == Resources.Editor_CreateNewTypeHolder__True ), ( this._textBox3.Text == Resources.Editor_CreateNewTypeHolder__True ) );
                try {
                    token.Base64Key = this._textBox1.Text;
                } catch (Exception e) {
                    MessageBox.Show( e.Message );
                    throw new NotSupportedException();
                }

                return ( token, this._textBox1.Text );
            }

            throw new NotSupportedException();
        }

        public (RememberType, RegInfo.Type) EditRememberType(RememberType baseType) {
            ResetAcc();
            this._comboBox1.DataSource   = Enum.GetValues( typeof(RegInfo.Type) );
            this._comboBox1.SelectedItem = baseType.Type;

            this._comboBox1.SelectedItem = baseType.Type.ToString();

            this._label1.Text   = Resources.Editor_CreateNewRememberType_DialogResult_;
            this._textBox1.Text = baseType.IsOkOrYes() ? Resources.Editor_CreateNewTypeHolder__True : "0";
            this._label2.Text   = Resources.Editor_CreateNewTypeHolder_Description_;
            this._textBox2.Text = baseType.Description;

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( Enum.TryParse<RegInfo.Type>( this._comboBox1.Text, out var enu ) )
                if ( ret == DialogResult.Yes )
                    return ( new RememberType( this._textBox2.Text, ( this._textBox1.Text == Resources.Editor_CreateNewTypeHolder__True ) ? DialogResult.Yes : DialogResult.No, enu ), enu );
                else {
                    return ( baseType, baseType.Type );
                }

            throw new NotSupportedException();
        }

        public (TypeHolder, RegInfo.Type) EditTypeHolder(TypeHolder baseTypeHolder, RegInfo.Type currenType) {
            ResetAcc();
            this._comboBox1.DataSource   = Enum.GetValues( typeof(RegInfo.Type) );
            this._comboBox1.SelectedItem = currenType;

            this._label1.Text   = Resources.Editor_CreateNewTypeHolder_OpenWith_;
            this._textBox1.Text = baseTypeHolder.OpenWith;
            this._label2.Text   = Resources.Editor_CreateNewTypeHolder_ArgumentsBeforePathToFile_;
            this._textBox2.Text = baseTypeHolder.ArgumentsBeforePathToFile;
            this._label3.Text   = Resources.Editor_CreateNewTypeHolder_ArgumentsAfterPathToFile_;
            this._textBox3.Text = baseTypeHolder.ArgumentsAfterPathToFile;
            this._label4.Text   = Resources.Editor_CreateNewTypeHolder_UserConfirm_ +Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._textBox4.Text = baseTypeHolder.UserConfirm ? "0" : Resources.Editor_CreateNewTypeHolder__True;
            this._label5.Text   = Resources.Editor_CreateNewTypeHolder_Description_;
            this._textBox5.Text = baseTypeHolder.Description;
            this._label6.Text   = Resources.Editor_CreateNewTypeHolder_CloseFileStream_ +Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._textBox6.Text = baseTypeHolder.CloseFileStream ? "0" : Resources.Editor_CreateNewTypeHolder__True;

            var ret = ShowDialog();

            if ( Enum.TryParse<RegInfo.Type>( this._comboBox1.Text, out var eum ) )
                if ( ret == DialogResult.Yes )
                    return ( new TypeHolder( this._textBox1.Text, this._textBox2.Text, this._textBox3.Text, this._textBox4.Text == Resources.Editor_CreateNewTypeHolder__True, this._textBox5.Text, this._textBox6.Text == Resources.Editor_CreateNewTypeHolder__True ), eum );
                else
                    return ( baseTypeHolder, eum );
            throw new NotSupportedException();
        }

        public (TokenItem, string) EditTokenItem(TokenItem baseTokenItem) {
            ResetAcc();
            //this.comboBox1.DataSource = Enum.GetValues( typeof(RegInfo.TYPE) );

            this._label1.Text   = Resources.Editor_CreateNewTokenItem_Base64Token_;
            this._textBox1.Text = baseTokenItem.Base64Key;
            this._label2.Text   = Resources.Editor_CreateNewTokenItem_Trusted_ +Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._textBox2.Text = baseTokenItem.Trusted ? Resources.Editor_CreateNewTypeHolder__True : "0";
            this._label3.Text   = Resources.Editor_CreateNewTokenItem_Remember_ +Resources.Editor_CreateNewTypeHolder___0_or_1_;
            this._textBox3.Text = baseTokenItem.Remember ? Resources.Editor_CreateNewTypeHolder__True : "0";

            var ret = ShowDialog();
            this.DialogResult = DialogResult.Retry;

            if ( ret == DialogResult.Yes ) {
                var token = new TokenItem( new Byte[64], ( this._textBox2.Text == Resources.Editor_CreateNewTypeHolder__True ), ( this._textBox3.Text == Resources.Editor_CreateNewTypeHolder__True ) );
                try {
                    token.Base64Key = this._textBox1.Text;
                } catch (Exception e) {
                    MessageBox.Show( e.Message );
                    return ( baseTokenItem, baseTokenItem.Base64Key );
                }

                return ( token, this._textBox1.Text );
            }

            return ( baseTokenItem, baseTokenItem.Base64Key );
        }

        private void _yesButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Yes;
            Close();
        }

        private void _noButton_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.No;
            Close();
        }

        #region UXDesiner
        
        // ReSharper disable RedundantDelegateCreation
        public Editor() { InitializeComponent(); }

        private Panel    _panel1;
        private Panel    _panel2;
        private Button   _yesButton;
        private Panel    _mArea;
        private Panel    _panel8;
        private TextBox  _textBox6;
        private Label    _label6;
        private Panel    _panel7;
        private TextBox  _textBox5;
        private Label    _label5;
        private Panel    _panel6;
        private TextBox  _textBox4;
        private Label    _label4;
        private Panel    _panel4;
        private TextBox  _textBox3;
        private Label    _label3;
        private Panel    _panel3;
        private TextBox  _textBox2;
        private Label    _label2;
        private Panel    _panel5;
        private TextBox  _textBox1;
        private Label    _label1;
        private Panel    _panel9;
        private Label    _label7;
        private ComboBox _comboBox1;
        private Button   _noButton;

        private void InitializeComponent() {
            this._panel1 = new Panel();
            this._mArea = new Panel();
            this._panel8 = new Panel();
            this._textBox6 = new TextBox();
            this._label6 = new Label();
            this._panel7 = new Panel();
            this._textBox5 = new TextBox();
            this._label5 = new Label();
            this._panel6 = new Panel();
            this._textBox4 = new TextBox();
            this._label4 = new Label();
            this._panel4 = new Panel();
            this._textBox3 = new TextBox();
            this._label3 = new Label();
            this._panel3 = new Panel();
            this._textBox2 = new TextBox();
            this._label2 = new Label();
            this._panel5 = new Panel();
            this._textBox1 = new TextBox();
            this._label1 = new Label();
            this._panel9 = new Panel();
            this._comboBox1 = new ComboBox();
            this._label7 = new Label();
            this._panel2 = new Panel();
            this._yesButton = new Button();
            this._noButton = new Button();
            this._panel1.SuspendLayout();
            this._mArea.SuspendLayout();
            this._panel8.SuspendLayout();
            this._panel7.SuspendLayout();
            this._panel6.SuspendLayout();
            this._panel4.SuspendLayout();
            this._panel3.SuspendLayout();
            this._panel5.SuspendLayout();
            this._panel9.SuspendLayout();
            this._panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            this._panel1.Controls.Add(this._mArea);
            this._panel1.Dock = DockStyle.Fill;
            this._panel1.Location = new System.Drawing.Point(0, 0);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(543, 277);
            this._panel1.TabIndex = 0;
            // 
            // mArea
            // 
            this._mArea.Controls.Add(this._panel8);
            this._mArea.Controls.Add(this._panel7);
            this._mArea.Controls.Add(this._panel6);
            this._mArea.Controls.Add(this._panel4);
            this._mArea.Controls.Add(this._panel3);
            this._mArea.Controls.Add(this._panel5);
            this._mArea.Controls.Add(this._panel9);
            this._mArea.Location = new System.Drawing.Point(12, 12);
            this._mArea.Name = "_mArea";
            this._mArea.Size = new System.Drawing.Size(519, 207);
            this._mArea.TabIndex = 4;
            // 
            // panel8
            // 
            this._panel8.Controls.Add(this._textBox6);
            this._panel8.Controls.Add(this._label6);
            this._panel8.Dock = DockStyle.Top;
            this._panel8.Location = new System.Drawing.Point(0, 166);
            this._panel8.Name = "_panel8";
            this._panel8.Size = new System.Drawing.Size(519, 27);
            this._panel8.TabIndex = 3;
            // 
            // textBox6
            // 
            this._textBox6.Dock = DockStyle.Fill;
            this._textBox6.Location = new System.Drawing.Point(249, 0);
            this._textBox6.Name = "_textBox6";
            this._textBox6.Size = new System.Drawing.Size(270, 27);
            this._textBox6.TabIndex = 0;
            // 
            // label6
            // 
            this._label6.Dock = DockStyle.Left;
            this._label6.Location = new System.Drawing.Point(0, 0);
            this._label6.Name = "_label6";
            this._label6.Size = new System.Drawing.Size(249, 27);
            this._label6.TabIndex = 1;
            this._label6.Text = @"label6";
            // 
            // panel7
            // 
            this._panel7.Controls.Add(this._textBox5);
            this._panel7.Controls.Add(this._label5);
            this._panel7.Dock = DockStyle.Top;
            this._panel7.Location = new System.Drawing.Point(0, 139);
            this._panel7.Name = "_panel7";
            this._panel7.Size = new System.Drawing.Size(519, 27);
            this._panel7.TabIndex = 3;
            // 
            // textBox5
            // 
            this._textBox5.Dock = DockStyle.Fill;
            this._textBox5.Location = new System.Drawing.Point(249, 0);
            this._textBox5.Name = "_textBox5";
            this._textBox5.Size = new System.Drawing.Size(270, 27);
            this._textBox5.TabIndex = 0;
            // 
            // label5
            // 
            this._label5.Dock = DockStyle.Left;
            this._label5.Location = new System.Drawing.Point(0, 0);
            this._label5.Name = "_label5";
            this._label5.Size = new System.Drawing.Size(249, 27);
            this._label5.TabIndex = 1;
            this._label5.Text = @"label5";
            // 
            // panel6
            // 
            this._panel6.Controls.Add(this._textBox4);
            this._panel6.Controls.Add(this._label4);
            this._panel6.Dock = DockStyle.Top;
            this._panel6.Location = new System.Drawing.Point(0, 112);
            this._panel6.Name = "_panel6";
            this._panel6.Size = new System.Drawing.Size(519, 27);
            this._panel6.TabIndex = 3;
            // 
            // textBox4
            // 
            this._textBox4.Dock = DockStyle.Fill;
            this._textBox4.Location = new System.Drawing.Point(249, 0);
            this._textBox4.Name = "_textBox4";
            this._textBox4.Size = new System.Drawing.Size(270, 27);
            this._textBox4.TabIndex = 0;
            // 
            // label4
            // 
            this._label4.Dock = DockStyle.Left;
            this._label4.Location = new System.Drawing.Point(0, 0);
            this._label4.Name = "_label4";
            this._label4.Size = new System.Drawing.Size(249, 27);
            this._label4.TabIndex = 1;
            this._label4.Text = @"label4";
            // 
            // panel4
            // 
            this._panel4.Controls.Add(this._textBox3);
            this._panel4.Controls.Add(this._label3);
            this._panel4.Dock = DockStyle.Top;
            this._panel4.Location = new System.Drawing.Point(0, 85);
            this._panel4.Name = "_panel4";
            this._panel4.Size = new System.Drawing.Size(519, 27);
            this._panel4.TabIndex = 3;
            // 
            // textBox3
            // 
            this._textBox3.Dock = DockStyle.Fill;
            this._textBox3.Location = new System.Drawing.Point(249, 0);
            this._textBox3.Name = "_textBox3";
            this._textBox3.Size = new System.Drawing.Size(270, 27);
            this._textBox3.TabIndex = 0;
            // 
            // label3
            // 
            this._label3.Dock = DockStyle.Left;
            this._label3.Location = new System.Drawing.Point(0, 0);
            this._label3.Name = "_label3";
            this._label3.Size = new System.Drawing.Size(249, 27);
            this._label3.TabIndex = 1;
            this._label3.Text = @"label3";
            // 
            // panel3
            // 
            this._panel3.Controls.Add(this._textBox2);
            this._panel3.Controls.Add(this._label2);
            this._panel3.Dock = DockStyle.Top;
            this._panel3.Location = new System.Drawing.Point(0, 58);
            this._panel3.Name = "_panel3";
            this._panel3.Size = new System.Drawing.Size(519, 27);
            this._panel3.TabIndex = 3;
            // 
            // textBox2
            // 
            this._textBox2.Dock = DockStyle.Fill;
            this._textBox2.Location = new System.Drawing.Point(249, 0);
            this._textBox2.Name = "_textBox2";
            this._textBox2.Size = new System.Drawing.Size(270, 27);
            this._textBox2.TabIndex = 0;
            // 
            // label2
            // 
            this._label2.Dock = DockStyle.Left;
            this._label2.Location = new System.Drawing.Point(0, 0);
            this._label2.Name = "_label2";
            this._label2.Size = new System.Drawing.Size(249, 27);
            this._label2.TabIndex = 1;
            this._label2.Text = @"label2";
            // 
            // panel5
            // 
            this._panel5.Controls.Add(this._textBox1);
            this._panel5.Controls.Add(this._label1);
            this._panel5.Dock = DockStyle.Top;
            this._panel5.Location = new System.Drawing.Point(0, 31);
            this._panel5.Name = "_panel5";
            this._panel5.Size = new System.Drawing.Size(519, 27);
            this._panel5.TabIndex = 2;
            // 
            // textBox1
            // 
            this._textBox1.Dock = DockStyle.Fill;
            this._textBox1.Location = new System.Drawing.Point(249, 0);
            this._textBox1.Name = "_textBox1";
            this._textBox1.Size = new System.Drawing.Size(270, 27);
            this._textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this._label1.Dock = DockStyle.Left;
            this._label1.Location = new System.Drawing.Point(0, 0);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(249, 27);
            this._label1.TabIndex = 1;
            this._label1.Text = @"label1";
            // 
            // panel9
            // 
            this._panel9.Controls.Add(this._comboBox1);
            this._panel9.Controls.Add(this._label7);
            this._panel9.Dock = DockStyle.Top;
            this._panel9.Location = new System.Drawing.Point(0, 0);
            this._panel9.Name = "_panel9";
            this._panel9.Size = new System.Drawing.Size(519, 31);
            this._panel9.TabIndex = 2;
            // 
            // comboBox1
            // 
            this._comboBox1.Dock = DockStyle.Fill;
            this._comboBox1.FormattingEnabled = true;
            this._comboBox1.Location = new System.Drawing.Point(249, 0);
            this._comboBox1.Name = "_comboBox1";
            this._comboBox1.Size = new System.Drawing.Size(270, 28);
            this._comboBox1.TabIndex = 1;
            // 
            // label7
            // 
            this._label7.Dock = DockStyle.Left;
            this._label7.Location = new System.Drawing.Point(0, 0);
            this._label7.Name = "_label7";
            this._label7.Size = new System.Drawing.Size(249, 31);
            this._label7.TabIndex = 2;
            this._label7.Text = @"label7";
            // 
            // panel2
            // 
            this._panel2.Controls.Add(this._yesButton);
            this._panel2.Controls.Add(this._noButton);
            this._panel2.Dock = DockStyle.Bottom;
            this._panel2.Location = new System.Drawing.Point(0, 232);
            this._panel2.Name = "_panel2";
            this._panel2.Size = new System.Drawing.Size(543, 45);
            this._panel2.TabIndex = 0;
            // 
            // _yesButton
            // 
            this._yesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254);
            this._yesButton.Location = new System.Drawing.Point(14, 8);
            this._yesButton.Margin = new Padding(5);
            this._yesButton.Name = "_yesButton";
            this._yesButton.Size = new System.Drawing.Size(85, 30);
            this._yesButton.TabIndex = 3;
            this._yesButton.Text = @"YES";
            this._yesButton.UseVisualStyleBackColor = true;
            this._yesButton.Click += new EventHandler(_yesButton_Click);
            // 
            // _noButton
            // 
            this._noButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254);
            this._noButton.Location = new System.Drawing.Point(109, 8);
            this._noButton.Margin = new Padding(5);
            this._noButton.Name = "_noButton";
            this._noButton.Size = new System.Drawing.Size(85, 30);
            this._noButton.TabIndex = 2;
            this._noButton.Text = @"NO";
            this._noButton.UseVisualStyleBackColor = true;
            this._noButton.Click += new EventHandler(_noButton_Click);
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size(543, 277);
            this.Controls.Add(this._panel2);
            this.Controls.Add(this._panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 254);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.Name = "Editor";
            this.FormClosing += new FormClosingEventHandler(Editor_FormClosing);
            this._panel1.ResumeLayout(false);
            this._mArea.ResumeLayout(false);
            this._panel8.ResumeLayout(false);
            this._panel8.PerformLayout();
            this._panel7.ResumeLayout(false);
            this._panel7.PerformLayout();
            this._panel6.ResumeLayout(false);
            this._panel6.PerformLayout();
            this._panel4.ResumeLayout(false);
            this._panel4.PerformLayout();
            this._panel3.ResumeLayout(false);
            this._panel3.PerformLayout();
            this._panel5.ResumeLayout(false);
            this._panel5.PerformLayout();
            this._panel9.ResumeLayout(false);
            this._panel2.ResumeLayout(false);
            ResumeLayout(false);

        }
        
        // ReSharper restore RedundantDelegateCreation
        #endregion

        private void Editor_FormClosing(object sender, FormClosingEventArgs e) {
            if ( this.DialogResult == DialogResult.Cancel ) e.Cancel = true;
        }
    }
}