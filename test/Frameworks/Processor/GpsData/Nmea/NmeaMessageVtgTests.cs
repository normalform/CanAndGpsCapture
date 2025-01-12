// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class NmeaMessageVtgTests
    {
        [Fact]
        public void Construction()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;

            var vtgData = new[]
            {
                "77.52",
                "T",
                "",
                "M",
                "0.088",
                "N",
                "0.163",
                "K",
                "D"
            };

            // Act
            var vtgMessage = new NmeaMessageVtg(vtgData, loggerMock);

            // Assert
            const int Precision = 3;
            Assert.Equal(77.52, vtgMessage.CourseOverGroundTrue, Precision);
            Assert.Equal(0.0, vtgMessage.CourseOverGroundMagnatic, Precision);
            Assert.Equal(0.088, vtgMessage.SpeedOverGroundKnots, Precision);
            Assert.Equal(0.163, vtgMessage.SpeedOverGroundKph, Precision);
            Assert.Equal(PositionFixFlag.DifferentialGnssFix, vtgMessage.PositionMode);
        }

        [Fact]
        public void ToString_Success()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>().Object;
            var vtgData = new[]
            {
                "77.52",
                "T",
                "",
                "M",
                "0.088",
                "N",
                "0.163",
                "K",
                "D"
            };

            // Act
            var vtgMessage = new NmeaMessageVtg(vtgData, loggerMock);
            var vtgString = vtgMessage.ToString();

            // Assert
            Assert.Equal("VTG: CourseOverGroundTrue: 77.52, CourseOverGroundMagnatic: 0, SpeedOverGroundKnots: 0.088, SpeedOverGroundKph: 0.163, PositionMode: DifferentialGnssFix", vtgString);
        }
    }
}
