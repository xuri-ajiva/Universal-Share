using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UniversalShare_2.Handlers;

namespace UniversalShare_2.Net {
    internal class SendHandler : NetBase {
        public void SendFile(TcpClient fileSocket, string fileName, string addToFileName = "") {
            var id = "";
            try {
                id = new Random().Next( 10000000, 19999999 ).ToString();

                SendRegisterStream( fileSocket, addToFileName + Path.GetFileName( fileName ), id );

                Thread.Sleep( 100 );

                SteamClient( fileSocket, fileName, id );
                
                Thread.Sleep( 100 );
                SendCloseStream( fileSocket,  id );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }
        }

        private void SendRegisterStream(TcpClient communicationSocket, string saveFileName, string id) {
            var idB       = Encoding.UTF8.GetBytes( id );
            var filenameB = Encoding.UTF8.GetBytes( saveFileName );

            var b = Parts_To_Buffer( ßProgram.T, idB, NetOption.CreateRegister, filenameB, filenameB.Length );

            communicationSocket.Client.Send( b );
        }

        public void SteamClient(TcpClient cl, string filename, string id) {
            var readerBytes = -1;

            var idB = Encoding.UTF8.GetBytes( id );

            Stream strm = File.OpenRead( filename );

            while ( readerBytes != 0 ) {
                var buffer = new byte[BUFFER_SIZE - HEATHER_SIZE];

                readerBytes = strm.Read( buffer, 0, BUFFER_SIZE - HEATHER_SIZE );

                if ( readerBytes <= 0 ) break;

                var x = Parts_To_Buffer( ßProgram.T, idB, NetOption.WriteInFile, buffer, readerBytes );

                cl.Client.Send( x, SocketFlags.None );
                Thread.Sleep( 10 );
            }

            strm.Close();
        }

        private void SendCloseStream(TcpClient communicationSocket, string id) {
            var idB       = Encoding.UTF8.GetBytes( id );

            var b = Parts_To_Buffer( ßProgram.T, idB, NetOption.CloseRegister,new byte[1], 1 );

            communicationSocket.Client.Send( b );
        }

        /// <inheritdoc />
        public SendHandler(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) { }
    }
}