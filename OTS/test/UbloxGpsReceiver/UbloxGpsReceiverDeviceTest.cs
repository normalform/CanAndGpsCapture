// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace UbloxGpsReceiver.Tests
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.IO.Ports;

    public class UbloxGpsReceiverDeviceTest
    {
        private readonly Mock<ILogger> mockLogger;
        private readonly Mock<ILoggerFactory> mockLoggerFactory;

        public UbloxGpsReceiverDeviceTest()
        {
            // Setup common mocks
            this.mockLogger = new Mock<ILogger>();
            this.mockLoggerFactory = new Mock<ILoggerFactory>();
            this.mockLoggerFactory.Setup(m => m.CreateLogger(It.IsAny<string>())).Returns(this.mockLogger.Object);
        }

        [Fact]
        public void Initialize_ShouldOpenSerialPort()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(false);

            // Act
            using var _ = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            // Assert
            mockSerialPort.Verify(sp => sp.Open(), Times.Once);
        }

        [Fact]
        public void Initialize_AlreadyOpened()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(true);

            // Act
            using var _ = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            // Assert
            mockSerialPort.Verify(sp => sp.Open(), Times.Never);
        }

        [Fact]
        public void Shutdown_ShouldCloseSerialPort()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(true);
            using var device = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            // Act
            device.Dispose();

            // Assert
            mockSerialPort.Verify(sp => sp.Close(), Times.Once);
        }

        [Fact]
        public void Shutdown_NotOpended_ShouldNotCloseSerialPort()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(false);
            using var device = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            // Act
            device.Dispose();

            // Assert
            mockSerialPort.Verify(sp => sp.Close(), Times.Never);
        }

        [Fact]
        public void GetData_WriteToOpenedPort()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(true);
            using var device = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            var data = "Test Data";

            // Act
            device.Write(data);

            // Assert
            mockSerialPort.Verify(sp => sp.Write(data), Times.Once);
        }

        [Fact]
        public void GetData_WriteToClosedPort()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(false);
            using var device = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            var data = "Test Data";

            // Act
            device.Write(data);

            // Assert
            mockSerialPort.Verify(sp => sp.Write(data), Times.Never);
        }

        [Fact]
        public void DataReceivedHandler_ShouldInvokeCallback()
        {
            // Arrange
            var mockSerialPort = new Mock<ISerialPort>();
            mockSerialPort.SetupGet(sp => sp.IsOpen).Returns(true);
            mockSerialPort.Setup(sp => sp.ReadExisting()).Returns("Test Data");
            using var device = new UbloxGpsReceiverDevice(mockSerialPort.Object, this.mockLoggerFactory.Object);

            var dataReceived = false;
            device.DataReceived += (sender, args) =>
            {
                Assert.Equal("Test Data", args.Data);
                dataReceived = true;
            };

            // Act
            var dummyEvent = new DataReceivedEventArgs(string.Empty);
            mockSerialPort.Raise(sp => sp.DataReceived += null, mockSerialPort.Object, dummyEvent);

            // Assert
            Assert.True(dataReceived);
        }
    }
}
