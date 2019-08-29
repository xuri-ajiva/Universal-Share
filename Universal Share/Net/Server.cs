using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Universal_Share.Net {
    public class Server : SharedComponents {
        public void Start() {
            TcpListener tcpFileListener = default;
            tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
            tcpFileListener.Start();
            while ( true ) {
                try {
                    SteamServer( tcpFileListener.AcceptTcpClient() );
                } catch (Exception e) {
                    Console.WriteLine( e.Message );
                }
            }
        }
    }
}