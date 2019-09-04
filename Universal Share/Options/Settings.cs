using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Options {
    public partial class Settings {
        [XmlIgnore] public bool Changed;


        public SerializableDictionary<RegInfo.Type, TypeHolder>   RegList      = new SerializableDictionary<RegInfo.Type, TypeHolder>();
        public SerializableDictionary<RegInfo.Type, RememberType> RememberType = new SerializableDictionary<RegInfo.Type, RememberType>();
        public SerializableDictionary<int, RegInfo>               IdStreamsMap = new SerializableDictionary<int, RegInfo>();
        public SerializableDictionary<string, TokenItem>          ToakenList   = new SerializableDictionary<string, TokenItem>();

        public Settings() { CreatedEventHandler(); }

        private void CreatedEventHandler() {
            this.RegList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
            this.RememberType.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.IdStreamsMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.ToakenList.OnDictionaryChanged   += (sender, args) => ChangedEventHandler();
        }

        //[DebuggerStepThrough]
        private void ChangedEventHandler() { this.Changed = true; }
    }


    public partial class Settings {
        public bool Execute(RegInfo regInfo) {
            if ( this.RegList.ContainsKey( regInfo.TypeP ) ) {
                var t = this.RegList.Get( regInfo.TypeP );
                if ( t.UserConfirm ) {
                    if ( this.RememberType.ContainsKey( regInfo.TypeP ) )
                        if ( this.RememberType.Get( regInfo.TypeP ).IsOkOrYes() )
                            goto start;
                        else { return false; }

                    if ( ßMainPoint.U.GetConfirm( regInfo, t ) ) {
                        goto start;
                    }

                    return false;
                }

                start:
                try {
                    if ( t.CloseFileStream ) regInfo.Stream?.Close();
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
            p.Settings = Load<Settings>( SettingsStatic.SavePathS ) ?? throw new NullReferenceException();
            p.Settings.CreatedEventHandler();
        }

        public static void Save(ßProgram p) { Save( p.Settings, SettingsStatic.SavePathS ); }


        private static T Load <T>(string path) {
            FileStream    fileStream    = null;
            try {
                if ( !File.Exists( path ) ) throw new IOException( "File Dose Not Exists" );
                var xmlSerializer = new XmlSerializer( typeof(T) );
                fileStream    = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
                var obj = (T) xmlSerializer.Deserialize( fileStream );
                fileStream.Close();
                return obj;
            } catch (Exception ex) {
                fileStream?.Close();
                fileStream?.Dispose();
                Console.WriteLine( ex.Message );
            }

            return default;
        }

        private static void Save <T>(T dataLayout, string path) {
            TextWriter    textWriter    = null;
            try {
                new FileInfo( path ).Directory?.Create();
                var xmlSerializer = new XmlSerializer( dataLayout.GetType() );
                textWriter    = new StreamWriter( path );
                xmlSerializer.Serialize( textWriter, dataLayout );
                textWriter.Close();
            } catch (Exception ex) {
                textWriter?.Close();
                textWriter?.Dispose();
                Console.WriteLine( ex.Message );
            }
        }
    }
}