// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    internal class CanableFeature(uint rawAvaliableFeature)
    {
        public const uint ListenOnlyBit = 0x01;
        public const uint LoopbackBit = 0x02;
        public const uint TripleSampleBit = 0x04;
        public const uint OneShotBit = 0x08;
        public const uint HwTimeStampBit = 0x10;
        public const uint IdentityBit = 0x20;
        public const uint UserIdBit = 0x40;
        public const uint PadPacketsToMaxPacketSizeBit = 0x80;
        public const uint FdBit = 0x100;
        public const uint RequestUsbQuirkLpc546XXBit = 0x200;
        public const uint BtConstExtBit = 0x400;
        public const uint TerminationBit = 0x800;
        public const uint BerrReportingBit = 0x1000;
        public const uint GetStateBit = 0x2000;

        private readonly uint rawAvaliableFeature = rawAvaliableFeature;

        public bool ListenOnly => (this.rawAvaliableFeature & ListenOnlyBit) != 0;
        public bool Loopback => (this.rawAvaliableFeature & LoopbackBit) != 0;
        public bool TripleSample => (this.rawAvaliableFeature & TripleSampleBit) != 0;
        public bool OneShot => (this.rawAvaliableFeature & OneShotBit) != 0;
        public bool HwTimeStamp => (this.rawAvaliableFeature & HwTimeStampBit) != 0;
        public bool Identity => (this.rawAvaliableFeature & IdentityBit) != 0;
        public bool UserId => (this.rawAvaliableFeature & UserIdBit) != 0;
        public bool PadPacketsToMaxPacketSize => (this.rawAvaliableFeature & PadPacketsToMaxPacketSizeBit) != 0;
        public bool Fd => (this.rawAvaliableFeature & FdBit) != 0;
        public bool RequestUsbQuirkLpc546XX => (this.rawAvaliableFeature & RequestUsbQuirkLpc546XXBit) != 0;
        public bool BtConstExt => (this.rawAvaliableFeature & BtConstExtBit) != 0;
        public bool Termination => (this.rawAvaliableFeature & TerminationBit) != 0;
        public bool BerrReporting => (this.rawAvaliableFeature & BerrReportingBit) != 0;
        public bool GetState => (this.rawAvaliableFeature & GetStateBit) != 0;

        public override string ToString()
        {
            return $"ListenOnly: {this.ListenOnly}, " +
                   $"Loopback: {this.Loopback}, " +
                   $"TripleSample: {this.TripleSample}, " +
                   $"OneShot: {this.OneShot}, " +
                   $"HwTimeStamp: {this.HwTimeStamp}, " +
                   $"Identity: {this.Identity}, " +
                   $"UserId: {this.UserId}, " +
                   $"PadPacketsToMaxPacketSize: {this.PadPacketsToMaxPacketSize}, " +
                   $"Fd: {this.Fd}, " +
                   $"RequestUsbQuirkLpc546XX: {this.RequestUsbQuirkLpc546XX}, " +
                   $"BtConstExt: {this.BtConstExt}, " +
                   $"Termination: {this.Termination}, " +
                   $"BerrReporting: {this.BerrReporting}, " +
                   $"GetState: {this.GetState}";
        }
    }
}
