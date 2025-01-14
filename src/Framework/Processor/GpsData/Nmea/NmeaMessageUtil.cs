// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using Microsoft.Extensions.Logging;

    internal static class NmeaMessageUtil
    {
        internal static DateTime ParseDateTime(string inputString, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                logger.LogError("Failed to parse time for empty string");
                return DateTime.MinValue;
            }

            var formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            if (DateTime.TryParseExact(inputString, "HHmmss.ff", formatProvider, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            else
            {
                logger.LogError("Failed to parse time: {inputString}", inputString);
                return DateTime.MinValue;
            }
        }

        internal static DateTime ParseDateTime(string inputStringHhMmSsFf, string inputStringDdMmYy, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputStringHhMmSsFf) || string.IsNullOrEmpty(inputStringDdMmYy))
            {
                logger.LogError("Failed to parse time: {inputStringHhMmSsFf}", inputStringHhMmSsFf);
                return DateTime.MinValue;
            }

            if (!int.TryParse(inputStringDdMmYy.AsSpan(4, 2), out int year))
            {
                logger.LogError("Failed to parse time: {inputStringDdMmYy}", inputStringDdMmYy);
                return DateTime.MinValue;
            }

            var fullYear = 2000 + year;
            var combinedString = string.Concat(inputStringDdMmYy.AsSpan(0, 4), fullYear.ToString(System.Globalization.CultureInfo.InvariantCulture), inputStringHhMmSsFf);
            var formatProvider = System.Globalization.CultureInfo.InvariantCulture;
            if (DateTime.TryParseExact(combinedString, "ddMMyyyyHHmmss.ff", formatProvider, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            else
            {
                logger.LogError("Failed to parse time: {combinedString}", combinedString);
                return DateTime.MinValue;
            }
        }

        internal static double ParseToDouble(string inputString, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return 0.0;
            }

            if (double.TryParse(inputString, out double output))
            {
                return output;
            }
            else
            {
                logger.LogError("Failed to parse to double: {inputString}", inputString);
                return 0.0;
            }
        }

        internal static int ParseToInt(string inputString, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return 0;
            }

            if (int.TryParse(inputString, out int output))
            {
                return output;
            }
            else
            {
                logger.LogError("Failed to parse to integer: {inputString}", inputString);
                return 0;
            }
        }

        internal static PositionFixFlag ParsePositionMode(string positionModeInput, ILogger logger)
        {
            return positionModeInput switch
            {
                "N" => PositionFixFlag.NoFix,
                "E" => PositionFixFlag.EstimatedFix,
                "A" => PositionFixFlag.AutonomousGnssFix,
                "D" => PositionFixFlag.DifferentialGnssFix,
                _ => PositionFixFlag.NoFix
            };
        }

        internal static LatitudeHemisphere ParseLatitudeHemisphere(string inputString, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                logger.LogWarning("Invalid intput string for LatitudeHemispher: {inputString}", inputString);
                return LatitudeHemisphere.North;
            }

            return inputString[0] == 'N' ? LatitudeHemisphere.North : LatitudeHemisphere.South;
        }

        internal static LongitudeHemisphere ParseLongitudeHemisphere(string inputString, ILogger logger)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                logger.LogWarning("Invalid intput string for LatitudeHemispher: {inputString}", inputString);
                return LongitudeHemisphere.West;
            }

            return inputString[0] == 'E' ? LongitudeHemisphere.East : LongitudeHemisphere.West;
        }

        internal static DataStatus ParseDataStatus(string dataStatusStr)
        {
            return dataStatusStr switch
            {
                "A" => DataStatus.Valid,
                "V" => DataStatus.Invalid,
                _ => DataStatus.Invalid
            };
        }
    }
}
