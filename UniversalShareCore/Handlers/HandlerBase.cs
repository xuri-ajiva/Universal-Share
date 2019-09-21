using UniversalShareCore.LowLvlHandler;

namespace UniversalShareCore.Handlers
{
    public class HandlerBase : LowLvlHandlerBase {
        protected IUiHandler _uiHandler;
        /// <inheritdoc />
        public HandlerBase(DataHandler dataHandler) : base( dataHandler.ExceptionHandler ) { this._uiHandler = dataHandler.UiHandler; }
    }
}
