// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    public class DeviceCanId
    {
        private const uint IdFlagExtended = 0x80000000;
        private const uint IdFlagRtr = 0x40000000;
        private const uint IdFlagError = 0x20000000;
        private const uint IdFlagExtendedMask = 0x1fffffff;
        private const uint IdFlagStandardMask = 0x000007ff;

        private readonly uint rawId;

        public DeviceCanId(uint rawId)
        {
            this.rawId = rawId;
            Id = this.rawId & (Extended ? IdFlagExtendedMask : IdFlagStandardMask);
        }

        public uint Id { get; }

        public bool Extended => (rawId & IdFlagExtended) != 0;

        // Remote Transmission Reuqest
        public bool Rtr => (rawId & IdFlagRtr) != 0;

        public bool HasError => (rawId & IdFlagError) != 0;

        public static DeviceCanId StandardCanId(uint id, bool rtr = false, bool error = false)
        {
            return new DeviceCanId(id | (rtr ? IdFlagRtr : 0) | (error ? IdFlagError : 0));
        }

        public static DeviceCanId ExtendedCanId(uint id, bool rtr = false, bool error = false)
        {
            return new DeviceCanId(id | IdFlagExtended | (rtr ? IdFlagRtr : 0) | (error ? IdFlagError : 0));
        }

        public override string ToString()
        {
            var extendedText = Extended ? ", Extended" : string.Empty;
            var errorText = HasError ? ", Error" : string.Empty;
            var rtrText = Rtr ? ", RTR" : string.Empty;
            return $"Id: {Id}(0x{Id:x}){rtrText}{extendedText}{errorText}, raw ID: 0x{rawId:x}";
        }
    }
}
