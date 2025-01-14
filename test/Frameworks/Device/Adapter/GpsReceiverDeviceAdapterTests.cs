﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Device.Adapter
{
    using CagCap.Frameworks.Device.Adapter;
    using CagCap.Frameworks.Device.UbloxGps;
    using Moq;

    public class GpsReceiverDeviceAdapterTests
    {
        [Fact]
        public async Task WriteAsync()
        {
            // Arrange
            var mockUbloxGpsReceiverDevice = new Mock<IUbloxGpsReceiverDevice>();
            var adapter = new GpsReceiverDeviceAdapter(mockUbloxGpsReceiverDevice.Object);
            var data = "test data";

            // Act
            await adapter.WriteAsync(data);

            // Assert
            mockUbloxGpsReceiverDevice.Verify(x => x.WriteAsync(data), Times.Once);
        }

        [Fact]
        public void DataReceived()
        {
            // Arrange
            var mockUbloxGpsReceiverDevice = new Mock<IUbloxGpsReceiverDevice>();
            var adapter = new GpsReceiverDeviceAdapter(mockUbloxGpsReceiverDevice.Object);
            var data = "received data";
            var eventRaised = false;

            adapter.DataReceived += (sender, e) =>
            {
                eventRaised = true;
                Assert.Equal(data, e); // Ensure the received data is correct
            };

            // Act
            mockUbloxGpsReceiverDevice.Raise(x => x.DataReceived += null, new object(), data);

            // Assert
            Assert.True(eventRaised);
        }
    }
}
