using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Universal_Share.Interface;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Net {
    public class NetBase {
        [DebuggerStepThrough]
        public static T[] SubArray <T>(T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy( data, index, result, 0, length );
            return result;
        }

        public       Socket CreateSocket() => new Socket( SocketType.Stream, ProtocolType.Tcp );
        public const string ID_NOT_EXIST          = "REGISTER DOSE NOT EXIST!";
        public const string ID_STREAM_NOT_EXIST   = "STREAM DOSE NOT EXIST!";
        public const string ID_REGISTER_SUCCESSES = "REGISTER SUCCESSES!";
        public const string UNKNOWN_ERROR         = "NO INFORMATION";
        public const string KEY_NOT_VALID         = "KEY_NOT_VALID";
        public const string SUCCESS               = "SUCCESS";
        public const string ID_ALREADY_EXISTS     = "ID_ALREADY_EXISTS";

        public const int    DEFAULT_FILE_PORT     = 4333;
        public const int    DEFAULT_TEXT_PORT     = 9999;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";
        public const int    DEFAULT_BUFFER_SIZE   = 32768;
        public const int    DEFAULT_HEATHER_SIZE  = 8;


        protected bool IsKeyVailed(byte[] toaken) {
            var base64Key = Convert.ToBase64String( toaken );

            if ( ßMainPoint.S.ToakenList.ContainsKey( base64Key ) ) {
                var ti = ßMainPoint.S.ToakenList.Get( base64Key );
                options.isEqual( toaken, ti.TokenBytes );
                if ( ti.remember ) return ti.Trusted;
            }

            if ( ßMainPoint.U.GetConfirm( new TokenItem( toaken, false, false ) ) ) {
                return true;
            }

            return false;
        }


        ///New
        ///
        public int FilePort;

        protected const int buffer_size = DEFAULT_BUFFER_SIZE;

        protected const int Id_size      = DEFAULT_HEATHER_SIZE;
        protected const int key_size     = Auth.LENGTH_B;
        protected const int option_size  = 4;
        protected const int heather_size = key_size + Id_size + option_size;

        [DebuggerStepThrough]
        /// <summary>
        /// return new Tuple&lt;byte[], byte[], byte[], byte[]&gt;( tokenArray, idArray, optionArray, contendArray );
        /// </summary>
        /// <param name="buffer">buffer read form network</param>
        /// <param name="readBytes">the bytes read</param>
        /// <returns>return new Tuple&amp;lt;byte[], byte[], byte[], byte[]&amp;gt;( tokenArray, idArray, optionArray, contendArray );</returns>
        protected (byte[], byte[], byte[], byte[]) Buffer_To_Parts(byte[] buffer, int readBytes) {
            byte[] tokenArray   = SubArray( buffer, 0,                                key_size );
            byte[] idArray      = SubArray( buffer, key_size,                         Id_size );
            byte[] optionArray  = SubArray( buffer, key_size           + Id_size,     option_size );
            byte[] contendArray = SubArray( buffer, key_size + Id_size + option_size, readBytes - ( key_size + Id_size + option_size ) );
            return ( tokenArray, idArray, optionArray, contendArray );
        }

        [DebuggerStepThrough]
        protected byte[] Parts_To_Buffer(byte[] tokenBytes, byte[] idBytes, byte[] option, byte[] contendBytes) {
            var buffer = new byte[buffer_size];
            tokenBytes.CopyTo( buffer, 0 );
            idBytes.CopyTo( buffer, tokenBytes.Length );
            option.CopyTo( buffer, tokenBytes.Length                        + idBytes.Length );
            contendBytes.CopyTo( buffer, tokenBytes.Length + idBytes.Length + option.Length );
            return buffer;
        }
    }
}