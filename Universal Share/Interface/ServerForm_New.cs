using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Universal_Share.Interface {
    public partial class ServerForm {
        public ListViewItem[] ListViewFind(ListView ls, string key, bool sub) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => { _tmp = ls.Items.Find( key, sub ); } ) );
            else
                _tmp = ls.Items.Find( key, sub );
            return _tmp;
        }

        public void ListViewAdd(ListView ls, ListViewItem item) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => { ls.Items.Add( item ); } ) );
            else
                ls.Items.Add( item );
        }

        public void ListViewRemove(ListView ls, ListViewItem item) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => { ls.Items.Remove( item ); } ) );
            else
                ls.Items.Remove( item );
        }
    }
}