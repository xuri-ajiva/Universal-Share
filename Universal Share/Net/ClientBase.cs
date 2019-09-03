using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Universal_Share.ProgMain;

namespace Universal_Share.Net {
    public class ClientBase : ServerBase {
        public (string, int) SendFile(IPAddress remoteHost, string fileName) {
            var id = 0;
            try {
                var ipendF = new IPEndPoint( remoteHost, FilePort );

                var fileSocket = new TcpClient();
                fileSocket.Connect( ipendF );

                id = new Random().Next( 10000000, 19999999 );

                var ret1 = SendRegisterStream( fileSocket, Path.GetFileName( fileName ), id );

                Thread.Sleep( 100 );

                SteamClient( fileSocket, fileName, id );

                Console.WriteLine( "Finished!" );
                fileSocket.Close();
                return ( ret1, id );
            } catch (Exception e) {
                return ( e.Message, id );
            }
        }

        private string SendRegisterStream(TcpClient communicationSocket, String saveFileName, int id) {
            byte[] idB       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] filanemeB = Encoding.UTF8.GetBytes( saveFileName );

            var b = Parts_To_Buffer( ßMainPoint.T, idB, Option.CREATE_REGISTER, filanemeB );

            communicationSocket.Client.Send( b );

            var buffer = new byte[buffer_size];
            var readet = communicationSocket.Client.Receive( buffer, SocketFlags.None );

            var t = Buffer_To_Parts( buffer, readet );

            return ( Encoding.UTF8.GetString( t.Item4 ) );
        }

        public void SteamClient(TcpClient cl, string filename, int id) {
            int    readerBytes = -1;
            byte[] buffer      = new byte[buffer_size - heather_size];

            byte[] idB = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            var blockCtr       = 0;
            var totalReadBytes = 0;

            while ( readerBytes != 0 ) {
                buffer      = new byte[buffer_size - heather_size];
                readerBytes = strm.Read( buffer, 0, buffer_size - heather_size );
                if ( readerBytes == -1 ) break;

                var ret = Parts_To_Buffer( ßMainPoint.T, idB, Option.SAVE_TO_FILE, buffer );

                cl.Client.Send( ret, SocketFlags.None );

                buffer = new byte[buffer_size];

                int readBytes2 = cl.Client.Receive( buffer, 0, buffer_size, SocketFlags.None );

                //var sp = Buffer_To_Parts( buffer, readBytes2 );

                var t = GlobalReversesProgresses( readBytes2, buffer );
                if ( t.Item2 == -1 ) Console.WriteLine( t.Item1 );

                //Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
                blockCtr++;
                totalReadBytes += readerBytes;
            }

            strm.Close();
        }
    }
}