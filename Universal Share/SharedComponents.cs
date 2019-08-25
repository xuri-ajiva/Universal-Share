#region using

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;

#endregion

namespace Universal_Share {
    internal class SharedComponents : NetworkFileSend {
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

        /* public SocketError SendFile(TcpClient cl, string filename, int id = -1) {
             var errorCode = SocketError.NotConnected;
             var readBytes = -1;
 
             var buffer = new byte[BUFFER_SIZE];
 
             id = id == -1 ? new Random().Next( 100000000, 999999999 ) : id;
 
             Stream strm = new FileStream( filename, FileMode.Open );
 
             var blockCtr       = 0;
             var totalReadBytes = 0;
             while ( readBytes != 0 ) {
                 readBytes = strm.Read( buffer, 0, BUFFER_SIZE );
                 blockCtr++;
                 totalReadBytes += readBytes;
 
                 Array.Copy( id_B, buffer, id_B.Length );
                 cl.Client.Send( buffer, 0, readBytes, SocketFlags.None, out errorCode );
             }
 
             cl.Close();
             strm.Close();
             return errorCode;
         }
         */
        public Tuple<SocketError, int> SendFile(TcpClient FileSocket, String filename, int id) {
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

                Console.WriteLine( "Paket: id = " + id + "    | " + Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
            }

            strm.Close();
            return new Tuple<SocketError, int>( errorCode, id );
        }

        public int SendRegister(TcpClient CommunicationSocket, String SaveFileName, int id) {
            byte[] id_B       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] Filaneme_B = Encoding.UTF8.GetBytes( SaveFileName );

            int saveFileLangth = Filaneme_B.Length;
            int bufflength     = this.HEATHER_SIZE + saveFileLangth;

            byte[] buffer = new byte[bufflength];

            Array.Copy( id_B,       0, buffer, 0, this.HEATHER_SIZE );
            Array.Copy( Filaneme_B, 0, buffer, this.HEATHER_SIZE, saveFileLangth );

            CommunicationSocket.Client.Send( buffer );

            return id;
        }

        public Tuple<SocketError, int, int, RegInfo> CreateRegister(TcpClient cl) {
            var buffer    = new byte[this.BUFFER_SIZE];
            var readBytes = cl.Client.Receive( buffer, 0, this.BUFFER_SIZE, SocketFlags.None, out var errorCode );

            if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ), out var id ) ) throw new NotSupportedException( "Can not get ID!" );
            if ( this.IdStreamsMap.ContainsKey( id ) ) throw new MissingPrimaryKeyException( "Key Already Exists" );

            var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( SubArray( buffer, this.HEATHER_SIZE, readBytes - this.HEATHER_SIZE ) ) ) ).Split( Path.GetInvalidFileNameChars() ) );
            var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

            Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) );
            Stream strm = new FileStream( finalSaveName, FileMode.OpenOrCreate );

            var regInfo = new RegInfo( strm, id, finalSaveName );
            this.IdStreamsMap.Add( id, regInfo );

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

                if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, this.HEATHER_SIZE ) ), out id ) ) throw new NotSupportedException( "Can not get ID!" );

                if ( !this.IdStreamsMap.ContainsKey( id ) ) throw new MissingPrimaryKeyException( "Can nor get MemoryStream" );
                this.IdStreamsMap[id].Stream.Write( buffer, this.HEATHER_SIZE, readBytes - this.HEATHER_SIZE );

                Console.WriteLine( "Paket: id = " + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

                blockCtr++;
                totalReadBytes += readBytes;
            }

            this.IdStreamsMap[id].Finished();
            this.IdStreamsMap.Remove( id );
            return new Tuple<SocketError, int, int, int>( errorCode, totalReadBytes, blockCtr, id );
        }

        public readonly Dictionary<int, RegInfo> IdStreamsMap = new Dictionary<int, RegInfo>();

        public static T[] SubArray <T>(T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy( data, index, result, 0, length );
            return result;
        }
    }
}