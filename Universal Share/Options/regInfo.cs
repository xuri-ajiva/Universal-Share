#region using

using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Serialization;

// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

namespace Universal_Share.Options {
    public class EventStream : INullable, IDisposable {
        [XmlIgnore] private readonly Stream _selfStream;

        public delegate void DictionaryChanged(object sender, TypeE args);

        public event DictionaryChanged OnStreamChanged;

        #region Overrides of Stream

        public void Flush() {
            this._selfStream.Flush();
            Interrupt( TypeE.Flush );
        }

        public long Seek(long offset, SeekOrigin origin) {
            var re = this._selfStream.Seek( offset, origin );
            Interrupt( TypeE.Seek );
            return re;
        }

        public void SetLength(long value) {
            this._selfStream.SetLength( value );
            Interrupt( TypeE.SetLength );
        }

        public int Read(byte[] buffer, int offset, int count) {
            var re = this._selfStream.Read( buffer, offset, count );
            Interrupt( TypeE.Read );
            return re;
        }

        public void Write(byte[] buffer, int offset, int count) {
            this._selfStream.Write( buffer, offset, count );
            Interrupt( TypeE.Write );
        }

        public bool CanRead {
            get {
                Interrupt( TypeE.CanRead );
                return this._selfStream.CanRead;
            }
        }

        public bool CanSeek {
            get {
                Interrupt( TypeE.CanSeek );
                return this._selfStream.CanSeek;
            }
        }

        public bool CanWrite {
            get {
                Interrupt( TypeE.CanWrite );
                return this._selfStream.CanWrite;
            }
        }

        public long Length {
            get {
                Interrupt( TypeE.Length );
                return this._selfStream.Length;
            }
        }

        public long Position {
            get {
                Interrupt( TypeE.PositionGet );
                return this._selfStream.Position;
            }
            set {
                this._selfStream.Position = value;
                Interrupt( TypeE.PositionSet );
            }
        }

        private void Interrupt(TypeE e) { this.OnStreamChanged?.Invoke( this, e ); }

        public enum TypeE {
            Write, Read, CanSeek, Seek, Flush, CanRead, CanWrite, Length, SetLength, PositionGet, PositionSet, Close, Dispose
        }

        #endregion

        public EventStream(Stream selfStream) { this._selfStream = selfStream; }

        public static explicit operator Stream(EventStream es) => es._selfStream;
        public static explicit operator EventStream(Stream s)  => new EventStream( s );

        #region Implementation of INullable

        public bool IsNull => this._selfStream == null;

        public long PositionWithoutEvent { get => this._selfStream.Position; set => this._selfStream.Position = value; }

        public void Close() {
            Interrupt( TypeE.Close );
            this._selfStream.Close();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose() {
            Interrupt( TypeE.Dispose );
            this._selfStream?.Dispose();
        }

        #endregion
    }


    [Serializable, XmlInclude( typeof(RegInfo) )]
    public partial struct RegInfo : IDisposable {
        public static readonly RegInfo     Empty;
        [XmlIgnore] public     EventStream Stream;
        public                 int         Id;
        public                 string      SaveFilePath;
        public                 string      SenderAuth;

        public long Position;

        public Type TypeP;
    }

    public partial struct RegInfo {
        public RegInfo(Stream stream, int id, string saveFilePath, string senderAuth, Type type, int positionInStream = 0) {
            this.TypeP         = type;
            this.Id           = id;
            this.SaveFilePath = saveFilePath;
            this.Position     = positionInStream;
            this.SenderAuth   = senderAuth;

            this.Stream = (EventStream) stream;
            ForceCreateStream();
            this.Stream.OnStreamChanged += StreamOnOnStreamChanged;
        }

        private void StreamOnOnStreamChanged(object sender, EventStream.TypeE args) {
            switch (args) {
                case EventStream.TypeE.Write:
                case EventStream.TypeE.Read:
                case EventStream.TypeE.Flush:
                case EventStream.TypeE.Length:
                case EventStream.TypeE.SetLength:
                case EventStream.TypeE.PositionGet:
                case EventStream.TypeE.PositionSet:
                case EventStream.TypeE.Close:
                case EventStream.TypeE.Dispose: {
                    this.Position = this.Stream.PositionWithoutEvent;
                    break;
                }
            }
        }

        public override string ToString() => "ID: " + this.Id + "  | FilePath: " + this.SaveFilePath + "  | Type: " + this.TypeP;

        public void Finished() {
            this.Stream?.Close();
            Dispose();
            this.Stream       = default;
            this.Id           = default;
            this.SaveFilePath = default;
        }

        public void Dispose() { this.Stream?.Dispose(); }

        public void CreateStream() {
            if ( this.Stream == null ) {
                this.Stream          = (EventStream) File.Open( this.SaveFilePath, FileMode.OpenOrCreate );
                this.Stream.Position = this.Position;
            }
        }

        public void ForceCreateStream() {
            this.Stream?.Dispose();

            this.Stream          = (EventStream) File.Open( this.SaveFilePath, FileMode.OpenOrCreate );
            this.Stream.Position = this.Position;
        }
    }

    public partial struct RegInfo {
        public enum Type {
            Unknown = -1,
            SingleFile,
            Archife,
            Text
        }

    }
}