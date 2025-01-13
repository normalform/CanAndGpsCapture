// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageUtilTests
    {
        [Fact]
        public void ParseDateTime_ValidTimeOnlyInput_ReturnsDateTime()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "123456.78";

            // Act
            var result = NmeaMessageUtil.ParseDateTime(inputString, logger.Object);

            // Assert
            Assert.Equal(new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 34, 56, 780), result);
        }

        [Fact]
        public void ParseDateTime_InvalidTimeOnlyInput_ReturnsMinDateTime()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "xxxxxx.yy";

            // Act
            var result = NmeaMessageUtil.ParseDateTime(inputString, logger.Object);

            // Assert
            Assert.Equal(DateTime.MinValue, result);
        }

        [Fact]
        public void ParseDateTime_ValidFullDateTimeInput_ReturnsDateTime()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputTimeString = "123456.78";
            var inputDateString = "010203";

            // Act
            var result = NmeaMessageUtil.ParseDateTime(inputTimeString, inputDateString, logger.Object);

            // Assert
            Assert.Equal(new DateTime(2003, 02, 01, 12, 34, 56, 780), result);
        }

        [Fact]
        public void ParseDateTime_InvalidFullDateTimeInput_ReturnsMinDateTime()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputTimeString = "xxxxxx.yy";
            var inputDateString = "kkkkkk";

            // Act
            var result = NmeaMessageUtil.ParseDateTime(inputTimeString, inputDateString, logger.Object);

            // Assert
            Assert.Equal(DateTime.MinValue, result);
        }

        [Fact]
        public void ParseToDouble_ValidInput_ReturnsDouble()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "123.456";

            // Act
            var result = NmeaMessageUtil.ParseToDouble(inputString, logger.Object);

            // Assert
            const int Precision = 4;
            Assert.Equal(123.456, result, Precision);
        }

        [Fact]
        public void ParseToDouble_InvalidInput_ReturnsNan()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "xxx.xxx";

            // Act
            var result = NmeaMessageUtil.ParseToDouble(inputString, logger.Object);

            // Assert
            Assert.Equal(double.NaN, result);
        }

        [Fact]
        public void ParseToInt_ValidInput_ReturnsDouble()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "123";

            // Act
            var result = NmeaMessageUtil.ParseToInt(inputString, logger.Object);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void ParseToInt_InvalidInput_ReturnsNan()
        {
            // Arrange
            var logger = new Mock<ILogger>();
            var inputString = "xxx";

            // Act
            var result = NmeaMessageUtil.ParseToInt(inputString, logger.Object);

            // Assert
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData('N', PositionFixFlag.NoFix)]
        [InlineData('E', PositionFixFlag.EstimatedFix)]
        [InlineData('A', PositionFixFlag.AutonomousGnssFix)]
        [InlineData('D', PositionFixFlag.DifferentialGnssFix)]
        [InlineData('X', PositionFixFlag.NoFix)]
        internal void ParsePositionMode(char input, PositionFixFlag expected)
        {
            // Arrange
            var logger = new Mock<ILogger>();

            // Act
            var result = NmeaMessageUtil.ParsePositionMode(input, logger.Object);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
