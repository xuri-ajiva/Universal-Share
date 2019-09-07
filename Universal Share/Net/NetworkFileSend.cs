using Universal_Share.ProgMain;

namespace Universal_Share.Net {
    public class NetworkFileSend : ClientBase {
        public NetworkFileSend() {

            this.BUFFER_SIZE = ßMainPoint.ST.BufferSize;
            this.FilePort = ßMainPoint.ST.Port;
        }
    }
}