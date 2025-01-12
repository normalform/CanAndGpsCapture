// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Recommended Minimum data
    /// </summary>
    internal class NmeaMessageRmc(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal DateTime Time { get; } = NmeaMessageUtil.ParseDateTime(dataVector[0], dataVector[8], logger);
        internal DataStatus Status { get; } = dataVector[1][0] == 'A' ? DataStatus.Valid : DataStatus.Invalid;
        internal double Latitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[2], logger);
        internal LatitudeHemisphere LatitudeHemisphere { get; } = dataVector[3][0] == 'N' ? LatitudeHemisphere.North : LatitudeHemisphere.South;
        internal double Longitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[4], logger);
        internal LongitudeHemisphere LongitudeHemisphere { get; } = dataVector[5][0] == 'E' ? LongitudeHemisphere.East : LongitudeHemisphere.West;
        internal double SpeedOverGroundKnots { get; } = (dataVector[6] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[6], logger) : 0.0;
        internal double CourseOverGroundDeg { get; } = (dataVector[7] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[7], logger) : 0.0;
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[11][0], logger);

        public override string ToString()
        {
            return $"RMC: Time: {this.Time}, Status: {this.Status}, Latitude: {this.Latitude}, LatitudeHemisphere: {this.LatitudeHemisphere}, Longitude: {this.Longitude}, LongitudeHemisphere: {this.LongitudeHemisphere}, SpeedOverGroundKnots: {this.SpeedOverGroundKnots}, CourseOverGroundDeg: {this.CourseOverGroundDeg}, PositionMode: {this.PositionMode}";
        }
    }
}
