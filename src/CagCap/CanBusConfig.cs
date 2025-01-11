// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class CanBusConfig
    {
        public required bool Enable { get; set; }
        public required int BitRate { get; set; }
        public required string SamplePoint { get; set; }
        public bool EnableListenOnly { get; set; }
        public bool EnableLoopback { get; set; }
        public bool EnableHwTimestamp { get; set; }
        public bool EnableIdentity { get; set; }
        public bool EnableUserId { get; set; }
        public bool EnablePadPacketsToMaxPacketSize { get; set; }
    }
}
