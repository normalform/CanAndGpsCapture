// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaProtocolTests
    {
        [Theory]
        [InlineData("$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47\r\n", typeof(NmeaMessageGga))]
        [InlineData("$GPVTG,,T,,M,0.088,N,0.163,K,D*22\r\n", typeof(NmeaMessageVtg))]
        [InlineData("$GPRMC,080827.00,A,4734.98631,N,12201.11522,W,0.083,,120125,,,D*68\r\n", typeof(NmeaMessageRmc))]
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
