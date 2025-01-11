// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Canable
{
    using System.Runtime.InteropServices;

    internal class CandleDataStructure
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleHostConfig
        {
            public uint ByteOrder;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleDeviceConfig
        {
            public byte reserved1;
            public byte reserved2;
            public byte reserved3;
            public byte icount;     // interface count
            public uint sw_version;
            public uint hw_version;
        }

        // See STM32F manual for the details
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleCapability
        {
            public uint Feature;
            public uint FclkCan;    // CAN clock frequency
            public uint Tseg1Min;   // Timing segment 1 minimum
            public uint Tseg1Max;   // Timing segment 1 maximum
            public uint Tseg2Min;   // Timing segment 2 minimum
            public uint Tseg2Max;   // Timing segment 2 maximum
            public uint SjwMax;     // Resynchronization jump width maximum
            public uint BrpMin;     // Baud rate prescaler minimum
            public uint BrpMax;     // Baud rate prescaler maximum
            public uint BrpInc;     // Baud rate prescaler increment
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleBitTiming
        {
            public uint PropSeg;
            public uint PhaseSeg1;
            public uint PhaseSeg2;
            public uint Sjw;
            public uint Brp;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleDeviceMode
        {
            public uint Mode;
            public uint Flags;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct CandleDataFrame
        {
            public uint EchoId;
            public uint CanId;
            public byte CanDlc; // Data Length Code
            public byte Channel;
            public byte Flags;
            public byte Reserved;
            public byte Data0;
            public byte Data1;
            public byte Data2;
            public byte Data3;
            public byte Data4;
            public byte Data5;
            public byte Data6;
            public byte Data7;
            public uint TimestampUs;
        }

        internal static CandleDataFrame FromByteArray(byte[] data)
        {
            if (data.Length < Marshal.SizeOf<CandleDataFrame>())
            {
                throw new ArgumentException("Data buffer is too small to contain a CandleDataFrame.");
            }

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                return Marshal.PtrToStructure<CandleDataFrame>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct BitTimingStruct
        {
            public uint PropSeg;
            public uint PhaseSeg1;
            public uint PhaseSeg2;
            public uint Sjw;
            public uint Brp;
        }

        internal record BitTiming(
            uint PropSeg,
            uint PhaseSeg1,
            uint PhaseSeg2,
            uint Sjw,
            uint Brp);
    }
}
