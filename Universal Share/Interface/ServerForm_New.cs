using System;
using System.Diagnostics;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Interface {
    public partial class MainFormP {
        public ListViewItem ListViewFind(ListView ls, string key, bool sub) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => { Tmp = ls.Items.Find( key, sub ); } ) );
            else
                Tmp = ls.Items.Find( key, sub );
            return Tmp?.Length > 0 ? Tmp?[0] : null;
        }

        public void ListViewAdd(ListView ls, ListViewItem item) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => {
                    ls.Items.Add( item );
                } ) );
            else
                ls.Items.Add( item );
        }

        public void ListViewRemove(ListView ls, ListViewItem item) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => { ls.Items.Remove( item ); } ) );
            else
                ls.Items.Remove( item );
        }

        private void ColsingHandler(object sender, FormClosingEventArgs e) {
            foreach ( var process in Process.GetProcessesByName( "Universal Share" ) ) process.Kill();
        }

        private void _idStreamMap_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;

            ßMainPoint.S.Execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( item.Text ) ) );
        }

        private void _TokenList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__TokenList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                ßMainPoint.S.ToakenList.Remove( item.Text );
            }
        }

        private void _RememberList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__RememberList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                ßMainPoint.S.RememberType.Remove( item.Text );
            }
        }

        private void _RegList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__RegList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                ßMainPoint.S.RegList.Remove( item.Text );
            }
        }
    }

    public partial class MainFormP {
        private void Changed <T, TV>(DictChangedEventArgs<T, TV> e) {
            switch (e.Type) {
                case TypeE.AddItem:    break;
                case TypeE.RemoveItem: break;
                case TypeE.Clear:      break;
                default:               return;
            }

            ListView ls = null;

            if ( typeof(T) == typeof(RegInfo.Type) ) {
                if ( typeof(TV) == typeof(RememberType) ) {
                    ls = this._RememberList;
                }
                else {
                    ls = this._RegList;
                }
            }
            else if ( typeof(T) == typeof(string) ) {
                ls = this._TokenList;
            }
            else if ( typeof(T) == typeof(int) ) {
                ls = this._idStreamMap;
            }

            switch (e.Type) {
                case TypeE.AddItem:
                    AddListItem( ls, e );
                    break;
                case TypeE.RemoveItem:
                    RemoveListItem( ls, e );
                    break;
                case TypeE.Clear:
                    ClearListItems( ls );
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void RememberTypeOnOnDictionaryChanged(object sender, DictChangedEventArgs<string, RememberType> e) { Changed( e ); }

        private void TokenListOnOnDictionaryChanged(object sender, DictChangedEventArgs<string, TokenItem> e) {
            switch (e.Type) {
                case TypeE.AddItem:    break;
                case TypeE.RemoveItem: break;
                case TypeE.Clear:      break;
                default:               return;
            }

            Changed( e );
        }

        private void RegListOnOnDictionaryChanged(object    sender, DictChangedEventArgs<string, TypeHolder> e) { Changed( e ); }
        private void IdStreamsMapOnDictionaryChanged(object sender, DictChangedEventArgs<int, RegInfo>       e) { Changed( e ); }
        private void ClearListItems(ListView                ls) { ls.Items.Clear(); }

        private void RemoveListItem <T, TV>(ListView ls, DictChangedEventArgs<T, TV> e) {
            var x = ListViewFind( ls, CreateName( e.Key.ToString() ), true );

            ListViewRemove( ls, x );
        }

        private void AddListItem <T, TV>(ListView ls, DictChangedEventArgs<T, TV> e) { ListViewAdd( ls, CreateFromDictChange( e ) ); }

        private ListViewItem CreateFromDictChange <T, TV>(DictChangedEventArgs<T, TV> e) {
            var key = CreateName( e.Key.ToString() );

            var it = new ListViewItem( key );
            it.Name     = key;
            it.ImageKey = key;
            try {
                if ( typeof(TV) == typeof(RegInfo) && e.Value is RegInfo rg ) {
                    it.SubItems.Add( rg.Extension.ToString() );
                    it.SubItems.Add( ( rg.Position.ToString() ) );
                    it.SubItems.Add( ( rg.SenderAuth ) );
                    it.SubItems.Add( ( rg.SaveFilePath ) );
                }
                else if ( typeof(TV) == typeof(TokenItem) && e.Value is TokenItem ti ) {
                    it.SubItems.Add( ti.Trusted.ToString() );
                    it.SubItems.Add( ( ti.Remember.ToString() ) );
                    it.SubItems.Add( ( ti.Description ) );
                }
                else if ( typeof(TV) == typeof(RememberType) && e.Value is RememberType rm ) {
                    it.SubItems.Add( rm.Type.ToString() );
                    it.SubItems.Add( ( rm.Value.ToString() ) );
                    it.SubItems.Add( ( rm.Description ) );
                }
                else if ( typeof(TV) == typeof(TypeHolder) && e.Value is TypeHolder th ) {
                    it.SubItems.Add( th.CloseFileStream.ToString() );
                    it.SubItems.Add( ( th.UserConfirm.ToString() ) );
                    it.SubItems.Add( ( th.OpenWith ) );
                    it.SubItems.Add( ( th.Arguments ) );
                    it.SubItems.Add( ( th.Description ) );
                }
            } catch { it.SubItems.AddRange( new[] { "error", "error", "error", "error", "error" } ); }

            return it;
        }

        private string CreateName(string eKey) => eKey;

        private void ForceUpdateAll_Click(object sender, EventArgs e) {
            this._RegList.Items.Clear();
            this._RememberList.Items.Clear();
            this._TokenList.Items.Clear();
            this._idStreamMap.Items.Clear();
            foreach ( var s in ßMainPoint.S.IdStreamsMap ) {
                this._idStreamMap.Items.Add( CreateFromDictChange( new DictChangedEventArgs<int, RegInfo>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RegList ) {
                this._RegList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, TypeHolder>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RememberType ) {
                this._RememberList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, RememberType>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.ToakenList ) {
                this._TokenList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, TokenItem>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( Control c in this.panel4.Controls ) {
                c.Refresh();
            }
        }
    }
}