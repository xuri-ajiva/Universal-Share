using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using static Universal_Share.SharedComponents;
namespace Universal_Share
{
    class Client
    {


        public void Start() {
            while ( true ) {
                var ipend = new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), FILE_PORT );
                var listener = new TcpClient();
                listener.Connect(ipend);

                SendFile( listener, "test.txt" );
                Console.WriteLine( "Finished!" );
            }
        }

        void SendFile(TcpClient cl, string filename) {
            SocketError errorCode = SocketError.NotConnected;
            int         readBytes = -1;
            
            byte[] buffer = new byte[BUFFER_SIZE];

            Stream strm = new FileStream( filename, FileMode.Open );

            int blockCtr       = 0;
            int totalReadBytes = 0;
            while ( readBytes != 0 ) {
                readBytes = strm.Read( buffer, 0, BUFFER_SIZE );
                blockCtr++;
                totalReadBytes += readBytes;
                cl.Client.Send( buffer, 0, readBytes, SocketFlags.None, out errorCode );
            }
            cl.Close();
            strm.Close();
            return;
        }
    }
}
