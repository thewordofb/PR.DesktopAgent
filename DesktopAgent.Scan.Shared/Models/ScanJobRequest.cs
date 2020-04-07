namespace DesktopAgent.Scan.Shared
{
    public sealed class ScanJobRequest : IScanJobRequest
    {
        public string DeviceId { get; set; }
        public int ResolutionDPI { get; set; }
        public int StartHorizontalPixel { get; set; }
        public int StartVerticalPixel { get; set; }
        public int WidthPixels { get; set; }
        public int HeightPixels { get; set; }
        public int BrightnessPercent { get; set; }
        public int ContrastPercent { get; set; }
        public int BitDepth { get; set; }
        public string Format { get; set; }
    }
}
