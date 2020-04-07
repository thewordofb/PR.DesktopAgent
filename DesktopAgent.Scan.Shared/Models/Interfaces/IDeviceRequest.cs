namespace DesktopAgent.Scan.Shared
{
    public enum DeviceFormat
    {
        BMP,
        PNG,
        GIF,
        JPEG,
        TIFF
    }

    public interface IDeviceRequest
    {
        string DeviceId { get; set; }
        int ResolutionDPI { get; set; }
        int StartHorizontalPixel { get; set; }
        int StartVerticalPixel { get; set; }
        int WidthPixels { get; set; }
        int HeightPixels { get; set; }
        int BrightnessPercent { get; set; }
        int ContrastPercent { get; set; }
        int BitDepth { get; set; }
        string Format { get; set; } //BMP, PNG, GIF, JPEG, TIFF
    }
}
