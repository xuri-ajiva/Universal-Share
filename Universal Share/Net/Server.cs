using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.Interface;

namespace Universal_Share.Net {
    public class Server : SharedComponents, ISharedAble {
        private TcpListener tcpFileListener;

        List<Thread> serverThreads = new List<Thread>();
        List<Thread> closedThreads = new List<Thread>();

        public void Start(IPAddress iPAddress) {
            this.tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
            this.tcpFileListener.Start();
            while ( true ) {
                this.closedThreads.AddRange( this.serverThreads.Where( x => !x.IsAlive ) );
                this.closedThreads.ForEach( x => this.serverThreads.Remove( x ) );
                this.closedThreads.RemoveAll( x => !x.IsAlive );

                var ls = this.tcpFileListener.AcceptTcpClient();

                var t = new Thread( () => SteamServer( ls ) );

                this.serverThreads.Add( t );
                t.Start();
            }
        }

        /// <inheritdoc />
        public void Abort() { this.tcpFileListener.Stop(); }
    }
}