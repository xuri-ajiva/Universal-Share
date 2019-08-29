using System;
using System.Net;
using System.Threading;

namespace Universal_Share.Net {
    public class Client : SharedComponents {
        public void Start() {
            var ip = IPAddress.Parse( "127.0.0.1" );

            while ( true ) {
                try {
                    SendFile2( ip, "..\\..\\..\\test.exe" );
                    Thread.Sleep( 1000 );
                } catch (Exception e) {
                    Console.WriteLine( e.Message );
                    Thread.Sleep( 1000 );
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}