// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Processor.GpsData.Nmea
{
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageGsvTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gsvData = new[]
            {
                "3",
                "1",
                "12",
                "04",
                "07",
                "144",
                "",
                "05",
                "23",
                "300",
                "16",
                "07",
                "73",
                "065",
                "18",
                "08",
                "39",
                "084",
                "10"
            };

            // Act
            var gsvMessage = new NmeaMessageGsv(gsvData, loggerMock);

            // Assert
            Assert.Equal(3, gsvMessage.NumberOfMessages);
            Assert.Equal(1, gsvMessage.MessageNumber);
            Assert.Equal(12, gsvMessage.NumberOfSatellitesInView);
            Assert.Equal(4, gsvMessage.SatelliteViews[0].Id);
            Assert.Equal(7, gsvMessage.SatelliteViews[0].Elevation);
            Assert.Equal(144, gsvMessage.SatelliteViews[0].Azimuth);
            Assert.Equal(0, gsvMessage.SatelliteViews[0].SignalToNoiseRatio);
            Assert.Equal(5, gsvMessage.SatelliteViews[1].Id);
            Assert.Equal(23, gsvMessage.SatelliteViews[1].Elevation);
            Assert.Equal(300, gsvMessage.SatelliteViews[1].Azimuth);
            Assert.Equal(16, gsvMessage.SatelliteViews[1].SignalToNoiseRatio);
            Assert.Equal(7, gsvMessage.SatelliteViews[2].Id);
            Assert.Equal(73, gsvMessage.SatelliteViews[2].Elevation);
            Assert.Equal(65, gsvMessage.SatelliteViews[2].Azimuth);
            Assert.Equal(18, gsvMessage.SatelliteViews[2].SignalToNoiseRatio);
            Assert.Equal(8, gsvMessage.SatelliteViews[3].Id);
            Assert.Equal(39, gsvMessage.SatelliteViews[3].Elevation);
            Assert.Equal(84, gsvMessage.SatelliteViews[3].Azimuth);
            Assert.Equal(10, gsvMessage.SatelliteViews[3].SignalToNoiseRatio);
        }

        [Fact]
        public void Construction_InvalidSignal()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gsvData = new[]
            {
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };

            // Act
            var gsvMessage = new NmeaMessageGsv(gsvData, loggerMock);

            // Assert
            Assert.Equal(0, gsvMessage.NumberOfMessages);
            Assert.Equal(0, gsvMessage.MessageNumber);
            Assert.Equal(0, gsvMessage.NumberOfSatellitesInView);
            Assert.Equal(0, gsvMessage.SatelliteViews[0].Id);
            Assert.Equal(0, gsvMessage.SatelliteViews[0].Elevation);
            Assert.Equal(0, gsvMessage.SatelliteViews[0].Azimuth);
            Assert.Equal(0, gsvMessage.SatelliteViews[0].SignalToNoiseRatio);
            Assert.Equal(0, gsvMessage.SatelliteViews[1].Id);
            Assert.Equal(0, gsvMessage.SatelliteViews[1].Elevation);
            Assert.Equal(0, gsvMessage.SatelliteViews[1].Azimuth);
            Assert.Equal(0, gsvMessage.SatelliteViews[1].SignalToNoiseRatio);
            Assert.Equal(0, gsvMessage.SatelliteViews[2].Id);
            Assert.Equal(0, gsvMessage.SatelliteViews[2].Elevation);
            Assert.Equal(0, gsvMessage.SatelliteViews[2].Azimuth);
            Assert.Equal(0, gsvMessage.SatelliteViews[2].SignalToNoiseRatio);
            Assert.Equal(0, gsvMessage.SatelliteViews[3].Id);
            Assert.Equal(0, gsvMessage.SatelliteViews[3].Elevation);
            Assert.Equal(0, gsvMessage.SatelliteViews[3].Azimuth);
            Assert.Equal(0, gsvMessage.SatelliteViews[3].SignalToNoiseRatio);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var gsvData = new[]
            {
                "3",
                "1",
                "12",
                "04",
                "07",
                "144",
                "",
                "05",
                "23",
                "300",
                "16",
                "07",
                "73",
                "065",
                "18",
                "08",
                "39",
                "084",
                "10"
            };
            var gsvMessage = new NmeaMessageGsv(gsvData, loggerMock);

            // Act
            var result = gsvMessage.ToString();

            // Assert
            Assert.Equal(
                "GSV: NumberOfMessages: 3, MessageNumber: 1, NumberOfSatellitesInView: 12, " +
                "SatelliteViews: [[Id: 4, Elevation: 7, Azimuth: 144, SignalToNoiseRatio: 0], " +
                "[Id: 5, Elevation: 23, Azimuth: 300, SignalToNoiseRatio: 16], " +
                "[Id: 7, Elevation: 73, Azimuth: 65, SignalToNoiseRatio: 18], " +
                "[Id: 8, Elevation: 39, Azimuth: 84, SignalToNoiseRatio: 10]]",
                result);
        }
    }
}
