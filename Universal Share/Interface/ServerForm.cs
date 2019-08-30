using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Universal_Share.Net;
using Universal_Share.Options;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Interface {
    public partial class ServerForm : Form {
        private Server _server;
        private Thread _serverThread;

        private void StartServer(Server s) {
            s.Start();
            Thread.Sleep( int.MaxValue );
        }

        public ServerForm(Server s) {
            this._server = s;

            this._serverThread = new Thread( () => StartServer( this._server ) );

            InitializeComponent();

            this._idStreamMap.Columns.Add( "Type",     90, HorizontalAlignment.Left );
            this._idStreamMap.Columns.Add( "Position", 50, HorizontalAlignment.Left );
            this._idStreamMap.Columns.Add( "Path",     -2, HorizontalAlignment.Left );
            this._TokenList.Columns.Add( "Trusted",     100, HorizontalAlignment.Left );
            this._TokenList.Columns.Add( "remember",    100, HorizontalAlignment.Left );
            this._TokenList.Columns.Add( "Description", -2,  HorizontalAlignment.Left );
            this._RememberList.Columns.Add( "Type",        100, HorizontalAlignment.Left );
            this._RememberList.Columns.Add( "Value",       100, HorizontalAlignment.Left );
            this._RememberList.Columns.Add( "Description", -2,  HorizontalAlignment.Left );
            this._RegList.Columns.Add( "CloseFileStream", 100, HorizontalAlignment.Left );
            this._RegList.Columns.Add( "UserConfirm",     100, HorizontalAlignment.Left );
            this._RegList.Columns.Add( "OpenWith",        100, HorizontalAlignment.Left );
            this._RegList.Columns.Add( "Arguments",       300, HorizontalAlignment.Left );
            this._RegList.Columns.Add( "Description",     -2,  HorizontalAlignment.Left );

            this._idStreamMap.Name  = "ID";
            this._TokenList.Name    = "TO";
            this._RememberList.Name = "RE";
            this._RegList.Name      = "RG";
        }

        private void ServerForm_Load(object sender, EventArgs e) {
            ßMainPoint.S.IdStreamsMap.OnDictionaryChanged += IdStreamsMapOnDictionaryChanged;
            ßMainPoint.S.RegList.OnDictionaryChanged      += RegListOnOnDictionaryChanged;
            ßMainPoint.S.ToakenList.OnDictionaryChanged   += ToakenListOnOnDictionaryChanged;
            ßMainPoint.S.RememberType.OnDictionaryChanged += RememberTypeOnOnDictionaryChanged;

            foreach ( var s in ßMainPoint.S.IdStreamsMap ) {
                _idStreamMap.Items.Add( CreateFromDictChange( this._idStreamMap, new DictChangedEventArgs<int, RegInfo>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RegList ) {
                this._RegList.Items.Add( CreateFromDictChange( this._RegList, new DictChangedEventArgs<RegInfo.TYPE, TypeHolder>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RememberType ) {
                this._RememberList.Items.Add( CreateFromDictChange( this._RememberList, new DictChangedEventArgs<RegInfo.TYPE, RememberType>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.ToakenList ) {
                this._TokenList.Items.Add( CreateFromDictChange( this._TokenList, new DictChangedEventArgs<string, TokenItem>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }
        }

        void Changed <T, V>(DictChangedEventArgs<T, V> e) {
            switch (e.Type) {
                case TypeE.AddItem:    break;
                case TypeE.RemoveItem: break;
                case TypeE.Clear:      break;
                case TypeE.TryGetValue:
                case TypeE.Contains:
                case TypeE.ContainsKey:
                case TypeE.CopyTo:
                case TypeE.Count:
                case TypeE.GetAt:
                case TypeE.SetAt:
                case TypeE.GetEnumerator:
                default: return;
            }

            ListView ls = null;

            if ( typeof(T) == typeof(RegInfo.TYPE) ) {
                if ( typeof(V) == typeof(RememberType) ) {
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

        private void RememberTypeOnOnDictionaryChanged(object sender, DictChangedEventArgs<RegInfo.TYPE, RememberType> e) { Changed( e ); }

        private void ToakenListOnOnDictionaryChanged(object sender, DictChangedEventArgs<string, TokenItem> e) { Changed( e ); }

        private void RegListOnOnDictionaryChanged(object sender, DictChangedEventArgs<RegInfo.TYPE, TypeHolder> e) { Changed( e ); }

        private void IdStreamsMapOnDictionaryChanged(object sender, DictChangedEventArgs<int, RegInfo> e) { Changed( e ); }

        private void ClearListItems(ListView ls) { ls.Clear(); }

        private void RemoveListItem <T, V>(ListView ls, DictChangedEventArgs<T, V> e) {
            // TODO: Make list refresh
            var x = ListViewFind( ls, CreateName( e.Key.ToString() ), true );

            foreach ( var l in x ) ListViewRemove( ls, l );

            Fullupdate( ls );
        }

        private void Fullupdate(ListView ls) {
            ClearListItems( ls );

            switch (ls.Name) {
                case "ID":

                    foreach ( var s in ßMainPoint.S.IdStreamsMap ) {
                        _idStreamMap.Items.Add( CreateFromDictChange( this._idStreamMap, new DictChangedEventArgs<int, RegInfo>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                    }

                    break;
                case "TO":
                    foreach ( var s in ßMainPoint.S.ToakenList ) {
                        this._TokenList.Items.Add( CreateFromDictChange( this._TokenList, new DictChangedEventArgs<string, TokenItem>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                    }

                    break;
                case "RE":
                    foreach ( var s in ßMainPoint.S.RememberType ) {
                        this._RememberList.Items.Add( CreateFromDictChange( this._RememberList, new DictChangedEventArgs<RegInfo.TYPE, RememberType>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                    }

                    break;
                case "RG":
                    foreach ( var s in ßMainPoint.S.RegList ) {
                        this._RegList.Items.Add( CreateFromDictChange( this._RegList, new DictChangedEventArgs<RegInfo.TYPE, TypeHolder>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                    }

                    break;
            }
        }

        public static ListViewItem[] _tmp;

        private void AddListItem <T, V>(ListView ls, DictChangedEventArgs<T, V> e) { ListViewAdd( ls, CreateFromDictChange( ls, e ) ); }

        ListViewItem CreateFromDictChange <T, V>(ListView ls, DictChangedEventArgs<T, V> e) {
            var it = new ListViewItem( CreateName( e.Key.ToString() ) );
            it.ImageKey = CreateName( e.Key.ToString() );
            try {
                if ( typeof(V) == typeof(RegInfo) && e.Value is RegInfo rg ) {
                    it.SubItems.Add( rg.Type.ToString() );
                    it.SubItems.Add( ( rg.Position.ToString() ) );
                    it.SubItems.Add( ( rg.SaveFilePath ) );
                }
                else if ( typeof(V) == typeof(TokenItem) && e.Value is TokenItem ti ) {
                    it.SubItems.Add( ti.Trusted.ToString() );
                    it.SubItems.Add( ( ti.remember.ToString() ) );
                    it.SubItems.Add( ( ti.Description ) );
                }
                else if ( typeof(V) == typeof(RememberType) && e.Value is RememberType rm ) {
                    it.SubItems.Add( rm.Type.ToString() );
                    it.SubItems.Add( ( rm.Value.ToString() ) );
                    it.SubItems.Add( ( rm.Description ) );
                }
                else if ( typeof(V) == typeof(TypeHolder) && e.Value is TypeHolder th ) {
                    it.SubItems.Add( th.CloseFileStream.ToString() );
                    it.SubItems.Add( ( th.UserConfirm.ToString() ) );
                    it.SubItems.Add( ( th.OpenWith ) );
                    it.SubItems.Add( ( th.ArgumentsBeforePathToFile + " %V% " + th.ArgumentsAfterPathToFile ) );
                    it.SubItems.Add( ( th.Description ) );
                }
            } catch { it.SubItems.AddRange( new[] { "error", "error", "error", "error", "error" } ); }

            return it;
        }

        private string CreateName(string eKey) => eKey;

        private void B_StartServer_Click(object sender, EventArgs e) {
            if ( !this._serverThread.IsAlive ) this._serverThread.Start();
        }

        private void ColsingHandler(object sender, FormClosingEventArgs e) {
            foreach ( var process in Process.GetProcessesByName( "Universal Share" ) ) process.Kill();
        }

        private void _idStreamMap_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;

            var reg = ls.SelectedItems[0].Text;
            ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( reg ) ) );
        }

        private void _TokenList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( MessageBox.Show( "Remove Toaken?\n" + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                ßMainPoint.S.ToakenList.Remove( item.Text );
            }
        }

        private void _RememberList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
        }

        private void _RegList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
        }

        private void ForceUpdateAll_Click(object sender, EventArgs e)
        {
            this._RegList.Items.Clear();
            this._RememberList.Items.Clear();
            this._TokenList.Items.Clear();
            this._idStreamMap.Items.Clear();
            foreach ( var s in ßMainPoint.S.IdStreamsMap ) {
                _idStreamMap.Items.Add( CreateFromDictChange( this._idStreamMap, new DictChangedEventArgs<int, RegInfo>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RegList ) {
                this._RegList.Items.Add( CreateFromDictChange( this._RegList, new DictChangedEventArgs<RegInfo.TYPE, TypeHolder>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.RememberType ) {
                this._RememberList.Items.Add( CreateFromDictChange( this._RememberList, new DictChangedEventArgs<RegInfo.TYPE, RememberType>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( var s in ßMainPoint.S.ToakenList ) {
                this._TokenList.Items.Add( CreateFromDictChange( this._TokenList, new DictChangedEventArgs<string, TokenItem>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
            }

            foreach ( Control c in this.panel4.Controls ) {
                c.Refresh();
            }

        }
    }
}