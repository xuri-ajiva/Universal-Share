#region using

using System;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable UnassignedReadonlyField
// ReSharper disable UnusedMember.Global

#endregion

namespace Universal_Share.Options {
    public class EventStream : INullable, IDisposable {
        [XmlIgnore] private Stream SelfStream;

        public delegate void DictionaryChanged(object sender, TypeE args);

        public event DictionaryChanged OnStreamChanged;

        #region Overrides of Stream

        public void Flush() {
            this.SelfStream.Flush();
            Interrupt( TypeE.Flush );
        }


        public long Seek(long offset, SeekOrigin origin) {
            var re = this.SelfStream.Seek( offset, origin );
            Interrupt( TypeE.Seek );
            return re;
        }


        public void SetLength(long value) {
            this.SelfStream.SetLength( value );
            Interrupt( TypeE.SetLength );
        }


        public int Read(byte[] buffer, int offset, int count) {
            var re = this.SelfStream.Read( buffer, offset, count );
            Interrupt( TypeE.Read );
            return re;
        }


        public void Write(byte[] buffer, int offset, int count) {
            this.SelfStream.Write( buffer, offset, count );
            Interrupt( TypeE.Write );
        }


        public bool CanRead {
            get {
                Interrupt( TypeE.CanRead );
                return this.SelfStream.CanRead;
            }
        }


        public bool CanSeek {
            get {
                Interrupt( TypeE.CanSeek );
                return this.SelfStream.CanSeek;
            }
        }


        public bool CanWrite {
            get {
                Interrupt( TypeE.CanWrite );
                return this.SelfStream.CanWrite;
            }
        }


        public long Length {
            get {
                Interrupt( TypeE.Length );
                return this.SelfStream.Length;
            }
        }


        public long Position {
            get {
                Interrupt( TypeE.PositionGet );
                return this.SelfStream.Position;
            }
            set {
                this.SelfStream.Position = value;
                Interrupt( TypeE.PositionSet );
            }
        }

        void Interrupt(TypeE e) { this.OnStreamChanged?.Invoke( this, e ); }

        public enum TypeE {
            Write, Read, CanSeek, Seek, Flush, CanRead, CanWrite, Length, SetLength, PositionGet, PositionSet, Close, Dispose
        }

        #endregion

        public EventStream(Stream selfStream) { this.SelfStream = selfStream; }

        public static explicit operator Stream(EventStream es) => es.SelfStream;
        public static explicit operator EventStream(Stream s)  => new EventStream( s );

        #region Implementation of INullable

        public bool IsNull => this.SelfStream == null;

        public long PositionWothoutEvent { get => this.SelfStream.Position; set => this.SelfStream.Position = value; }

        public void Close() {
            Interrupt( TypeE.Close );
            this.SelfStream.Close();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose() {
            Interrupt( TypeE.Dispose );
            this.SelfStream?.Dispose();
        }

        #endregion
    }


    [Serializable, XmlInclude( typeof(RegInfo) )]
    public partial struct RegInfo : IDisposable {
        public static readonly RegInfo     Empty;
        [XmlIgnore] public     EventStream Stream;
        public                 int         ID;
        public                 string      SaveFilePath;

        public long Position;

        public TYPE Type;
    }

    public partial struct RegInfo {
        public RegInfo(Stream stream, int id, string saveFilePath, TYPE type, int positionInStream = 0) {
            this.Type         = type;
            this.ID           = id;
            this.SaveFilePath = saveFilePath;
            this.Position     = positionInStream;

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
                    this.Position = this.Stream.PositionWothoutEvent;
                    break;
                }
            }
        }

        public override string ToString() => "ID: " + this.ID + "  | Stream: " + this.Stream + "  | FilePath: " + this.SaveFilePath + "  | Type: " + this.Type;

        public void Finished() {
            this.Stream?.Close();
            Dispose();
            this.Stream       = default;
            this.ID           = default;
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
        public enum TYPE {
            UNKNOWN = -1,
            SINGLE_FILE,
            ARCHIFE,
            TEXT
        }
    }
}