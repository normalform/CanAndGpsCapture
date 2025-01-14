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

    public class NmeaMessageGllTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gllData = new[]
            {
                "4916.45",
                "N",
                "12311.12",
                "W",
                "225444.00",
                "A",
                "A"
            };

            // Act
            var gllMessage = new NmeaMessageGll(gllData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(4916.45, gllMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, gllMessage.LatitudeHemisphere);
            Assert.Equal(12311.12, gllMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.West, gllMessage.LongitudeHemisphere);
            var expectedTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 22, 54, 44, DateTimeKind.Utc);
            Assert.Equal(expectedTime, gllMessage.Time);
            Assert.Equal(DataStatus.Valid, gllMessage.DataStatus);
            Assert.Equal(PositionFixFlag.AutonomousGnssFix, gllMessage.PositionMode);
        }

        [Fact]
        public void Construction_InvalidSignal()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gllData = new[]
            {
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };

            // Act
            var gllMessage = new NmeaMessageGll(gllData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(0.0, gllMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, gllMessage.LatitudeHemisphere);
            Assert.Equal(0.0, gllMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.West, gllMessage.LongitudeHemisphere);
            Assert.Equal(DateTime.MinValue, gllMessage.Time);
            Assert.Equal(DataStatus.Invalid, gllMessage.DataStatus);
            Assert.Equal(PositionFixFlag.NoFix, gllMessage.PositionMode);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var gllData = new[]
            {
                "4916.45",
                "N",
                "12311.12",
                "W",
                "225444.00",
                "A",
                "A"
            };
            var gllMessage = new NmeaMessageGll(gllData, loggerMock);

            // Act
            var result = gllMessage.ToString();

            // Assert
            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;
            var currentDay = DateTime.Today.Day;
            Assert.Equal($"GLL: Latitude: 4916.45, LatitudeHemisphere: North, Longitude: 12311.12, LongitudeHemisphere: West, Time: {currentMonth}/{currentDay}/{currentYear} 10:54:44 PM, DataStatus: Valid, PositionMode: AutonomousGnssFix", result);
        }
    }
}
