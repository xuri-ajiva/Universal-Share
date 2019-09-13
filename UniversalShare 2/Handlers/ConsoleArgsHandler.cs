using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.Interface;
using UniversalShare_2.Net;

namespace UniversalShare_2.Handlers {
    public class ConsoleArgsHandler {
        private readonly                                    SettingsHandler _settingsHandler;
        [DllImport( "kernel32.dll" )] private static extern IntPtr          GetConsoleWindow();

        [DllImport( "user32.dll" )] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private IntPtr handle;

        private bool        _serverArg = false;
        private bool        _ui        = true;
        private bool        _autosave  = true;
        private bool        _console   = true;
        private ISharedAble _currentState;


        public void ProgArgs(string[] args) {
            PreInitialize();
            foreach ( var s in args ) {
                ProgArg( s );
            }

            Initialize();
            PostInitialize();
        }

        private void ProgArg(string s) {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (s.ToLower()) {
                case "": return;
                case "s":
                    this._serverArg = true;
                    break;
                case "c":
                    this._serverArg = false;
                    break;
                case "noui":
                    this._ui = false;
                    break;
                case "ui":
                    this._ui = true;
                    break;
                case "autosave":
                    this._autosave = false;
                    break;
                case "noconsole":
                    this._console = false;
                    ShowWindow( this.handle, SW_HIDE );
                    break;
            }
        }

        private void PreInitialize() { this.handle = GetConsoleWindow(); }

        private void Initialize() {
            this._settingsHandler.LoadSettings();
            if ( this._autosave ) {
                this._settingsHandler.SavingLoop = true;
            }
        }

        private void PostInitialize() {
            //ST._AuthPrivate = new Auth( new[] { (byte) ( isServer ? 0 : 1 ), (byte) ( ui ? 0 : 1 ), (byte) ( console ? 0 : 1 ) } );
            //SetSavePath( isServer );

            //SavePropagation.Load( P );

            //var t = new Thread( () => {
            //    while ( true ) {
            //        if ( S.Changed ) SavePropagation.Save( P );
            //        Thread.Sleep( 1000 );
            //    }
            //
            //    // ReSharper disable once FunctionNeverReturns
            //} );
            //t.Start();

            if ( this._ui )
                CreateUi( this._serverArg );
            else {
                if ( this._serverArg )
                    this._currentState = new Server( ßProgram.EH, ßProgram.U );
                else
                    this._currentState = new Client( ßProgram.EH, ßProgram.U );
                this._currentState.Start( this._serverArg ? IPAddress.Any : IPAddress.Parse( ßProgram.U.UserInput.GetString( "Bitte IP Addresse Eintragen", "127.0.0.1" ) ) );
            }
        }

        public void CreateUi(bool isServer) {
            if ( isServer )
                this._currentState = new Server( ßProgram.EH, ßProgram.U );
            else
                this._currentState = new Client( ßProgram.EH, ßProgram.U );

            var form = new MainFormP( this._currentState, isServer ) { Text = this._currentState.GetType().Name };

            var t = new Thread( () => {
                Application.Run( form );
            } );

            t.SetApartmentState( ApartmentState.STA );
            t.Start();
        }

        public ConsoleArgsHandler(string[] args, SettingsHandler settingsHandler) {
            this._settingsHandler = settingsHandler;
            ProgArgs( args );
        }

        public ConsoleArgsHandler(SettingsHandler settingsHandler) { this._settingsHandler = settingsHandler; }
    }
}