using System.Diagnostics;

namespace Universal_Share.Net {
    public class Option {
        public static readonly byte[] ERROR           = { 255, 255, 255, 255 };
        public static readonly byte[] SUCCESS         = { 201, 201, 201, 201 };
        public static readonly byte[] SAVE_TO_FILE    = { 103, 2, 1, 1 };
        public static readonly byte[] CREATE_REGISTER = { 103, 4, 1, 1 };
        public static readonly byte[] FILE_TYPE       = { 109, 9, 0, 0 };

        [DebuggerStepThrough]
        public static bool isEqual(byte[] x1, byte[] x2) {
            if ( x1 == null && x2 == null ) return false;
            if ( x1.Length != x2.Length ) return false;
            for ( int i = 0; i < x1.Length; i++ ) {
                if ( x1[i] != x2[i] ) return false;
            }

            return true;
        }

        public static Range OperationType(byte[] bytes) {
            if ( bytes.Length > 0 ) {
                if ( bytes[0] <= 100 ) return Range.Server;
                if ( bytes[0] <= 200 ) return Range.Server;
                if ( bytes[0] <= 255 ) return Range.Special;
            }

            return Range.None;
            //return bytes.Length > 0
            //    ? ( bytes[0] <= 255
            //        ? Range.Special
            //        : bytes[0] <= 200
            //            ? Range.Client
            //            : bytes[0] <= 255
            //                ? Range.Special
            //                : Range.None )
            //    : Range.None;
        }

        public enum Range : byte {
            Server  = 1,
            Client  = 2,
            Special = 3,
            None    = 99,
        }
    }
}