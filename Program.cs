using System;
using System.Runtime.InteropServices;

namespace ScreenResolutionChanger
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwFlags);

        [DllImport("user32.dll")]
        public static extern int EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;

            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;

            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: ScreenResolutionChanger.exe <width> <height>");
                return;
            }

            if (int.TryParse(args[0], out int width) && int.TryParse(args[1], out int height))
            {
                ChangeScreenResolution(width, height);
            }
            else
            {
                Console.WriteLine("Invalid width and/or height. Please enter valid integer values.");
            }
        }

        public static void ChangeScreenResolution(int width, int height)
        {
            DEVMODE dm = GetCurrentScreenSettings();
            if (dm.dmSize == 0)
            {
                Console.WriteLine("Failed to get the current screen settings.");
                return;
            }

            dm.dmPelsWidth = width;
            dm.dmPelsHeight = height;

            int result = ChangeDisplaySettings(ref dm, 0);

            if (result == 0)
            {
                Console.WriteLine($"Screen resolution successfully changed to {width}x{height}.");
            }
            else
            {
                Console.WriteLine($"Failed to change the screen resolution. Error code: {result}");
            }
        }

        public static DEVMODE GetCurrentScreenSettings()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));

            if (0 == EnumDisplaySettings(null, -1, ref dm))
            {
                dm.dmSize = 0;
            }

            return dm;
        }
    }
}
