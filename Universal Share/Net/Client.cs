using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Universal_Share.Net {
    public class Client : SharedComponents, ISharedAble {
        private TcpClient _tcpClient;

        public void Start(IPAddress ipAddress) {
            //var ip = IPAddress.Parse( "127.0.0.1" );
            this._tcpClient = OpenIpAddress( ipAddress );
            while ( true ) {
                try {
                    SendFile( this._tcpClient, "..\\..\\..\\test.exe" );
                    Thread.Sleep( 1000 );
                } catch (Exception e) {
                    Console.WriteLine( e.Message );
                    Thread.Sleep( 1000 );
                }
            }

            // ReSharper disable once FunctionNeverReturns
        }

        /// <inheritdoc />
        public void Abort() {
            this._tcpClient.Close();
        }
    }
}