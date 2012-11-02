using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;

namespace RdpIpUpd
{
    [Serializable]
    internal class Settings
    {
        private Settings()
        {
        }

        public static Settings GetSettings()
        {
            return Load();
        }

        private static Settings Load()
        {
            using(var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                if (isoFile.FileExists("settings.xml"))
                {
                    using (var stream = isoFile.OpenFile("settings.xml", FileMode.Open))
                    {
                        try
                        {
                            var formatter = new BinaryFormatter();
                            return (Settings)formatter.Deserialize(stream);
                        }
                        catch (Exception)
                        {
                            return new Settings();
                        }
                        
                    }
                }
            }
            return new Settings();
        }

        public void Save()
        {
            using (var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null))
            {
                using (var stream = isoFile.OpenFile("settings.xml", FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, this);
                }
            }
        }


        public string RdpPath { get; set; }
    }
}
