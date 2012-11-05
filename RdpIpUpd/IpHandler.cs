using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RdpIpUpd
{
    class IpHandler : IDisposable
    {
        private readonly Settings _settings;
        private readonly NetworkListener _listener;
        private readonly object _lock = new object();

        public IpHandler(Settings settings, NetworkListener listener)
        {
            _settings = settings;
            _listener = listener;
            _listener.AddressChanged += AddressChanged;
            _listener.Listen();
        }

        public void ForceUpdate()
        {
            ChangeAddress(_listener.GetAddress());
        }

        private void AddressChanged(object sender, IpAddressEventArgs e)
        {
            ChangeAddress(e.IpAddress);
        }

        private void ChangeAddress(string ipAddress)
        {
            lock (_lock)
            {
                var address = ReadAddressFromRdpFile(_settings.RdpPath);
                if (address != ipAddress)
                {
                    WriteAddressToRdpFile(_settings.RdpPath, ipAddress);
                }
            }
        }

        private static void WriteAddressToRdpFile(string rdpPath, string ipAddress)
        {
            var lines = GetRdpContentsAsync(rdpPath);
            var newLine = "full address:s:" + ipAddress;
            var addressLine = lines.FindIndex(l => l.StartsWith("full address:", StringComparison.CurrentCultureIgnoreCase));
            if (addressLine >= 0)
            {
                lines.RemoveAt(addressLine);
            }
            else
            {
                addressLine = 0;
            }
            lines.Insert(addressLine, newLine);
            WriteRdpFile(rdpPath, lines);
        }

        private static string ReadAddressFromRdpFile(string rdpPath)
        {
            var address = "";
            var lines = GetRdpContentsAsync(rdpPath);
            var addressLine =
                lines.FirstOrDefault(l => l.StartsWith("full address:", StringComparison.CurrentCultureIgnoreCase));
            if (addressLine != null)
            {
                address = addressLine.Substring(13);
            }
            return address;
        }


        private static List<string> GetRdpContentsAsync(string rdpPath)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(rdpPath)) return lines;
            if (!File.Exists(rdpPath)) return lines;

            try
            {
                using (var stream = new StreamReader(rdpPath))
                {
                    var contents = stream.ReadToEnd();
                    lines.AddRange(contents.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
                }
            }
            catch (Exception)
            {
                return new List<string>();
            }
            
            return lines;
        }

        private static void WriteRdpFile(string rdpPath, IEnumerable<string> lines)
        {
            try
            {
                using (var stream = new StreamWriter(rdpPath, false))
                {
                    foreach (var line in lines)
                    {
                        stream.WriteLine(line);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void Dispose()
        {
            _listener.StopListening();
        }
    }
}
