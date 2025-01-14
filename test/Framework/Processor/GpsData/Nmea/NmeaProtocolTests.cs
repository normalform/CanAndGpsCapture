// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaProtocolTests
    {
        [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47\r\n", typeof(NmeaMessageGga))]
        [InlineData("$GPGLL,4734.96497,N,12201.14301,W,183101.00,A,A*77\r\n", typeof(NmeaMessageGll))]
        [InlineData("$GPGSA,A,3,14,08,30,27,05,09,07,,,,,,2.06,1.03,1.78*0A\r\n", typeof(NmeaMessageGsa))]
        [InlineData("$GPGSV,3,1,12,04,07,144,,05,23,300,16,07,73,065,18,08,39,084,10*7C\r\n", typeof(NmeaMessageGsv))]
        [InlineData("$GPRMC,080827.00,A,4734.98631,N,12201.11522,W,0.083,,120125,,,D*68\r\n", typeof(NmeaMessageRmc))]
        [InlineData("$GPTXT,01,01,02,HW  UBX-G70xx   00070000 *77\r\n", typeof(NmeaMessageTxt))]
        [InlineData("$GPVTG,,T,,M,0.088,N,0.163,K,D*22\r\n", typeof(NmeaMessageVtg))]
        [InlineData("$GPNOP,,T,,M,0.088,N,0.163,K,D*36\r\n", typeof(NmeaMessageNull))]
        public void Process_Gsa(string inputNmeaString, Type expectedType)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var nmeaProtocol = new NmeaProtocol(loggerMock);

            // Act
            INmeaMessage? nmeaMessage = null;
            foreach (var messageChar in inputNmeaString)
            {
                nmeaMessage = nmeaProtocol.Process(messageChar);
            }

            // Assert
            Assert.NotNull(nmeaMessage);
            Assert.IsType(expectedType, nmeaMessage);
        }
    }
}
