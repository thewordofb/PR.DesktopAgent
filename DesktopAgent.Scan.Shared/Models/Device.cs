using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public sealed class Device : IDevice
    {
        public string DeviceId { get; set; }
        public string FullDeviceId { get; set; }
        public string Type { get; set; }  //Camera, Scanner, Video, Unknown
        public IEnumerable<DeviceProperty> Properties { get; set; } = new List<DeviceProperty>();
    }
}
