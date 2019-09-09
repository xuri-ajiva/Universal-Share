using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.Net;
using Universal_Share.Options;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Interface {
    public partial class MainFormP : Form {
        private readonly NetworkFileSend _server;
        private          Thread          _serverThread;
        private readonly bool            _isServer;

        private const string BASE_ITEM = "BASE_ITEM";

        private void StartServer(ISharedAble s, IPAddress ipAddress) { s.Start( ipAddress ); }

        public MainFormP(ISharedAble s, bool isServer) {
            this._serverThread = new Thread( () => { } );
            this._isServer     = isServer;
            switch (s) {
                case Client c:
                    this._server = c;
                    break;
                case Server se:
                    this._server = se;
                    break;
            }

            InitializeComponent();

            this._idStreamMap.Columns.Add( "Type",        90,  HorizontalAlignment.Left );
            this._idStreamMap.Columns.Add( "Position",    50,  HorizontalAlignment.Left );
            this._idStreamMap.Columns.Add( "SenderToken", 100, HorizontalAlignment.Left );
            this._idStreamMap.Columns.Add( "Path",        -2,  HorizontalAlignment.Left );
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

            this.B_StartServer.Text = "Start " + s.GetType().Name;
        }


        public static ListViewItem[] Tmp;


        private ListView.SelectedListViewItemCollection _currentItem;

        #region invokerHandlers

        private void ClearListItems(ListView ls) {
            if ( ls.InvokeRequired )
                ls.Invoke( new Action( () => {
                    ls.Items.Clear();
                } ) );
            else
                ls.Items.Clear();
        }

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

        #endregion

        #region MenüItem

        private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this._currentItem[0].ListView.Name) {
                case "ID":
                    ßMainPoint.S.Execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( this._currentItem[0].Text ) ) );
                    break;
                case "TO":
                    try {
                        ( var r20, var r10 ) = ßMainPoint.E.CreateNewTokenItem();
                        ßMainPoint.S.ToakenList.Add( r10, r20 );
                    } catch {
                        // 
                    }

                    break;
                case "RE":
                    try {
                        ( var r21, var r11 ) = ßMainPoint.E.CreateNewRememberType();
                        ßMainPoint.S.RememberType.Add( r11, r21 );
                    } catch {
                        //
                    }

                    break;
                case "RG":
                    try {
                        ( var r22, var r12 ) = ßMainPoint.E.CreateNewTypeHolder();
                        ßMainPoint.S.RegList.Add( r12, r22 );
                    } catch {
                        //
                    }

                    break;
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this._currentItem[0].ListView.Name) {
                case "ID":
                    foreach ( ListViewItem item in this._currentItem ) {
                        ßMainPoint.S.IdStreamsMap.Remove( int.Parse( item.Name ) );
                    }

                    break;
                case "TO":
                    foreach ( ListViewItem item in this._currentItem ) {
                        ßMainPoint.S.ToakenList.Remove( item.Name );
                    }

                    break;
                case "RE":
                    foreach ( ListViewItem item in this._currentItem ) {
                        ßMainPoint.S.RememberType.Remove( item.Name );
                    }

                    break;
                case "RG":
                    foreach ( ListViewItem item in this._currentItem ) {
                        ßMainPoint.S.RegList.Remove( item.Name );
                    }

                    break;
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this._currentItem[0].ListView.Name) {
                case "ID":
                    //ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( this.currentItem[0].Text ) ) );
                    break;
                case "TO":
                    try {
                        ( var r20, var r10 ) = ßMainPoint.E.EditTokenItem( ßMainPoint.S.ToakenList.Get( this._currentItem[0].Text ) );
                        ßMainPoint.S.ToakenList.Remove( this._currentItem[0].Text );
                        ßMainPoint.S.ToakenList.Add( r10, r20 );
                    } catch {
                        // ignored
                    }

                    break;
                case "RE":
                    try {
                        var name = this._currentItem[0].SubItems[0].Text;
                        ( var r21, var r11 ) = ßMainPoint.E.EditRememberType( ßMainPoint.S.RememberType.Get( name ) );
                        ßMainPoint.S.RememberType.Remove( name );
                        ßMainPoint.S.RememberType.Add( name, r21 );
                    } catch {
                        // ignored
                    }

                    break;
                case "RG":
                    try {
                        var name = this._currentItem[0].SubItems[0].Text;
                        ( var r22, var r12 ) = ßMainPoint.E.EditTypeHolder( ßMainPoint.S.RegList.Get( name ), name );
                        ßMainPoint.S.RegList.Remove( r12 );
                        ßMainPoint.S.RegList.Add( r12, r22 );
                    } catch (Exception exsa) {
                        // ignored
                    }

                    break;
            }

            ForceUpdateAll_Click( null, null );
        }

        #endregion

        #region Events

        private void ServerForm_Load(object sender, EventArgs e) {
            ßMainPoint.S.IdStreamsMap.OnDictionaryChanged += IdStreamsMapOnDictionaryChanged;
            ßMainPoint.S.RegList.OnDictionaryChanged      += RegListOnOnDictionaryChanged;
            ßMainPoint.S.ToakenList.OnDictionaryChanged   += TokenListOnOnDictionaryChanged;
            ßMainPoint.S.RememberType.OnDictionaryChanged += RememberTypeOnOnDictionaryChanged;

            ForceUpdateAll_Click( null, null );
        }

        private void B_StartServer_Click(object sender, EventArgs e) {
            if ( this._isServer ) {
                if ( !this._serverThread.IsAlive ) {
                    this._serverThread = null;
                    GC.Collect();
                    this._serverThread = new Thread( () => StartServer( this._server as ISharedAble, IPAddress.Any ) );

                    this._serverThread.Start();
                }
            }
            else {
                if ( !this._serverThread.IsAlive ) {
                    try {
                        this._serverThread = null;
                        GC.Collect();
                        var ip = ßMainPoint.U.GetString( "Bitte IP Addresse Eintragen", "127.0.0.1" );
                        this._serverThread = new Thread( () => StartServer( this._server as ISharedAble, IPAddress.Parse( ip ) ) );

                        this._serverThread.Start();
                    } catch (Exception ex) {
                        MessageBox.Show( ex.Message );
                    }
                }
            }
        }

        private void Stop_Click(object sender, EventArgs e) {
            if ( this._serverThread.IsAlive ) {
                var s = this._server as ISharedAble;
                s.Abort();

                this._serverThread.Abort();
            }
        }

        private void _idStreamMap_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this._currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = @"Start";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _TokenList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this._currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = @"New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _RememberList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this._currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = @"New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _RegList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this._currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = @"New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void B_SendFile_Click(object sender, EventArgs e) {
            if ( !( this._server is ISharedAble server ) ) return;
            using ( OpenFileDialog f = new OpenFileDialog { Multiselect = true } ) {
                if ( f.ShowDialog( this ) == DialogResult.OK ) {
                    var t = new Thread( () => {
                        foreach ( var s in f.FileNames ) {
                            this._server.SendFile( server.GetTcpClient(), s );
                        }
                    } );
                    t.Start();
                }
            }
        }

        private void B_Settings_Click(object sender, EventArgs e) { ßMainPoint.SF.ShowDialog( this ); }

        private void B_clodeStreams_Click(object sender, EventArgs e) {
            lock (ßMainPoint.S.IdStreamsMap) {
                foreach ( var s in ßMainPoint.S.IdStreamsMap ) {
                    s.Value.CloseStream();
                }
            }
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

        #endregion

        #region delegatedEvents

        private void Changed <T, TV>(DictChangedEventArgs<T, TV> e) {
            switch (e.Type) {
                case TypeE.AddItem:    break;
                case TypeE.RemoveItem: break;
                case TypeE.Clear:      break;
                default:               return;
            }

            ListView ls = null;

            if ( typeof(T) == typeof(string) ) {
                ls = typeof(TV) == typeof(RememberType) ? this._RememberList : this._RegList;
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
        private void TokenListOnOnDictionaryChanged(object    sender, DictChangedEventArgs<string, TokenItem>    e) { Changed( e ); }
        private void RegListOnOnDictionaryChanged(object      sender, DictChangedEventArgs<string, TypeHolder>   e) { Changed( e ); }
        private void IdStreamsMapOnDictionaryChanged(object   sender, DictChangedEventArgs<int, RegInfo>         e) { Changed( e ); }

        #endregion

        #region listHandler

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

        #endregion

        private void ColsingHandler(object sender, FormClosingEventArgs e) {
            Environment.Exit( 0 );
            //foreach ( var process in Process.GetProcessesByName( "Universal Share" ) ) process.Kill();
        }
    }
}