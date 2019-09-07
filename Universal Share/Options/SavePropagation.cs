using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Options {
    public partial class SavePropagation {
        [XmlIgnore] public bool Changed;


        public SerializableDictionary<string, TypeHolder>   RegList      = new SerializableDictionary<string, TypeHolder>();
        public SerializableDictionary<string, RememberType> RememberType = new SerializableDictionary<string, RememberType>();
        public SerializableDictionary<int, RegInfo>         IdStreamsMap = new SerializableDictionary<int, RegInfo>();
        public SerializableDictionary<string, TokenItem>    ToakenList   = new SerializableDictionary<string, TokenItem>();

        private readonly List<Process> _processes = new List<Process>();

        public SavePropagation() { CreatedEventHandler(); }

        [DebuggerStepThrough]
        private void CreatedEventHandler() {
            this.RegList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
            this.RememberType.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.IdStreamsMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.ToakenList.OnDictionaryChanged   += (sender, args) => ChangedEventHandler();
        }

        [DebuggerStepThrough] private void ChangedEventHandler() { this.Changed = true; }
    }

    public partial class SavePropagation {
        public bool Execute(RegInfo regInfo) {
            if ( this.RegList.ContainsKey( regInfo.Extension ) ) {
                var t = this.RegList.Get( regInfo.Extension );
                if ( t.UserConfirm ) {
                    if ( this.RememberType.ContainsKey( regInfo.Extension ) )
                        if ( this.RememberType.Get( regInfo.Extension ).IsOkOrYes() )
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

                    this.currentMapStr = ßMainPoint.ST.ReplaceString;
                    this.currentRep    = regInfo.SaveFilePath;
                    ProcessStartInfo info = new ProcessStartInfo( Map( t.OpenWith ), Map( t.Arguments ) ) { ErrorDialog = true, UseShellExecute = true };

                    var neP = Process.Start( info );
                    this._processes.Add( neP );
                    return true;
                } catch (Exception e) {
                    Console.WriteLine( e );
                    return false;
                }
            }
            else {
                ( var r22, var r12 ) = ßMainPoint.E.EditTypeHolder( new TypeHolder( "", "", true, "", true ), regInfo.Extension );
                ßMainPoint.S.RegList.Add( r12, r22 );
                Execute( regInfo );
            }

            return false;
        }

        private string currentMapStr;
        private string currentRep;

        [DebuggerStepThrough] private string Map(string s)                            { return map( s, this.currentMapStr, this.currentRep ); }
        private static                string map(string s, string mapStr, string rep) { return s.Replace( mapStr, rep ); }

        //[DebuggerStepThrough]
        public static void Load(ßProgram p) {
            try {
                p.SavePropagation = Load<SavePropagation>( SettingsStatic.SavePathS ) ?? throw new NullReferenceException();
            } catch (Exception e) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write( e.Message  );
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine( Resources.XmlLoad_DeleatingFile + SettingsStatic.SavePathS );
                Console.BackgroundColor = ConsoleColor.Black;
                try {
                    File.Delete( SettingsStatic.SavePathS );
                } catch (Exception ex) {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine( ex.Message );
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

            try {            p.SettingsStatic = Load<SettingsStatic>( SettingsStatic.SavePathStaticS ) ?? throw new NullReferenceException();
            } catch (Exception e) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine( e.Message + Resources.XmlLoad_DeleatingFile + SettingsStatic.SavePathStaticS);
                Console.BackgroundColor = ConsoleColor.Black;
                try {
                    File.Delete( SettingsStatic.SavePathStaticS );
                } catch (Exception ex) {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine( ex.Message );
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }


            p.SavePropagation.CreatedEventHandler();
        }

        public static void Save(ßProgram p) {
            Save( p.SavePropagation, SettingsStatic.SavePathS );
            Save( p.SettingsStatic,  SettingsStatic.SavePathStaticS );
        }

        private static T Load <T>(string path) {
            FileStream fileStream = null;
            try {
                if ( !File.Exists( path ) ) throw new IOException( "File Dose Not Exists" );
                var xmlSerializer = new XmlSerializer( typeof(T) );
                fileStream = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
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
            TextWriter textWriter = null;
            try {
                new FileInfo( path ).Directory?.Create();
                var xmlSerializer = new XmlSerializer( dataLayout.GetType() );
                textWriter = new StreamWriter( path );
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