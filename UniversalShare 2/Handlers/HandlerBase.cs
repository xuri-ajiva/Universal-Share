using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalShare_2.Handlers
{
    public class HandlerBase : LowLvlHandlerBase {
        protected UiHandler _uiHandler;
        /// <inheritdoc />
        public HandlerBase(ExceptionHandler exceptionHandler, UiHandler uiHandler) : base( exceptionHandler ) { this._uiHandler = uiHandler; }
    }
}
