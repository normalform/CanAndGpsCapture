// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Processor.GpsData
{
    using CagCap.DomainObject.Device.Gps;
    using CagCap.Framework.Processor.GpsData;
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class GpsDataProcessorTests
    {
        private readonly Mock<ILoggerFactory> loggerFactoryMock;

        public GpsDataProcessorTests()
        {
            var loggerMock = new Mock<ILogger>();
            loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
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

        [Fact]
        public void Process_DataReceivedEvent_ShouldBeInvoked()
        {
            // Arrange
            string rawData = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47\r\n";
            var gpsDataProcessor = new GpsDataProcessor(loggerFactoryMock.Object);

            bool eventInvoked = false;
            gpsDataProcessor.DataReceived += (sender, data) => eventInvoked = true;

            // Act
            gpsDataProcessor.Process(rawData);

            // Assert
            Assert.True(eventInvoked);
        }
    }
}
