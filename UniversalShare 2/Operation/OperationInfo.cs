using System.IO;
using System.Xml.Serialization;

namespace UniversalShare_2.Operation {
    public struct OperationInfo {
        public string _id;
        public string _fileName;
        public string _senderAuth;


        [XmlIgnore] public Stream Stream;

        public OperationInfo(string fileName, string id, string senderAuth) {
            this._fileName   = fileName;
            this._id         = id;
            this._senderAuth = senderAuth;
            this.Stream      = File.Open( fileName, FileMode.Append );
        }

        public void CloseStream() {
            if ( this.Stream == null ) return;
            this.Stream?.Flush();
            this.Stream?.Close();
            this.Stream?.Dispose();
        }

        public void Finished() {
            this.Stream?.Close();
            Dispose();
            this.Stream    = default;
            this._id       = default;
            this._fileName = default;
        }

        public void Dispose() { this.Stream?.Dispose(); }

        public void CreateStream() {
            if ( this.Stream == null ) {
                this.Stream = File.Open( this._fileName, FileMode.Append );
            }
        }

        public void ForceCreateNewStream() {
            this.Stream?.Dispose();
            this.Stream = File.Open( this._fileName, FileMode.Append );
        }
    }
}