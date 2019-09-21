using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UniversalShareCore.Operation {
    public struct TokenItem {
        [XmlIgnore] public byte[] TokenBytes { [DebuggerStepThrough] get; private set; }

        public TokenItem(byte[] tokenBytes, bool trusted, bool remember, string description = "") {
            this.TokenBytes  = tokenBytes;
            this.Trusted     = trusted;
            this.Remember    = remember;
            this.Description = description;
        }

        public string Description;

        public string Base64Key { get => Convert.ToBase64String( this.TokenBytes ); set => this.TokenBytes = Convert.FromBase64String( value ); }

        public bool Trusted;
        public bool Remember;
    }
}