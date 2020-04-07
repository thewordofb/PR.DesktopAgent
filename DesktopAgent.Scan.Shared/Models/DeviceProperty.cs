using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public sealed class DeviceProperty : IDeviceProperty
    {
        public bool IsReadonly { get; set; }
        public bool IsVector { get; set; }
        public string Name { get; set; }
        public string PropertyId { get; set; }
        public int PropertyType { get; set; }
        public dynamic Value { get; set; }

        public string SubType { get; set; }
        public dynamic SubTypeDefault { get; set; }
        public int SubTypeMax { get; set; }
        public int SubTypeMin { get; set; }
        public int SubTypeStep { get; set; }
        public List<dynamic> SubTypeValues { get; set; } = new List<dynamic>();
    }
}
