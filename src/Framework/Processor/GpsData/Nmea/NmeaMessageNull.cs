// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Null NMEA message
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class NmeaMessageNull() : INmeaMessage
    {
    }
}
