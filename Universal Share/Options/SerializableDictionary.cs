#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Universal_Share.Options {
    [XmlRoot( "dictionary" )]
    public class SerializableDictionary <TK, TV> : Dictionary<TK, TV>, IXmlSerializable {
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(System.Xml.XmlReader reader) {
            XmlSerializer keySerializer   = new XmlSerializer( typeof(TK) );
            XmlSerializer valueSerializer = new XmlSerializer( typeof(TV) );

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if ( wasEmpty ) return;

            while ( reader.NodeType != System.Xml.XmlNodeType.EndElement ) {
                reader.ReadStartElement( "item" );

                reader.ReadStartElement( "key" );
                TK key = (TK) keySerializer.Deserialize( reader );
                reader.ReadEndElement();

                reader.ReadStartElement( "value" );
                TV value = (TV) valueSerializer.Deserialize( reader );
                reader.ReadEndElement();

                this.Add( key, value );

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            XmlSerializer keySerializer   = new XmlSerializer( typeof(TK) );
            XmlSerializer valueSerializer = new XmlSerializer( typeof(TV) );

            foreach ( TK key in this.Keys ) {
                writer.WriteStartElement( "item" );

                writer.WriteStartElement( "key" );
                keySerializer.Serialize( writer, key );
                writer.WriteEndElement();

                writer.WriteStartElement( "value" );
                TV value = this[key];
                valueSerializer.Serialize( writer, value );
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }

        #endregion


        public delegate void DictionaryChanged(object sender, DictChangedEventArgs<TK, TV> e);

        public event DictionaryChanged     OnDictionaryChanged;
        [DebuggerStepThrough] private void Invoke(TypeE type, TK key, TV value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Key = key, Value = value } );
        [DebuggerStepThrough] private void Invoke(TypeE type, TK key) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Key = key } );
        [DebuggerStepThrough] private void Invoke(TypeE type)           => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type } );
        [DebuggerStepThrough] private void Invoke(TypeE type, TV value) => this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Value = value } );


        #region Implementation of IEnumerable

        /// <inheritdoc />
        public bool Contains(TK key) {
            Invoke( TypeE.ContainsKey, key );
            return this.ContainsKey( key );
        }

        public new bool ContainsKey(TK key) {
            Invoke( TypeE.ContainsKey, key );
            return base.ContainsKey( key );
        }

        /// <inheritdoc />
        public new void Add(TK key, TV value) {
            Invoke( TypeE.AddItem, key, value );
            base.Add( key, value );
        }

        /// <inheritdoc />
        public new void Clear() {
            Invoke( TypeE.Clear );
            base.Clear();
        }

        /// <inheritdoc />
        public new void Remove(TK key) {
            Invoke( TypeE.RemoveItem, key );
            base.Remove( key );
        }

        /// <inheritdoc />
        [XmlIgnore]
        public new TV this[TK key] {
            get {
                Invoke( TypeE.GetAt, key );
                return base[key];
            }
            set {
                Invoke( TypeE.SetAt, key, value );
                base[key] = value;
            }
        }

        /// <inheritdoc />
        public TV Get(TK key) {
            Invoke( TypeE.GetAt, key );
            return base[key];
        }

        #endregion


        /// <inheritdoc />
        [XmlIgnore]
        public int TCount => this.Count;
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
        SetAt, GetEnumerator
    }
}