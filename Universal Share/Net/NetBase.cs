using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Universal_Share.Interface;
using Universal_Share.Options;
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
        public const string TOKEN_NOT_ACCEPTED    = "TOKEN_NOT_ACCEPTED";

        public const int    DEFAULT_FILE_PORT     = 4333;
        public const int    DEFAULT_TEXT_PORT     = 9999;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";
        public const int    DEFAULT_BUFFER_SIZE   = 32768;
        public const int    DEFAULT_HEATHER_SIZE  = 8;


        protected bool IsKeyVailed(byte[] toaken) {
            var base64Key = Convert.ToBase64String( toaken );

            if ( ßMainPoint.S.ToakenList.ContainsKey( base64Key ) ) {
                var ti = ßMainPoint.S.ToakenList.Get( base64Key );
                Option.isEqual( toaken, ti.TokenBytes );
                if ( ti.remember ) return ti.Trusted;
            }

            if ( ßMainPoint.U.GetConfirm( new TokenItem( toaken, false, false ) ) ) {
                return true;
            }

            return false;
        }

        public int FilePort;

        protected const int buffer_size = DEFAULT_BUFFER_SIZE;

        protected const int Id_size      = DEFAULT_HEATHER_SIZE;
        protected const int key_size     = Auth.LENGTH_B;
        protected const int option_size  = 4;
        protected const int heather_size = key_size + Id_size + option_size;


        /// <summary>
        /// return new Tuple&lt;byte[], byte[], byte[], byte[]&gt;( tokenArray, idArray, optionArray, contendArray );
        /// </summary>
        /// <param name="buffer">buffer read form network</param>
        /// <param name="readBytes">the bytes read</param>
        /// <returns>return new Tuple&amp;lt;byte[], byte[], byte[], byte[]&amp;gt;( tokenArray, idArray, optionArray, contendArray );</returns>
        [DebuggerStepThrough]
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

        protected (string, int) GlobalReversesProgresses(int readBytes, byte[] buffer) {
            ( var token, var idB, var option, var conetnd ) = Buffer_To_Parts( buffer, readBytes );

            if ( !IsKeyVailed( token ) ) return ( TOKEN_NOT_ACCEPTED, -1 );
            if ( !int.TryParse( Encoding.UTF8.GetString( idB ), out var id ) ) return ( ID_NOT_EXIST, -1 );

            string message = "";
            var    idBX    = 0;

            switch (Option.OperationType( option )) {
                case Option.Range.Server:
                    ( message, idBX ) = processOptionsServer( token, id, option, conetnd );
                    break;
                case Option.Range.Client:
                    ( message, idBX ) = processOptionsClient( token, id, option, conetnd );
                    break;
                case Option.Range.Special:
                    ( message, idBX ) = processOptionsSpecial( token, id, option, conetnd );
                    break;
                case Option.Range.None: return ( UNKNOWN_ERROR, id );
                default:                throw new ArgumentOutOfRangeException();
            }

            var ret = message == SUCCESS ? SUCCESS : message;
            Console.WriteLine( "Paket: id = [{0}] , Token(0,8) = [{1}]", id, string.Join( ", ", SubArray( token, 0, 8 ) ) );
            return ( ret, id );
        }

        protected (string, int) processOptionsSpecial(byte[] tokenBytes, int id, byte[] option, byte[] contend) {
            if ( Net.Option.isEqual( option, Net.Option.ERROR ) ) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( Encoding.UTF8.GetString( contend ).Replace( Encoding.UTF8.GetString( new byte[] { 0 } ), "" ) );
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( Net.Option.isEqual( option, Net.Option.SUCCESS ) ) {
                //Console.WriteLine( Encoding.UTF8.GetString( contend ) );
            }

            return ( SUCCESS, id );
        }

        protected (string, int) processOptionsServer(byte[] tokenBytes, int id, byte[] option, byte[] contend) {
            try {
                if ( Net.Option.isEqual( option, Net.Option.SAVE_TO_FILE ) ) {
                    var regi = ßMainPoint.S.IdStreamsMap.Get( id );
                    regi.CreateStream();
                    regi.Stream.Write( contend, 0, contend.Length );
                }
                else if ( Net.Option.isEqual( option, Net.Option.CREATE_REGISTER ) ) {
                    if ( ßMainPoint.S.IdStreamsMap.Contains( id ) ) return ( ID_ALREADY_EXISTS, id );

                    var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( contend ) ) ).Split( Path.GetInvalidFileNameChars() ) );
                    var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

                    Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) );

                    var regInfo = new RegInfo( null, id, finalSaveName, Convert.ToBase64String( tokenBytes ), RegInfo.TYPE.SINGLE_FILE );
                    ßMainPoint.S.IdStreamsMap.Add( id, regInfo );
                }
            } catch (Exception e) {
                return ( e.Message, id );
            }

            return ( SUCCESS, id );
        }

        protected (string, int) processOptionsClient(byte[] tokenBytes, int id, byte[] option, byte[] contend) { return ( SUCCESS, id ); }
    }
}