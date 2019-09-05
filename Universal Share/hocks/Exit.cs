using System.Runtime.InteropServices;
using Universal_Share.ProgMain;
// ReSharper disable InconsistentNaming

namespace Universal_Share.hocks {
    public class Exit {
        [DllImport( "Kernel32" )] private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);

        private static EventHandler _handler;

        public enum CtrlType {
            CTRL_C_EVENT        = 0,
            CTRL_BREAK_EVENT    = 1,
            CTRL_CLOSE_EVENT    = 2,
            CTRL_LOGOFF_EVENT   = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig) { return ßMainPoint.Exit( sig ); }

        public static void CreateHock() {
            _handler += Handler;
            SetConsoleCtrlHandler( _handler, true );
        }
    }
}