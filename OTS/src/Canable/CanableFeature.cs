// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    public class CanableFeature(uint rawAvaliableFeature)
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

        public bool ListenOnly => (rawAvaliableFeature & ListenOnlyBit) != 0;
        public bool Loopback => (rawAvaliableFeature & LoopbackBit) != 0;
        public bool TripleSample => (rawAvaliableFeature & TripleSampleBit) != 0;
        public bool OneShot => (rawAvaliableFeature & OneShotBit) != 0;
        public bool HwTimeStamp => (rawAvaliableFeature & HwTimeStampBit) != 0;
        public bool Identity => (rawAvaliableFeature & IdentityBit) != 0;
        public bool UserId => (rawAvaliableFeature & UserIdBit) != 0;
        public bool PadPacketsToMaxPacketSize => (rawAvaliableFeature & PadPacketsToMaxPacketSizeBit) != 0;
        public bool Fd => (rawAvaliableFeature & FdBit) != 0;
        public bool RequestUsbQuirkLpc546XX => (rawAvaliableFeature & RequestUsbQuirkLpc546XXBit) != 0;
        public bool BtConstExt => (rawAvaliableFeature & BtConstExtBit) != 0;
        public bool Termination => (rawAvaliableFeature & TerminationBit) != 0;
        public bool BerrReporting => (rawAvaliableFeature & BerrReportingBit) != 0;
        public bool GetState => (rawAvaliableFeature & GetStateBit) != 0;

        public override string ToString()
        {
            return $"ListenOnly: {ListenOnly}, Loopback: {Loopback}, TripleSample: {TripleSample}, OneShot: {OneShot}, HwTimeStamp: {HwTimeStamp}, Identity: {Identity}, UserId: {UserId}, PadPacketsToMaxPacketSize: {PadPacketsToMaxPacketSize}, Fd: {Fd}, RequestUsbQuirkLpc546XX: {RequestUsbQuirkLpc546XX}, BtConstExt: {BtConstExt}, Termination: {Termination}, BerrReporting: {BerrReporting}, GetState: {GetState}";
        }
    }
}
