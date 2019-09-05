#region using

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics;

#endregion

namespace Universal_Share.Options {
    [XmlRoot( "dictionary" )]
    public class SerializableDictionary <TK, TV> : Dictionary<TK, TV>, IXmlSerializable {
        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema() { return null; }

        public void ReadXml(System.Xml.XmlReader reader) {
            var keySerializer   = new XmlSerializer( typeof(TK) );
            var valueSerializer = new XmlSerializer( typeof(TV) );

            var wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if ( wasEmpty ) return;

            while ( reader.NodeType != System.Xml.XmlNodeType.EndElement ) {
                reader.ReadStartElement( "item" );

                reader.ReadStartElement( "key" );
                var key = (TK) keySerializer.Deserialize( reader );
                reader.ReadEndElement();

                reader.ReadStartElement( "value" );
                var value = (TV) valueSerializer.Deserialize( reader );
                reader.ReadEndElement();

                Add( key, value );

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer) {
            var keySerializer   = new XmlSerializer( typeof(TK) );
            var valueSerializer = new XmlSerializer( typeof(TV) );
            try {
                foreach ( var key in this.Keys ) {
                    writer.WriteStartElement( "item" );

                    writer.WriteStartElement( "key" );
                    keySerializer.Serialize( writer, key );
                    writer.WriteEndElement();

                    writer.WriteStartElement( "value" );
                    var value = this[key];
                    valueSerializer.Serialize( writer, value );
                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            } catch { writer.Close(); }
        }

        #endregion


        public delegate void DictionaryChanged(object sender, DictChangedEventArgs<TK, TV> e);

        public event DictionaryChanged OnDictionaryChanged;

        [DebuggerStepThrough]
        private void Invoke(TypeE type, TK key, TV value) {
            try { this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Key = key, Value = value } ); } catch (Exception e) {
                Console.WriteLine( e );
            }
        }

        [DebuggerStepThrough]
        private void Invoke(TypeE type, TK key) {
            try { this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Key = key } ); } catch (Exception e) {
                Console.WriteLine( e );
            }
        }

        [DebuggerStepThrough]
        private void Invoke(TypeE type) {
            try { this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type } ); } catch (Exception e) {
                Console.WriteLine( e );
            }
        }

        [DebuggerStepThrough]
        private void Invoke(TypeE type, TV value) {
            try { this.OnDictionaryChanged?.Invoke( this, new DictChangedEventArgs<TK, TV> { Type = type, Value = value } ); } catch (Exception e) {
                Console.WriteLine( e );
            }
        }

        #region Implementation of IEnumerable

        [DebuggerStepThrough]
        public new bool ContainsKey(TK key) {
            Invoke( TypeE.ContainsKey, key );
            return base.ContainsKey( key );
        }

        [DebuggerStepThrough]
        public new void Add(TK key, TV value) {
            Invoke( TypeE.AddItem, key, value );
            base.Add( key, value );
        }

        [DebuggerStepThrough]
        public new void Clear() {
            Invoke( TypeE.Clear );
            base.Clear();
        }

        [DebuggerStepThrough]
        public new void Remove(TK key) {
            Invoke( TypeE.RemoveItem, key );
            base.Remove( key );
        }

        [XmlIgnore]
        public new TV this[TK key] {
            [DebuggerStepThrough]
            get {
                Invoke( TypeE.GetAt, key );
                return base[key];
            }
            [DebuggerStepThrough]
            set {
                Invoke( TypeE.SetAt, key, value );
                base[key] = value;
            }
        }

        [DebuggerStepThrough]
        public TV Get(TK key) {
            Invoke( TypeE.GetAt, key );
            return base[key];
        }

        #endregion


        [XmlIgnore] public new int Count => base.Count;
    }

    public class DictChangedEventArgs <TK, TV> : EventArgs {
        public TypeE Type { get; set; }

        public TK Key { get; set; }

        public TV Value { get; set; }
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