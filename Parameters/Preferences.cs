using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OpenSwimScoreboard.Parameters
{
    public static class Preferences
    {
        public enum DataModeType
        {
            Serial,
            Parallel,
        }

        public static Form MainInterfaceForm { get; set; }

        public static bool UseOfflineDataOnly { get; set; } = false;
        public static DataModeType DataMode { get; set; }
        public static int NumLanes { get; set; } = 6;
        public static string CurrentEvent { get; set; }
        public static string CurrentHeat { get; set; }
        public static string ErrorMessages { get; set; }

        public static string InputSerialPort
        {
            get
            {
                return GetSetting("InputSerialPort");
            }
            set
            { 
                SetSetting("InputSerialPort", value);
            }
        }

        public static string OutputSerialPort
        {
            get
            {
                return GetSetting("OutputSerialPort");
            }
            set
            { 
                SetSetting("OutputSerialPort", value);
            }
        }

        public static int? BaudRate
        {
            get
            {
                int baudRate;
                if(int.TryParse(GetSetting("BaudRate"), out baudRate))
                {
                    return baudRate;
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    SetSetting("BaudRate", value.ToString());
                }
                else
                {
                    SetSetting("BaudRate", null);
                }
            }
        }

        public static bool WriteScoreboardDataFile { get; set; } = false;

        private static string GetSetting(string key)
        {
            if (ConfigurationManager.AppSettings.HasKeys() && ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                return ConfigurationManager.AppSettings[key];
            }
            return null;
        }
        
        private static void SetSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (ConfigurationManager.AppSettings.HasKeys() && ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                config.AppSettings.Settings[key].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified, true);  
            ConfigurationManager.RefreshSection("appSettings");  
        }
    }
}
