using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using UniversalShare_2.Operation;
using UniversalShare_2.Oprions;

namespace UniversalShare_2.Handlers {
    public class DataHandler {
        [XmlIgnore] public bool Changed;


        public readonly SerializableDictionary<string, OperationInfo> OperationIdMap = new SerializableDictionary<string, OperationInfo>();
        public readonly SerializableDictionary<string, TokenItem>     TokenList      = new SerializableDictionary<string, TokenItem>();

        private readonly List<Process> _processes = new List<Process>();

        public DataHandler() { CreatedEventHandler(); }

        [DebuggerStepThrough]
        private void CreatedEventHandler() {
            this.OperationIdMap.OnDictionaryChanged += (sender, args) => ChangedEventHandler();
            this.TokenList.OnDictionaryChanged      += (sender, args) => ChangedEventHandler();
        }

        [DebuggerStepThrough] private void ChangedEventHandler() { this.Changed = true; }

        public void Execute(object get) {

        }
    }
}