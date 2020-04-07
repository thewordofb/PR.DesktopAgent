namespace DesktopAgent.Scan.Shared
{
    public sealed class DeviceRequest : IDeviceRequest
    {
        public string DeviceId { get; set; } = string.Empty;
        public int ResolutionDPI { get; set; } = 600;
        public int StartHorizontalPixel { get; set; } = 0;
        public int StartVerticalPixel { get; set; } = 0;
        public int WidthPixels { get; set; } = 1010;
        public int HeightPixels { get; set; } = 620;
        public int BrightnessPercent { get; set; } = 0;
        public int ContrastPercent { get; set; } = 0;
        public int BitDepth { get; set; } = 48;
        public string Format { get; set; } //BMP, PNG, GIF, JPEG, TIFF
    }
}
