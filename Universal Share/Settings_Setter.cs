using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Universal_Share {
    public partial class Settings {
        public bool RegList_Contains(RegInfo.TYPE      type) { return this.RegList.Any( x => x.RegType      == type ); }
        public bool RememberType_Contains(RegInfo.TYPE type) { return this.RememberType.Any( x => x.RegType == type ); }
        public bool IdStreamsMap_Contains(int          id)   { return this.IdStreamsMap.Any( x => x.Int32   == id ); }

        public TypeHolder   RegList_Get(RegInfo.TYPE      type) { return this.RegList.First( x => x.RegType == type ).TypeHolder; }
        public DialogResult RememberType_Get(RegInfo.TYPE type) { return this.RememberType.First( x => x.RegType == type ).DialogResult; }
        public RegInfo      IdStreamsMap_Get(int          id)   { return this.IdStreamsMap.First( x => x.Int32 == id ).RegInfo; }

        public void RegList_Remove(RegInfo.TYPE type) {
            this.RegList.Remove( this.RegList.First( x => x.RegType == type ) );
            this.Changed = true;
        }

        public void RememberType_Remove(RegInfo.TYPE type) {
            this.RememberType.Remove( this.RememberType.First( x => x.RegType == type ) );
            this.Changed = true;
        }

        public void IdStreamsMap_Remove(int id) {
            this.IdStreamsMap.Remove( this.IdStreamsMap.First( x => x.Int32 == id ) );
            this.Changed = true;
        }

        public void RegList_Add(RegInfo.TYPE type, TypeHolder typeHolder) {
            if ( !RegList_Contains( type ) ) this.RegList.Add( new RegTypeTypeHolder( type, typeHolder ) );
            this.Changed = true;
        }

        public void RememberType_Add(RegInfo.TYPE type, DialogResult value) {
            if ( !RememberType_Contains( type ) ) this.RememberType.Add( new RegTypeBoolean( type, value ) );
            this.Changed = true;
        }

        public void IdStreamsMap_Add(int id, RegInfo regInfo) {
            if ( !IdStreamsMap_Contains( id ) ) this.IdStreamsMap.Add( new Int32RegInfo( id, regInfo ) );
            this.Changed = true;
        }

        [Serializable, XmlInclude( typeof(RegTypeTypeHolder) )]
        public struct RegTypeTypeHolder {
            [XmlElement( "Type" )]       public RegInfo.TYPE RegType;
            [XmlElement( "TypeHolder" )] public TypeHolder   TypeHolder;

            public RegTypeTypeHolder(RegInfo.TYPE regType, TypeHolder typeHolder) {
                this.RegType    = regType;
                this.TypeHolder = typeHolder;
            }
        }

        [Serializable, XmlInclude( typeof(RegTypeBoolean) )]
        public struct RegTypeBoolean {
            [XmlElement( "Type" )]  public RegInfo.TYPE RegType;
            [XmlElement( "Vaule" )] public DialogResult DialogResult;

            public RegTypeBoolean(RegInfo.TYPE regType, DialogResult dialogResult) {
                this.RegType      = regType;
                this.DialogResult = dialogResult;
            }
        }

        [Serializable, XmlInclude( typeof(Int32RegInfo) )]
        public struct Int32RegInfo {
            [XmlElement( "Id" )]      public int     Int32;
            [XmlElement( "regInfo" )] public RegInfo RegInfo;

            public Int32RegInfo(int int32, RegInfo regInfo) {
                this.Int32   = int32;
                this.RegInfo = regInfo;
            }
        }
    }

    public class SerializableDictionary <TK, TV> {
        public TK type;
        public TV Key;

        private Dictionary<TK, TV> _BackupDict = new Dictionary<TK, TV>();

        public List<ValueType_L<TK, TV>> DictionaryTypes = new List<ValueType_L<TK, TV>>();

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
        public bool Contains(TK key) => this._BackupDict.ContainsKey( key );

        /// <inheritdoc />
        public void Add(TK key, TV value) {
            if ( this._BackupDict.ContainsKey( key ) ) return;
            this._BackupDict.Add( key, value );
            this.DictionaryTypes.Add( new ValueType_L<TK, TV>( key, value ) );
        }

        /// <inheritdoc />
        public void Clear() {
            this._BackupDict.Clear();
            this.DictionaryTypes.Clear();
        }

        /// <inheritdoc />
        public void Remove(TK key) {
            var item = this.DictionaryTypes.Find( x => x.Key.Equals( this._BackupDict[key] ) );
            this.DictionaryTypes.Remove( item );
            this._BackupDict.Remove( key );
        }

        /// <inheritdoc />
        public TV this[TK key] { get => this._BackupDict[key]; set => this._BackupDict[key] = value; }

        /// <inheritdoc />
        public ICollection Keys => this._BackupDict.Keys;

        /// <inheritdoc />
        public ICollection Values => this._BackupDict.Values;

        /// <inheritdoc />
        public IEnumerator GetEnumerator() { return this._BackupDict.GetEnumerator(); }

        #endregion

        #region Implementation of ICollection

        /// <inheritdoc />
        public void CopyTo(ValueType_L<TK, TV>[] array, int index) => this.DictionaryTypes.CopyTo( array, index );

        /// <inheritdoc />
        public int Count => this.DictionaryTypes.Count;

        #endregion
    }
}