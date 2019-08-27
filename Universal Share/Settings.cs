using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Universal_Share {
    public partial class Settings {
        [XmlIgnore] public bool Changed = false;



        //[XmlIgnore] public readonly EventDictionary<RegInfo.TYPE, TypeHolder> RegList      = new EventDictionary<RegInfo.TYPE, TypeHolder>();
        //[XmlIgnore] public readonly EventDictionary<RegInfo.TYPE, bool>       RememberType = new EventDictionary<RegInfo.TYPE, bool>();
        //[XmlIgnore] public readonly EventDictionary<int, RegInfo>             IdStreamsMap = new EventDictionary<int, RegInfo>();

        public List<RegTypeTypeHolder> RegList      = new List<RegTypeTypeHolder>();
        public List<RegTypeBoolean>    RememberType = new List<RegTypeBoolean>();
        public List<Int32RegInfo>      IdStreamsMap = new List<Int32RegInfo>();

        // 
    }


    public partial class Settings {
        public bool execute(RegInfo regInfo) {
            if ( this.RegList_Contains( regInfo.Type ) ) {
                var t  = RegList_Get( regInfo.Type );
                var f1 = true;
                var f2 = true;
                var f3 = t.UserConfirm;
                if ( f3 ) {
                    if ( this.RememberType_Contains( regInfo.Type ) ) {
                        var dr = this.RememberType_Get( regInfo.Type );
                        f1 = dr == DialogResult.Yes || dr == DialogResult.OK;
                    }

                    if ( f1 ) f2 = ßMainPoint.U.GetConfirm( regInfo, t );
                    if (!f2 ) return false;
                    if ( !f1 ) return false;
                }

                try {
                    if ( t.CloseFileStream ) regInfo.Stream.Close();
                    Process.Start( t.OpenWith, t.ArgumentsBeforePathToFile + " " + regInfo.SaveFilePath + " " + t.ArgumentsAfterPathToFile );
                    return true;
                } catch (Exception e) {
                    Console.WriteLine( e );
                    return false;
                }
            }

            return false;
        }

        public static void Load(ßProgram p) { p.settings = LOAD<Settings>( SettingsStatic.SAVE_PATH_S ); }
        public static void Save(ßProgram p) { SAVE( p.settings, SettingsStatic.SAVE_PATH_S ); }

        public static T LOAD <T>(string path) {
            if ( !File.Exists( path ) ) throw new TypeAccessException();
            XmlSerializer xmlSerializer = new XmlSerializer( typeof(T) );
            FileStream    fileStream    = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
            T             obj           = (T) xmlSerializer.Deserialize( (Stream) fileStream );
            fileStream.Close();
            return obj;
        }

        public static void SAVE <T>(T dataLayout, string path) {
            new FileInfo( path ).Directory?.Create();
            XmlSerializer xmlSerializer = new XmlSerializer( dataLayout.GetType() );
            TextWriter    textWriter    = (TextWriter) new StreamWriter( path );
            xmlSerializer.Serialize( textWriter, (object) dataLayout );
            textWriter.Close();
        }
    }
}