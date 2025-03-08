/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace Gavilya.Helpers
{
	public static class DesktopMonitorHelper
	{
		public const int ERROR_SUCCESS = 0;

		public enum QUERY_DEVICE_CONFIG_FLAGS : uint
		{
			QDC_ALL_PATHS = 0x00000001,
			QDC_ONLY_ACTIVE_PATHS = 0x00000002,
			QDC_DATABASE_CURRENT = 0x00000004
		}

		public enum DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY : uint
		{
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_OTHER = 0xFFFFFFFF,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HD15 = 0,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SVIDEO = 1,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPOSITE_VIDEO = 2,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_COMPONENT_VIDEO = 3,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DVI = 4,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_HDMI = 5,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_LVDS = 6,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_D_JPN = 8,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDI = 9,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EXTERNAL = 10,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_DISPLAYPORT_EMBEDDED = 11,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EXTERNAL = 12,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_UDI_EMBEDDED = 13,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_SDTVDONGLE = 14,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_MIRACAST = 15,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_INTERNAL = 0x80000000,
			DISPLAYCONFIG_OUTPUT_TECHNOLOGY_FORCE_UINT32 = 0xFFFFFFFF
		}

		public enum DISPLAYCONFIG_SCANLINE_ORDERING : uint
		{
			DISPLAYCONFIG_SCANLINE_ORDERING_UNSPECIFIED = 0,
			DISPLAYCONFIG_SCANLINE_ORDERING_PROGRESSIVE = 1,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED = 2,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_UPPERFIELDFIRST = DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED,
			DISPLAYCONFIG_SCANLINE_ORDERING_INTERLACED_LOWERFIELDFIRST = 3,
			DISPLAYCONFIG_SCANLINE_ORDERING_FORCE_UINT32 = 0xFFFFFFFF
		}

		public enum DISPLAYCONFIG_ROTATION : uint
		{
			DISPLAYCONFIG_ROTATION_IDENTITY = 1,
			DISPLAYCONFIG_ROTATION_ROTATE90 = 2,
			DISPLAYCONFIG_ROTATION_ROTATE180 = 3,
			DISPLAYCONFIG_ROTATION_ROTATE270 = 4,
			DISPLAYCONFIG_ROTATION_FORCE_UINT32 = 0xFFFFFFFF
		}

		public enum DISPLAYCONFIG_SCALING : uint
		{
			DISPLAYCONFIG_SCALING_IDENTITY = 1,
			DISPLAYCONFIG_SCALING_CENTERED = 2,
			DISPLAYCONFIG_SCALING_STRETCHED = 3,
			DISPLAYCONFIG_SCALING_ASPECTRATIOCENTEREDMAX = 4,
			DISPLAYCONFIG_SCALING_CUSTOM = 5,
			DISPLAYCONFIG_SCALING_PREFERRED = 128,
			DISPLAYCONFIG_SCALING_FORCE_UINT32 = 0xFFFFFFFF
		}

		public enum DISPLAYCONFIG_PIXELFORMAT : uint
		{
			DISPLAYCONFIG_PIXELFORMAT_8BPP = 1,
			DISPLAYCONFIG_PIXELFORMAT_16BPP = 2,
			DISPLAYCONFIG_PIXELFORMAT_24BPP = 3,
			DISPLAYCONFIG_PIXELFORMAT_32BPP = 4,
			DISPLAYCONFIG_PIXELFORMAT_NONGDI = 5,
			DISPLAYCONFIG_PIXELFORMAT_FORCE_UINT32 = 0xffffffff
		}

		public enum DISPLAYCONFIG_MODE_INFO_TYPE : uint
		{
			DISPLAYCONFIG_MODE_INFO_TYPE_SOURCE = 1,
			DISPLAYCONFIG_MODE_INFO_TYPE_TARGET = 2,
			DISPLAYCONFIG_MODE_INFO_TYPE_FORCE_UINT32 = 0xFFFFFFFF
		}

		public enum DISPLAYCONFIG_DEVICE_INFO_TYPE : uint
		{
			DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME = 1,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_PREFERRED_MODE = 3,
			DISPLAYCONFIG_DEVICE_INFO_GET_ADAPTER_NAME = 4,
			DISPLAYCONFIG_DEVICE_INFO_SET_TARGET_PERSISTENCE = 5,
			DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_BASE_TYPE = 6,
			DISPLAYCONFIG_DEVICE_INFO_FORCE_UINT32 = 0xFFFFFFFF
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct LUID
		{
			public uint LowPart;
			public int HighPart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_PATH_SOURCE_INFO
		{
			public LUID adapterId;
			public uint id;
			public uint modeInfoIdx;
			public uint statusFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_PATH_TARGET_INFO
		{
			public LUID adapterId;
			public uint id;
			public uint modeInfoIdx;
			DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
			DISPLAYCONFIG_ROTATION rotation;
			DISPLAYCONFIG_SCALING scaling;
			DISPLAYCONFIG_RATIONAL refreshRate;
			DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
			public bool targetAvailable;
			public uint statusFlags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_RATIONAL
		{
			public uint Numerator;
			public uint Denominator;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_PATH_INFO
		{
			public DISPLAYCONFIG_PATH_SOURCE_INFO sourceInfo;
			public DISPLAYCONFIG_PATH_TARGET_INFO targetInfo;
			public uint flags;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_2DREGION
		{
			public uint cx;
			public uint cy;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_VIDEO_SIGNAL_INFO
		{
			public ulong pixelRate;
			public DISPLAYCONFIG_RATIONAL hSyncFreq;
			public DISPLAYCONFIG_RATIONAL vSyncFreq;
			public DISPLAYCONFIG_2DREGION activeSize;
			public DISPLAYCONFIG_2DREGION totalSize;
			public uint videoStandard;
			public DISPLAYCONFIG_SCANLINE_ORDERING scanLineOrdering;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_TARGET_MODE
		{
			public DISPLAYCONFIG_VIDEO_SIGNAL_INFO targetVideoSignalInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTL
		{
			int x;
			int y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_SOURCE_MODE
		{
			public uint width;
			public uint height;
			public DISPLAYCONFIG_PIXELFORMAT pixelFormat;
			public POINTL position;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct DISPLAYCONFIG_MODE_INFO_UNION
		{
			[FieldOffset(0)]
			public DISPLAYCONFIG_TARGET_MODE targetMode;
			[FieldOffset(0)]
			public DISPLAYCONFIG_SOURCE_MODE sourceMode;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_MODE_INFO
		{
			public DISPLAYCONFIG_MODE_INFO_TYPE infoType;
			public uint id;
			public LUID adapterId;
			public DISPLAYCONFIG_MODE_INFO_UNION modeInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS
		{
			public uint value;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAYCONFIG_DEVICE_INFO_HEADER
		{
			public DISPLAYCONFIG_DEVICE_INFO_TYPE type;
			public uint size;
			public LUID adapterId;
			public uint id;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DISPLAYCONFIG_TARGET_DEVICE_NAME
		{
			public DISPLAYCONFIG_DEVICE_INFO_HEADER header;
			public DISPLAYCONFIG_TARGET_DEVICE_NAME_FLAGS flags;
			public DISPLAYCONFIG_VIDEO_OUTPUT_TECHNOLOGY outputTechnology;
			public ushort edidManufactureId;
			public ushort edidProductCodeId;
			public uint connectorInstance;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string monitorFriendlyDeviceName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string monitorDevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DEVMODE
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
			public short dmSpecVersion;
			public short dmDriverVersion;
			public short dmSize;
			public short dmDriverExtra;
			public uint dmFields;
			public int dmPositionX;
			public int dmPositionY;
			public uint dmDisplayOrientation;
			public uint dmDisplayFixedOutput;
			public short dmColor;
			public short dmDuplex;
			public short dmYResolution;
			public short dmTTOption;
			public short dmCollate;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
			public short dmLogPixels;
			public short dmBitsPerPel;
			public int dmPelsWidth;
			public int dmPelsHeight;
			public int dmDisplayFlags;
			public int dmDisplayFrequency;
			public int dmICMMethod;
			public int dmDitherType;
			public int dmReserved1;
			public int dmReserved2;
			public int dmPanningWidth;
			public int dmPanningHeight;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DISPLAY_DEVICE
		{
			public int cb;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string DeviceName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceString;
			public uint StateFlags;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceKey;
		}


		[DllImport("user32.dll")]
		public static extern int GetDisplayConfigBufferSizes(
			QUERY_DEVICE_CONFIG_FLAGS Flags,
			out uint NumPathArrayElements,
			out uint NumModeInfoArrayElements
		);

		[DllImport("user32.dll")]
		public static extern int QueryDisplayConfig(
			QUERY_DEVICE_CONFIG_FLAGS Flags,
			ref uint NumPathArrayElements,
			[Out] DISPLAYCONFIG_PATH_INFO[] PathInfoArray,
			ref uint NumModeInfoArrayElements,
			[Out] DISPLAYCONFIG_MODE_INFO[] ModeInfoArray,
			IntPtr CurrentTopologyId
		);

		[DllImport("user32.dll")]
		public static extern int DisplayConfigGetDeviceInfo(
			ref DISPLAYCONFIG_TARGET_DEVICE_NAME deviceName
		);

		[DllImport("user32.dll")]
		public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, uint dwflags, IntPtr lParam);

		public static string MonitorFriendlyName(LUID adapterId, uint targetId)
		{
			DISPLAYCONFIG_TARGET_DEVICE_NAME deviceName = new DISPLAYCONFIG_TARGET_DEVICE_NAME();
			deviceName.header.size = (uint)Marshal.SizeOf(typeof(DISPLAYCONFIG_TARGET_DEVICE_NAME));
			deviceName.header.adapterId = adapterId;
			deviceName.header.id = targetId;
			deviceName.header.type = DISPLAYCONFIG_DEVICE_INFO_TYPE.DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME;
			int error = DisplayConfigGetDeviceInfo(ref deviceName);
			if (error != ERROR_SUCCESS)
				throw new Win32Exception(error);
			return deviceName.monitorFriendlyDeviceName;
		}

		public static List<Monitor> GetMonitors()
		{
			List<Monitor> monitors = [new(Properties.Resources.MonitorAuto,"-1")];
			int error = GetDisplayConfigBufferSizes(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS,
				out uint PathCount, out uint ModeCount);
			
			if (error != ERROR_SUCCESS)
				throw new Win32Exception(error);

			DISPLAYCONFIG_PATH_INFO[] DisplayPaths = new DISPLAYCONFIG_PATH_INFO[PathCount];
			DISPLAYCONFIG_MODE_INFO[] DisplayModes = new DISPLAYCONFIG_MODE_INFO[ModeCount];
			error = QueryDisplayConfig(QUERY_DEVICE_CONFIG_FLAGS.QDC_ONLY_ACTIVE_PATHS,
				ref PathCount, DisplayPaths, ref ModeCount, DisplayModes, IntPtr.Zero);
			
			if (error != ERROR_SUCCESS)
				throw new Win32Exception(error);

			for (int i = 0; i < ModeCount; i++)
			{
				if (DisplayModes[i].infoType == DISPLAYCONFIG_MODE_INFO_TYPE.DISPLAYCONFIG_MODE_INFO_TYPE_TARGET)
				{
					Monitor monitor = new(
						MonitorFriendlyName(DisplayModes[i].adapterId, DisplayModes[i].id),
						DisplayModes[i].id.ToString());
					monitors.Add(monitor);
				}
			}
			return monitors;
		}
	}

	public class Monitor(string name, string deviceID)
	{
		public string Name { get; set; } = name;
		public string DeviceID { get; set; } = deviceID;

		public Monitor() : this(string.Empty, string.Empty)
		{
			Name = Properties.Resources.MonitorAuto;
			DeviceID = "-1";
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object? obj)
		{
			return obj is Monitor monitor && (DeviceID == monitor.DeviceID || Name == monitor.Name);
		}
	}
}
