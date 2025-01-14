// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Course over ground and Ground speed
    /// </summary>
    internal class NmeaMessageVtg(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal double CourseOverGroundTrue { get; } = NmeaMessageUtil.ParseToDouble(dataVector[0], logger);
        internal double CourseOverGroundMagnatic { get; } = NmeaMessageUtil.ParseToDouble(dataVector[2], logger);
        internal double SpeedOverGroundKnots { get; } = NmeaMessageUtil.ParseToDouble(dataVector[4], logger);
        internal double SpeedOverGroundKph { get; } = NmeaMessageUtil.ParseToDouble(dataVector[6], logger);
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[8], logger);

        public override string ToString()
        {
            return $"VTG: CourseOverGroundTrue: {this.CourseOverGroundTrue}, CourseOverGroundMagnatic: {this.CourseOverGroundMagnatic}, SpeedOverGroundKnots: {this.SpeedOverGroundKnots}, SpeedOverGroundKph: {this.SpeedOverGroundKph}, PositionMode: {this.PositionMode}";
        }
    }
}
