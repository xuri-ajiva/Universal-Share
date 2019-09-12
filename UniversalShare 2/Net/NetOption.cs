using System.Diagnostics;

namespace UniversalShare_2.Net {
    public class NetOption {
        public static readonly byte[] Error          = { 255, 255, 255, 255 };
        public static readonly byte[] Success        = { 201, 201, 201, 201 };
        public static readonly byte[] WriteInFile     = { 103, 2, 1, 1 };
        public static readonly byte[] CreateRegister = { 103, 4, 1, 1 };
        public static readonly byte[] CloseRegister = { 103, 4, 1, 2 };
        public static readonly byte[] FileType       = { 109, 9, 0, 0 };

        [DebuggerStepThrough]
        public static bool IsEqual(byte[] x1, byte[] x2) {
            if ( x1 == null || x2 == null ) return false;
            if ( x1.Length != x2.Length ) return false;
            for ( var i = 0; i < x1.Length; i++ ) {
                if ( x1[i] != x2[i] ) return false;
            }

            return true;
        }

        public static Range OperationType(byte[] bytes) {
            if ( bytes.Length <= 0 ) return Range.None;

            return ( bytes[0] <= 100 ) ? ( Range.Server ) : ( ( bytes[0] <= 200 ) ? ( Range.Server ) : ( ( bytes[0] <= 255 ) ? ( Range.Special ) : ( Range.None ) ) );
        }

        public enum Range : byte {
            Server  = 1,
            Client  = 2,
            Special = 3,
            None    = 99,
        }
    }
}