﻿using System;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Universal_Share.ProgMain;

namespace Universal_Share.Options {
    public partial class Settings {
        [XmlIgnore] public bool Changed = false;


        public SerializableDictionary<RegInfo.TYPE, TypeHolder>   RegList      = new SerializableDictionary<RegInfo.TYPE, TypeHolder>();
        public SerializableDictionary<RegInfo.TYPE, DialogResult> RememberType = new SerializableDictionary<RegInfo.TYPE, DialogResult>();
        public SerializableDictionary<int, RegInfo>               IdStreamsMap = new SerializableDictionary<int, RegInfo>();

        public Settings() { CreatedEventHandler(); }

        public void CreatedEventHandler() {
            this.RegList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
            this.RememberType.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.IdStreamsMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
        }
        [DebuggerStepThrough]
        private void ChangedEventHandler() {
            this.Changed = true;
        }
    }


    public partial class Settings {
        public bool execute(RegInfo regInfo) {
            if ( this.RegList.Contains( regInfo.Type ) ) {
                var t  = this.RegList.Get( regInfo.Type );
                var f1 = true;
                var f2 = true;
                var f3 = t.UserConfirm;
                if ( f3 ) {
                    if ( this.RememberType.Contains( regInfo.Type ) ) {
                        var dr = this.RememberType.Get( regInfo.Type );
                        f1 = dr == DialogResult.Yes || dr == DialogResult.OK;
                    }

                    if ( f1 ) f2 = ßMainPoint.U.GetConfirm( regInfo, t );
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
            p.settings = LOAD<Settings>( SettingsStatic.SAVE_PATH_S );
            p.settings.CreatedEventHandler();
        }

        public static void Save(ßProgram p) {
            SAVE( p.settings, SettingsStatic.SAVE_PATH_S );
        }



        public static T LOAD <T>(string path) {
            if ( !File.Exists( path ) ) throw new IOException( "File Dose Not Exists" );
            XmlSerializer xmlSerializer = new XmlSerializer( typeof(T) );
            FileStream    fileStream    = new FileStream( path, FileMode.Open, FileAccess.Read, FileShare.Read );
            T             obj           = (T) xmlSerializer.Deserialize( fileStream );
            fileStream.Close();
            return obj;
        }

        public static void SAVE <T>(T dataLayout, string path) {
            new FileInfo( path ).Directory?.Create();
            XmlSerializer xmlSerializer = new XmlSerializer( dataLayout.GetType() );
            TextWriter    textWriter    = new StreamWriter( path );
            xmlSerializer.Serialize( textWriter, dataLayout );
            textWriter.Close();
        }
    }
}