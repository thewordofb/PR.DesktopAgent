using DesktopAgent.Scan.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesktopAgent.Scan.Services
{
    public class ScannerService : IScannerService
    {
        public IEnumerable<IDevice> GetAllScanners()
        {
            return ScannerProvider.GetDevices();
        }

        public IDevice GetScannerById(string deviceId)
        {
            IEnumerable<IDevice> devices = ScannerProvider.GetDevices();

            foreach(Device device in devices)
            {
                if(device.DeviceId.Contains(deviceId))
                {
                    return device;
                }
            }

            return null;
        }
    }
}
