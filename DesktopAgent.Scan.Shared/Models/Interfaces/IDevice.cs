using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public enum DeviceType
    {
        Camera,
        Scanner,
        Video,
        Unknown
    }

    public interface IDevice
    {
        string DeviceId { get; set; }
        string FullDeviceId { get; set; }
        string Type { get; set; }  //Camera, Scanner, Video, Unknown
        IEnumerable<DeviceProperty> Properties { get; set; } 
    }
}
