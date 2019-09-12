using System;
using System.Windows.Forms;
using UniversalShare_2;
/*
namespace Universal_Share.Interface {
    public partial class SettingsEditor : Form {
        public SettingsEditor() { InitializeComponent(); }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e) { this.F_bufferSize.Text = Math.Pow( 2, (double) this.numericUpDown1.Value ).ToString(); }

        private void B_OK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            B_Save_Click( null, null );
            ßProgram.D.Changed = true;
            this.Close();
        }

        private void F_port_ValueChanged(object sender, EventArgs e) {
            int.TryParse( this.F_port.Text, out var pop );
            //ßProgram.ST.Port = pop;
        }

        private void F_replaceString_TextChanged(object sender, EventArgs e) {
            ßProgram.ST.ReplaceString = this.F_replaceString.Text;
        }

        private void B_Save_Click(object sender, EventArgs e) {
            int.TryParse( this.F_port.Text, out var pop1 );
            ßProgram.ST.Port          = pop1;
            ßProgram.ST.ReplaceString = this.F_replaceString.Text;
            int.TryParse( this.F_bufferSize.Text, out var pop2 );
            ßProgram.ST.BufferSize = pop2;

            SavePropagation.Save( ßProgram.P );
        }

        private void F_bufferSize_TextChanged(object sender, EventArgs e) {
            int.TryParse( this.F_bufferSize.Text, out var pop );
            ßProgram.ST.BufferSize = pop;
        }

        private void SettingsEditor_Load(object sender, EventArgs e) {
            this.F_port.Maximum       = (int) Math.Pow( 2, 16 );
            this.F_port.Text          = ßProgram.ST.Port.ToString();
            this.F_bufferSize.Text    = ßProgram.ST.BufferSize.ToString();
            this.F_replaceString.Text = ßProgram.ST.ReplaceString;

            for ( int i = 0; i < 30; i++ ) {
                if ( (int) Math.Pow( 2, i + 1 ) == ßProgram.ST.BufferSize ) {
                    this.numericUpDown1.Value = i + 1;
                    return;
                }
            }
        }
    }
}*/