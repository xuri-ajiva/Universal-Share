using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universal_Share {
    public struct regInfo : IDisposable {
        public static regInfo Empty;
        public        Stream  Stream;
        public        int     ID;
        private       string  SaveFilePath;

        public regInfo(Stream stream, int id, string saveFilePath) {
            this.ID           = id;
            this.Stream       = stream;
            this.SaveFilePath = saveFilePath;
        }

        public override string ToString() { return "ID: " + this.ID + "  | Stream: " + this.Stream + "  | FilePath: " + this.SaveFilePath; }

        public void Finished() {
            this.Stream?.Close();
            this.Dispose();
            this.Stream       = default;
            this.ID           = default;
            this.SaveFilePath = default;
        }

        public void Dispose() { this.Stream?.Dispose(); }
    }
}