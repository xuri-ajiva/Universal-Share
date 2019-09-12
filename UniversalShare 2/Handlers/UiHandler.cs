#region using

using System;
using System.Windows.Forms;
using UniversalShare_2.Interface;
using UniversalShare_2.Operation;

#endregion

namespace UniversalShare_2.Handlers {
    public class UiHandler {
        public UserInput UserInput;

        private readonly ExceptionHandler _handler;

        public UiHandler(ExceptionHandler exceptionHandler) {
            this._handler = exceptionHandler;
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
            } catch (Exception e) {
                this._handler.EscalateException( e );
            }
            this.UserInput = new UserInput();
        }

        public void ShowUI(Form _form) { Application.Run( _form ); }
        public void CloseUI()          { Application.Exit(); }

        public bool GetConfirm(TokenItem tokenItem) { return true; }
    }
}