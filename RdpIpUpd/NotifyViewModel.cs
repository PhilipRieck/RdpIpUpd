using System;
using System.Windows;
using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using RdpIpUpd.Config;

namespace RdpIpUpd
{
    internal class NotifyViewModel : PropertyChangedBase
    {
        private TaskbarIcon _taskbarIcon;
        private readonly Lazy<IWindowManager> _windowManager;
        private readonly Func<ConfigViewModel> _configFactory;
        private readonly Settings _settings;
        private readonly IpHandler _handler;

        public NotifyViewModel(Lazy<IWindowManager> windowManager, Func<ConfigViewModel> configFactory, Settings settings, IpHandler handler)
        {
            _windowManager = windowManager;
            _configFactory = configFactory;
            _settings = settings;
            _handler = handler;
            _canUpdate = false;
            InitializeIcon();
            Update();
        }

        private void InitializeIcon()
        {
            _taskbarIcon = (TaskbarIcon) Application.Current.FindResource("NetWatcherNotify");
            if (_taskbarIcon != null)
            {
                _taskbarIcon.DataContext = this;
            }
        }

        public void Stop()
        {
            _taskbarIcon.Visibility = Visibility.Collapsed;
        }

        public void Exit()
        {
            Stop();
            Application.Current.Shutdown();
        }

        public void Configure()
        {
            var model = _configFactory();
            _windowManager.Value.ShowDialog(model);
            NotifyOfPropertyChange(() => CanUpdate);
        }

        public void Update()
        {
            _handler.ForceUpdate();
        }

        private bool _canUpdate;
        public bool CanUpdate
        {
            get { return !string.IsNullOrWhiteSpace(_settings.RdpPath); }
            set { _canUpdate = value; }
        }
    }
}