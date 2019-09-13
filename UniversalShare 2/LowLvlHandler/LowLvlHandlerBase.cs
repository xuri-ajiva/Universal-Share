using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalShare_2.Handlers
{
    public class LowLvlHandlerBase {
        protected ExceptionHandler _exceptionHandler;
        public LowLvlHandlerBase(ExceptionHandler exceptionHandler) { this._exceptionHandler = exceptionHandler; }
    }
}
