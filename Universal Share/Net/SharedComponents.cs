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

    public partial class SharedComponents : NetworkFileSend {
       //public int    BufferSize;

        public SharedComponents(int filePort = DEFAULT_FILE_PORT, int textPort = DEFAULT_TEXT_PORT, int bufferSize = DEFAULT_BUFFER_SIZE, string saveLocation = DEFAULT_SAVE_LOCATION) {
        //    this.FilePort     = filePort;
        //    this.TextPort     = textPort;
        //    this.BufferSize   = bufferSize;
        //    this.SaveLocation = saveLocation;
        }

        public SharedComponents() {
           // this.FilePort     = DEFAULT_FILE_PORT;
           // this.TextPort     = DEFAULT_TEXT_PORT;
           // this.SaveLocation = DEFAULT_SAVE_LOCATION;
           // this.BufferSize   = DEFAULT_BUFFER_SIZE;
           // this.HeatherSize  = DEFAULT_HEATHER_SIZE;
        }

    }
    /*
    public partial class SharedComponents {
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
    }*/
}