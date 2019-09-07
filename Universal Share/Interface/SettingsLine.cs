using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Universal_Share.Interface
{
    public partial class SettingsLine : UserControl
    {
        public SettingsLine(ref string value,ref string info) {
            InitializeComponent();
            
            this.label1.Text = info;
            this.richTextBox1.Text = value;
        }

        public SettingsLine() {
            InitializeComponent();
        }
    }
}
