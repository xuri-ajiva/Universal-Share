using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using UniversalShare_2.Handlers;
using UniversalShare_2.Interface;
using UniversalShare_2.Net;

namespace UniversalShare_2 {
    internal static class ßProgram {
        // ReSharper disable once InconsistentNaming
        public static ExceptionHandler @EH { [DebuggerStepThrough] get; private set; }
        public static DataHandler      D   { [DebuggerStepThrough] get; private set; }
        public static UiHandler        U   { [DebuggerStepThrough] get; private set; }
        public static byte[]           T   { [DebuggerStepThrough] get; private set; }
        public static Editor           E   { [DebuggerStepThrough] get; private set; }

        //public static ExceptionHandler T { [DebuggerStepThrough] get; private set; }


        private static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            
            U = new UiHandler( EH );
            EH = new ExceptionHandler();
            D  = new DataHandler();
            T  = new byte[KeyHandler.LENGTH_B];
            E = new Editor();

            //DebugHandler.Start();

            new Thread( () => new ConsoleArgsHandler( new string[] { "s" } ) ).Start();
            new Thread( () => new ConsoleArgsHandler( new string[] { "c" } ) ).Start();
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

    internal class NetType {
        private SendHandler     _send;
        private ReversesHandler _reverses;

        public NetType() {
            this._send     = new SendHandler( ßProgram.EH, ßProgram.U );
            this._reverses = new ReversesHandler( ßProgram.EH, ßProgram.U );
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
                TcpClient cl = NetBase.CreateClient( IPAddress.Parse( "127.0.0.1" ) );
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