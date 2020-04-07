using System.Collections.Generic;

namespace DesktopAgent.Scan.Shared
{
    public interface IDeviceProperty
    {
        bool IsReadonly { get; set; }
        bool IsVector { get; set; }
        string Name { get; set; }
        string PropertyId { get; set; }
        int PropertyType { get; set; }
        dynamic Value { get; set; }

        string SubType { get; set; }
        dynamic SubTypeDefault { get; set; }
        int SubTypeMax { get; set; }
        int SubTypeMin { get; set; }
        int SubTypeStep { get; set; }
        List<dynamic> SubTypeValues { get; set; }
    }
}
