// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
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
        internal LatitudeHemisphere LatitudeHemisphere { get; } = NmeaMessageUtil.ParseLatitudeHemisphere(dataVector[1], logger);
        internal double Longitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[2], logger);
        internal LongitudeHemisphere LongitudeHemisphere { get; } = NmeaMessageUtil.ParseLongitudeHemisphere(dataVector[3], logger);
        internal DateTime Time { get; } = NmeaMessageUtil.ParseDateTime(dataVector[4], logger);
        internal DataStatus DataStatus { get; } = NmeaMessageUtil.ParseDataStatus(dataVector[5]);
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[6], logger);

        public override string ToString()
        {
            return $"GLL: Latitude: {this.Latitude}, " +
                   $"LatitudeHemisphere: {this.LatitudeHemisphere}, " +
                   $"Longitude: {this.Longitude}, " +
                   $"LongitudeHemisphere: {this.LongitudeHemisphere}, " +
                   $"Time: {this.Time}, " +
                   $"DataStatus: {this.DataStatus}, " +
                   $"PositionMode: {this.PositionMode}";
        }
    }
}
