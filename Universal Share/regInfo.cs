using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Universal_Share {
    [Serializable, XmlInclude( typeof(RegInfo) )]
    public partial struct RegInfo : IDisposable {
        public static readonly RegInfo Empty;
        [XmlIgnore] public     Stream  Stream;
        public                 int     ID;
        public                 string  SaveFilePath;

        public TYPE Type;
    }

    public partial struct RegInfo {
        public RegInfo(Stream stream, int id, string saveFilePath, TYPE type) {
            this.Type         = type;
            this.ID           = id;
            this.Stream       = stream;
            this.SaveFilePath = saveFilePath;
        }

        public override string ToString() { return "ID: " + this.ID + "  | Stream: " + this.Stream + "  | FilePath: " + this.SaveFilePath + "  | Type: " + this.Type; }

        public void Finished() {
            this.Stream?.Close();
            this.Dispose();
            this.Stream       = default;
            this.ID           = default;
            this.SaveFilePath = default;
        }

        public void Dispose() { this.Stream?.Dispose(); }
    }

    public partial struct RegInfo {
        public enum TYPE {
            UNKNOWN = -1,
            SINGLE_FILE,
            ARCHIFE,
            TEXT
        }
    }
}