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

    using System.Linq;

    public class CanBusTransiverTests
    {
        private readonly Mock<ICanableDevice> mockCanableDevice;
        private readonly ICanableDevice canableDevice;
        private readonly Mock<ILoggerFactory> mockLoggerFactory;
        private readonly ILoggerFactory loggerFactory;
        private readonly Mock<ILogger<CanBusTransiver>> mockLogger;

        public CanBusTransiverTests()
        {
            this.mockCanableDevice = new Mock<ICanableDevice>();
            this.canableDevice = this.mockCanableDevice.Object;

            this.mockLoggerFactory = new Mock<ILoggerFactory>();
            this.loggerFactory = this.mockLoggerFactory.Object;
            this.mockLogger = new Mock<ILogger<CanBusTransiver>>();
            this.mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(this.mockLogger.Object);
        }

        [Fact]
        public void SendMessage_WithStandardMessage()
        {
            // Arrange
            var deviceCanId = DeviceCanId.StandardCanId(123u);
            var canBusTransiver = new CanBusTransiver(this.canableDevice, this.loggerFactory);
            var deviceMessage = new DeviceCanMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
            var message = new CanMessage(deviceMessage);

            // Act
            canBusTransiver.SendMessage(message);

            // Assert
            this.VerifySendMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
        }

        [Fact]
        public void SendMessage_WithExtendedMessage()
        {
            // Arrange
            var deviceCanId = DeviceCanId.ExtendedCanId(123u);
            var canBusTransiver = new CanBusTransiver(this.canableDevice, this.loggerFactory);
            var deviceMessage = new DeviceCanMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
            var message = new CanMessage(deviceMessage);

            // Act
            canBusTransiver.SendMessage(message);

            // Assert
            this.VerifySendMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
        }

        [Fact]
        public void DataReceived()
        {
            // Arrange
            var deviceCanId = new DeviceCanId(123u);
            var canBusTransiver = new CanBusTransiver(this.canableDevice, this.loggerFactory);
            var deviceMessage = new DeviceCanMessage(deviceCanId, [0x01, 0x02, 0x03, 0x04]);
            var message = new CanMessage(deviceMessage);

            bool eventRaised = false;
            canBusTransiver.DataReceived += (sender, args) =>
            {
                eventRaised = true;
                Assert.Equal(message.Id, args.Message.Id);
                Assert.Equal(message.Data, args.Message.Data);
            };

            // Act
            this.mockCanableDevice.Raise(x => x.DataReceived += null, new object(), new DeviceCanMessageEventArgs(deviceMessage));

            // Assert
            Assert.True(eventRaised);
        }

        private void VerifySendMessage(DeviceCanId deviceCanId, byte[] expectedData)
        {
            this.mockCanableDevice.Verify(x => x.SendMessage(It.Is<DeviceCanMessage>(msg =>
                msg.Id.Extended == deviceCanId.Extended &&
                msg.Id.Id == deviceCanId.Id &&
                msg.Id.Rtr == deviceCanId.Rtr &&
                msg.Id.HasError == deviceCanId.HasError &&
                msg.Data.SequenceEqual(expectedData)
            )), Times.Once);
        }
    }
}
