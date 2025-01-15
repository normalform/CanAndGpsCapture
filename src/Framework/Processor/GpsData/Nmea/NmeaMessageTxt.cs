// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
    using Microsoft.Extensions.Logging;

    internal enum NmeaTextMessageType
    {
        Error,
        Warning,
        Notice,
        User
    }

    /// <summary>
    /// Text Transmission
    /// </summary>
    internal class NmeaMessageTxt(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal int NumberOfMessages { get; } = NmeaMessageUtil.ParseToInt(dataVector[0], logger);
        internal int MessageNumber { get; } = NmeaMessageUtil.ParseToInt(dataVector[1], logger);
        internal NmeaTextMessageType TextMessageType { get; } = ParseNmeaTextMessageType(dataVector[2]);
        internal string Message { get; } = dataVector[3];

        internal static NmeaTextMessageType ParseNmeaTextMessageType(string textMessageType)
        {
            if (string.IsNullOrEmpty(textMessageType))
            {
                return NmeaTextMessageType.Error;
            }

            return textMessageType switch
            {
                "00" => NmeaTextMessageType.Error,
                "01" => NmeaTextMessageType.Warning,
                "02" => NmeaTextMessageType.Notice,
                "07" => NmeaTextMessageType.User,
                _ => NmeaTextMessageType.Error,
            };
        }

        public override string ToString()
        {
            return $"TXT: NumberOfMessages: {this.NumberOfMessages}, " +
                   $"MessageNumber: {this.MessageNumber}, " +
                   $"TextMessageType: {this.TextMessageType}, " +
                   $"Message: {this.Message}";
        }
    }
}
