using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Serialization;
using UniversalShare_2.Handlers;

namespace UniversalShare_2.Net {
    internal class ReversesHandler : SendHandler {
        [XmlIgnore] public Dictionary<string, List<string>> _returns = new Dictionary<string, List<string>>();
        [XmlIgnore] public List<Thread> threads = new List<Thread>();

        public void SteamServer(TcpClient cl, bool replyOnSUCCESS) {
            var readBytes = -1;
            var buffer    = new byte[BUFFER_SIZE];

            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None );
                if ( readBytes <= 0 ) break;

                var t = new Thread( () => {
                    ( var message, var idRet ) = GlobalReversesProgresses( buffer, readBytes,cl );
                    if ( !this._returns.ContainsKey( idRet ) ) this._returns.Add( idRet, new List<string>() );
                    this._returns[idRet].Add( message );
                } );
                this.threads.Add( t );
                t.Start();
            }
        }

        /// <inheritdoc />
        public ReversesHandler(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) { }
    }
}