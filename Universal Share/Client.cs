using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Universal_Share {
    public class Client : SharedComponents {
        public void Start() {
            var ip = IPAddress.Parse( "127.0.0.1" );

            while ( true ) {
                try {
                    SendFile( ip, "..\\..\\..\\test.txt" );
                    Thread.Sleep( 1000 );
                } catch (Exception e) {
                    Console.WriteLine( e.Message );
                    Thread.Sleep( 1000 );
                }
            }
        }
    }
}