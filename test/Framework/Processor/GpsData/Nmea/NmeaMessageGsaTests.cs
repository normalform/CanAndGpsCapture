// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Processor.GpsData.Nmea
{
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageGsaTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gsaData = new[]
            {
                "A",
                "3",
                "14",
                "08",
                "30",
                "27",
                "05",
                "09",
                "07",
                "",
                "",
                "",
                "",
                "",
                "2.06",
                "1.03",
                "1.78"
            };

            // Act
            var gsaMessage = new NmeaMessageGsa(gsaData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(OperationMode.Automatic, gsaMessage.OperationMode);
            Assert.Equal(NavMode.Fix3D, gsaMessage.NavMode);
            Assert.Equal([14, 08, 30, 27, 05, 09, 07], gsaMessage.SatelliteNumbers);
            Assert.Equal(2.06, gsaMessage.PositionDilutionOfPrecision, Precision);
            Assert.Equal(1.03, gsaMessage.HorizontalDilutionOfPrecision, Precision);
            Assert.Equal(1.78, gsaMessage.VerticalDilutionOfPrecision, Precision);
        }

        [Fact]
        public void Construction_InvalidSignal()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var gsaData = new[]
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
                ""
            };

            // Act
            var gsaMessage = new NmeaMessageGsa(gsaData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(OperationMode.Automatic, gsaMessage.OperationMode);
            Assert.Equal(NavMode.NoFix, gsaMessage.NavMode);
            Assert.Equal([], gsaMessage.SatelliteNumbers);
            Assert.Equal(0.0, gsaMessage.PositionDilutionOfPrecision, Precision);
            Assert.Equal(0.0, gsaMessage.HorizontalDilutionOfPrecision, Precision);
            Assert.Equal(0.0, gsaMessage.VerticalDilutionOfPrecision, Precision);
        }

        [Theory]
        [InlineData("M", OperationMode.Manual)]
        [InlineData("A", OperationMode.Automatic)]
        [InlineData("", OperationMode.Automatic)]
        internal void ParseOperationMode(string operationModeStr, OperationMode expectedOperationMode)
        {
            // Arrange & Act
            var operationMode = NmeaMessageGsa.ParseOperationMode(operationModeStr);

            // Assert
            Assert.Equal(expectedOperationMode, operationMode);
        }

        [Theory]
        [InlineData("1", NavMode.NoFix)]
        [InlineData("2", NavMode.Fix2D)]
        [InlineData("3", NavMode.Fix3D)]
        internal void ParseNavMode(string navModeStr, NavMode expectedNavMode)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            // Act
            var navMode = NmeaMessageGsa.ParseNavMode(navModeStr, loggerMock);

            // Assert
            Assert.Equal(expectedNavMode, navMode);
        }

        [Fact]
        public void ParseSatelliteNumbers()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var dataVector = new[] { "14", "08", "30", "27", "05", "09", "07", "", "", "", "", "", "", "", "" };

            // Act
            var satelliteNumbers = NmeaMessageGsa.ParseSatelliteNumbers(dataVector, loggerMock);

            // Assert
            Assert.Equal([14, 8, 30, 27, 5, 9, 7], satelliteNumbers);
        }

        [Fact]
        public void ParseSatelliteNumbers_WithInvalidNumber()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var dataVector = new[] { "14", "08", "30", "27", "05", "09", "07", "A", "B", "C", "D", "E", "F", "G", "H" };

            // Act
            var satelliteNumbers = NmeaMessageGsa.ParseSatelliteNumbers(dataVector, loggerMock);

            // Assert
            Assert.Equal([14, 8, 30, 27, 5, 9, 7], satelliteNumbers);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var gsaData = new[]
            {
                "A",
                "3",
                "14",
                "08",
                "30",
                "27",
                "05",
                "09",
                "07",
                "",
                "",
                "",
                "",
                "",
                "2.06",
                "1.03",
                "1.78"
            };
            var gsaMessage = new NmeaMessageGsa(gsaData, loggerMock);

            // Act
            var result = gsaMessage.ToString();

            // Assert
            Assert.Equal("GSA: OperationMode: Automatic, NavMode: Fix3D, SatelliteNumber: 14, 8, 30, 27, 5, 9, 7, PositionDilutionOfPrecision: 2.06, HorizontalDilutionOfPrecision: 1.03, VerticalDilutionOfPrecision: 1.78", result);
        }
    }
}
