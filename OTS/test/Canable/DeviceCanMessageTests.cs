// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable.Tests
{
    using Canable;

    public class DeviceCanMessageTests
    {
        [Fact]
        public void CanMessage_Constructor_SetsProperties()
        {
            // Arrange
            var id = new DeviceCanId(0x123);
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            var canMessage = new DeviceCanMessage(id, data, timestamp);

            // Assert
            Assert.Equal(id, canMessage.Id);
            Assert.Equal(data.Length, canMessage.Dlc);
            Assert.Equal(data, canMessage.Data);
            Assert.Equal(timestamp, canMessage.Timestamp);
        }

        [Fact]
        public void CanMessage_Constructor_SetsTimestampToNow()
        {
            // Arrange
            var id = new DeviceCanId(0x123);
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04 };

            // Act
            var canMessage = new DeviceCanMessage(id, data);

            // Assert
            Assert.Equal(id, canMessage.Id);
            Assert.Equal(data.Length, canMessage.Dlc);
            Assert.Equal(data, canMessage.Data);
            Assert.True((DateTime.Now - canMessage.Timestamp).TotalSeconds < 1);
        }

        [Fact]
        public void CanMessage_EmptyData_SetsDlcToZero()
        {
            // Arrange
            var id = new DeviceCanId(0x123);
            var data = Array.Empty<byte>();

            // Act
            var canMessage = new DeviceCanMessage(id, data);

            // Assert
            Assert.Equal(id, canMessage.Id);
            Assert.Equal(0, canMessage.Dlc);
            Assert.Equal(data, canMessage.Data);
            Assert.True((DateTime.Now - canMessage.Timestamp).TotalSeconds < 1);
        }
    }
}
