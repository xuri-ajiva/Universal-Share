using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Universal_Share.Interface;
using UniversalShare_2.Interface;
using UniversalShareCore.Handlers;

namespace UniversalShare_2 {
    class FormUiHandler : IUiHandler {
        public Editor    E;
        public MainFormP M;
        public UserInput U;
        public FormUiHandler(MainFormP m, Editor e, UserInput u) {
            this.M = m;
            this.E = e;
            this.U = u;
        }

        #region Implementation of IUiHandler

        /// <inheritdoc />
        public void CloseUI() { }

        /// <inheritdoc />
        public void ShowUI(Form _form) { }

        /// <inheritdoc />
        public bool ConfirmHandler(dynamic info) { return true; }

        #endregion
    }
}