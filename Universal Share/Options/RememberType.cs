using System;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Universal_Share.Options {
    [Serializable]
    public struct RememberType {
        [XmlAttribute] public DialogResult Value;
        public                string       Description;
        [XmlAttribute] public string Type;

        public RememberType(string description, DialogResult value, string type) {
            this.Description = description;
            this.Value       = value;
            this.Type        = type;
        }

        public bool IsOkOrYes() { return this.Value == DialogResult.Yes || this.Value == DialogResult.OK; }
    }
}