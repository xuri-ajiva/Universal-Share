using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniversalShareCore.Handlers;
using UniversalShareCore.LowLvlHandler;

namespace UniversalShareCore.Net {
    public class Server : Shared, ISharedAble {
        /// <inheritdoc />
        public Server( DataHandler dataHandler) : base( dataHandler ) {
            this._send     = new SendHandler( dataHandler);
            this._reverses = new ReversesHandler(  dataHandler );
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