using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
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
        [DllImport( "kernel32.dll" )] static extern IntPtr GetConsoleWindow();

        [DllImport( "user32.dll" )] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const         int  SW_HIDE        = 0;
        const         int  SW_SHOW        = 5;
        private const bool DEBUG          = true;
        private const bool SERVER         = false;
        private const bool START_OPPOSITE = true;

        private static ßProgram       _prgMain;
        private static Editor         _editor;
        private static UserInput      _userInput;
        private static SettingsEditor _settingsEditor;

        public static byte[]          K  => ST._AuthPrivate.KeyBytes;
        public static byte[]          T  => ST._AuthPrivate.TokenBytes;
        public static SavePropagation S  { get => _prgMain.SavePropagation; set => _prgMain.SavePropagation = value; }
        public static UserInput       U  { get => _userInput;               private set => _userInput = value; }
        public static ßProgram        P  => _prgMain;
        public static Editor          E  => _editor;
        public static SettingsEditor  SF => _settingsEditor;
        public static SettingsStatic  ST => _prgMain.SettingsStatic;

        private static IntPtr handle = GetConsoleWindow();

        private static ISharedAble _currentState;

        public static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( true );
            PreInitialize();
            //Application.Run( new SettingsEditor() );
            //Environment.Exit( 0 );

            if ( args.Length > 0 ) {
                ProgArgs( args );
            }
            else if ( DEBUG ) {
                if ( args.Length == 0 ) {
                    if ( START_OPPOSITE ) Process.Start( System.Reflection.Assembly.GetEntryAssembly()?.Location, SERVER ? "C" : "S noui" );
                    InitializeAll( SERVER );
                }
                else {
                    ProgArgs( args );
                }
            }
            else
                ProgArgs( args );
        }

        private static void ProgArgs(string[] args) {
            foreach ( var s in args ) {
                ProgArg( s );
            }

            InitializeAll( serverArg, ui );
        }

        static bool serverArg = false;
        static bool ui        = true;
        static bool console   = true;

        private static void ProgArg(string s) {
            switch (s.ToLower()) {
                case "": return;
                case "s":
                    serverArg = true;
                    break;
                case "c":
                    serverArg = false;
                    break;
                case "noui":
                    ui = false;
                    break;
                case "ui":
                    ui = true;
                    break;
                case "noconsole":
                    console = false;
                    ShowWindow( handle, SW_HIDE );
                    break;
            }
        }

        private static void PreInitialize() {
            _prgMain        = new ßProgram();
            U               = new UserInput();
            _editor         = new Editor();
            _settingsEditor = new SettingsEditor();

            hocks.Exit.CreateHock();
        }

        [STAThread]
        public static void CreateUi(bool isServer) {
            Application.EnableVisualStyles();
            if ( isServer )
                _currentState = new Server();
            else
                _currentState = new Client();

            var form = new MainFormP( _currentState, isServer ) { Text = _currentState.GetType().Name };

            var t = new Thread( () => {
                Application.Run( form );
            } );

            t.SetApartmentState( ApartmentState.STA );
            t.Start();
        }

        public static void SetSavePath(bool isServer) {
            if ( isServer ) {
                Console.WriteLine( @"Server..." );
                Console.Title                  = @"server";
                SettingsStatic.SavePathS       = "S_" + SettingsStatic.SavePathS;
                SettingsStatic.SavePathStaticS = "S_" + SettingsStatic.SavePathStaticS;
                return;
            }

            Console.WriteLine( @"Client..." );
            Console.Title                  = @"client";
            SettingsStatic.SavePathS       = "C_" + SettingsStatic.SavePathS;
            SettingsStatic.SavePathStaticS = "C_" + SettingsStatic.SavePathStaticS;
        }

        public static bool Exit(hocks.Exit.CtrlType sig = hocks.Exit.CtrlType.CTRL_BREAK_EVENT) {
            Console.WriteLine( sig.ToString() );
            switch (sig) {
                case hocks.Exit.CtrlType.CTRL_C_EVENT:        return ui;
                case hocks.Exit.CtrlType.CTRL_LOGOFF_EVENT:   return true;
                case hocks.Exit.CtrlType.CTRL_SHUTDOWN_EVENT: return true;
                case hocks.Exit.CtrlType.CTRL_CLOSE_EVENT:    return true;
                default:                                      return false;
            }
        }

        public static void InitializeAll(bool isServer, bool ui = true) {
            ST._AuthPrivate = new Auth( new[] { (byte) ( isServer ? 0 : 1 ), (byte) ( ui ? 0 : 1 ), (byte) ( console ? 0 : 1 ) } );
            SetSavePath( isServer );

            SavePropagation.Load( P );

            var t = new Thread( () => {
                while ( true ) {
                    if ( S.Changed ) SavePropagation.Save( P );
                    Thread.Sleep( 1000 );
                }

                // ReSharper disable once FunctionNeverReturns
            } );
            t.Start();

            try {
                S.RegList.Add( ".exe", new TypeHolder( "cmd", "/c echo %V% && timeout 3", true, "descript", false ) );
            } catch {
                // ignored
            }

            if ( ui )
                CreateUi( isServer );
            else {
                if ( isServer )
                    _currentState = new Server();
                else
                    _currentState = new Client();
                _currentState.Start( isServer ? IPAddress.Any : IPAddress.Parse( ßMainPoint.U.GetString( "Bitte IP Addresse Eintragen", "127.0.0.1" ) ) );
            }
        }
    }
}