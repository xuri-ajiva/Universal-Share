using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Universal_Share.ProgMain;

namespace Universal_Share.Net {
    public class ClientBase : ServerBase {
        protected TcpClient OpenIpAddress(IPAddress ipAddress) {
            var ipendF     = new IPEndPoint( ipAddress, this.FilePort );
            var fileSocket = new TcpClient();
            fileSocket.Connect( ipendF );
            return fileSocket;
        }


        public (string, int) SendFile(TcpClient fileSocket, string fileName) {
            var id = 0;
            try {
                id = new Random().Next( 10000000, 19999999 );

                var ret1 = SendRegisterStream( fileSocket, Path.GetFileName( fileName ), id );

                Thread.Sleep( 100 );

                SteamClient( fileSocket, fileName, id );
                return ( ret1, id );
            } catch (Exception e) {
                return ( e.Message, id );
            }
        }

        private string SendRegisterStream(TcpClient communicationSocket, String saveFileName, int id) {
            var idB       = Encoding.UTF8.GetBytes( id.ToString() );
            var filanemeB = Encoding.UTF8.GetBytes( saveFileName );

            var b = Parts_To_Buffer( ßMainPoint.T, idB, Option.CreateRegister, filanemeB );

            communicationSocket.Client.Send( b );

            var buffer = new byte[BUFFER_SIZE];
            var readet = communicationSocket.Client.Receive( buffer, SocketFlags.None );

            var t = Buffer_To_Parts( buffer, readet );

            return ( Encoding.UTF8.GetString( t.Item4 ) );
        }

        public void SteamClient(TcpClient cl, string filename, int id) {
            var    readerBytes = -1;

            var idB = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            //var blockCtr       = 0;
            //var totalReadBytes = 0;

            while ( readerBytes != 0 ) {
                var buffer = new byte[BUFFER_SIZE - HEATHER_SIZE];
                readerBytes = strm.Read( buffer, 0, BUFFER_SIZE - HEATHER_SIZE );
                if ( readerBytes == -1 ) break;

                var ret = Parts_To_Buffer( ßMainPoint.T, idB, Option.SaveToFile, buffer );

                cl.Client.Send( ret, SocketFlags.None );

                buffer = new byte[BUFFER_SIZE];

                var readBytes2 = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None );

                //var sp = Buffer_To_Parts( buffer, readBytes2 );

                var t = GlobalReversesProgresses( readBytes2, buffer );
                if ( t.Item2 == -1 ) Console.WriteLine( t.Item1 );

                //Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
                //blockCtr++;
                //totalReadBytes += readerBytes;
            }

            strm.Close();
        }
    }
}