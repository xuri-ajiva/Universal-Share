﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace UniversalShare_2.Handlers {
    [Serializable]
    public struct KeyHandler {
        private readonly SHA512 _sha512;

        public static readonly KeyHandler Emthy;

        public const byte LENGTH_B = 64;

        public KeyHandler(byte[] state, string node = "") {
            this._sha512 = SHA512.Create();
            if ( string.IsNullOrEmpty( node ) ) {
                node =  new FingerPrint().MakeIdS + Encoding.UTF8.GetString( state );
                node += new Random().NextDouble().ToString( CultureInfo.CurrentCulture );
            }

            this.KeyBytes   = this._sha512.ComputeHash( Encoding.UTF8.GetBytes( node ) );
            this.TokenBytes = this._sha512.ComputeHash( this.KeyBytes );
        }

        private KeyHandler(byte[] key) {
            this._sha512    = SHA512.Create();
            this.KeyBytes   = key;
            this.TokenBytes = this._sha512.ComputeHash( this.KeyBytes );
        }

        public string XMLKeyBytes   { get => Convert.ToBase64String( this.KeyBytes );   set => this.KeyBytes = Convert.FromBase64String( value ); }
        public string XMLTokenBytes { get => Convert.ToBase64String( this.TokenBytes ); set => this.TokenBytes = Convert.FromBase64String( value ); }

        [XmlIgnore] public byte[] KeyBytes { [DebuggerStepThrough] get; private set; }

        [XmlIgnore] public byte[] TokenBytes { [DebuggerStepThrough] get; private set; }

        public static explicit operator byte[](KeyHandler k) => k.TokenBytes;
        public static explicit operator KeyHandler(byte[] b) => new KeyHandler( b );
    }
}