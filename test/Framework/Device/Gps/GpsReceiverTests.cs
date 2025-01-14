// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Device.Gps
{
    using CagCap.DomainObject;
    using CagCap.Framework.Device.Gps;
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class GpsReceiverTests
    {
        const int Tolerance = 6;

        [Fact]
        public void GetGpsData()
        {
            // Arrange
            var mockGpsReceiverDevice = new Mock<IGpsReceiverDevice>();
            var mockGpsDataProcessor = new Mock<IGpsDataProcessor>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            int numberOfdataReceivedCalled = 0;
            GpsData? savedGpsData = null;

            var gpsReceiver = new GpsReceiver(mockGpsReceiverDevice.Object, mockGpsDataProcessor.Object, mockLoggerFactory.Object);
            gpsReceiver.DataReceived += (sender, e) =>
            {
                numberOfdataReceivedCalled++;
                savedGpsData = e;
            };

            mockGpsDataProcessor.Setup(p => p.Process("GGA mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGga(mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSA mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsa(mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV0 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(0, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV1 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(1, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV2 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(2, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV3 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(3, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("VTG mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageVtg(mockLogger.Object));
            });

            // Act
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GGA mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSA mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV0 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV1 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV2 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV3 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "VTG mock event");

            var gpsData = gpsReceiver.GpsData;

            // Assert
            // Check the GPS data that came through the event before it received the VTG message.
            Assert.Equal(1, numberOfdataReceivedCalled);
            Assert.NotNull(savedGpsData);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 13, 20, 00, DateTimeKind.Utc), savedGpsData.Time);
            Assert.Equal(4734.99051, savedGpsData.Latitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, savedGpsData.LatitudeHemisphere);
            Assert.Equal(12201.11923, savedGpsData.Longitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, savedGpsData.LatitudeHemisphere);
            Assert.Equal(145.3, savedGpsData.Altitude, Tolerance);
            Assert.Equal(0, savedGpsData.Speed, Tolerance);
            Assert.Equal(0, savedGpsData.CourseTrue);
            Assert.Equal(0, savedGpsData.CourseMagnetic);
            Assert.Equal(5, savedGpsData.NumberOfSatellites);
            Assert.Equal(17, savedGpsData.Satellites[0].Id);
            Assert.Equal(11, savedGpsData.Satellites[1].Id);
            Assert.Equal(48, savedGpsData.Satellites[2].Id);
            Assert.Equal(6, savedGpsData.Satellites[3].Id);
            Assert.Equal(24, savedGpsData.Satellites[4].Id);

            // Check the static GPS data that includes the last VTG message.
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 13, 20, 00, DateTimeKind.Utc), gpsData.Time);
            Assert.Equal(4734.99051, gpsData.Latitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, gpsData.LatitudeHemisphere);
            Assert.Equal(12201.11923, gpsData.Longitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, gpsData.LatitudeHemisphere);
            Assert.Equal(145.3, gpsData.Altitude, Tolerance);
            Assert.Equal(0.719, gpsData.Speed, Tolerance);
            Assert.Equal(0, gpsData.CourseTrue);
            Assert.Equal(0, gpsData.CourseMagnetic);
            Assert.Equal(5, gpsData.NumberOfSatellites);
            Assert.Equal(17, gpsData.Satellites[0].Id);
            Assert.Equal(11, gpsData.Satellites[1].Id);
            Assert.Equal(48, gpsData.Satellites[2].Id);
            Assert.Equal(6, gpsData.Satellites[3].Id);
            Assert.Equal(24, gpsData.Satellites[4].Id);
        }

        [Fact]
        public void TimeoutBehavior()
        {
            // Arrange
            var mockGpsReceiverDevice = new Mock<IGpsReceiverDevice>();
            var mockGpsDataProcessor = new Mock<IGpsDataProcessor>();
            var mockLogger = new Mock<ILogger>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            int numberOfdataReceivedCalled = 0;
            var savedGpsData = new List<GpsData>();

            var gpsReceiver = new GpsReceiver(mockGpsReceiverDevice.Object, mockGpsDataProcessor.Object, mockLoggerFactory.Object);
            gpsReceiver.DataReceived += (sender, e) =>
            {
                numberOfdataReceivedCalled++;
                savedGpsData.Add(e);
            };

            mockGpsDataProcessor.Setup(p => p.Process("GGA mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGga(mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSA mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsa(mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV0 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(0, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV1 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(1, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV2 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(2, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("GSV3 mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageGsv(3, mockLogger.Object));
            });
            mockGpsDataProcessor.Setup(p => p.Process("VTG mock event")).Callback(() =>
            {
                mockGpsDataProcessor.Raise(processor => processor.DataReceived += null, new object(), CreateNmeaMessageVtg(mockLogger.Object));
            });

            // Act
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GGA mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSA mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV0 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV1 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV2 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "GSV3 mock event");
            mockGpsReceiverDevice.Raise(device => device.DataReceived += null, new object(), "VTG mock event");

            // Act
            gpsReceiver.SimulateTimeoutForTesting();
            var gpsData = gpsReceiver.GpsData;

            // Assert
            Assert.Equal(2, numberOfdataReceivedCalled);
            Assert.NotNull(savedGpsData);
            Assert.Equal(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 13, 20, 00, DateTimeKind.Utc), savedGpsData[0].Time);
            Assert.Equal(4734.99051, savedGpsData[0].Latitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, savedGpsData[0].LatitudeHemisphere);
            Assert.Equal(12201.11923, savedGpsData[0].Longitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, savedGpsData[0].LatitudeHemisphere);
            Assert.Equal(145.3, savedGpsData[0].Altitude, Tolerance);
            Assert.Equal(0, savedGpsData[0].Speed, Tolerance);
            Assert.Equal(0, savedGpsData[0].CourseTrue);
            Assert.Equal(0, savedGpsData[0].CourseMagnetic);
            Assert.Equal(5, savedGpsData[0].NumberOfSatellites);
            Assert.Equal(17, savedGpsData[0].Satellites[0].Id);
            Assert.Equal(11, savedGpsData[0].Satellites[1].Id);
            Assert.Equal(48, savedGpsData[0].Satellites[2].Id);
            Assert.Equal(6, savedGpsData[0].Satellites[3].Id);
            Assert.Equal(24, savedGpsData[0].Satellites[4].Id);

            Assert.Equal(DateTime.MinValue, savedGpsData[1].Time);
            Assert.Equal(0.0, savedGpsData[1].Latitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, savedGpsData[1].LatitudeHemisphere);
            Assert.Equal(0.0, savedGpsData[1].Longitude, Tolerance);
            Assert.Equal(LongitudeHemisphere.West, savedGpsData[1].LongitudeHemisphere);
            Assert.Equal(0.0, savedGpsData[1].Altitude, Tolerance);
            Assert.Equal(0.0, savedGpsData[1].Speed, Tolerance);
            Assert.Equal(0.0, savedGpsData[1].CourseTrue, Tolerance);
            Assert.Equal(0.0, savedGpsData[1].CourseMagnetic, Tolerance);
            Assert.Equal(0, savedGpsData[1].NumberOfSatellites);
            Assert.Empty(savedGpsData[1].Satellites);

            Assert.Equal(DateTime.MinValue, gpsData.Time);
            Assert.Equal(0.0, gpsData.Latitude, Tolerance);
            Assert.Equal(LatitudeHemisphere.North, gpsData.LatitudeHemisphere);
            Assert.Equal(0.0, gpsData.Longitude, Tolerance);
            Assert.Equal(LongitudeHemisphere.West, gpsData.LongitudeHemisphere);
            Assert.Equal(0.0, gpsData.Altitude, Tolerance);
            Assert.Equal(0.0, gpsData.Speed, Tolerance);
            Assert.Equal(0.0, gpsData.CourseTrue, Tolerance);
            Assert.Equal(0.0, gpsData.CourseMagnetic, Tolerance);
            Assert.Equal(0, gpsData.NumberOfSatellites);
            Assert.Empty(gpsData.Satellites);
        }

        private static NmeaMessageGga CreateNmeaMessageGga(ILogger logger)
        {
            var ggaData = new[]
            {
                    "231320.00",
                    "4734.99051",
                    "N",
                    "12201.11923",
                    "W",
                    "2",
                    "05",
                    "1.87",
                    "145.3",
                    "M",
                    "-18.7",
                    "M",
                    "",
                    "0000"
                };

            return new NmeaMessageGga(ggaData, logger);
        }

        private static NmeaMessageGsa CreateNmeaMessageGsa(ILogger logger)
        {
            var gsaData = new[]
            {
                    "A",
                    "3",
                    "17",
                    "11",
                    "48",
                    "06",
                    "24",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "4.32",
                    "1.87",
                    "3.89"
                };

            return new NmeaMessageGsa(gsaData, logger);
        }

        private static NmeaMessageGsv CreateNmeaMessageGsv(int index, ILogger logger)
        {
            var gsvData = new string[][]
            {
                    new[]
                    {
                        "4",
                        "1",
                        "16",
                        "01",
                        "05",
                        "029",
                        "",
                        "02",
                        "01",
                        "029",
                        "",
                        "03",
                        "05",
                        "059",
                        "",
                        "06",
                        "39",
                        "147",
                        "07",
                    },
                    new[]
                    {
                        "4",
                        "2",
                        "16",
                        "11",
                        "14",
                        "176",
                        "20",
                        "12",
                        "28",
                        "287",
                        "25",
                        "13",
                        "03",
                        "203",
                        "13",
                        "14",
                        "24",
                        "100",
                        "16",
                    },
                    new[]
                    {
                        "4",
                        "3",
                        "16",
                        "15",
                        "04",
                        "234",
                        "",
                        "17",
                        "47",
                        "057",
                        "22",
                        "19",
                        "81",
                        "074",
                        "09",
                        "22",
                        "46",
                        "091",
                        "18"
                    },
                    new[]
                    {
                        "4",
                        "4",
                        "16",
                        "24",
                        "55",
                        "276",
                        "30",
                        "32",
                        "02",
                        "342",
                        "",
                        "46",
                        "35",
                        "190",
                        "",
                        "48",
                        "35",
                        "184",
                        "30"
                    }
            };

            return new NmeaMessageGsv(gsvData[index], logger);
        }

        private static NmeaMessageVtg CreateNmeaMessageVtg(ILogger logger)
        {
            var vtgData = new[]
            {
                    "",
                    "T",
                    "",
                    "M",
                    "0.388",
                    "N",
                    "0.719",
                    "K",
                    "D"
                };

            return new NmeaMessageVtg(vtgData, logger);
        }
    }
}
