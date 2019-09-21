using System.Net;
using System.Net.Sockets;
using System.Threading;
using UniversalShareCore.Handlers;
using UniversalShareCore.LowLvlHandler;

namespace UniversalShareCore.Net {
    public class Client : Shared, ISharedAble {
        /// <inheritdoc />
        private SendHandler _send;

        private ReversesHandler _reverses;

        private TcpClient tcpClient;

        public Client(DataHandler uiHandler) : base( uiHandler ) {
            this._send     = new SendHandler(  uiHandler );
            this._reverses = new ReversesHandler(  uiHandler );
        }

        #region Implementation of ISharedAble

        /// <inheritdoc />
        public void Start(IPAddress ipAddress) {
            TcpClient cl = NetBase.CreateClient( IPAddress.Parse( "127.0.0.1" ), this._dataHandler );
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