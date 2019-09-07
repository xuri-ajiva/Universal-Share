using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Universal_Share.Net {
    public class Server : SharedComponents, ISharedAble {
        private         TcpListener     _tcpFileListener;
        public readonly List<TcpClient> tcpClients = new List<TcpClient>();

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
                this.tcpClients.Add( ls );

                Console.WriteLine( "New Socket!" );
                var t = new Thread( () => SteamServer( ls,true ) );

                this._serverThreads.Add( t );
                t.Start();
            }

            // ReSharper disable once FunctionNeverReturns
        }

        /// <inheritdoc />
        public void Abort() { this._tcpFileListener.Stop(); }

        /// <inheritdoc />
        public TcpClient GetTcpClient() => this.tcpClients.Count > 0 ? this.tcpClients[0] : null;

        /// <inheritdoc />
        public TcpListener GetTcpListener() => this._tcpFileListener;
    }
}