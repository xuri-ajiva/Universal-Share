using System.Net.Sockets;
using UniversalShare_2.Handlers;

namespace UniversalShare_2.Net {
    internal class ReversesHandler : SendHandler {
        public void SteamServer(TcpClient cl, bool replyOnSUCCESS) {
            var readBytes = -1;
            var buffer    = new byte[BUFFER_SIZE];

            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None  );
                if ( readBytes <= 0 ) break;
                ( var message, var idRet ) = GlobalReversesProgresses( buffer, readBytes );
            }
        }

        /// <inheritdoc />
        public ReversesHandler(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) { }
    }
}