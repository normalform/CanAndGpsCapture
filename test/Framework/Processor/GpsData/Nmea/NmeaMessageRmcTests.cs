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

    public class NmeaMessageRmcTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var rmcData = new[]
            {
                "080827.00",
                "A",
                "4734.98631",
                "N",
                "12201.11522",
                "W",
                "0.083",
                "",
                "120125",
                "",
                "",
                "D"
            };

            // Act
            var rmcMessage = new NmeaMessageRmc(rmcData, loggerMock);

            // Assert
            const int Precision = 3;
            var expectedTime = new DateTime(2025, 01, 12, 08, 08, 27, DateTimeKind.Utc);
            Assert.Equal(expectedTime, rmcMessage.Time);
            Assert.Equal(DataStatus.Valid, rmcMessage.Status);
            Assert.Equal(4734.98631, rmcMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, rmcMessage.LatitudeHemisphere);
            Assert.Equal(12201.11522, rmcMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.West, rmcMessage.LongitudeHemisphere);
            Assert.Equal(0.083, rmcMessage.SpeedOverGroundKnots, Precision);
            Assert.Equal(PositionFixFlag.DifferentialGnssFix, rmcMessage.PositionMode);
        }

        [Fact]
        public void Construction_InvalidSignal()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var rmcData = new[]
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
                ""
            };

            // Act
            var rmcMessage = new NmeaMessageRmc(rmcData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(DateTime.MinValue, rmcMessage.Time);
            Assert.Equal(DataStatus.Invalid, rmcMessage.Status);
            Assert.Equal(0.0, rmcMessage.Latitude, Precision);
            Assert.Equal(LatitudeHemisphere.North, rmcMessage.LatitudeHemisphere);
            Assert.Equal(0.0, rmcMessage.Longitude, Precision);
            Assert.Equal(LongitudeHemisphere.West, rmcMessage.LongitudeHemisphere);
            Assert.Equal(0.0, rmcMessage.SpeedOverGroundKnots, Precision);
            Assert.Equal(PositionFixFlag.NoFix, rmcMessage.PositionMode);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var rmcData = new[]
            {
                "080827.00",
                "A",
                "4734.98631",
                "N",
                "12201.11522",
                "W",
                "0.083",
                "",
                "120125",
                "",
                "",
                "D"
            };

            // Act
            var rmcMessage = new NmeaMessageRmc(rmcData, loggerMock);

            // Assert
            var expectedString = "RMC: Time: 1/12/2025 8:08:27 AM, Status: Valid, Latitude: 4734.98631, LatitudeHemisphere: North, Longitude: 12201.11522, LongitudeHemisphere: West, SpeedOverGroundKnots: 0.083, CourseOverGroundDeg: 0, PositionMode: DifferentialGnssFix";
            Assert.Equal(expectedString, rmcMessage.ToString());
        }
    }
}
