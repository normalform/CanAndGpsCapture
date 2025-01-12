// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using Microsoft.Extensions.Logging;

    public enum PositionFixFlag
    {
        NoFix = 0,
        AutonomousGnssFix = 1,
        DifferentialGnssFix = 2,
        EstimatedFix = 6
    };

    /// <summary>
    /// Global positioning system fix data
    /// </summary>
    internal class NmeaMessageGga(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal DateTime Time { get; } = NmeaMessageUtil.ParseDateTime(dataVector[0], logger);
        internal double Latitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[1], logger);
        internal LatitudeHemisphere LatitudeHemisphere { get; } = dataVector[2][0] == 'N' ? LatitudeHemisphere.North : LatitudeHemisphere.South;
        internal double Longitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[3], logger);
        internal LongitudeHemisphere LongitudeHemisphere { get; } = dataVector[4][0] == 'E' ? LongitudeHemisphere.East : LongitudeHemisphere.West;
        internal PositionFixFlag Quality { get; } = ParseQuality(dataVector[5], logger);
        internal int Satellites { get; } = NmeaMessageUtil.ParseToInt(dataVector[6], logger);
        internal double HorizontalDilutionOfPrecision { get; } = NmeaMessageUtil.ParseToDouble(dataVector[7], logger);
        internal double Altitude { get; } = NmeaMessageUtil.ParseToDouble(dataVector[8], logger);
        internal double GeoidSeparation { get; } = NmeaMessageUtil.ParseToDouble(dataVector[10], logger);
        internal int AgeOfDifferentialCorrections { get; } = NmeaMessageUtil.ParseToInt(dataVector[12], logger);
        internal int DifferentialStationId { get; } = NmeaMessageUtil.ParseToInt(dataVector[13], logger);

        internal static PositionFixFlag ParseQuality(string qualityStr, ILogger logger)
        {
            if (int.TryParse(qualityStr, out int quality) && Enum.IsDefined(typeof(PositionFixFlag), quality))
            {
                return (PositionFixFlag)quality;
            }
            else
            {
                logger.LogError("Failed to parse quality: {qualityStr}", qualityStr);
                return PositionFixFlag.NoFix;
            }
        }

        public override string ToString()
        {
            return $"GGA: Time: {this.Time}, Latitude: {this.Latitude}, LatitudeHemisphere: {this.LatitudeHemisphere}, Longitude: {this.Longitude}, LongitudeHemisphere: {this.LongitudeHemisphere}, Quality: {this.Quality}, Satellites: {this.Satellites}, HorizontalDilutionOfPrecision: {this.HorizontalDilutionOfPrecision}, Altitude: {this.Altitude}, GeoidSeparation: {this.GeoidSeparation}, AgeOfDifferentialCorrections: {this.AgeOfDifferentialCorrections}, DifferentialStationId: {this.DifferentialStationId}";
        }
    }
}
