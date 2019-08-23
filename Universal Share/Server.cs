using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Universal_Share.SharedComponents;

namespace Universal_Share {
    class Server {
        private Socket _socket;


        public Server() { this._socket = CreateSocket(); }


        public void Start() {
            var listener = new TcpListener( IPAddress.Any, FILE_PORT );
            listener.Start();
            while ( true ) {
                Console.WriteLine( "Waiting for connections ..." );
                var ret = HandleClient( listener.AcceptTcpClient(), "testfile" );

                Console.WriteLine( ret.Item1 );
                Console.WriteLine( ret.Item2 );
                Console.WriteLine( ret.Item3 );
                Console.WriteLine( ret.Item4 );
                Console.WriteLine( "Finished!" );
            }
        }

        Tuple<SocketError, int, int, string> HandleClient(TcpClient cl, string filename) {
            SocketError errorCode = SocketError.NotConnected;
            int         readBytes = -1;

            byte[] buffer = new byte[BUFFER_SIZE];

            var finalFileName = DEFAULT_SAVE_LOCATION + string.Concat( ( filename + DateTime.Now ).Split( Path.GetInvalidFileNameChars() ) );
            Console.WriteLine( "Saving as: "          + finalFileName );

            Directory.CreateDirectory( Path.GetDirectoryName( finalFileName ) );
            Stream strm = new FileStream( finalFileName, FileMode.OpenOrCreate );

            int blockCtr       = 0;
            int totalReadBytes = 0;
            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None, out errorCode );
                blockCtr++;
                totalReadBytes += readBytes;
                strm.Write( buffer, 0, readBytes );
            }

            cl.Close();
            strm.Close();
            return new Tuple<SocketError, int, int, string>( errorCode, totalReadBytes, blockCtr, finalFileName );
        }
    }
}