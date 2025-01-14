// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData
{
    using CagCap.DomainObject;
    using CagCap.DomainObject.Device;
    using CagCap.Frameworks.Processor.GpsData;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class GpsDataProcessorTests
    {
        private readonly Mock<ILoggerFactory> loggerFactoryMock;

        public GpsDataProcessorTests()
        {
            var loggerMock = new Mock<ILogger>();
            this.loggerFactoryMock = new Mock<ILoggerFactory>();
            this.loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        }

        [Fact]
        public void Process_ValidData_ShouldProcessCorrectly()
        {
            // Arrange
            string rawData = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47\r\n";
            var gpsDataProcessor = new GpsDataProcessor(loggerFactoryMock.Object);
            INmeaMessage? dataReceivedCalled = null;
            gpsDataProcessor.DataReceived += (sender, data) => dataReceivedCalled = data;

            // Act
            gpsDataProcessor.Process(rawData);

            // Assert
            Assert.NotNull(dataReceivedCalled);
            Assert.IsType<NmeaMessageGga>(dataReceivedCalled);
        }

        [Fact]
        public void Process_EmptyData()
        {
            // Arrange
            var gpsDataProcessor = new GpsDataProcessor(loggerFactoryMock.Object);
            INmeaMessage? dataReceivedCalled = null;
            gpsDataProcessor.DataReceived += (sender, data) => dataReceivedCalled = data;

            // Act
            gpsDataProcessor.Process(string.Empty);

            // Assert
            Assert.Null(dataReceivedCalled);
        }
    }
}
