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
    class Server : SharedComponents {
        private Socket _socket;


        public Server() { this._socket = CreateSocket(); }


        public void Start() {
            StartFileHandler();
            StartTextHandler();
        }
    }
}