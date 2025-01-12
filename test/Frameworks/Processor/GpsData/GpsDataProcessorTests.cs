// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData
{
    using CagCap.Frameworks.Device.UbloxGps;
    using CagCap.Frameworks.Processor.GpsData;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class GpsDataProcessorTests
    {
        private readonly Mock<ILoggerFactory> loggerFactoryMock;
        private readonly Mock<IGpsReceiverDevice> gpsReceiverDeviceMock;

        public GpsDataProcessorTests()
        {
            var loggerMock = new Mock<ILogger>();
            this.loggerFactoryMock = new Mock<ILoggerFactory>();
            this.loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
            this.gpsReceiverDeviceMock = new Mock<IGpsReceiverDevice>();
        }

        [Fact]
        public void Process_ValidData_ShouldProcessCorrectly()
        {
            // Arrange
            string rawData = "$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47\r\n";
            var gpsDataProcessor = new GpsDataProcessor(gpsReceiverDeviceMock.Object, loggerFactoryMock.Object);
            INmeaMessage? dataReceivedCalled = null;
            gpsDataProcessor.DataReceived += (sender, data) => dataReceivedCalled = data;

            // Act
            gpsReceiverDeviceMock.Raise(device => device.DataReceived += null, [gpsReceiverDeviceMock.Object, rawData]);

            // Assert
            Assert.NotNull(dataReceivedCalled);
            Assert.IsType<NmeaMessageGga>(dataReceivedCalled);
        }

        [Fact]
        public void Process_EmptyData()
        {
            // Arrange
            var gpsDataProcessor = new GpsDataProcessor(gpsReceiverDeviceMock.Object, loggerFactoryMock.Object);
            INmeaMessage? dataReceivedCalled = null;
            gpsDataProcessor.DataReceived += (sender, data) => dataReceivedCalled = data;

            // Act
            gpsReceiverDeviceMock.Raise(device => device.DataReceived += null, [gpsReceiverDeviceMock.Object, string.Empty]);

            // Assert
            Assert.Null(dataReceivedCalled);
        }
    }
}
