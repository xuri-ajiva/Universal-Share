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
        public const int    FILE_PORT             = 4333;
        public const int    TEXT_PORT             = 9999;
        public const string DEFAULT_SAVE_LOCATION = ".\\saved\\";

        public const int BUFFER_SIZE  = 32768;
        public const int HEATHER_SIZE = 8;


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
            var errorCode = SocketError.NotConnected;
            int    readBytes = -1;
            byte[] buffer    = new byte[BUFFER_SIZE];

            // id = id == -1 ? new Random().nextInt(99999999) + 10000000 : id;
            byte[] id_B = Encoding.UTF8.GetBytes( id.ToString() );

            Stream strm = new FileStream( filename, FileMode.Open );

            int blockCtr       = 0;
            int totalReadBytes = 0;

            while ( readBytes != 0 ) {
                readBytes = strm.Read( buffer, HEATHER_SIZE, BUFFER_SIZE - HEATHER_SIZE );
                if ( readBytes == -1 ) break;
                blockCtr++;
                totalReadBytes += readBytes;

                Array.Copy( id_B, 0, buffer, 0, id_B.Length );

                FileSocket.Client.Send( buffer, 0, readBytes + HEATHER_SIZE,SocketFlags.None ,out errorCode);

                Console.WriteLine(  "Paket: id = " + id + "    | " + Encoding.UTF8.GetString(SubArray( buffer,0,HEATHER_SIZE )) + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );
            }

            strm.Close();
            return new Tuple<SocketError, int>( errorCode, id );
        }

       public  int SendRegister(TcpClient CommunicationSocket, String SaveFileName, int id) {
            byte[] id_B       = Encoding.UTF8.GetBytes( id.ToString() );
            byte[] Filaneme_B = Encoding.UTF8.GetBytes( SaveFileName );

            int saveFileLangth = Filaneme_B.Length;
            int bufflength     = HEATHER_SIZE + saveFileLangth;

            byte[] buffer = new byte[bufflength];

            Array.Copy( id_B,       0, buffer, 0,            HEATHER_SIZE );
            Array.Copy( Filaneme_B, 0, buffer, HEATHER_SIZE, saveFileLangth );

            CommunicationSocket.Client.Send( buffer );

            return id ;
        }

        public Tuple<SocketError, int, int, regInfo> CreateRegister(TcpClient cl) {
            var buffer    = new byte[BUFFER_SIZE];
            var readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None, out var errorCode );

            if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, HEATHER_SIZE ) ), out var id ) ) throw new NotSupportedException( "Can not get ID!" );
            if ( this.IdStreamsMap.ContainsKey( id ) ) throw new MissingPrimaryKeyException( "Key Already Exists" );

            var finalFileName = string.Concat( ( DateTime.Now + ( Encoding.UTF8.GetString( SubArray( buffer, HEATHER_SIZE, readBytes - HEATHER_SIZE ) ) ) ).Split( Path.GetInvalidFileNameChars() ) );
            var finalSaveName = DEFAULT_SAVE_LOCATION + finalFileName;

            Directory.CreateDirectory( Path.GetDirectoryName( finalSaveName ) );
            Stream strm = new FileStream( finalSaveName, FileMode.OpenOrCreate );

            var reginfo = new regInfo( strm, id, finalSaveName );
            this.IdStreamsMap.Add( id, reginfo );

            Console.WriteLine( "Saving in: " + finalFileName + "  as:" + finalSaveName );
            Console.WriteLine( "Paket: id = "                          + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

            return new Tuple<SocketError, int, int, regInfo>( errorCode, readBytes, id, reginfo );
        }

        public Tuple<SocketError, int, int, int> HandleClient(TcpClient cl) {
            var errorCode = SocketError.NotConnected;
            var readBytes = -1;

            var buffer = new byte[BUFFER_SIZE];

            var blockCtr       = 0;
            var totalReadBytes = 0;
            int id             = -1;
            while ( readBytes != 0 ) {
                readBytes = cl.Client.Receive( buffer, 0, BUFFER_SIZE, SocketFlags.None, out errorCode );
                if ( readBytes <= 0 ) break;

                if ( !int.TryParse( Encoding.UTF8.GetString( SubArray( buffer, 0, HEATHER_SIZE ) ), out id ) ) throw new NotSupportedException( "Can not get ID!" );

                if ( !this.IdStreamsMap.ContainsKey( id ) ) throw new MissingPrimaryKeyException( "Can nor get MemoryStream" );
                this.IdStreamsMap[id].Stream.Write( buffer, HEATHER_SIZE, readBytes - HEATHER_SIZE );

                Console.WriteLine( "Paket: id = " + "  :  [{0}]", string.Join( ", ", SubArray( buffer, 0, 8 ) ) );

                blockCtr++;
                totalReadBytes += readBytes;
            }

            this.IdStreamsMap[id].Finished();
            this.IdStreamsMap.Remove( id );
            return new Tuple<SocketError, int, int, int>( errorCode, totalReadBytes, blockCtr, id );
        }

        public Dictionary<int, regInfo> IdStreamsMap = new Dictionary<int, regInfo>();

        public static T[] SubArray <T>(T[] data, int index, int length) {
            T[] result = new T[length];
            Array.Copy( data, index, result, 0, length );
            return result;
        }
    }
}