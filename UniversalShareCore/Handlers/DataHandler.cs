using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UniversalShareCore.LowLvlHandler;
using UniversalShareCore.Operation;
using UniversalShareCore.Oprions;

namespace UniversalShareCore.Handlers {
    public class DataHandler {
        [XmlIgnore] public ExceptionHandler ExceptionHandler;
        [XmlIgnore] public bool       Changed;
        [XmlIgnore] public IUiHandler UiHandler;
        [XmlIgnore] public SettingsHandler SettingsHandler;
        [XmlIgnore] public KeyHandler KeyHandler;

        public SerializableDictionary<string, OperationInfo> OperationIdMap = new SerializableDictionary<string, OperationInfo>();
        public SerializableDictionary<string, TokenItem>     TokenList      = new SerializableDictionary<string, TokenItem>();


        public DataHandler(ExceptionHandler exceptionHandler,IUiHandler uiHandler, SettingsHandler settingsHandler, KeyHandler keyHandler) {
            this.UiHandler = uiHandler;
            this.SettingsHandler = settingsHandler;
            this.KeyHandler = keyHandler;
            this.ExceptionHandler = exceptionHandler;
            CreatedEventHandler();
        }


        private readonly List<Process> _processes = new List<Process>();

        [DebuggerStepThrough]
        private void CreatedEventHandler() {
            this.OperationIdMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.TokenList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
        }

        [DebuggerStepThrough] private void ChangedEventHandler() { this.Changed = true; }

        public void Execute(object get) { }
    }
}