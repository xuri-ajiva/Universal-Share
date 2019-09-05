using System;
using System.Net.Sockets;
using System.Text;
using Universal_Share.ProgMain;

namespace Universal_Share.Net {
    public class ServerBase : NetBase {
        public void SteamServer(TcpClient cl) {
            Console.WriteLine( @"Server Started" );
            //var errorCode = SocketError.NotConnected;

            var readBytes = -1;
            var buffer    = new byte[BUFFER_SIZE];

            //var blockCtr       = 0;
            //var totalReadBytes = 0;

            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None/*, out errorCode*/ );
                if ( readBytes <= 0 ) break;
                ( var message, var idRet ) = GlobalReversesProgresses( buffer, readBytes );

                var option = message == SUCCESS ? Option.Success : Option.Error;

                var messageBytesResponce = Encoding.UTF8.GetBytes( message );

                cl.Client.Send( Parts_To_Buffer( ßMainPoint.T, Encoding.UTF8.GetBytes( idRet.ToString() ), option, messageBytesResponce, messageBytesResponce.Length ) );

                //blockCtr++;
                //totalReadBytes += readBytes;
            }
        }
    }
}