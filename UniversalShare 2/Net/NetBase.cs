using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniversalShare_2.Handlers;
using UniversalShare_2.Operation;

namespace UniversalShare_2.Net {
    public partial class NetBase { //public static bool LOCKED = false;
        protected ExceptionHandler _exceptionHandler;
        protected UiHandler        UiHandler;

        public NetBase(ExceptionHandler _exceptionHandler, UiHandler uiHandler) {
            this.UiHandler        = uiHandler;
            this._exceptionHandler = _exceptionHandler;
            FilePort               = 9999;
        }

        [DebuggerStepThrough]
        protected bool IsKeyVailed(byte[] toaken) {
            var base64Key = Convert.ToBase64String( toaken );

            if ( ßProgram.D.TokenList.ContainsKey( base64Key ) ) {
                var ti = ßProgram.D.TokenList.Get( base64Key );
                NetOption.IsEqual( toaken, ti.TokenBytes );
                if ( ti.Remember ) return ti.Trusted;
            }

            if ( this.UiHandler.GetConfirm( new TokenItem( toaken, false, false ) ) ) {
                return true;
            }

            return false;
        }

        public static int FilePort;

        protected int BUFFER_SIZE = DEFAULT_BUFFER_SIZE;

        protected const int ID_SIZE      = DEFAULT_HEATHER_SIZE;
        protected const int KEY_SIZE     = KeyHandler.LENGTH_B;
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

        //[DebuggerStepThrough]
        protected byte[] Parts_To_Buffer(byte[] tokenBytes, byte[] idBytes, byte[] option, byte[] contendBytes, int ContenLength) {
            var buffer = new byte[ContenLength + HEATHER_SIZE];
            tokenBytes.CopyTo( buffer, 0 );
            idBytes.CopyTo( buffer, tokenBytes.Length );
            option.CopyTo( buffer, tokenBytes.Length + idBytes.Length );

            SubArray( contendBytes, 0, ContenLength ).CopyTo( buffer, tokenBytes.Length + idBytes.Length + option.Length );
            return buffer;
        }

        protected (string, string) GlobalReversesProgresses(byte[] buffer, int readBytes) {
            buffer = SubArray( buffer, 0, readBytes );

            ( var token, var idB, var option, var conetnd ) = Buffer_To_Parts( buffer, readBytes );

            var id = Encoding.UTF8.GetString( idB );

            if ( !IsKeyVailed( token ) ) return ( TOKEN_NOT_ACCEPTED, UNKNOWN_ERROR );

            string message = ProcessOptionsUniversal( token, id, option, conetnd, readBytes );

            var ret = message == SUCCESS ? SUCCESS : message;
            Console.WriteLine( "Paket: id = [{0}], Token(0,8) = [{1}]", id, string.Join( ", ", SubArray( token, 0, 8 ) ) );
            return ( ret, id );
        }

        protected string ProcessOptionsUniversal(byte[] tokenBytes, string id, byte[] option, byte[] contend, int readBytes) {
            try {
                if ( NetOption.IsEqual( option, NetOption.Error ) ) {
                    this._exceptionHandler.EscalateException( new Exception( Encoding.UTF8.GetString( contend ).Replace( Encoding.UTF8.GetString( new byte[] { 0 } ), "" ) ) );
                }
                else if ( NetOption.IsEqual( option, NetOption.WriteInFile ) ) {
                    if ( !ßProgram.D.OperationIdMap.ContainsKey( id ) ) return ( ID_NOT_EXIST );

                    var rehi = ßProgram.D.OperationIdMap.Get( id );
                    rehi.CreateStream();
                    rehi.Stream.Write( contend, 0, readBytes - ( KEY_SIZE + ID_SIZE + OPTION_SIZE ) );
                    rehi.Stream.Flush();
                }
                else if ( NetOption.IsEqual( option, NetOption.CreateRegister ) ) {
                    if ( ßProgram.D.OperationIdMap.ContainsKey( id ) ) return ( ID_ALREADY_EXISTS );

                    var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( contend ) ) ).Split( Path.GetInvalidFileNameChars() ) );
                    var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

                    Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) ?? throw new InvalidOperationException() );

                    var regInfo = new OperationInfo( finalSaveName, id, Convert.ToBase64String( tokenBytes ) );
                    ßProgram.D.OperationIdMap.Add( id, regInfo );
                }
                else if ( NetOption.IsEqual( option, NetOption.CloseRegister ) ) {
                    if ( !ßProgram.D.OperationIdMap.ContainsKey( id ) ) return ( ID_NOT_EXIST );

                    var rehi = ßProgram.D.OperationIdMap.Get( id );
                    rehi.CloseStream();
                    rehi.Finished();
                    rehi.Dispose();
                }
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
                return e.Message;
            }

            /*
            Performance???
             switch (NetOption.OperationType( option )) {
             case NetOption.Range.Server: break;
                case NetOption.Range.Client: break;
                case NetOption.Range.Special: break;
                case NetOption.Range.None: return  UNKNOWN_ERROR;
                default: throw new ArgumentOutOfRangeException();
            }*/
            return SUCCESS;
        }
    }

    public partial class NetBase {
        public static Random Random = new Random( new Random().Next( int.MaxValue ) );

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
        public const int    DEFAULT_BUFFER_SIZE   = 16777216;
        public const int    DEFAULT_HEATHER_SIZE  = 8;


        public static TcpClient CreateClient(IPAddress iPAddress) {
            var ipendF     = new IPEndPoint( iPAddress, FilePort );
            var fileSocket = new TcpClient();
            fileSocket.Connect( ipendF );
            return fileSocket;
        }

        public static TcpListener CreateLisener() => new TcpListener( IPAddress.Any, FilePort );
    }
}