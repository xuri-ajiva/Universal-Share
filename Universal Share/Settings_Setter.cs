using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Universal_Share {
    public partial class Settings {
        public bool         RegList_Contains(RegInfo.TYPE      type) { return this.RegList.Any( x => x.RegType      == type ); }
        public bool         RememberType_Contains(RegInfo.TYPE type) { return this.RememberType.Any( x => x.RegType == type ); }
        public bool         IdStreamsMap_Contains(int          id)   { return this.IdStreamsMap.Any( x => x.Int32   == id ); }

        public TypeHolder   RegList_Get(RegInfo.TYPE           type) { return this.RegList.First( x => x.RegType == type ).TypeHolder; }
        public DialogResult RememberType_Get(RegInfo.TYPE      type) { return this.RememberType.First( x => x.RegType == type ).DialogResult; }
        public RegInfo      IdStreamsMap_Get(int               id)   { return this.IdStreamsMap.First( x => x.Int32 == id ).RegInfo; }

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
}