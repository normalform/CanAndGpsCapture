﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject.Tests
{
    using CagCap.DomainObject.Device.Gps;
    using System.Collections.ObjectModel;

    public class GpsDataTests
    {
        [Fact]
        public void ToStringTest()
        {
            // Arrange
            var gpsData = new GpsData
            {
                Latitude = 51.5074,
                LatitudeHemisphere = LatitudeHemisphere.North,
                Longitude = 0.1278,
                LongitudeHemisphere = LongitudeHemisphere.West,
                Altitude = 0,
                Speed = 0,
                CourseTrue = 0,
                NumberOfSatellites = 0,
                Satellites = new ReadOnlyCollection<SatelliteView>([new SatelliteView(1, 0, 0, 0)])
            };

            // Act
            var result = gpsData.ToString();

            // Assert
            Assert.Equal("Latitude: 51.5074 North, Longitude: 0.1278 West, Altitude: 0m, Speed: 0km/h, Course (True): 0°, satellites: [Id: 1, Elevation: 0, Azimuth: 0, SNR: 0]", result);
        }
    }
}
