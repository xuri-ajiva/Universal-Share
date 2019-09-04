using System.Xml.Serialization;

namespace Universal_Share.Options {
    public static class SettingsStatic {
        
        [XmlIgnore] public static string SavePathS = "config.xml";
    }
}