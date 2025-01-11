// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class GpsReceiverConfig
    {
        public required bool Enable { get; set; }
        public required string Port { get; set; }
        public required int BaudRate { get; set; }
    }
}
