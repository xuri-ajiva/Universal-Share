using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.hocks;
using Universal_Share.Interface;
using Universal_Share.Net;
using Universal_Share.Options;
using Universal_Share.Security;

#pragma warning disable 162

//// ReSharper disable ConditionIsAlwaysTrueOrFalse
//// ReSharper disable HeuristicUnreachableCode
//#pragma warning disable 162

namespace Universal_Share.ProgMain {
    public static class ßMainPoint {
        private const bool DEBUG          = true;
        private const bool SERVER         = true;
        private const bool START_OPPOSITE = true;

        private static ßProgram  _prgMain;
        private static Auth      _auth;
        private static Editor    _editor;
        private static UserInput _userInput;

        public static byte[]    K => _auth.KeyBytes;
        public static byte[]    T => _auth.TokenBytes;
        public static Settings  S { get => _prgMain.settings; set => _prgMain.settings = value; }
        public static UserInput U { get => _userInput;        private set => _userInput = value; }
        public static ßProgram  P => _prgMain;
        public static Editor    E => _editor;

        private static ISharedAble CurrentState;

        public static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( true );
            PreInitialize();

            if ( DEBUG ) {
                if ( args.Length == 0 ) {
                    if ( SERVER ) {
                        SettingsStatic.SAVE_PATH_S = "S_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Server..." );
                        Console.Title = "server";
                        InitializeAll();
                        if ( START_OPPOSITE ) Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "C" );
                        CreateUi( true );
                    }
                    else {
                        SettingsStatic.SAVE_PATH_S = "C_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Client..." );
                        Console.Title = "client";
                        InitializeAll();
                        if ( START_OPPOSITE ) Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, "S" );
                        CreateUi( false );
                    }
                }
                else {
                    StartNormal( args );
                }
            }
            else
                StartNormal( args );
        }

        private static void PreInitialize() {
            _prgMain = new ßProgram();
            U        = new UserInput();
            _auth    = new Auth();
            _editor  = new Editor();

            hocks.Exit.CreateHock();
        }

        public static void CreateUi(bool isServer) {
            Application.EnableVisualStyles();
            if ( isServer )
                CurrentState = new Server();
            else
                CurrentState = new Client();

            var form = new ServerForm( CurrentState, isServer ) { Text = CurrentState.GetType().Name };
            Application.Run( form );
        }

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

        public static void InitializeAll() {
            try {
                Settings.Load( P );
            } catch (Exception e) {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine( e.Message + Resources.XmlLoad_DeleatingFile );
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
                    if ( S.Changed ) Settings.Save( P );
                    Thread.Sleep( 1000 );
                }
            } );
            t.Start();

            _auth = new Auth( SettingsStatic.SAVE_PATH_S );
            try {
                S.RegList.Add( RegInfo.TYPE.SINGLE_FILE, new TypeHolder( "cmd", "/c echo", " && timeout 3", true, "descript", false ) );
            } catch { }
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
                        InitializeAll();
                        CreateUi( true );
                        break;
                    case "c":
                        SettingsStatic.SAVE_PATH_S = "C_" + SettingsStatic.SAVE_PATH_S;
                        Console.WriteLine( "starting Client..." );
                        Console.Title = "client";
                        InitializeAll();
                        CreateUi( false );
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