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
using Universal_Share.ProgMain;
using Universal_Share.Security;
using static Universal_Share.Net.options;

// ReSharper disable UnusedMethodReturnValue.Global
// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable FunctionNeverReturns

#endregion

namespace Universal_Share.Net {
    public class options {
        public static readonly byte[] ERROR           = new byte[] { 255, 255, 255, 255 };
        public static readonly byte[] SUCCESS         = new byte[] { 1, 1, 1, 1 };
        public static readonly byte[] SAVE_TO_FILE    = new byte[] { 3, 2, 1, 1 };
        public static readonly byte[] CREATE_REGISTER = new byte[] { 3, 4, 1, 1 };

        [DebuggerStepThrough]
        public static bool isEqual(byte[] x1, byte[] x2) {
            if ( x1.Length != x2.Length ) return false;
            for ( int i = 0; i < x1.Length; i++ ) {
                if ( x1[i] != x2[i] ) return false;
            }

            return true;
        }
    }

    public class SharedComponents : NetworkFileSend {
        public const  string ID_NOT_EXIST          = "REGISTER DOSE NOT EXIST!";
        public const  string ID_STREAM_NOT_EXIST   = "STREAM DOSE NOT EXIST!";
        public const  string ID_REGISTER_SUCCESSES = "REGISTER SUCCESSES!";
        public const  string UNKNOWN_ERROR         = "NO INFORMATION";
        public const  string KEY_NOT_VALID         = "KEY_NOT_VALID";
        private const string SUCCESS               = "SUCCESS";
        private const string ID_ALREADY_EXISTS     = "ID_ALREADY_EXISTS";


        public const int    DEFAULT_FILE_PORT     = 4333;
        public const int    DEFAULT_TEXT_PORT     = 9999;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";
        public const int    DEFAULT_BUFFER_SIZE   = 32768;
        public const int    DEFAULT_HEATHER_SIZE  = 8;

        public int    FilePort;
        public int    TextPort;
        public string SaveLocation;
        public int    BufferSize;
        public int    HeatherSize;

        public SharedComponents(int filePort = DEFAULT_FILE_PORT, int textPort = DEFAULT_TEXT_PORT, int bufferSize = DEFAULT_BUFFER_SIZE, string saveLocation = DEFAULT_SAVE_LOCATION) {
            this.FilePort     = filePort;
            this.TextPort     = textPort;
            this.BufferSize   = bufferSize;
            this.SaveLocation = saveLocation;
        }

        public SharedComponents() {
            this.FilePort     = DEFAULT_FILE_PORT;
            this.TextPort     = DEFAULT_TEXT_PORT;
            this.SaveLocation = DEFAULT_SAVE_LOCATION;
            this.BufferSize   = DEFAULT_BUFFER_SIZE;
            this.HeatherSize  = DEFAULT_HEATHER_SIZE;
        }

        public Socket CreateSocket() => new Socket( SocketType.Stream, ProtocolType.Tcp );

        /// client ///
        public Tuple<string, int> SendFile(IPAddress remoteHost, string fileName) {
            var id = 0;
            try {
                var ipendF = new IPEndPoint( remoteHost, this.FilePort );
                var ipendR = new IPEndPoint( remoteHost, this.TextPort );

                var register = new TcpClient();
                register.Connect( ipendR );

                var fileSocket = new TcpClient();
                fileSocket.Connect( ipendF );

                id = new Random().Next( 10000000, 19999999 );

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

        public Tuple<string, int> SendFile2(IPAddress remoteHost, string fileName) {
            var id = 0;
            try {
                var ipendF = new IPEndPoint( remoteHost, this.FilePort );

                var fileSocket = new TcpClient();
                fileSocket.Connect( ipendF );

                id = new Random().Next( 10000000, 19999999 );

                var ret1 = SendRegisterStream( fileSocket, Path.GetFileName( fileName ), id );

                Thread.Sleep( 100 );

                SteamClient( fileSocket, fileName, id );

                Console.WriteLine( "Finished!" );
                fileSocket.Close();
                return ret1;
            } catch (Exception e) {
                return new Tuple<string, int>( e.Message, id );
            }
        }


        private Tuple<string, int> SendFileArray(TcpClient fileSocket, String filename, int id) {
            var    errorCode = SocketError.NotConnected;
            int    readBytes = -1;
            byte[] buffer    = new byte[this.BufferSize];

            // id = id == -1 ? new Random().nextInt(99999999) + 10000000 : id;
            byte[] idB = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            while ( readBytes != 0 ) {
                readBytes = strm.Read( buffer, this.HeatherSize, this.BufferSize - this.HeatherSize );
                if ( readBytes == -1 ) break;

                Array.Copy( idB, 0, buffer, 0, idB.Length );

                fileSocket.Client.Send( buffer, 0, readBytes + this.HeatherSize, SocketFlags.None, out errorCode );

                int readBytes2 = fileSocket.Client.Receive( buffer, 0, this.BufferSize, SocketFlags.None );

                if ( readBytes2 > 0 )
                    switch (Encoding.UTF8.GetString( SubArray( buffer, 0, readBytes2 ) )) {
                        case ID_REGISTER_SUCCESSES: break;
                        case ID_NOT_EXIST:          return new Tuple<string, int>( ID_NOT_EXIST,        -1 );
                        case ID_STREAM_NOT_EXIST:   return new Tuple<string, int>( ID_STREAM_NOT_EXIST, -2 );
                        default:                    return new Tuple<string, int>( UNKNOWN_ERROR,       -3 );
                    }

                Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
            }

            strm.Close();
            fileSocket.Close();
            return new Tuple<string, int>( errorCode.ToString(), id );
        }

        private Tuple<string, int> SendRegister(TcpClient communicationSocket, String saveFileName, int id) {
            byte[] idB       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] filanemeB = Encoding.UTF8.GetBytes( saveFileName );

            int saveFileLangth = filanemeB.Length;
            int bufflength     = this.HeatherSize + saveFileLangth;

            byte[] buffer = new byte[bufflength];

            Array.Copy( idB,       0, buffer, 0,                this.HeatherSize );
            Array.Copy( filanemeB, 0, buffer, this.HeatherSize, saveFileLangth );

            communicationSocket.Client.Send( buffer );

            communicationSocket.Close();
            return new Tuple<string, int>( "", id );
        }

        /// server ///
        public Tuple<SocketError, int, int, RegInfo> CreateRegister(TcpClient cl) {
            var buffer    = new byte[this.BufferSize];
            var readBytes = cl.Client.Receive( buffer, 0, this.BufferSize, SocketFlags.None, out var errorCode );

            if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ), out var id ) ) throw new NotSupportedException( "Can not get ID!" );
            if ( ßMainPoint.S.IdStreamsMap.Contains( id ) ) throw new MissingPrimaryKeyException( "Key Already Exists" );

            var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( SubArray( buffer, this.HeatherSize, readBytes - this.HeatherSize ) ) ) ).Split( Path.GetInvalidFileNameChars() ) );
            var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

            Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) ?? throw new InvalidOperationException() );
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

            var buffer = new byte[this.BufferSize];

            var blockCtr       = 0;
            var totalReadBytes = 0;
            int id             = -1;
            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, this.BufferSize, SocketFlags.None, out errorCode );
                if ( readBytes <= 0 ) break;

                if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ), out id ) ) {
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

                ßMainPoint.S.IdStreamsMap.Get( id ).Stream.Write( buffer, this.HeatherSize, readBytes - this.HeatherSize );

                Console.WriteLine( "Paket: id = " + id + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

                blockCtr++;
                totalReadBytes += readBytes;

                cl.Client.Send( Encoding.UTF8.GetBytes( ID_REGISTER_SUCCESSES ) );
            }

            if ( ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( id ) ) ) {
                ßMainPoint.S.IdStreamsMap.Get( id ).Finished();
                ßMainPoint.S.IdStreamsMap.Remove( id );
            }

            return new Tuple<SocketError, int, int, int>( errorCode, totalReadBytes, blockCtr, id );
        }


        /// <summary>
        /// ///////Nesa
        /// </summary>
        const int buffer_size = DEFAULT_BUFFER_SIZE;

        const         int Id_size      = DEFAULT_HEATHER_SIZE;
        const         int key_size     = Auth.LENGTH_B;
        const         int option_size  = 4;
        private const int heather_size = key_size + Id_size + option_size;

        [DebuggerStepThrough]
        /// <summary>
        /// return new Tuple&lt;byte[], byte[], byte[], byte[]&gt;( tokenArray, idArray, optionArray, contendArray );
        /// </summary>
        /// <param name="buffer">buffer read form network</param>
        /// <param name="readBytes">the bytes read</param>
        /// <returns>return new Tuple&amp;lt;byte[], byte[], byte[], byte[]&amp;gt;( tokenArray, idArray, optionArray, contendArray );</returns>
        Tuple<byte[], byte[], byte[], byte[]> Buffer_To_Parts(byte[] buffer, int readBytes) {
            byte[] tokenArray   = SubArray( buffer, 0,                                key_size );
            byte[] idArray      = SubArray( buffer, key_size,                         Id_size );
            byte[] optionArray  = SubArray( buffer, key_size           + Id_size,     option_size );
            byte[] contendArray = SubArray( buffer, key_size + Id_size + option_size, readBytes - ( key_size + Id_size + option_size ) );
            return new Tuple<byte[], byte[], byte[], byte[]>( tokenArray, idArray, optionArray, contendArray );
        }

        [DebuggerStepThrough]
        byte[] Parts_To_Buffer(byte[] tokenBytes, byte[] idBytes, byte[] option, byte[] contendBytes) {
            var buffer = new byte[buffer_size];
            tokenBytes.CopyTo( buffer, 0 );
            idBytes.CopyTo( buffer, tokenBytes.Length );
            option.CopyTo( buffer, tokenBytes.Length                        + idBytes.Length );
            contendBytes.CopyTo( buffer, tokenBytes.Length + idBytes.Length + option.Length );
            return buffer;
        }

        public Tuple<string, int> BufferBandServer(int readBytes, byte[] buffer) {
            var sorted = Buffer_To_Parts( buffer, readBytes );

            if ( !IsKeyVailed( sorted.Item1 ) ) return new Tuple<string, int>( KEY_NOT_VALID, -1 );

            if ( !int.TryParse( Encoding.UTF8.GetString( sorted.Item2 ), out var id ) ) return new Tuple<string, int>( ID_NOT_EXIST, id );

            processOptionsServer( id, sorted.Item3, sorted.Item4 );

            Console.WriteLine( "Paket: id = [{0}] , Token = [{1}]", string.Join( ", ", sorted.Item2 ), string.Join( ", ", sorted.Item1 ) );
            return new Tuple<string, int>( SUCCESS, id );
        }

        public Tuple<string, int> BufferBandClinet(int readBytes, byte[] buffer) {
            var sorted = Buffer_To_Parts( buffer, readBytes );

            if ( !IsKeyVailed( sorted.Item1 ) ) return new Tuple<string, int>( KEY_NOT_VALID, -1 );

            if ( !int.TryParse( Encoding.UTF8.GetString( sorted.Item2 ), out var id ) ) return new Tuple<string, int>( ID_NOT_EXIST, id );

            processOptionsClient( id, sorted.Item3, sorted.Item4 );

            Console.WriteLine( "Paket: id = [{0}] , Token = [{1}]", string.Join( ", ", sorted.Item2 ), string.Join( ", ", sorted.Item1 ) );
            return new Tuple<string, int>( SUCCESS, id );
        }

        private Tuple<string, int> processOptionsClient(int id, byte[] option, byte[] contend) {
            if ( isEqual( option, ERROR ) ) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( Encoding.UTF8.GetString( contend ) );
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if ( isEqual( option, options.SUCCESS ) ) {
                //Console.WriteLine( Encoding.UTF8.GetString( contend ) );
            }

            return new Tuple<string, int>( SUCCESS, id );
        }

        private Tuple<string, int> processOptionsServer(int id, byte[] option, byte[] contend) {
            if ( isEqual( option, SAVE_TO_FILE ) ) {
                var regi = ßMainPoint.S.IdStreamsMap.Get( id );
                regi.CreateStream();
                regi.Stream.Write( contend, 0, contend.Length );
            }
            else if ( isEqual( option, CREATE_REGISTER ) ) {
                if ( ßMainPoint.S.IdStreamsMap.Contains( id ) ) return new Tuple<string, int>( ID_ALREADY_EXISTS, id );

                var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( contend ) ) ).Split( Path.GetInvalidFileNameChars() ) );
                var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

                Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) );

                var regInfo = new RegInfo( null, id, finalSaveName, RegInfo.TYPE.SINGLE_FILE );
                ßMainPoint.S.IdStreamsMap.Add( id, regInfo );
            }

            return new Tuple<string, int>( SUCCESS, id );
        }

        private bool IsKeyVailed(byte[] toaken) { return true; }


        public void SteamServer(TcpClient cl) {
            var errorCode = SocketError.NotConnected;

            var readBytes = -1;
            var buffer    = new byte[buffer_size];

            var blockCtr       = 0;
            var totalReadBytes = 0;
            int id             = -1;

            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, this.BufferSize, SocketFlags.None, out errorCode );
                if ( readBytes <= 0 ) break;
                var ret = this.BufferBandServer( readBytes, buffer );

                cl.Client.Send( Parts_To_Buffer( ßMainPoint.T, Encoding.UTF8.GetBytes( ret.Item2.ToString() ), options.SUCCESS, Encoding.UTF8.GetBytes( ret.Item1 ) ) );

                blockCtr++;
                totalReadBytes += readBytes;
            }

            // if ( ßMainPoint.S.execute( ßMainPoint.S.IdStreamsMap.Get( id ) ) ) {
            //     ßMainPoint.S.IdStreamsMap.Get( id ).Finished();
            //     ßMainPoint.S.IdStreamsMap.Remove( id );
            // }
        }

        private Tuple<string, int> SendRegisterStream(TcpClient communicationSocket, String saveFileName, int id) {
            byte[] idB       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] filanemeB = Encoding.UTF8.GetBytes( saveFileName );

            var b = Parts_To_Buffer( ßMainPoint.T, idB, CREATE_REGISTER, filanemeB );

            var buffer = new byte[buffer_size];
            communicationSocket.Client.Send( b );
            var readet = communicationSocket.Client.Receive( buffer, SocketFlags.None );

            var t = Buffer_To_Parts( buffer, readet );

            return new Tuple<string, int>( Encoding.UTF8.GetString( t.Item4 ), id );
        }

        public void SteamClient(TcpClient cl, string filename, int id) {
            int    readerBytes = -1;
            byte[] buffer      = new byte[buffer_size - heather_size];

            byte[] idB = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            var blockCtr       = 0;
            var totalReadBytes = 0;

            while ( readerBytes != 0 ) {
                buffer = new byte[buffer_size - heather_size];
                readerBytes = strm.Read( buffer, 0, buffer_size - heather_size );
                if ( readerBytes == -1 ) break;

                var ret = Parts_To_Buffer( ßMainPoint.T, idB, SAVE_TO_FILE, buffer );

                cl.Client.Send( ret, SocketFlags.None );

                buffer = new byte[buffer_size];

                int readBytes2 = cl.Client.Receive( buffer, 0, this.BufferSize, SocketFlags.None );

                //var sp = Buffer_To_Parts( buffer, readBytes2 );

                BufferBandClinet( readBytes2, buffer );

                //Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HeatherSize ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
                blockCtr++;
                totalReadBytes += readerBytes;
            }

            strm.Close();
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
                tcpFileListener = new TcpListener( IPAddress.Any, this.FilePort );
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
                register   = new TcpListener( IPAddress.Any, this.TextPort );
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