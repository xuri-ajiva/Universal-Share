using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using UniversalShare_2;
using UniversalShare_2.Properties;
using UniversalShareCore.Net;
using UniversalShareCore.Operation;
using UniversalShareCore.Oprions;

namespace Universal_Share.Interface {
    public partial class MainFormP : Form {
        private readonly Shared _server;
        private          Thread _serverThread;
        private readonly bool   _isServer;

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
            try {
                switch (this._currentItem[0].ListView.Name) {
                    case "ID":
                        ßProgram.D.Execute( ßProgram.D.OperationIdMap.Get( this._currentItem[0].Text ) );
                        break;
                    case "TO":
                        try {
                            ( var r20, var r10 ) = ßProgram.E.CreateNewTokenItem();
                            ßProgram.D.TokenList.Add( r10, r20 );
                        } catch {
                            // 
                        }

                        break;
                    case "RE":
                        try {
                            //( var r21, var r11 ) = ßProgram.E.CreateNewRememberType();
                            //ßProgram.D.RememberType.Add( r11, r21 );
                        } catch {
                            //
                        }

                        break;
                    case "RG":
                        try {
                            //( var r22, var r12 ) = ßProgram.E.CreateNewTypeHolder();
                            //ßProgram.D.RegList.Add( r12, r22 );
                        } catch {
                            //
                        }

                        break;
                }
            } catch (Exception ex) {
                ßProgram.D.ExceptionHandler.EscalateException( ex );
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                switch (this._currentItem[0].ListView.Name) {
                    case "ID":
                        foreach ( ListViewItem item in this._currentItem ) {
                            ßProgram.D.OperationIdMap.Remove( item.Name );
                        }

                        break;
                    case "TO":
                        foreach ( ListViewItem item in this._currentItem ) {
                            ßProgram.D.TokenList.Remove( item.Name );
                        }

                        break;
                    case "RE":
                        foreach ( ListViewItem item in this._currentItem ) {
                            //ßProgram.D.RememberType.Remove( item.Name );
                        }

                        break;
                    case "RG":
                        foreach ( ListViewItem item in this._currentItem ) {
                            // ßProgram.D.RegList.Remove( item.Name );
                        }

                        break;
                }
            } catch (Exception ex) {
                ßProgram.D.ExceptionHandler.EscalateException( ex );
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                switch (this._currentItem[0].ListView.Name) {
                    case "ID":
                        //ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( this.currentItem[0].Text ) ) );
                        break;
                    case "TO":
                        try {
                            ( var r20, var r10 ) = ßProgram.E.EditTokenItem( ßProgram.D.TokenList.Get( this._currentItem[0].Text ) );
                            ßProgram.D.TokenList.Remove( this._currentItem[0].Text );
                            ßProgram.D.TokenList.Add( r10, r20 );
                        } catch {
                            // ignored
                        }

                        break;
                    case "RE":
                        try {
                            //var name = this._currentItem[0].SubItems[0].Text;
                            //( var r21, var r11 ) = ßProgram.E.EditRememberType( ßProgram.D.RememberType.Get( name ) );
                            //ßProgram.D.RememberType.Remove( name );
                            //ßProgram.D.RememberType.Add( name, r21 );
                        } catch {
                            // ignored
                        }

                        break;
                    case "RG":
                        try {
                            //var name = this._currentItem[0].SubItems[0].Text;
                            //( var r22, var r12 ) = ßProgram.E.EditTypeHolder( ßProgram.D.RegList.Get( name ), name );
                            //ßProgram.D.RegList.Remove( r12 );
                            //ßProgram.D.RegList.Add( r12, r22 );
                        } catch  {
                            // ignored
                        }

                        break;
                }

                ForceUpdateAll_Click( null, null );
            } catch (Exception ex) {
                ßProgram.D.ExceptionHandler.EscalateException( ex );
            }
        }

        #endregion

        #region Events

        private void ServerForm_Load(object sender, EventArgs e) {
            if ( ßProgram.D.OperationIdMap != null ) ßProgram.D.OperationIdMap.OnDictionaryChanged += IdStreamsMapOnDictionaryChanged;
            //ßProgram.D.RegList.OnDictionaryChanged      += RegListOnOnDictionaryChanged;
            if ( ßProgram.D.TokenList != null ) ßProgram.D.TokenList.OnDictionaryChanged += TokenListOnOnDictionaryChanged;
            //ßProgram.D.RememberType.OnDictionaryChanged += RememberTypeOnOnDictionaryChanged;

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
                        var ip = ßProgram.F.U.GetString( "Bitte IP Addresse Eintragen", "127.0.0.1" );
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

        private void B_Settings_Click(object sender, EventArgs e) {
            //ßProgram.SF.ShowDialog( this );
        }

        private void B_clodeStreams_Click(object sender, EventArgs e) {
            lock (ßProgram.D.OperationIdMap) {
                foreach ( var s in ßProgram.D.OperationIdMap ) {
                    s.Value.CloseStream();
                }
            }
        }

        private void _idStreamMap_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;

            ßProgram.D.Execute( ßProgram.D.OperationIdMap.Get( item.Text ) );
        }

        private void _TokenList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__TokenList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                ßProgram.D.OperationIdMap.Remove( item.Text );
            }
        }

        private void _RememberList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__RememberList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                //ßProgram.D.RememberType.Remove( item.Text );
            }
        }

        private void _RegList_DoubleClick(object sender, EventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems[0];
            if ( item.Text == BASE_ITEM ) return;
            if ( MessageBox.Show( Resources.ServerForm__RegList_DoubleClick_ + item.Text, "", MessageBoxButtons.YesNoCancel ) == DialogResult.Yes ) {
                //ßProgram.D.RegList.Remove( item.Text );
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
                ls = /*typeof(TV) == typeof(RememberType) ? this._RememberList : */this._RegList;
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

        // private void RememberTypeOnOnDictionaryChanged(object sender, DictChangedEventArgs<string, RememberType> e) { Changed( e ); }
        private void TokenListOnOnDictionaryChanged(object sender, DictChangedEventArgs<string, TokenItem> e) { Changed( e ); }

        // private void RegListOnOnDictionaryChanged(object      sender, DictChangedEventArgs<string, TypeHolder>   e) { Changed( e ); }
        private void IdStreamsMapOnDictionaryChanged(object sender, DictChangedEventArgs<string, OperationInfo> e) { Changed( e ); }

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
                if ( typeof(TV) == typeof(OperationInfo) && e.Value is OperationInfo rg ) {
                    it.SubItems.Add( rg._id.ToString() );
                    it.SubItems.Add( ( rg.Stream?.Length.ToString() ) );
                    it.SubItems.Add( ( rg._senderAuth ) );
                    it.SubItems.Add( ( rg._fileName ) );
                }
                else if ( typeof(TV) == typeof(TokenItem) && e.Value is TokenItem ti ) {
                    it.SubItems.Add( ti.Trusted.ToString() );
                    it.SubItems.Add( ( ti.Remember.ToString() ) );
                    it.SubItems.Add( ( ti.Description ) );
                }

                //else if ( typeof(TV) == typeof(RememberType) && e.Value is RememberType rm ) {
                //    it.SubItems.Add( rm.Type.ToString() );
                //    it.SubItems.Add( ( rm.Value.ToString() ) );
                //    it.SubItems.Add( ( rm.Description ) );
                //}
                //else if ( typeof(TV) == typeof(TypeHolder) && e.Value is TypeHolder th ) {
                //    it.SubItems.Add( th.CloseFileStream.ToString() );
                //    it.SubItems.Add( ( th.UserConfirm.ToString() ) );
                //    it.SubItems.Add( ( th.OpenWith ) );
                //    it.SubItems.Add( ( th.Arguments ) );
                //    it.SubItems.Add( ( th.Description ) );
                //}
            } catch { it.SubItems.AddRange( new[] { "error", "error", "error", "error", "error" } ); }

            return it;
        }

        private string CreateName(string eKey) => eKey;

        private void ForceUpdateAll_Click(object sender, EventArgs e) {
            try {
                this._RegList.Items.Clear();
                this._RememberList.Items.Clear();
                this._TokenList.Items.Clear();
                this._idStreamMap.Items.Clear();
                foreach ( var s in ßProgram.D.OperationIdMap ) {
                    this._idStreamMap.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, OperationInfo>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                }

                //foreach ( var s in ßProgram.D.RegList ) {
                //    this._RegList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, TypeHolder>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                //}
                //
                //foreach ( var s in ßProgram.D.RememberType ) {
                //    this._RememberList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, RememberType>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                //}

                foreach ( var s in ßProgram.D.TokenList ) {
                    this._TokenList.Items.Add( CreateFromDictChange( new DictChangedEventArgs<string, TokenItem>() { Key = s.Key, Type = TypeE.AddItem, Value = s.Value } ) );
                }

                foreach ( Control c in this.panel4.Controls ) {
                    c.Refresh();
                }
            } catch (Exception ex) {
                ßProgram.D.ExceptionHandler.EscalateException( ex );
            }
        }

        #endregion

        private void ColsingHandler(object sender, FormClosingEventArgs e) {
            Environment.Exit( 0 );
            //foreach ( var process in Process.GetProcessesByName( "Universal Share" ) ) process.Kill();
        }
    }
}