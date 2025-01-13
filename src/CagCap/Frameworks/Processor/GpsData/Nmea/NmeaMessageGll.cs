// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using Microsoft.Extensions.Logging;

    internal enum DataStatus
    {
        Valid,
        Invalid
    };

    /// <summary>
    /// Latitude and longitude, with time of position fix and status
    /// </summary>
    internal class NmeaMessageGll(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal double Latitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[0], logger);
        internal LatitudeHemisphere LatitudeHemisphere { get; } = dataVector[1][0] == 'N' ? LatitudeHemisphere.North : LatitudeHemisphere.South;
        internal double Longitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[2], logger);
        internal LongitudeHemisphere LongitudeHemisphere { get; } = dataVector[3][0] == 'E' ? LongitudeHemisphere.East : LongitudeHemisphere.West;
        internal DateTime Time { get; } = NmeaMessageUtil.ParseDateTime(dataVector[4], logger);
        internal DataStatus DataStatus { get; } = dataVector[5][0] == 'A' ? DataStatus.Valid : DataStatus.Invalid;
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[6][0], logger);

        public override string ToString()
        {
            return $"GLL: Latitude: {this.Latitude}, LatitudeHemisphere: {this.LatitudeHemisphere}, Longitude: {this.Longitude}, LongitudeHemisphere: {this.LongitudeHemisphere}, Time: {this.Time}, DataStatus: {this.DataStatus}, PositionMode: {this.PositionMode}";
        }
    }
}
