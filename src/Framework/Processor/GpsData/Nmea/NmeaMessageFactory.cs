﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
    using Microsoft.Extensions.Logging;

    internal static class NmeaMessageFactory
    {
        public static INmeaMessage Create(string address, string[] dataVector, ILogger logger)
        {
            switch (address)
            {
                case "GPGGA":
                    return new NmeaMessageGga(dataVector, logger);
                case "GPGLL":
                    return new NmeaMessageGll(dataVector, logger);
                case "GPGSA":
                    return new NmeaMessageGsa(dataVector, logger);
                case "GPGSV":
                    return new NmeaMessageGsv(dataVector, logger);
                case "GPRMC":
                    return new NmeaMessageRmc(dataVector, logger);
                case "GPTXT":
                    return new NmeaMessageTxt(dataVector, logger);
                case "GPVTG":
                    return new NmeaMessageVtg(dataVector, logger);

                default:
                    logger.LogWarning("Unsupported message {address}", address);
                    return new NmeaMessageNull();
            }
        }
    }
}
