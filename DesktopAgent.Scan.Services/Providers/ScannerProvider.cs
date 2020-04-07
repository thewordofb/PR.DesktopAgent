using DesktopAgent.Scan.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DesktopAgent.Scan.Services
{
    class ScannerProvider
    {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 0x00000001;
            public const uint FLATBED = 0x00000002;
        }
        class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEED_READY = 0x00000001;
        }
        class WIA_PROPERTIES
        {
            public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
            public const uint WIA_DIP_FIRST = 2;
            public const uint WIA_DPA_FIRST = WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPC_FIRST = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            //
            // Scanner only device properties (DPS)
            //
            public const uint WIA_DPS_FIRST = WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = WIA_DPS_FIRST + 13;
            public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = WIA_DPS_FIRST + 14;
        }

        /// <summary>
        /// Use scanner to scan an image (with user selecting the scanner from a dialog).
        /// </summary>
        /// <returns>Scanned images.</returns>
        public static List<Image> Scan()
        {
            WIA.ICommonDialog dialog = new WIA.CommonDialog();
            WIA.Device device = dialog.ShowSelectDevice
                (WIA.WiaDeviceType.UnspecifiedDeviceType, true, false);
            if (device != null)
            {
                return Scan(device.DeviceID);
            }
            else
            {
                throw new Exception("You must select a device for scanning.");
            }
        }
        /// <summary>
        /// Use scanner to scan an image (scanner is selected by its unique id).
        /// </summary>
        /// <param name="scannerName"></param>
        /// <returns>Scanned images.</returns>
        public static List<Image> Scan(string scannerId)
        {
            List<Image> images = new List<Image>();
            bool hasMorePages = true;
            while (hasMorePages)
            {
                // select the correct scanner using the provided scannerId parameter
                WIA.DeviceManager manager = new WIA.DeviceManager();
                WIA.Device device = null;
                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == scannerId)
                    {
                        // connect to scanner
                        device = info.Connect();
                        break;
                    }
                }
                // device was not found
                if (device == null)
                {
                    // enumerate available devices
                    string availableDevices = "";
                    foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                    {
                        availableDevices += info.DeviceID + "\n";
                    }
                    // show error with available devices
                    throw new Exception("The device with provided ID could not be found.  Available Devices:\n" + availableDevices);
                }
                WIA.Item item = device.Items[1] as WIA.Item;
                try
                {
                    // scan image
                    WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                    WIA.ImageFile image = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatBMP, false);
                    // save to temp file
                    string fileName = Path.GetTempFileName();
                    File.Delete(fileName);
                    image.SaveFile(fileName);
                    image = null;
                    // add file to output list
                    images.Add(Image.FromFile(fileName));
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    item = null;
                    //determine if there are any more pages waiting
                    WIA.Property documentHandlingSelect = null;
                    WIA.Property documentHandlingStatus = null;
                    foreach (WIA.Property prop in device.Properties)
                    {
                        if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_SELECT)
                            documentHandlingSelect = prop;
                        if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_STATUS)
                            documentHandlingStatus = prop;
                    }
                    // assume there are no more pages
                    hasMorePages = false;
                    // may not exist on flatbed scanner but required for feeder
                    if (documentHandlingSelect != null)
                    {
                        // check for document feeder
                        if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) &
                        WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
                        {
                            hasMorePages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) &
                            WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
                        }
                    }
                }
            }
            return images;
        }
        /// <summary>
        /// Gets the list of available WIA devices.
        /// </summary>
        /// <returns></returns>
        public static List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();
            WIA.DeviceManager manager = new WIA.DeviceManager();
            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                Device device = new Device();

                switch (info.Type)
                {
                    case WIA.WiaDeviceType.CameraDeviceType:
                        device.Type = "Camera";
                        break;
                    case WIA.WiaDeviceType.ScannerDeviceType:
                        device.Type = "Scanner";
                        break;
                    case WIA.WiaDeviceType.VideoDeviceType:
                        device.Type = "Video";
                        break;
                    default:
                        device.Type = "Unknown";
                        break;
                }

                device.FullDeviceId = info.DeviceID;
                device.DeviceId = info.DeviceID.Split('\\')[0];
                List<DeviceProperty> deviceProperties = new List<DeviceProperty>();
                foreach (WIA.Property property in info.Properties)
                {
                    DeviceProperty deviceProperty = new DeviceProperty();
                    deviceProperty.IsReadonly = property.IsReadOnly;
                    deviceProperty.IsVector = property.IsVector;
                    deviceProperty.Name = property.Name;
                    deviceProperty.PropertyId = property.PropertyID.ToString();
                    deviceProperty.PropertyType = property.Type;
                    deviceProperty.Value = property.get_Value();

                    try
                    {
                        switch (property.SubType)
                        {
                            case WIA.WiaSubType.FlagSubType:
                                deviceProperty.SubType = "Flag";
                                break;
                            case WIA.WiaSubType.ListSubType:
                                deviceProperty.SubType = "List";
                                break;
                            case WIA.WiaSubType.RangeSubType:
                                deviceProperty.SubType = "Range";
                                break;
                            case WIA.WiaSubType.UnspecifiedSubType:
                                deviceProperty.SubType = "Unspecified";
                                break;
                            default:
                                deviceProperty.SubType = string.Empty;
                                break;
                        }

                        deviceProperty.SubTypeDefault = property.SubTypeDefault;
                        deviceProperty.SubTypeMax = property.SubTypeMax;
                        deviceProperty.SubTypeMin = property.SubTypeMin;
                        deviceProperty.SubTypeStep = property.SubTypeStep;

                        foreach (var v in property.SubTypeValues)
                        {
                            deviceProperty.SubTypeValues.Add(v);
                        }
                    }
                    catch (Exception ex)
                    {
                        deviceProperty.SubType = string.Empty;
                    }

                    deviceProperties.Add(deviceProperty);
                }
                device.Properties = deviceProperties;

                devices.Add(device);
            }
            return devices;
        }

        /*                                           
          public class ScannerService              
          {                                        
              public static void Scan()
              {
                  try
                  {
                      CommonDialogClass commonDialogClass = new CommonDialogClass();
                      Device scannerDevice = commonDialogClass.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false, false);
                      if (scannerDevice != null)
                      {
                          Item scannnerItem = scannerDevice.Items[1];
                          AdjustScannerSettings(scannnerItem, 600, 0, 0, 1010, 620, 0, 0);
                          object scanResult = commonDialogClass.ShowTransfer(scannnerItem, WIA.FormatID.wiaFormatPNG, false);
                          if (scanResult != null)
                          {
                              ImageFile image = (ImageFile)scanResult;
                              SaveImageToJpgFile(image, Constants.ScannedImageLocation);
                          }
                      }
                  }
                  catch (System.Runtime.InteropServices.COMException)
                  {
                      MessageBox.Show("Problem with scanning device. Please ensure that the scanner is properly connected and switched on", "Inweon Grain Management System");
                  }
              }

              private static void AdjustScannerSettings(IItem scannnerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel,
                  int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents)
              {
                  const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
                  const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
                  const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
                  const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
                  const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
                  const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
                  const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
                  const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
                  const string WIA_SCAN_BIT_DEPTH = "4104";
                  SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
                  SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
                  SetWIAProperty(scannnerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
                  SetWIAProperty(scannnerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
                  //SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BIT_DEPTH, 48);
                  SetWIAProperty(scannnerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
                  SetWIAProperty(scannnerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
              }

              private static void SetWIAProperty(IProperties properties, object propName, object propValue)
              {
                  Property prop = properties.get_Item(ref propName);
                  prop.set_Value(ref propValue);
              }

              private static void SaveImageToJpgFile(ImageFile image, string fileName)
              {
                  ImageProcess imgProcess = new ImageProcess();
                  object convertFilter = "Convert";
                  string convertFilterID = imgProcess.FilterInfos.get_Item(ref convertFilter).FilterID;
                  imgProcess.Filters.Add(convertFilterID, 0);
                  SetWIAProperty(imgProcess.Filters[imgProcess.Filters.Count].Properties, "FormatID", FormatID.wiaFormatJPEG);
                  image = imgProcess.Apply(image);
                  image.SaveFile(fileName);
              }
          }
          */
    }
}
