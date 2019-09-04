using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Universal_Share.Options;
using Universal_Share.ProgMain;
using Universal_Share.Security;

namespace Universal_Share.Net {
    public class NetBase {
        [DebuggerStepThrough]
        public static T[] SubArray <T>(T[] data, int index, int length) {
            var result = new T[length];
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
                Option.IsEqual( toaken, ti.TokenBytes );
                if ( ti.Remember ) return ti.Trusted;
            }

            if ( ßMainPoint.U.GetConfirm( new TokenItem( toaken, false, false ) ) ) {
                return true;
            }

            return false;
        }

        public int FilePort;

        protected const int BUFFER_SIZE = DEFAULT_BUFFER_SIZE;

        protected const int ID_SIZE      = DEFAULT_HEATHER_SIZE;
        protected const int KEY_SIZE     = Auth.LENGTH_B;
        protected const int OPTION_SIZE  = 4;
        protected const int HEATHER_SIZE = KEY_SIZE + ID_SIZE + OPTION_SIZE;


        /// <summary>
        /// return new Tuple&lt;byte[], byte[], byte[], byte[]&gt;( tokenArray, idArray, optionArray, contendArray );
        /// </summary>
        /// <param name="buffer">buffer read form network</param>
        /// <param name="readBytes">the bytes read</param>
        /// <returns>return new Tuple&amp;lt;byte[], byte[], byte[], byte[]&amp;gt;( tokenArray, idArray, optionArray, contendArray );</returns>
        [DebuggerStepThrough]
        protected (byte[], byte[], byte[], byte[]) Buffer_To_Parts(byte[] buffer, int readBytes) {
            var tokenArray   = SubArray( buffer, 0,                                KEY_SIZE );
            var idArray      = SubArray( buffer, KEY_SIZE,                         ID_SIZE );
            var optionArray  = SubArray( buffer, KEY_SIZE           + ID_SIZE,     OPTION_SIZE );
            var contendArray = SubArray( buffer, KEY_SIZE + ID_SIZE + OPTION_SIZE, readBytes - ( KEY_SIZE + ID_SIZE + OPTION_SIZE ) );
            return ( tokenArray, idArray, optionArray, contendArray );
        }

        [DebuggerStepThrough]
        protected byte[] Parts_To_Buffer(byte[] tokenBytes, byte[] idBytes, byte[] option, byte[] contendBytes) {
            var buffer = new byte[BUFFER_SIZE];
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

            string message;

            switch (Option.OperationType( option )) {
                case Option.Range.Server:
                    ( message ) = ProcessOptionsServer( token, id, option, conetnd );
                    break;
                case Option.Range.Client:
                    ( message ) = ProcessOptionsClient( token, id, option, conetnd );
                    break;
                case Option.Range.Special:
                    ( message ) = ProcessOptionsSpecial( token, id, option, conetnd );
                    break;
                case Option.Range.None: return ( UNKNOWN_ERROR, id );
                default:                throw new ArgumentOutOfRangeException();
            }

            var ret = message == SUCCESS ? SUCCESS : message;
            Console.WriteLine( Resources.NetBase_PaketAndToaken, id, string.Join( ", ", SubArray( token, 0, 8 ) ) );
            return ( ret, id );
        }

        protected string ProcessOptionsSpecial(byte[] tokenBytes, int id, byte[] option, byte[] contend) {
            if ( Option.IsEqual( option, Option.Error ) ) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( Encoding.UTF8.GetString( contend ).Replace( Encoding.UTF8.GetString( new byte[] { 0 } ), "" ) );
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( Option.IsEqual( option, Option.Success ) ) {
                //Console.WriteLine( Encoding.UTF8.GetString( contend ) );
            }

            return ( SUCCESS );
        }

        protected string ProcessOptionsServer(byte[] tokenBytes, int id, byte[] option, byte[] contend) {
            try {
                if ( Option.IsEqual( option, Option.SaveToFile ) ) {
                    var regi = ßMainPoint.S.IdStreamsMap.Get( id );
                    regi.CreateStream();
                    regi.Stream.Write( contend, 0, contend.Length );
                }
                else if ( Option.IsEqual( option, Option.CreateRegister ) ) {
                    if ( ßMainPoint.S.IdStreamsMap.ContainsKey( id ) ) return ( ID_ALREADY_EXISTS );

                    var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( contend ) ) ).Split( Path.GetInvalidFileNameChars() ) );
                    var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

                    Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) ?? throw new InvalidOperationException() );

                    var regInfo = new RegInfo( null, id, finalSaveName, Convert.ToBase64String( tokenBytes ), RegInfo.Type.SingleFile );
                    ßMainPoint.S.IdStreamsMap.Add( id, regInfo );
                }
            } catch (Exception e) {
                return ( e.Message );
            }

            return ( SUCCESS );
        }

        protected string ProcessOptionsClient(byte[] tokenBytes, int id, byte[] option, byte[] contend) { return ( SUCCESS ); }
    }
}