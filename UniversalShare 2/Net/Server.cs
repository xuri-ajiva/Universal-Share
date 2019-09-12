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
    internal class Server : Shared, ISharedAble {
        /// <inheritdoc />
        public Server(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) {
            this._send     = new SendHandler( ßProgram.EH, ßProgram.U );
            this._reverses = new ReversesHandler( ßProgram.EH, ßProgram.U );
        }

        List<TcpClient> tcpClients = new List<TcpClient>();

        #region Implementation of ISharedAble

        /// <inheritdoc />
        public void Start(IPAddress ipAddress) {
            TcpListener Tlisener = NetBase.CreateLisener();
            Tlisener.Start();

            while ( true ) {
                var cl = Tlisener.AcceptTcpClient();
                this.tcpClients.Add( cl );
                new Thread( () => this._reverses.SteamServer( cl, false ) ).Start();
                //Thread.Sleep( 100 );
                //new Thread( () => this._send.SendFile( cl, "..\\..\\..\\test.txt", "Server()" ) ).Start();
            }
        }

        /// <inheritdoc />
        public void Abort() { }

        /// <inheritdoc />
        public TcpClient GetTcpClient() { return this.tcpClients?[0];}

        #endregion

        private SendHandler _send;

        private ReversesHandler _reverses;

    }
}