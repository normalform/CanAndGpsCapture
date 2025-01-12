// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using Microsoft.Extensions.Logging;

    internal static class NmeaMessageUtil
    {
        internal static DateTime ParseDateTime(string inputString, ILogger logger)
        {
            if (DateTime.TryParseExact(inputString, "HHmmss.ff", null, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            else
            {
                logger.LogError("Failed to parse time: {timeStr}", inputString);
                return DateTime.MinValue;
            }
        }

        internal static DateTime ParseDateTime(string inputStringHhMmSsFf, string inputStringDdMmYy, ILogger logger)
        {
            var combinedString = inputStringDdMmYy + inputStringHhMmSsFf;
            if (DateTime.TryParseExact(combinedString, "ddMMyyHHmmss.ff", null, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            else
            {
                logger.LogError("Failed to parse time: {timeStr}", combinedString);
                return DateTime.MinValue;
            }
        }

        internal static DateTime ParseDateTimeDdMmYy(string inputString, ILogger logger)
        {
            if (DateTime.TryParseExact(inputString, "ddMMyy", null, System.Globalization.DateTimeStyles.None, out DateTime time))
            {
                return time;
            }
            else
            {
                logger.LogError("Failed to parse time: {timeStr}", inputString);
                return DateTime.MinValue;
            }
        }

        internal static double ParseToDouble(string inputString, ILogger logger)
        {
            if (double.TryParse(inputString, out double output))
            {
                return output;
            }
            else
            {
                logger.LogError("Failed to parse to double: {inputString}", inputString);
                return double.NaN;
            }
        }


        internal static int ParseToInt(string inputString, ILogger logger)
        {
            if (inputString == string.Empty)
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

        internal static PositionFixFlag ParsePositionMode(char positionModeChar, ILogger logger)
        {
            switch (positionModeChar)
            {
                case 'N':
                    return PositionFixFlag.NoFix;
                case 'E':
                    return PositionFixFlag.EstimatedFix;
                case 'A':
                    return PositionFixFlag.AutonomousGnssFix;
                case 'D':
                    return PositionFixFlag.DifferentialGnssFix;
                default:
                    logger.LogError("Failed to parse position mode: {positionModeChar}", positionModeChar);
                    return PositionFixFlag.NoFix;
            }
        }
    }
}
