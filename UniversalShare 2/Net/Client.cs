using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Universal_Share.Interface;
using UniversalShare_2.Handlers;

namespace UniversalShare_2.Net {
    internal class Client : Shared, ISharedAble {
        /// <inheritdoc />
        private SendHandler _send;

        private ReversesHandler _reverses;

        private TcpClient tcpClient;

        public Client(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) {
            this._send     = new SendHandler( ßProgram.EH, ßProgram.U );
            this._reverses = new ReversesHandler( ßProgram.EH, ßProgram.U );
        }

        #region Implementation of ISharedAble

        /// <inheritdoc />
        public void Start(IPAddress ipAddress) {
            TcpClient cl = NetBase.CreateClient( IPAddress.Parse( "127.0.0.1" ) );
            this.tcpClient = cl;

            new Thread( () => this._reverses.SteamServer( cl, false ) ).Start();
            Thread.Sleep( 200 );
            this._send.SendFile( cl, "..\\..\\..\\test.rar", "____" );
        }

        /// <inheritdoc />
        public void Abort() { }

        /// <inheritdoc />
        public TcpClient GetTcpClient() => this.tcpClient;

        #endregion
    }
}