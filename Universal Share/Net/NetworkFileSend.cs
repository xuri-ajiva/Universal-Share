using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace Universal_Share.Net {
    public partial class NetworkFileSend : ClientBase {
        public NetworkFileSend() { this.FilePort = 9999; }
    }
}