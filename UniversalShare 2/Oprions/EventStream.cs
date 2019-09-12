using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml.Serialization;

namespace UniversalShare_2.Oprions
{
    public class EventStream : INullable, IDisposable {
        [XmlIgnore] private readonly Stream _selfStream;

        public delegate void DictionaryChanged(object sender, TypeE args);

        public event DictionaryChanged OnStreamChanged;

        #region Overrides of Stream

        public void Flush() {
            this._selfStream.Flush();
            Interrupt( TypeE.Flush );
        }

        public void FlushWithoutEvent() { this._selfStream.Flush(); }

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

        public void CloseWithoutEvent() { this._selfStream.Close(); }

        #endregion

        #region Implementation of IDisposable

        public void Dispose() {
            Interrupt( TypeE.Dispose );
            this._selfStream?.Dispose();
        }

        public void DisposeWithoutEvent() { this._selfStream?.Dispose(); }

        #endregion
    }

}
