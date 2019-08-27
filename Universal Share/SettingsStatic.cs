using System.Xml.Serialization;

namespace Universal_Share {
    public static class SettingsStatic {
        
        [XmlIgnore] public static string SAVE_PATH_S = "config.xml";
    }
}