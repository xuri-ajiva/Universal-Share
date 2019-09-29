#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UniversalShareCore.Oprions;

#endregion

namespace UniversalShare_2.Interface {
    public partial class ListFromDirectory <TK, TV> : UserControl {
        public Func<KeyValuePair<TK, TV>, ListViewItem> GetItem;


        public ListFromDirectory() { InitializeComponent(); }

        public void SetColumns(IEnumerable<ColumnHeader> ch) { this.listView1.Columns.AddRange( ch.ToArray() ); }

        public void UpdateAll(SerializableDictionary<TK, TV> dic) {
            if ( this.GetItem == null ) throw new NullReferenceException( "pleas init " + nameof(this.GetItem) );

            this.listView1.Items.Clear();
            foreach ( var i in dic ) {
                this.listView1.Items.Add( this.GetItem( i ) );
            }
        }

        public void AddItem(KeyValuePair<TK, TV> iKeyValuePair) {
            if ( this.GetItem == null ) throw new NullReferenceException( "pleas init " + nameof(this.GetItem) );

            if ( this.listView1.InvokeRequired )
                this.listView1.Invoke( new Action( () => {
                    this.listView1.Items.Add( GetItem( iKeyValuePair ) );
                } ) );
        }

        public void AddItem(string name) {
            if ( this.listView1.InvokeRequired )
                this.listView1.Invoke( new Action( () => {
                    this.listView1.Items.Add( name );
                } ) );
        }

        public void RemoveItemByKey(string key) {
            if ( this.listView1.InvokeRequired ) this.listView1.Invoke( new Action( () => { this.listView1.Items.RemoveByKey( key ); } ) );
        }

        public void Remove(ListViewItem item) {
            if ( this.listView1.InvokeRequired ) this.listView1.Invoke( new Action( () => { this.listView1.Items.Remove( item ); } ) );
        }

        public void RemoveItem(int index) {
            if ( this.listView1.InvokeRequired ) this.listView1.Invoke( new Action( () => { this.listView1.Items.RemoveAt( index ); } ) );
        }

        public void SetItemCreationMathode(Func<KeyValuePair<TK, TV>, ListViewItem> methode) { this.GetItem = methode; }

        public ListViewItem[] Find(string key, bool searchAllSubItems = true) { return this.listView1.Items.Find( key, searchAllSubItems ); }

        public int IndexOfKey(string key) { return this.listView1.Items.IndexOfKey( key ); }

        public bool ContainsKey(string key) { return this.listView1.Items.ContainsKey( key ); }

        public int Count() { return this.listView1.Items.Count; }

        public ListView.ColumnHeaderCollection Columns { get { return this.listView1.Columns; } }

        public View View { get { return this.listView1.View; } set { this.listView1.View = value; } }

        public void Clear() { this.listView1.Items.Clear(); }


        #region Unsafe

        public unsafe ListView GetListView() { return this.listView1; }

        #endregion
    }
}