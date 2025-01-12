// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Course over ground and Ground speed
    /// </summary>
    internal class NmeaMessageVtg(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal double CourseOverGroundTrue { get; } = (dataVector[0] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[0], logger) : 0.0;
        internal double CourseOverGroundMagnatic { get; } = (dataVector[2] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[2], logger) : 0.0;
        internal double SpeedOverGroundKnots { get; } = (dataVector[4] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[4], logger) : 0.0;
        internal double SpeedOverGroundKph { get; } = (dataVector[6] != string.Empty) ? NmeaMessageUtil.ParseToDouble(dataVector[6], logger) : 0.0;
        internal PositionFixFlag PositionMode { get; } = NmeaMessageUtil.ParsePositionMode(dataVector[8][0], logger);

        public override string ToString()
        {
            return $"VTG: CourseOverGroundTrue: {this.CourseOverGroundTrue}, CourseOverGroundMagnatic: {this.CourseOverGroundMagnatic}, SpeedOverGroundKnots: {this.SpeedOverGroundKnots}, SpeedOverGroundKph: {this.SpeedOverGroundKph}, PositionMode: {this.PositionMode}";
        }
    }
}
