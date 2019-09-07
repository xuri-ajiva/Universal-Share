using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Serialization;
using Universal_Share.Net;

namespace Universal_Share.Security {
    [Serializable]
    public class Auth {
        private readonly SHA512 _sha512 = SHA512.Create();

        public static readonly Auth Emthy;

        public const byte LENGTH_B = 64;

        public Auth() {
            var node = new BuildId().MakeIdS + new Random().NextDouble().ToString( CultureInfo.CurrentCulture );
            this.KeyBytes   = this._sha512.ComputeHash( Encoding.UTF8.GetBytes( node ) );
            this.TokenBytes = this._sha512.ComputeHash( this.KeyBytes );
        }

        public Auth(byte[] state, string node = "") {
            if ( string.IsNullOrEmpty( node ) ) {
                node =  new BuildId().MakeIdS + Encoding.UTF8.GetString( state );
                node += new Random().NextDouble().ToString( CultureInfo.CurrentCulture );
            }

            this.KeyBytes   = this._sha512.ComputeHash( Encoding.UTF8.GetBytes( node ) );
            this.TokenBytes = this._sha512.ComputeHash( this.KeyBytes );
        }

        public string XMLKeyBytes   { get => Convert.ToBase64String( this.KeyBytes );   set => this.KeyBytes = Convert.FromBase64String( value ); }
        public string XMLTokenBytes { get => Convert.ToBase64String( this.TokenBytes ); set => this.TokenBytes = Convert.FromBase64String( value ); }

        [XmlIgnore] public byte[] KeyBytes { [DebuggerStepThrough] get; private set; }

        [XmlIgnore] public byte[] TokenBytes { [DebuggerStepThrough] get; private set; }
    }

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