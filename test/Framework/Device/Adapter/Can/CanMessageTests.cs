// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Device.Adapter.Can
{
    using CagCap.Framework.Device.Adapter.Can;
    using Canable;

    public class CanMessageTests
    {
        [Fact]
        public void CanMessage_Constructor_SetsProperties()
        {
            // Arrange
            var id = new DeviceCanId(0x123);
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var deviceCanMessage = new DeviceCanMessage(id, data, timestamp);

            // Act
            var canMessage = new CanMessage(deviceCanMessage);

            // Assert
            Assert.Equal(new CanId(id), canMessage.Id);
            Assert.Equal(data.Length, canMessage.Dlc);
            Assert.Equal(data, canMessage.Data);
            Assert.Equal(timestamp, canMessage.Timestamp);
            Assert.Equal(deviceCanMessage.ToString(), canMessage.ToString());
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

        [Fact]
        public void CanMessage_HashCode_IsConsistent()
        {
            // Arrange
            var id = new DeviceCanId(0x123);
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
            var timestamp = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var deviceCanMessage = new DeviceCanMessage(id, data, timestamp);
            var canMessage1 = new CanMessage(deviceCanMessage);
            var canMessage2 = new CanMessage(deviceCanMessage);

            // Act
            var hashCode1 = canMessage1.GetHashCode();
            var hashCode2 = canMessage2.GetHashCode();

            // Assert
            Assert.Equal(hashCode1, hashCode2);
        }
    }
}
