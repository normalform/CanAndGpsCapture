// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageGgaTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var ggaData = new[]
            {
                "123519.00",
                "4807.038",
                "N",
                "01131.000",
                "E",
                "1",
                "08",
                "0.9",
                "545.4",
                "M",
                "46.9",
                "M",
                "",
                ""
            };

            // Act
            var ggaMessage = new NmeaMessageGga(ggaData, loggerMock);

            // Assert
            const int Precision = 3;
            var expectedTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 35, 19);
            Assert.Equal(expectedTime, ggaMessage.Time);
            Assert.Equal(4807.038, ggaMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, ggaMessage.LatitudeHemisphere);
            Assert.Equal(01131.000, ggaMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.East, ggaMessage.LongitudeHemisphere);
            Assert.Equal(PositionFixFlag.AutonomousGnssFix, ggaMessage.Quality);
            Assert.Equal(8, ggaMessage.Satellites);
            Assert.Equal(0.9, ggaMessage.HorizontalDilutionOfPrecision, Precision);
            Assert.Equal(545.4, ggaMessage.Altitude, Precision);
            Assert.Equal(46.9, ggaMessage.GeoidSeparation, Precision);
            Assert.Equal(0, ggaMessage.AgeOfDifferentialCorrections);
            Assert.Equal(0, ggaMessage.DifferentialStationId);
        }

        [Fact]
        public void Construction_InvliadSignal()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var ggaData = new[]
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
                ""
            };

            // Act
            var ggaMessage = new NmeaMessageGga(ggaData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(DateTime.MinValue, ggaMessage.Time);
            Assert.Equal(0.0, ggaMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, ggaMessage.LatitudeHemisphere);
            Assert.Equal(0.0, ggaMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.West, ggaMessage.LongitudeHemisphere);
            Assert.Equal(PositionFixFlag.NoFix, ggaMessage.Quality);
            Assert.Equal(0, ggaMessage.Satellites);
            Assert.Equal(0.0, ggaMessage.HorizontalDilutionOfPrecision, Precision);
            Assert.Equal(0.0, ggaMessage.Altitude, Precision);
            Assert.Equal(0.0, ggaMessage.GeoidSeparation, Precision);
            Assert.Equal(0, ggaMessage.AgeOfDifferentialCorrections);
            Assert.Equal(0, ggaMessage.DifferentialStationId);
        }

        [Theory]
        [InlineData("0", PositionFixFlag.NoFix)]
        [InlineData("1", PositionFixFlag.AutonomousGnssFix)]
        [InlineData("2", PositionFixFlag.DifferentialGnssFix)]
        [InlineData("3", PositionFixFlag.NoFix)]
        [InlineData("4", PositionFixFlag.NoFix)]
        [InlineData("5", PositionFixFlag.NoFix)]
        [InlineData("6", PositionFixFlag.EstimatedFix)]
        internal void ParseQuality(string qualityIn, PositionFixFlag expected)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            // Act
            var quality = NmeaMessageGga.ParseQuality(qualityIn, loggerMock);

            // Assert
            Assert.Equal(expected, quality);
        }

        [Fact]
        internal void ToString_Sccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var ggaData = new[]
            {
                "123519.00",
                "4807.038",
                "N",
                "01131.000",
                "E",
                "1",
                "08",
                "0.9",
                "545.4",
                "M",
                "46.9",
                "M",
                "",
                ""
            };
            var ggaMessage = new NmeaMessageGga(ggaData, loggerMock);

            // Act
            var result = ggaMessage.ToString();

            // Assert
            var currentYear = DateTime.Today.Year;
            var currentMonth = DateTime.Today.Month;
            var currentDay = DateTime.Today.Day;
            Assert.Equal($"GGA: Time: {currentMonth}/{currentDay}/{currentYear} 12:35:19 PM, Latitude: 4807.038, LatitudeHemisphere: North, Longitude: 1131, LongitudeHemisphere: East, Quality: AutonomousGnssFix, Satellites: 8, HorizontalDilutionOfPrecision: 0.9, Altitude: 545.4, GeoidSeparation: 46.9, AgeOfDifferentialCorrections: 0, DifferentialStationId: 0", result);
        }
    }
}
