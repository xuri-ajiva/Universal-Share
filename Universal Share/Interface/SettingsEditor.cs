using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share.ProgMain;

namespace Universal_Share.Interface {
    public partial class SettingsEditor : Form {
        public SettingsEditor() { InitializeComponent(); }

        private void NumericUpDown1_ValueChanged(object sender, EventArgs e) { this.F_bufferSize.Text = Math.Pow( 2, (double) this.numericUpDown1.Value ).ToString(); }

        private void B_OK_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            B_Save_Click( null, null );
            ßMainPoint.S.Changed = true;
            this.Close();
        }

        private void F_port_ValueChanged(object sender, EventArgs e) {
            int.TryParse( this.F_port.Text, out var pop );
            ßMainPoint.ST.Port = pop;
        }

        private void F_replaceString_TextChanged(object sender, EventArgs e) { ßMainPoint.ST.ReplaceString = this.F_replaceString.Text; }

        private void B_Save_Click(object sender, EventArgs e) {
            int.TryParse( this.F_port.Text, out var pop1 );
            ßMainPoint.ST.Port          = pop1;
            ßMainPoint.ST.ReplaceString = this.F_replaceString.Text;
            int.TryParse( this.F_bufferSize.Text, out var pop2 );
            ßMainPoint.ST.BufferSize = pop2;

            SavePropagation.Save( ßMainPoint.P );
        }

        private void F_bufferSize_TextChanged(object sender, EventArgs e) {
            int.TryParse( this.F_bufferSize.Text, out var pop );
            ßMainPoint.ST.BufferSize = pop;
        }

        private void SettingsEditor_Load(object sender, EventArgs e) {
            this.F_port.Maximum       = (int) Math.Pow( 2, 16 );
            this.F_port.Text          = ßMainPoint.ST.Port.ToString();
            this.F_bufferSize.Text    = ßMainPoint.ST.BufferSize.ToString();
            this.F_replaceString.Text = ßMainPoint.ST.ReplaceString;

            for ( int i = 0; i < 30; i++ ) {
                if ( (int) Math.Pow( 2, i + 1 ) == ßMainPoint.ST.BufferSize ) {
                    this.numericUpDown1.Value = i + 1;
                    return;
                }
            }
        }
    }
}