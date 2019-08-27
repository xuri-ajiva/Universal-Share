using System;
using System.Collections;
using System.Collections.Generic;

namespace Universal_Share {
    public class EventDictionary <K, V> : IDictionary<K, V> {
        public delegate void DictionaryChanged(object sender, DictChangedEventArgs<K, V> e);

        public event DictionaryChanged OnDictionaryChanged;

        private IDictionary<K, V> innerDict;

        public EventDictionary() { this.innerDict = new Dictionary<K, V>(); }

        public bool ContainsKey(K key) {
            Invoke( TypeE.ContainsKey, key );
            return this.innerDict.ContainsKey( key );
        }

        public void Add(K key, V value) {
            Invoke( TypeE.AddItem, key, value );
            this.innerDict.Add( key, value );
        }

        private void Invoke(TypeE t, K key, V value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<K, V>() { Type = TypeE.AddItem, Key = key, Value = value } );
        private void Invoke(TypeE t, K key) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<K, V>() { Type = TypeE.AddItem, Key = key } );
        private void Invoke(TypeE t)          => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<K, V>() { Type = TypeE.AddItem } );
        private void Invoke(TypeE t, V value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<K, V>() { Type = TypeE.AddItem, Value = value } );

        public bool Remove(K key) {
            Invoke( TypeE.RemoveItem, key );
            return this.innerDict.Remove( key );
        }

        public bool TryGetValue(K key, out V value) {
            this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<K, V>() { Type = TypeE.AddItem, Key = key } );
            return this.innerDict.TryGetValue( key, out value );
        }

        public V this[K key] {
            get {
                Invoke( TypeE.GetAt, key );
                return this.innerDict[key];
            }
            set {
                Invoke( TypeE.SetAt, key, value );
                this.innerDict[key] = value;
            }
        }

        public ICollection<K> Keys {
            get {
                Invoke( TypeE.GetAt );
                return this.innerDict.Keys;
            }
        }

        public ICollection<V> Values {
            get {
                Invoke( TypeE.GetAt );
                return this.innerDict.Values;
            }
        }


        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
            Invoke( TypeE.GetEnumerator );
            return this.innerDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            Invoke( TypeE.GetEnumerator );
            return this.innerDict.GetEnumerator();
        }

        public void Add(KeyValuePair<K, V> item) {
            Invoke( TypeE.AddItem, item.Key, item.Value );
            this.innerDict.Add( item );
        }

        public void Clear() {
            Invoke( TypeE.Clear );
            this.innerDict.Clear();
        }

        public bool Contains(KeyValuePair<K, V> item) {
            Invoke( TypeE.Contains, item.Key, item.Value );
            return this.innerDict.Contains( item );
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) {
            Invoke( TypeE.CopyTo );
            this.innerDict.CopyTo( array, arrayIndex );
        }

        public bool Remove(KeyValuePair<K, V> item) {
            Invoke( TypeE.RemoveItem, item.Key, item.Value );
            return this.innerDict.Remove( item );
        }

        public int Count {
            get {
                Invoke( TypeE.Count );
                return this.innerDict.Count;
            }
        }

        public bool IsReadOnly { get { return this.innerDict.IsReadOnly; } }
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