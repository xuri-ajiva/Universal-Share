using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Universal_Share {
    [Serializable]
    public class SerializableDictionary <TK, TV> {
        public delegate void DictionaryChanged(object sender, DictChangedEventArgs<TK, TV> e);

        public event DictionaryChanged OnDictionaryChanged;

        private void Invoke(TypeE type, TK key, TV value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV>() { Type = type, Key = key, Value = value } );
        private void Invoke(TypeE type, TK key) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV>() { Type = type, Key = key } );
        private void Invoke(TypeE type)           => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV>() { Type = type } );
        private void Invoke(TypeE type, TV value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV>() { Type = type, Value = value } );


        [XmlIgnore] public TK type;
        [XmlIgnore] public TV Key;

        [XmlIgnore] private Dictionary<TK, TV> _BackupDict = new Dictionary<TK, TV>();

        public List<ValueType_L<TK, TV>> DictionaryTypes = new List<ValueType_L<TK, TV>>();

        [Serializable]
        public struct ValueType_L <T, V> {
            public T Type;
            public V Key;

            public ValueType_L(T type, V key) {
                this.Type = type;
                this.Key  = key;
            }
        }

        #region Implementation of IEnumerable

        /// <inheritdoc />
        public bool Contains(TK key) {
            Invoke( TypeE.ContainsKey, key );
            return this._BackupDict.ContainsKey( key );
        }

        /// <inheritdoc />
        public void Add(TK key, TV value) {
            if ( this._BackupDict.ContainsKey( key ) ) return;
            this._BackupDict.Add( key, value );
            this.DictionaryTypes.Add( new ValueType_L<TK, TV>( key, value ) );
        }

        /// <inheritdoc />
        public void Clear() {
            Invoke( TypeE.Clear );
            this._BackupDict.Clear();
            this.DictionaryTypes.Clear();
        }

        /// <inheritdoc />
        public void Remove(TK key) {
            Invoke( TypeE.RemoveItem, key );
            var item = this.DictionaryTypes.Find( x => x.Key.Equals( this._BackupDict[key] ) );
            this.DictionaryTypes.Remove( item );
            this._BackupDict.Remove( key );
        }

        /// <inheritdoc />
        [XmlIgnore]
        public TV this[TK key] {
            get {
                Invoke( TypeE.GetAt, key );
                return this._BackupDict[key];
            }
            set {
                Invoke( TypeE.SetAt, key, value );
                this._BackupDict[key] = value;
            }
        }


        /// <inheritdoc />
        public TV Get(TK key) {
            Invoke( TypeE.GetAt, key );
            return this[key];
        }

        /// <inheritdoc />
        [XmlIgnore]
        public ICollection Keys => this._BackupDict.Keys;

        /// <inheritdoc />
        [XmlIgnore]
        public ICollection Values => this._BackupDict.Values;

        /// <inheritdoc />
        public IEnumerator GetEnumerator() { return this._BackupDict.GetEnumerator(); }

        #endregion

        #region Implementation of ICollection

        /// <inheritdoc />
        public void CopyTo(ValueType_L<TK, TV>[] array, int index) => this.DictionaryTypes.CopyTo( array, index );

        /// <inheritdoc />
        [XmlIgnore]
        public int Count => this.DictionaryTypes.Count;

        #endregion
    }
    public class DictChangedEventArgs <K, V> : EventArgs {
        public TypeE Type { get; set; }

        public K Key { get; set; }

        public V Value { get; set; }
    }

    public enum TypeE {
        AddItem,
        RemoveItem,
        TryGetValue,
        Clear,
        Contains,
        ContainsKey,
        CopyTo,
        Count,
        GetAt,
        SetAt, GetEnumerator,
    }
}