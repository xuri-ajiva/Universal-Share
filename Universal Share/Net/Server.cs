using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Universal_Share.Net {
    public class Server : SharedComponents {
        public void Start() {
            while ( true ) {
                TcpListener tcpFileListener = default;
                try {
                    tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
                    tcpFileListener.Start();
                    SteamServer( tcpFileListener.AcceptTcpClient() );

                    tcpFileListener.Start();
                } catch (Exception e) {
                    Console.WriteLine( e.Message );
                }
            }
        }
    }
}