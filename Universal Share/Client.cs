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
    class Client :SharedComponents {
        public void Start() {
            var ipend_F = new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), FILE_PORT );
            var ipend_R = new IPEndPoint( IPAddress.Parse( "127.0.0.1" ), TEXT_PORT );

            while ( true ) {
                try {
                    var register = new TcpClient();
                    register.Connect( ipend_R );
                    
                    var fileSocket = new TcpClient();
                    fileSocket.Connect( ipend_F );

                    var id = new Random().Next(100000000,999999999);

                    SendRegister( register, "test.txt", id );

                    SendFile( fileSocket, "..\\..\\..\\test.txt",id );
                    Console.WriteLine( "Finished!" );
                    register.Close();
                    fileSocket.Close();
                    
                    Thread.Sleep( 1000 );
                } catch {
                    Thread.Sleep( 1000 );
                }
            }
        }


    }
}