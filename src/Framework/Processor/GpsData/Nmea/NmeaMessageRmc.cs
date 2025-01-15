// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Recommended Minimum data
    /// </summary>
    internal class NmeaMessageRmc(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal DateTime Time { get; } = NmeaMessageUtil.ParseDateTime(dataVector[0], dataVector[8], logger);
        internal DataStatus Status { get; } = NmeaMessageUtil.ParseDataStatus(dataVector[1]);
        internal double Latitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[2], logger);
        internal LatitudeHemisphere LatitudeHemisphere { get; } = NmeaMessageUtil.ParseLatitudeHemisphere(dataVector[3], logger);
        internal double Longitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[4], logger);
        internal LongitudeHemisphere LongitudeHemisphere { get; } = NmeaMessageUtil.ParseLongitudeHemisphere(dataVector[5], logger);
        internal double SpeedOverGroundKnots { get; } = !string.IsNullOrEmpty(dataVector[6]) ? NmeaMessageUtil.ParseToDouble(dataVector[6], logger) : 0.0;
        internal double CourseOverGroundDeg { get; } = !string.IsNullOrEmpty(dataVector[7]) ? NmeaMessageUtil.ParseToDouble(dataVector[7], logger) : 0.0;
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[11], logger);

        public override string ToString()
        {
            return $"RMC: Time: {Time}, Status: {Status}, Latitude: {Latitude}, LatitudeHemisphere: {LatitudeHemisphere}, Longitude: {Longitude}, LongitudeHemisphere: {LongitudeHemisphere}, SpeedOverGroundKnots: {SpeedOverGroundKnots}, CourseOverGroundDeg: {CourseOverGroundDeg}, PositionMode: {PositionMode}";
        }
    }
}
