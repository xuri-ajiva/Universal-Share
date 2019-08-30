using System;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Options {
    public partial class Settings {
        [XmlIgnore] public bool Changed = false;


        public SerializableDictionary<RegInfo.TYPE, TypeHolder>   RegList      = new SerializableDictionary<RegInfo.TYPE, TypeHolder>();
        public SerializableDictionary<RegInfo.TYPE, RememberType> RememberType = new SerializableDictionary<RegInfo.TYPE, RememberType>();
        public SerializableDictionary<int, RegInfo>               IdStreamsMap = new SerializableDictionary<int, RegInfo>();
        public SerializableDictionary<string, TokenItem>          ToakenList   = new SerializableDictionary<string, TokenItem>();

        public Settings() { CreatedEventHandler(); }

        public void CreatedEventHandler() {
            this.RegList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
            this.RememberType.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.IdStreamsMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
        }

        [DebuggerStepThrough] private void ChangedEventHandler() { this.Changed = true; }
    }


    public partial class Settings {
        public bool execute(RegInfo regInfo) {
            if ( this.RegList.Contains( regInfo.Type ) ) {
                var t  = this.RegList.Get( regInfo.Type );
                var f1 = false;
                var f2 = true;
                var f3 = t.UserConfirm;
                if ( f3 ) {
                    if ( this.RememberType.Contains( regInfo.Type ) ) {
                        var dr = this.RememberType.Get( regInfo.Type );
                        f1 = dr.IsOkOrYes();
                    }

                    if ( !f1 ) f2 = ßMainPoint.U.GetConfirm( regInfo, t );
                    if ( !f2 ) return false;
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

        public static void Load(ßProgram p) {
            p.settings = LOAD<Settings>( SettingsStatic.SAVE_PATH_S ) ?? throw new NullReferenceException();
            p.settings.CreatedEventHandler();
        }

        public static void Save(ßProgram p) { SAVE( p.settings, SettingsStatic.SAVE_PATH_S ); }


        public static T LOAD <T>(string path) {
            XmlSerializer xmlSerializer = null;
            FileStream    fileStream    = null;
            try {
                if ( !File.Exists( path ) ) throw new IOException( "File Dose Not Exists" );
                xmlSerializer = new XmlSerializer( typeof(T) );
                fileStream    = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
                T obj = (T) xmlSerializer.Deserialize( fileStream );
                fileStream.Close();
                return obj;
            } catch (Exception ex) {
                xmlSerializer = null;
                fileStream?.Close();
                fileStream?.Dispose();
                Console.WriteLine( ex.Message );
            }

            return default;
        }

        public static void SAVE <T>(T dataLayout, string path) {
            XmlSerializer xmlSerializer = null;
            TextWriter    textWriter    = null;
            try {
                new FileInfo( path ).Directory?.Create();
                xmlSerializer = new XmlSerializer( dataLayout.GetType() );
                textWriter    = new StreamWriter( path );
                xmlSerializer.Serialize( textWriter, dataLayout );
                textWriter.Close();
            } catch (Exception ex) {
                xmlSerializer = null;
                textWriter?.Close();
                textWriter?.Dispose();
                Console.WriteLine( ex.Message );
            }
        }
    }
}