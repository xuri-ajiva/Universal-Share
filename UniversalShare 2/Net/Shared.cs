using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalShare_2.Handlers;

namespace UniversalShare_2.Net
{
    class Shared : ReversesHandler
    {
        /// <inheritdoc />
        public Shared(ExceptionHandler _exceptionHandler, UiHandler uiHandler) : base( _exceptionHandler, uiHandler ) { }
    }
}
