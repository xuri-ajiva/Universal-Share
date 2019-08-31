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

        const string BASE_ITEM = "BASE_ITEM";

        private void StartServer(Server s) {
            s.Start();
            Thread.Sleep( int.MaxValue );
        }

        public ServerForm(Server s) {
            this._server = s;

            this._serverThread = new Thread( () => StartServer( this._server ) );

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
        }

        private void ServerForm_Load(object sender, EventArgs e) {
            //this._RegList.Items.Add( BASE_ITEM ).SubItems.AddRange( new[] { "-", "", "", "", "", "" } );
            //this._RememberList.Items.Add( BASE_ITEM ).SubItems.AddRange( new[] { "-", "", "", "", "", "" } );
            //this._TokenList.Items.Add( BASE_ITEM ).SubItems.AddRange( new[] { "-", "", "", "", "", "" } );
            //this._idStreamMap.Items.Add( BASE_ITEM ).SubItems.AddRange( new[] { "-", "", "", "", "", "" } );

            ßMainPoint.S.IdStreamsMap.OnDictionaryChanged += IdStreamsMapOnDictionaryChanged;
            ßMainPoint.S.RegList.OnDictionaryChanged      += RegListOnOnDictionaryChanged;
            ßMainPoint.S.ToakenList.OnDictionaryChanged   += TokenListOnOnDictionaryChanged;
            ßMainPoint.S.RememberType.OnDictionaryChanged += RememberTypeOnOnDictionaryChanged;

            ForceUpdateAll_Click( null, null );
        }

        public static ListViewItem[] _tmp;

        private void B_StartServer_Click(object sender, EventArgs e) {
            if ( !this._serverThread.IsAlive ) this._serverThread.Start();
        }

        private void Stop_Click(object sender, EventArgs e) { this._serverThread.Abort(); }

        private ListView.SelectedListViewItemCollection currentItem;

        private void _idStreamMap_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this.currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = "Start";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _TokenList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this.currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = "New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _RememberList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this.currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = "New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void _RegList_MouseClick(object sender, MouseEventArgs e) {
            if ( !( sender is ListView ls ) ) return;
            var item = ls.SelectedItems;
            if ( item[0].Text == BASE_ITEM ) return;
            this.currentItem = item;

            this.contextMenuStrip1.Items["newToolStripMenuItem"].Text = "New";
            this.contextMenuStrip1.Show( Cursor.Position );
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this.currentItem[0].ListView.Name) {
                case "ID":
                    ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( this.currentItem[0].Text ) ) );
                    break;
                case "TO":
                    try {
                        ( var r20, var r10 ) = ßMainPoint.E.CreateNewTokenItem();
                        ßMainPoint.S.ToakenList.Add( r10, r20 );
                    } catch { }

                    break;
                case "RE":
                    try {
                        ( var r21, var r11 ) = ßMainPoint.E.CreateNewRememberType();
                        ßMainPoint.S.RememberType.Add( r11, r21 );
                    } catch { }

                    break;
                case "RG":
                    try {
                        ( var r22, var r12 ) = ßMainPoint.E.CreateNewTypeHolder();
                        ßMainPoint.S.RegList.Add( r12, r22 );
                    } catch { }

                    break;
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this.currentItem[0].ListView.Name) {
                case "ID":
                    foreach ( ListViewItem item in this.currentItem ) {
                        ßMainPoint.S.IdStreamsMap.Remove( int.Parse( item.Name ) );
                    }

                    break;
                case "TO":
                    foreach ( ListViewItem item in this.currentItem ) {
                        ßMainPoint.S.ToakenList.Remove( item.Name );
                    }

                    break;
                case "RE":
                    foreach ( ListViewItem item in this.currentItem ) {
                        Enum.TryParse<RegInfo.TYPE>( item.Name, out var ty1 );
                        ßMainPoint.S.RememberType.Remove( ty1 );
                    }

                    break;
                case "RG":
                    foreach ( ListViewItem item in this.currentItem ) {
                        Enum.TryParse<RegInfo.TYPE>( item.Name, out var ty2 );
                        ßMainPoint.S.RegList.Remove( ty2 );
                    }

                    break;
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (this.currentItem[0].ListView.Name) {
                case "ID":
                    //ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( int.Parse( this.currentItem[0].Text ) ) );
                    break;
                case "TO":
                    try {
                        ( var r20, var r10 ) = ßMainPoint.E.EditTokenItem( ßMainPoint.S.ToakenList.Get( this.currentItem[0].Text ) );
                        ßMainPoint.S.ToakenList.Remove( this.currentItem[0].Text );
                        ßMainPoint.S.ToakenList.Add( r10, r20 );
                    } catch { }

                    break;
                case "RE":
                    try {
                        if ( Enum.TryParse<RegInfo.TYPE>( this.currentItem[0].SubItems[0].Text, out var enu1 ) ) {
                            ( var r21, var r11 ) = ßMainPoint.E.EditRememberType( ßMainPoint.S.RememberType.Get( enu1 ) );
                            ßMainPoint.S.RememberType.Remove( enu1 );
                            ßMainPoint.S.RememberType.Add( r11, r21 );
                        }
                    } catch { }

                    break;
                case "RG":
                    try {
                        if ( Enum.TryParse<RegInfo.TYPE>( this.currentItem[0].SubItems[0].Text, out var enu2 ) ) {
                            ( var r22, var r12 ) = ßMainPoint.E.EditTypeHolder( ßMainPoint.S.RegList.Get( enu2 ), enu2 );
                            ßMainPoint.S.RegList.Remove( enu2 );
                            ßMainPoint.S.RegList.Add( r12, r22 );
                        }
                    } catch { }

                    break;
            }

            ForceUpdateAll_Click( null, null );
        }
    }
}