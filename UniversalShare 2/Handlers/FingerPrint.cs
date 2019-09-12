using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace UniversalShare_2.Handlers {
    internal class FingerPrint {
        private static readonly SHA512       Sha512 = SHA512.Create();
        private readonly        List<string> _fref  = new List<string>();


        private void GrabHardware() {
            this._fref.RemoveRange( 0, this._fref.Count );
            this._fref.AddRange( GetNetworkMacs() );
            this._fref.Add( Environment.UserName );
            this._fref.Add( Environment.MachineName );
            this._fref.Add( Environment.OSVersion.VersionString );
        }

        private IEnumerable<string> GetNetworkMacs() {
            foreach ( var n in NetworkInterface.GetAllNetworkInterfaces() ) {
                yield return Convert.ToBase64String( n.GetPhysicalAddress().GetAddressBytes() );
            }
        }

        private string _id = "";

        private void MakeId() {
            this._id = "";
            foreach ( var pref in this._fref ) {
                this._id += Encoding.UTF8.GetString( Sha512.ComputeHash( Encoding.UTF8.GetBytes( pref ) ) );
            }
        }

        private const int CHUNK = 4;
        private string ComputeId() {
            var ret = "";
            GrabHardware();
            MakeId();

            var intervall = Sha512.ComputeHash( new[] { (byte) 000, (byte) 111, (byte) 222, (byte) 123 } ).Length / CHUNK;

            for ( var i = 0; i < CHUNK; i++ ) {

                var byt = Sha512.ComputeHash( Encoding.Unicode.GetBytes( this._id.Substring( intervall * i, intervall ) ) ).ToList();
                byt.Add( 111 );
                byt.Add( 111 );

                ret += Convert.ToBase64String( byt.ToArray() );
            }

            return ret;
        }

        public string MakeIdS => ComputeId();
        public string FullId => ComputeFullId();

        private string ComputeFullId() {
            GrabHardware();
            MakeId();
            return Convert.ToBase64String( Encoding.UTF8.GetBytes( this._id ) );
        }
    }
}