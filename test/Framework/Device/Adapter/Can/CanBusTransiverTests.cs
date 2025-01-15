// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Device.Adapter.Can
{
    using CagCap.Framework.Device.Adapter.Can;
    using Canable;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CanBusTransiverTests
    {
        [Fact]
        public void SendMessage()
        {
            // Arrange
            var mockCanableDevice = new Mock<ICanableDevice>();
            var canableDevice = mockCanableDevice.Object;

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var loggerFactory = mockLoggerFactory.Object;
            var mockLogger = new Mock<ILogger<CanBusTransiver>>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var deviceCanId = new DeviceCanId(123u);
            var canBusTransiver = new CanBusTransiver(canableDevice, loggerFactory);
            var deviceMessage = new DeviceCanMessage(deviceCanId, [ 0x01, 0x02, 0x03, 0x04 ]);
            var message = new CanMessage(deviceMessage);

            // Act
            canBusTransiver.SendMessage(message);

            // Assert
            mockCanableDevice.Verify(x => x.SendMessage(It.Is<DeviceCanMessage>(msg =>
                msg.Id.Extended == deviceCanId.Extended &&
                msg.Id.Id == deviceCanId.Id &&
                msg.Id.Rtr == deviceCanId.Rtr &&
                msg.Id.HasError == deviceCanId.HasError &&
                msg.Data.SequenceEqual(new byte[] { 0x01, 0x02, 0x03, 0x04 })
            )), Times.Once);
        }

        [Fact]
        public void DataReceived()
        {
            // Arrange
            var mockCanableDevice = new Mock<ICanableDevice>();
            var canableDevice = mockCanableDevice.Object;

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var loggerFactory = mockLoggerFactory.Object;
            var mockLogger = new Mock<ILogger<CanBusTransiver>>();
            mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(mockLogger.Object);

            var deviceCanId = new DeviceCanId(123u);
            var canBusTransiver = new CanBusTransiver(canableDevice, loggerFactory);

            var deviceMessage = new DeviceCanMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
            var message = new CanMessage(deviceMessage);

            bool eventRaised = false;
            canBusTransiver.DataReceived += (sender, args) =>
            {
                eventRaised = true;
                Assert.Equal(message.Id, args.Id);
                Assert.Equal(message.Data, args.Data);
            };

            // Act
            mockCanableDevice.Raise(x => x.DataReceived += null, new object(), deviceMessage);

            // Assert
            Assert.True(eventRaised);
        }
    }
}
