// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using Microsoft.Extensions.Logging;

    internal enum OperationMode
    {
        Manual,
        Automatic
    };

    internal enum NavMode
    {
        NoFix = 1,
        Fix2D = 2,
        Fix3D = 3
    };

    /// <summary>
    /// GNSS dilution of precision and active satellites
    /// </summary>
    internal class NmeaMessageGsa(string[] dataVector, ILogger logger) : INmeaMessage
    {
        private const int SatelliteCount = 12;

        internal OperationMode OperationMode { get; } = ParseOperationMode(dataVector[0][0]);
        internal NavMode NavMode { get; } = ParseNavMode(dataVector[1], logger);
        internal int[] SatelliteNumbers { get; } = ParseSatelliteNumbers(dataVector[2..], logger);
        internal double PositionDilutionOfPrecision { get; } = NmeaMessageUtil.ParseToDouble(dataVector[14], logger);
        internal double HorizontalDilutionOfPrecision { get; } = NmeaMessageUtil.ParseToDouble(dataVector[15], logger);
        internal double VerticalDilutionOfPrecision { get; } = NmeaMessageUtil.ParseToDouble(dataVector[16], logger);

        internal static OperationMode ParseOperationMode(char operationModeChar)
        {
            return operationModeChar == 'A' ? OperationMode.Automatic : OperationMode.Manual;
        }

        internal static NavMode ParseNavMode(string navModeStr, ILogger logger)
        {
            if (int.TryParse(navModeStr, out int navMode) && Enum.IsDefined(typeof(NavMode), navMode))
            {
                return (NavMode)navMode;
            }
            else
            {
                logger.LogError("Failed to parse Nav mode: {navModeStr}", navModeStr);
                return NavMode.NoFix;
            }
        }

        internal static int[] ParseSatelliteNumbers(string[] dataVector, ILogger logger)
        {
            var satelliteNumbers = new List<int>();
            for (int satelliteIndex = 0; satelliteIndex < SatelliteCount; satelliteIndex++)
            {
                var satelliteNumberStr = dataVector[satelliteIndex];
                if (!string.IsNullOrEmpty(satelliteNumberStr))
                {
                    if (int.TryParse(dataVector[satelliteIndex], out int satelliteNumber))
                    {
                        satelliteNumbers.Add(satelliteNumber);
                    }
                    else
                    {
                        logger.LogError("Failed to parse satellite number: {satelliteNumber}", dataVector[satelliteIndex]);
                    }
                }
            }

            return [.. satelliteNumbers];
        }

        public override string ToString()
        {
            var satelliteNumbers = string.Join(", ", this.SatelliteNumbers);
            return $"GSA: OperationMode: {this.OperationMode}, NavMode: {this.NavMode}, SatelliteNumber: {satelliteNumbers}, PositionDilutionOfPrecision: {this.PositionDilutionOfPrecision}, HorizontalDilutionOfPrecision: {this.HorizontalDilutionOfPrecision}, VerticalDilutionOfPrecision: {this.VerticalDilutionOfPrecision}";
        }
    }
}
