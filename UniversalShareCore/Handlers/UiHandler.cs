#region using

using System;
using System.Windows.Forms;
using UniversalShareCore.LowLvlHandler;

#endregion

namespace UniversalShareCore.Handlers {
    public class UiHandler : LowLvlHandlerBase, IUiHandler {
        public IUiHandler uiHandler;


        public UiHandler(ExceptionHandler exceptionHandler, IUiHandler uiHandler) : base( exceptionHandler ) {
            try {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }

            this.uiHandler = uiHandler;
        }

        public void ShowUI(Form _form) {
            try {
                this.uiHandler.ShowUI( _form );
            } catch (Exception e) {
                this._exceptionHandler.EscalateException( e );
            }
        }

        public bool ConfirmHandler(dynamic info) => this.uiHandler.ConfirmHandler( info );

        public void CloseUI() { this.uiHandler.CloseUI(); }

        public bool GetConfirm(dynamic info) => this.uiHandler.ConfirmHandler( info );
    }

    public interface IUiHandler {
        void CloseUI();
        void ShowUI(Form            _form);
        bool ConfirmHandler(dynamic info);
    }
}