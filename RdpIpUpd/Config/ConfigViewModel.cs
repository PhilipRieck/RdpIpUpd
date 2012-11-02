using Caliburn.Micro;
using Microsoft.Win32;

namespace RdpIpUpd.Config
{
    internal class ConfigViewModel : Screen
    {

        private readonly Settings _settings;

        public ConfigViewModel(Settings settings)
        {
            _settings = settings;
            RdpFilePath = _settings.RdpPath;
        }

        private string _rdpFilePath;

        public string RdpFilePath
        {
            get { return _rdpFilePath; }
            set
            {
                if( _rdpFilePath != value)
                {
                    _rdpFilePath = value;
                    NotifyOfPropertyChange(()=> RdpFilePath);
                }
            }
        }


        public void Save()
        {
            _settings.RdpPath = RdpFilePath;
            _settings.Save();
            TryClose(true);
        }

        public void Browse()
        {
            var fileDialog = new OpenFileDialog
                                 {
                                     DefaultExt = ".rdp",
                                     Filter = "Remote Desktop Connection (*.rdp)|*.rdp",
                                     CheckFileExists = true,
                                     Multiselect = false
                                 };
            var result = fileDialog.ShowDialog();
            if (result == true)
            {
                RdpFilePath = fileDialog.FileName;
            }
        }

        public void Cancel()
        {
            TryClose(false);
        }

    }
}
