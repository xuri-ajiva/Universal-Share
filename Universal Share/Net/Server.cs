using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Universal_Share.Net {
    public class Server : SharedComponents, ISharedAble {
        private TcpListener _tcpFileListener;

        private readonly List<Thread> _serverThreads = new List<Thread>();
        private readonly List<Thread> _closedThreads = new List<Thread>();

        public void Start(IPAddress iPAddress) {
            this._tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
            this._tcpFileListener.Start();
            while ( true ) {
                this._closedThreads.AddRange( this._serverThreads.Where( x => !x.IsAlive ) );
                this._closedThreads.ForEach( x => this._serverThreads.Remove( x ) );
                this._closedThreads.RemoveAll( x => !x.IsAlive );

                var ls = this._tcpFileListener.AcceptTcpClient();

                var t = new Thread( () => SteamServer( ls ) );

                this._serverThreads.Add( t );
                t.Start();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        /// <inheritdoc />
        public void Abort() { this._tcpFileListener.Stop(); }

        /// <inheritdoc />
        public TcpClient GetTcpClient() => throw new NotSupportedException();

        /// <inheritdoc />
        public TcpListener GetTcpListener() => this._tcpFileListener;
    }
}