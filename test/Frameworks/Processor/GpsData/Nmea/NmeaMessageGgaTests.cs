// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageGgaTests
    {
        [Theory]
        [InlineData("0", PositionFixFlag.NoFix)]
        [InlineData("1", PositionFixFlag.AutonomousGnssFix)]
        [InlineData("2", PositionFixFlag.DifferentialGnssFix)]
        [InlineData("3", PositionFixFlag.NoFix)]
        [InlineData("4", PositionFixFlag.NoFix)]
        [InlineData("5", PositionFixFlag.NoFix)]
        [InlineData("6", PositionFixFlag.EstimatedFix)]
        public void ParseQuality(string qualityIn, PositionFixFlag expected)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            // Act
            var quality = NmeaMessageGga.ParseQuality(qualityIn, loggerMock);

            // Assert
            Assert.Equal(expected, quality);
        }
    }
}
