﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Device.Adapter.Gps
{
    using CagCap.Framework.Device.Adapter.Gps;
    using Moq;
    using UbloxGpsReceiver;

    public class GpsReceiverDeviceTests
    {
        [Fact]
        public void WriteAsync()
        {
            // Arrange
            var mockUbloxGpsReceiverDevice = new Mock<IUbloxGpsReceiverDevice>();
            var adapter = new GpsReceiverDevice(mockUbloxGpsReceiverDevice.Object);
            var data = "test data";

            // Act
            adapter.Write(data);

            // Assert
            mockUbloxGpsReceiverDevice.Verify(x => x.Write(data), Times.Once);
        }

        [Fact]
        public void DataReceived()
        {
            // Arrange
            var mockUbloxGpsReceiverDevice = new Mock<IUbloxGpsReceiverDevice>();
            var adapter = new GpsReceiverDevice(mockUbloxGpsReceiverDevice.Object);
            var data = "received data";
            var eventRaised = false;

            adapter.DataReceived += (sender, e) =>
            {
                eventRaised = true;
                Assert.Equal(data, e.Data);
            };

            // Act
            mockUbloxGpsReceiverDevice.Raise(x => x.DataReceived += null, new object(), new DataReceivedEventArgs(data));

            // Assert
            Assert.True(eventRaised);
        }
    }
}
