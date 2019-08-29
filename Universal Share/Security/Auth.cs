using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;

namespace Universal_Share.Security {
  public  class Auth {
        SHA512 _sha512 = SHA512.Create();

        public const byte LENGTH_B = 64;

        public Auth(string node = "") {
            if ( string.IsNullOrEmpty( node ) ) node = new Random().NextDouble().ToString( CultureInfo.CurrentCulture );
            this.KeyBytes  = this._sha512.ComputeHash( Encoding.UTF8.GetBytes( node ) );
            this.TokenBytes = this._sha512.ComputeHash( this.KeyBytes );
        }

        public byte[] KeyBytes { [DebuggerStepThrough] get; }

        public byte[] TokenBytes { [DebuggerStepThrough] get; }
    }
}