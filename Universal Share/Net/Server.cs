using System.Net.Sockets;

namespace Universal_Share.Net {
   public class Server : SharedComponents {
        private Socket _socket;


        public Server() { this._socket = CreateSocket(); }


        public void Start() {
            StartFileHandler();
            StartTextHandler();
        }
    }
}