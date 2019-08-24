using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Universal_Share {
    class Server : SharedComponents {
        private Socket _socket;


        public Server() { this._socket = CreateSocket(); }


        public void Start() {
            
            var tcpFileListener = new TcpListener( IPAddress.Any, FILE_PORT );
            var register = new TcpListener( IPAddress.Any, TEXT_PORT );

            Thread fileThread = new Thread( () => FileHandler(tcpFileListener) );
            Thread textThread = new Thread(() => TextHandler(register));
            
            register.Start();
            tcpFileListener.Start();
            
            fileThread.Start();
            textThread.Start();
        }

        private void TextHandler(TcpListener listener) {
            while ( true ) {
                
                Console.WriteLine("[{0}] Waiting for connections ..." , System.Reflection.MethodBase.GetCurrentMethod() );
                var reg = CreateRegister( listener.AcceptTcpClient() );

                Console.WriteLine( reg.Item1 );
                Console.WriteLine( reg.Item2 );
                Console.WriteLine( reg.Item3 );
                Console.WriteLine( reg.Item4 );
                Console.WriteLine( "Finished!" );
            }
        }

        private void FileHandler(TcpListener listener) {
            while ( true ) {
                
                Console.WriteLine("[{0}] Waiting for connections ..." , System.Reflection.MethodBase.GetCurrentMethod() );
                var ret = HandleClient( listener.AcceptTcpClient());
                
                Console.WriteLine( ret.Item1 );
                Console.WriteLine( ret.Item2 );
                Console.WriteLine( ret.Item3 );
                Console.WriteLine( ret.Item4 );
                Console.WriteLine( "Finished!" );
            }
        }
    }
}