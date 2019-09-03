using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Universal_Share.Interface;

namespace Universal_Share.Net {
    public class Server : SharedComponents {
        public void Start() {
            var tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
            tcpFileListener.Start();
            while ( true ) {
                SteamServer( tcpFileListener.AcceptTcpClient() );
            }
        }

        public void CreateUi() {
            Application.EnableVisualStyles();
            Application.Run( new ServerForm( this ) );
        }
    }
}