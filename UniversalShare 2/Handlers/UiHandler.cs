#region using

using System;
using System.Windows.Forms;
using UniversalShare_2.Interface;
using UniversalShare_2.Operation;

#endregion

namespace UniversalShare_2.Handlers {
    public class UiHandler : LowLvlHandlerBase {
        public UserInput UserInput;


        public UiHandler(ExceptionHandler exceptionHandler) : base( exceptionHandler ) {
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }

            this.UserInput = new UserInput();
        }

        public void ShowUI(Form _form) {
            try {
                Application.Run( _form );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }
        }

        public void CloseUI() { Application.Exit(); }

        public bool GetConfirm(TokenItem tokenItem) => this.UserInput.GetConfirm( tokenItem );
    }
}