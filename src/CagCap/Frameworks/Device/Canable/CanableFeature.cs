// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Canable
{
    internal class CanableFeature(uint rawAvaliableFeature)
    {
        internal const uint ListenOnlyBit = 0x01;
        internal const uint LoopbackBit = 0x02;
        internal const uint TripleSampleBit = 0x04;
        internal const uint OneShotBit = 0x08;
        internal const uint HwTimeStampBit = 0x10;
        internal const uint IdentityBit = 0x20;
        internal const uint UserIdBit = 0x40;
        internal const uint PadPacketsToMaxPacketSizeBit = 0x80;
        internal const uint FdBit = 0x100;
        internal const uint RequestUsbQuirkLpc546XXBit = 0x200;
        internal const uint BtConstExtBit = 0x400;
        internal const uint TerminationBit = 0x800;
        internal const uint BerrReportingBit = 0x1000;
        internal const uint GetStateBit = 0x2000;

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
            return $"ListenOnly: {ListenOnly}, Loopback: {Loopback}, TripleSample: {TripleSample}, OneShot: {OneShot}, HwTimeStamp: {HwTimeStamp}, Identity: {Identity}, UserId: {UserId}, PadPacketsToMaxPacketSize: {PadPacketsToMaxPacketSize}, Fd: {Fd}, RequestUsbQuirkLpc546XX: {RequestUsbQuirkLpc546XX}, BtConstExt: {BtConstExt}, Termination: {Termination}, BerrReporting: {BerrReporting}, GetState: {GetState}";
        }
    }
}
