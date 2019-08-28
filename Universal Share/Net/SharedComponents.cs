#region using

using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Universal_Share.Options;

#endregion

namespace Universal_Share.Net {
    public class SharedComponents : NetworkFileSend {
        public const string ID_NOT_EXIST          = "REGISTER DOSE NOT EXIST!";
        public const string ID_STREAM_NOT_EXIST   = "STREAM DOSE NOT EXIST!";
        public const string ID_REGISTER_SUCCESSES = "REGISTER SUCCESSES!";
        public const string UNKNOWN_ERROR         = "NO INFORMATION";


        public const int    DEFAULT_FILE_PORT     = 4333;
        public const int    DEFAULT_TEXT_PORT     = 9999;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";
        public const int    DEFAULT_BUFFER_SIZE   = 32768;
        public const int    DEFAULT_HEATHER_SIZE  = 8;

        public int    FILE_PORT;
        public int    TEXT_PORT;
        public string SAVE_LOCATION;
        public int    BUFFER_SIZE;
        public int    HEATHER_SIZE;

        public SharedComponents(int filePort = DEFAULT_FILE_PORT, int textPort = DEFAULT_TEXT_PORT, int bufferSize = DEFAULT_BUFFER_SIZE, string saveLocation = DEFAULT_SAVE_LOCATION) {
            this.FILE_PORT     = filePort;
            this.TEXT_PORT     = textPort;
            this.BUFFER_SIZE   = bufferSize;
            this.SAVE_LOCATION = saveLocation;
        }

        public SharedComponents() {
            this.FILE_PORT     = DEFAULT_FILE_PORT;
            this.TEXT_PORT     = DEFAULT_TEXT_PORT;
            this.SAVE_LOCATION = DEFAULT_SAVE_LOCATION;
            this.BUFFER_SIZE   = DEFAULT_BUFFER_SIZE;
            this.HEATHER_SIZE  = DEFAULT_HEATHER_SIZE;
        }

        public Socket CreateSocket() => new Socket( SocketType.Stream, ProtocolType.Tcp );

        /// client ///
        public Tuple<string, int> SendFile(IPAddress remoteHost, string fileName) {
            var id = 0;
            try {
                var ipend_F = new IPEndPoint( remoteHost, this.FILE_PORT );
                var ipend_R = new IPEndPoint( remoteHost, this.TEXT_PORT );

                var register = new TcpClient();
                register.Connect( ipend_R );

                var fileSocket = new TcpClient();
                fileSocket.Connect( ipend_F );

                id = new Random().Next( 100000000, 999999999 );

                var ret1 = SendRegister( register, Path.GetFileName( fileName ), id );

                Thread.Sleep( 100 );

                SendFileArray( fileSocket, fileName, id );

                Console.WriteLine( "Finished!" );
                register.Close();
                fileSocket.Close();
                return ret1;
            } catch (Exception e) {
                return new Tuple<string, int>( e.Message, id );
            }
        }

        private Tuple<string, int> SendFileArray(TcpClient FileSocket, String filename, int id) {
            var    errorCode = SocketError.NotConnected;
            int    readBytes = -1;
            byte[] buffer    = new byte[this.BUFFER_SIZE];

            // id = id == -1 ? new Random().nextInt(99999999) + 10000000 : id;
            byte[] id_B = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            int blockCtr       = 0;
            int totalReadBytes = 0;

            while ( readBytes != 0 ) {
                readBytes = strm.Read( buffer, this.HEATHER_SIZE, this.BUFFER_SIZE - this.HEATHER_SIZE );
                if ( readBytes == -1 ) break;
                blockCtr++;
                totalReadBytes += readBytes;

                Array.Copy( id_B, 0, buffer, 0, id_B.Length );

                FileSocket.Client.Send( buffer, 0, readBytes + this.HEATHER_SIZE, SocketFlags.None, out errorCode );

                int readBytes2 = FileSocket.Client.Receive( buffer, 0, this.BUFFER_SIZE, SocketFlags.None );

                if ( readBytes2 > 0 )
                    switch (Encoding.UTF8.GetString( SubArray( buffer, 0, readBytes2 ) )) {
                        case ID_REGISTER_SUCCESSES: break;
                        case ID_NOT_EXIST:          return new Tuple<string, int>( ID_NOT_EXIST,        -1 );
                        case ID_STREAM_NOT_EXIST:   return new Tuple<string, int>( ID_STREAM_NOT_EXIST, -2 );
                        default:                    return new Tuple<string, int>( UNKNOWN_ERROR,       -3 );
                    }

                Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
            }

            strm.Close();
            FileSocket.Close();
            return new Tuple<string, int>( errorCode.ToString(), id );
        }

        private Tuple<string, int> SendRegister(TcpClient CommunicationSocket, String SaveFileName, int id) {
            byte[] id_B       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] Filaneme_B = Encoding.UTF8.GetBytes( SaveFileName );

            int saveFileLangth = Filaneme_B.Length;
            int bufflength     = this.HEATHER_SIZE + saveFileLangth;

            byte[] buffer = new byte[bufflength];

            Array.Copy( id_B,       0, buffer, 0,                 this.HEATHER_SIZE );
            Array.Copy( Filaneme_B, 0, buffer, this.HEATHER_SIZE, saveFileLangth );

            CommunicationSocket.Client.Send( buffer );

            CommunicationSocket.Close();
            return new Tuple<string, int>( "", id );
        }

        /// server ///
        public Tuple<SocketError, int, int, RegInfo> CreateRegister(TcpClient cl) {
            var buffer    = new byte[this.BUFFER_SIZE];
            var readBytes = cl.Client.Receive( buffer, 0, this.BUFFER_SIZE, SocketFlags.None, out var errorCode );

            if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ), out var id ) ) throw new NotSupportedException( "Can not get ID!" );
            if ( ßMainPoint.S.IdStreamsMap.Contains( id ) ) throw new MissingPrimaryKeyException( "Key Already Exists" );

            var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( SubArray( buffer, this.HEATHER_SIZE, readBytes - this.HEATHER_SIZE ) ) ) ).Split( Path.GetInvalidFileNameChars() ) );
            var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

            Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) );
            Stream strm = new FileStream( finalSaveName, FileMode.OpenOrCreate );

            var regInfo = new RegInfo( strm, id, finalSaveName, RegInfo.TYPE.SINGLE_FILE );
            ßMainPoint.S.IdStreamsMap.Add( id, regInfo );

            Console.WriteLine( "Saving in: " + finalFileName + "  as:" + finalSaveName );
            Console.WriteLine( "Paket: id = "                          + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

            return new Tuple<SocketError, int, int, RegInfo>( errorCode, readBytes, id, regInfo );
        }

        public Tuple<SocketError, int, int, int> HandleClient(TcpClient cl) {
            var errorCode = SocketError.NotConnected;
            var readBytes = -1;

            var buffer = new byte[this.BUFFER_SIZE];

            var blockCtr       = 0;
            var totalReadBytes = 0;
            int id             = -1;
            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, this.BUFFER_SIZE, SocketFlags.None, out errorCode );
                if ( readBytes <= 0 ) break;

                if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ), out id ) ) {
                    cl.Client.Send( Encoding.UTF8.GetBytes( ID_NOT_EXIST ) );
                    Console.WriteLine( "Can not get ID!" );
                    return new Tuple<SocketError, int, int, int>( SocketError.OperationAborted, -1, -1, -1 );
                    //throw new NotSupportedException( "Can not get ID!" );
                }

                if ( !ßMainPoint.S.IdStreamsMap.Contains( id ) ) {
                    cl.Client.Send( Encoding.UTF8.GetBytes( ID_STREAM_NOT_EXIST ) );
                    Console.WriteLine( "MemoryStream Dose Not Exists" );
                    return new Tuple<SocketError, int, int, int>( SocketError.OperationAborted, -1, -1, -1 );
                    //throw new MissingPrimaryKeyException( "MemoryStream Dose Not Exists" );
                }

                ßMainPoint.S.IdStreamsMap.Get(id).Stream.Write( buffer, this.HEATHER_SIZE, readBytes - this.HEATHER_SIZE );

                Console.WriteLine( "Paket: id = " + id + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

                blockCtr++;
                totalReadBytes += readBytes;

                cl.Client.Send( Encoding.UTF8.GetBytes( ID_REGISTER_SUCCESSES ) );
            }

            if ( ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get(id  ) ) ) {
                ßMainPoint.S.IdStreamsMap.Get(id  ).Finished();
                ßMainPoint.S.IdStreamsMap.Remove( id );
            }

            return new Tuple<SocketError, int, int, int>( errorCode, totalReadBytes, blockCtr, id );
        }


        private void TextHandler(TcpListener listener) {
            while ( true ) {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write( "[{0}]", System.Reflection.MethodBase.GetCurrentMethod().Name );
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine( " Waiting for connections ..." );

                var reg = CreateRegister( listener.AcceptTcpClient() );

                Console.WriteLine( reg.Item1 );
                Console.WriteLine( reg.Item2 );
                Console.WriteLine( reg.Item3 );
                Console.WriteLine( reg.Item4.ToString() );
                Console.WriteLine( "Finished!" );
            }
        }

        private void FileHandler(TcpListener listener) {
            while ( true ) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write( "[{0}]", System.Reflection.MethodBase.GetCurrentMethod().Name );
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine( " Waiting for connections ..." );

                var ret = HandleClient( listener.AcceptTcpClient() );

                Console.WriteLine( ret.Item1 );
                Console.WriteLine( ret.Item2 );
                Console.WriteLine( ret.Item3 );
                Console.WriteLine( ret.Item4 );
                Console.WriteLine( "Finished!" );
            }
        }


        public Tuple<string, Thread, TcpListener> StartFileHandler() {
            Thread      fileThread      = default;
            TcpListener tcpFileListener = default;
            try {
                tcpFileListener = new TcpListener( IPAddress.Any, this.FILE_PORT );
                fileThread      = new Thread( () => FileHandler( tcpFileListener ) );

                tcpFileListener.Start();
                fileThread.Start();

                return new Tuple<string, Thread, TcpListener>( "Finished", fileThread, tcpFileListener );
            } catch (Exception e) {
                return new Tuple<string, Thread, TcpListener>( e.Message, fileThread, tcpFileListener );
            }
        }

        public Tuple<string, Thread, TcpListener> StartTextHandler() {
            Thread      textThread = default;
            TcpListener register   = default;
            try {
                register   = new TcpListener( IPAddress.Any, this.TEXT_PORT );
                textThread = new Thread( () => TextHandler( register ) );

                register.Start();
                textThread.Start();

                return new Tuple<string, Thread, TcpListener>( "Finished", textThread, register );
            } catch (Exception e) {
                return new Tuple<string, Thread, TcpListener>( e.Message, textThread, register );
            }
        }

        [DebuggerStepThrough]
        public static T[] SubArray <T>(T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy( data, index, result, 0, length );
            return result;
        }
    }
}