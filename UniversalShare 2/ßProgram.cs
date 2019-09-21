using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.Interface;
using UniversalShare_2.Interface;
using UniversalShareCore.Handlers;
using UniversalShareCore.LowLvlHandler;
using UniversalShareCore.Net;

namespace UniversalShare_2 {
    internal static class ßProgram {
        // ReSharper disable once InconsistentNaming
        public static DataHandler     D  { [DebuggerStepThrough] get; private set; }
        public static Editor          E  { [DebuggerStepThrough] get; private set; }
        public static SettingsHandler SH { [DebuggerStepThrough] get; private set; }
        public static FormUiHandler   F  { [DebuggerStepThrough] get; internal set; }

        //public static ExceptionHandler T { [DebuggerStepThrough] get; private set; }


        private static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );

            //_formUiHandler= new FormUiHandler( new MainFormP(  ),  );

            D  = new DataHandler( new ExceptionHandler(), F, SH, new KeyHandler() );
            E  = new Editor();
            SH = new SettingsHandler( D );

            //DebugHandler.Start();

            new Thread( () => new ConsoleArgsHandler( new string[] { "s" }, new SettingsHandler( D, "S" ) ) ).Start();
            new Thread( () => new ConsoleArgsHandler( new string[] { "c" }, new SettingsHandler( D, "C" ) ) ).Start();
            // Application.Run(new Form1());
            Console.ReadLine();
        }
    }

    internal class DebugHandler {
        public static void Start() {
            new Thread( () => new NetType().Client() ).Start();
            new Thread( () => new NetType().Server() ).Start();
        }
    }

    public class NetType {
        private SendHandler     _send;
        private ReversesHandler _reverses;

        public NetType() {
            this._send     = new SendHandler( ßProgram.D );
            this._reverses = new ReversesHandler( ßProgram.D );
        }

        public void Server() {
            TcpListener Tlisener = NetBase.CreateLisener();
            Tlisener.Start();

            while ( true ) {
                var cl = Tlisener.AcceptTcpClient();
                new Thread( () => this._reverses.SteamServer( cl, false ) ).Start();
                //Thread.Sleep( 100 );
                //new Thread( () => this._send.SendFile( cl, "..\\..\\..\\test.txt", "Server()" ) ).Start();
            }
        }

        public void Client() {
            for ( int i = 0; i < 4; i++ ) {
                TcpClient cl = NetBase.CreateClient( IPAddress.Parse( "127.0.0.1" ), ßProgram.D );
                new Thread( () => this._reverses.SteamServer( cl, false ) ).Start();
                Thread.Sleep( 200 );
                //new Thread( () => {
                var i1 = i;
                for ( int J = 0; J < 10; J++ ) {
//                        Thread.Sleep( 1 );
                    this._send.SendFile( cl, "..\\..\\..\\test.rar", "__" + i1 + "_" + J + "__" );
                }

                //} ).Start();
            }
        }
    }
}