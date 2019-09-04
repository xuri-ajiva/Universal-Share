using System;
using System.Xml.Serialization;

namespace Universal_Share.Options {
    [Serializable, XmlInclude( typeof(TypeHolder) )]
    public struct TypeHolder {
        // ReSharper disable once UnusedMember.Global
        public static readonly TypeHolder Empty = new TypeHolder();

        public TypeHolder(string openWith, string argumentsBeforePathToFile, string argumentsAfterPathToFile, bool userConfirm, string description, bool closeFileStream) {
            this.OpenWith                  = openWith;
            this.UserConfirm               = userConfirm;
            this.Description               = description;
            this.CloseFileStream           = closeFileStream;
            this.ArgumentsAfterPathToFile  = argumentsAfterPathToFile;
            this.ArgumentsBeforePathToFile = argumentsBeforePathToFile;
        }

        public string OpenWith                  { get; set; }
        public string ArgumentsBeforePathToFile { get; set; }
        public string ArgumentsAfterPathToFile  { get; set; }
        public bool   UserConfirm               { get; set; }
        public bool   CloseFileStream           { get; set; }
        public string Description               { get; set; }
    }
}