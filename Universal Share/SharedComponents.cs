using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Universal_Share {
    static class SharedComponents {
        public const int FILE_PORT = 4333;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";

        
        public const int BUFFER_SIZE = 32768;

        public static Socket CreateSocket() => new Socket( SocketType.Stream, ProtocolType.Tcp );
    }
}