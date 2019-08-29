using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Universal_Share.hocks;
using Universal_Share.Interface;
using Universal_Share.Net;
using Universal_Share.Options;
using Universal_Share.Security;

//// ReSharper disable ConditionIsAlwaysTrueOrFalse
//// ReSharper disable HeuristicUnreachableCode
//#pragma warning disable 162

namespace Universal_Share.ProgMain {
    public static class ßMainPoint {
        private const bool DEBUG          = true;
        private const bool SERVER         = false;
        private const bool START_OPPOSITE = true;

        private static readonly ßProgram  _prgMain   = new ßProgram();
        private static          UserInput _userInput = new UserInput();
        private static          Auth      _auth      = new Auth();

        public static byte[]    K       { get => _auth.KeyBytes; }
        public static byte[]    T       { get => _auth.TokenBytes; }
        public static Settings  S       { get => _prgMain.settings; set => _prgMain.settings = value; }
        public static UserInput U       { get => _userInput;        set => _userInput = value; }
        public static ßProgram  PrgMain => _prgMain;

        public static void Main(string[] args) {
            PreInizialze();

            S.RegList.Add( RegInfo.TYPE.SINGLE_FILE, new TypeHolder( "cmd", "/c echo", " && pause", true, "descript", false ) );

            if ( DEBUG ) {
                if ( args.Length == 0 ) {
                    if ( SERVER ) {
                        SettingsStatic.SAVE_PATH_S = "S_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Server..." );
                        Console.Title = "server";
                        InizialzeAll();
                        if ( START_OPPOSITE ) Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "C" );
                        new Server().Start();
                    }
                    else {
                        SettingsStatic.SAVE_PATH_S = "C_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Client..." );
                        Console.Title = "client";
                        InizialzeAll();
                        if ( START_OPPOSITE ) Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "S" );
                        new Client().Start();
                    }
                }
                else {
                    StartNormal( args );
                }
            }
            else
                StartNormal( args );
        }

        private static void PreInizialze() { hocks.Exit.CreateHock(); }

        public static bool Exit(hocks.Exit.CtrlType sig = hocks.Exit.CtrlType.CTRL_BREAK_EVENT) {
            Console.WriteLine( sig.ToString() );
            switch (sig) {
                case hocks.Exit.CtrlType.CTRL_C_EVENT:        return true;
                case hocks.Exit.CtrlType.CTRL_LOGOFF_EVENT:   return true;
                case hocks.Exit.CtrlType.CTRL_SHUTDOWN_EVENT: return true;
                case hocks.Exit.CtrlType.CTRL_CLOSE_EVENT:    return true;
                default:                                      return false;
            }
        }

        public static void InizialzeAll() {
            try {
                Settings.Load( PrgMain );
            } catch (Exception e) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine( e.Message + "\n Die Settings Datei Wird Gelöscht..." );
                Console.BackgroundColor = ConsoleColor.Black;
                try {
                    File.Delete( SettingsStatic.SAVE_PATH_S );
                } catch (Exception ex) {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine( ex.Message );
                    Console.BackgroundColor = ConsoleColor.Black;
                }
            }

            var t = new Thread( () => {
                while ( true ) {
                    if ( S.Changed ) Settings.Save( PrgMain );
                    Thread.Sleep( 1000 );
                }
            } );
            t.Start();
        }

        private static void StartNormal(string[] args) {
            if ( args.Length > 0 ) {
                Console.WriteLine( "Normal Start" );
                Console.WriteLine( args[0] );
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (args[0].ToLower()) {
                    case "s":
                        SettingsStatic.SAVE_PATH_S = "S_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Server..." );
                        Console.Title = "server";
                        InizialzeAll();
                        new Server().Start();
                        break;
                    case "c":
                        SettingsStatic.SAVE_PATH_S = "C_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Client..." );
                        Console.Title = "client";
                        InizialzeAll();
                        new Client().Start();
                        break;
                }
            }
            else {
                Console.WriteLine( "C / S" );
            }

            Console.WriteLine( "Pause ..." );
            Console.ReadKey();
        }
    }
}