using System.Xml.Serialization;
using Universal_Share.Net;
using Universal_Share.Security;

namespace Universal_Share.Options {
    public class SettingsStatic {
        [XmlIgnore] public static string SavePathS       = "resources.xml";
        [XmlIgnore] public static string SavePathStaticS = "config.xml";

        public int    Port          = 1000;
        public string ReplaceString = "%V%";
        public int    BufferSize    = 16777216;
        public Auth   _AuthPrivate  = Auth.Emthy;
    }
}