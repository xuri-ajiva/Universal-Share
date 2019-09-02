using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Universal_Share.Options;
using Universal_Share.ProgMain;

namespace Universal_Share.Net
{
    public  class ServerBase : NetBase
    {
        public (string, int) BufferBandServer(int readBytes, byte[] buffer) {
            ( var token, var idB, var option, var conetnd ) = Buffer_To_Parts( buffer, readBytes );


            if ( !int.TryParse( Encoding.UTF8.GetString( idB ), out var id ) ) return ( ID_NOT_EXIST, id );

            if ( !IsKeyVailed( token ) ) {
                return ( TOKEN_NOT_ACCEPTED, id );
            }
            ( var message, var idBX) =  processOptionsServer( token, id, option, conetnd );
            var ret = message == SUCCESS ? SUCCESS : message;
            Console.WriteLine( "Paket: id = [{0}] , Token(0,8) = [{1}]",  id , string.Join( ", ", SubArray( token,0,8 ) ) );
            return ( ret, id );
        }
        
        public void SteamServer(TcpClient cl) {
            var errorCode = SocketError.NotConnected;

            var readBytes = -1;
            var buffer    = new byte[buffer_size];

            var blockCtr       = 0;
            var totalReadBytes = 0;

            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, buffer_size, SocketFlags.None, out errorCode );
                if ( readBytes <= 0 ) break;
                ( var message, var idRet ) = this.GlobalReversesProgresses( readBytes, buffer );

                var option = message == SUCCESS ? Net.Option.SUCCESS : Net.Option.ERROR;

                cl.Client.Send( Parts_To_Buffer( ßMainPoint.T, Encoding.UTF8.GetBytes( idRet.ToString() ),option , Encoding.UTF8.GetBytes( message ) ) );

                blockCtr++;
                totalReadBytes += readBytes;
            }
        }
    }
}
