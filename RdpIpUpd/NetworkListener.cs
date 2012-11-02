using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RdpIpUpd
{
    class NetworkListener
    {
        public event EventHandler<IpAddressEventArgs> AddressChanged;
        private string _lastAddress;

        private void OnAddressChanged(string newAddress)
        {
            if (AddressChanged != null)
            {
                AddressChanged(this, new IpAddressEventArgs {IpAddress = newAddress});
            }
        }
        
        public void Listen()
        {
            _lastAddress = GetAddress();
            NetworkChange.NetworkAddressChanged += AddressWasChanged;
        }

        public void StopListening()
        {
            NetworkChange.NetworkAddressChanged -= AddressWasChanged;
        }

        private void AddressWasChanged(object sender, EventArgs e)
        {
            var address = GetAddress();
            if (String.CompareOrdinal(address, _lastAddress) != 0)
            {
                _lastAddress = address;
                OnAddressChanged(address);
            }
        }

        public string GetAddress()
        {
            var adapters = NetworkInterface.GetAllNetworkInterfaces();

            var adapter = adapters.FirstOrDefault(a =>
                                a.OperationalStatus == OperationalStatus.Up
                                && a.Supports(NetworkInterfaceComponent.IPv4));

            if (adapter == null) return String.Empty;

            var ipProperties = adapter.GetIPProperties();
            var address = ipProperties.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork);
            return address != null ? address.Address.ToString() : String.Empty;
        }
    }

    public class IpAddressEventArgs : EventArgs
    {
        public string IpAddress { get; set; }
    }
}