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
            this.listView1.Items.Add( this.GetItem( iKeyValuePair ) );
        }

        public void AddItem(string name) { this.listView1.Items.Add( name ); }

        public void RemoveItemByKey(string key)   => this.listView1.Items.RemoveByKey( key );
        public void Remove(ListViewItem    item)  => this.listView1.Items.Remove( item );
        public void RemoveItem(int         index) => this.listView1.Items.RemoveAt( index );

        public ListViewItem[] Find(string key, bool searchAllSubItems = true) => this.listView1.Items.Find( key, searchAllSubItems );

        public int IndexOfKey(string key) => this.listView1.Items.IndexOfKey( key );

        public bool ContainsKey(string key) => this.listView1.Items.ContainsKey( key );

        public int Count() => this.listView1.Items.Count;

        public ListView.ColumnHeaderCollection Columns => this.listView1.Columns;

        #region Unsafe

        public unsafe ListView GetListView() => this.listView1;

        #endregion
    }
}