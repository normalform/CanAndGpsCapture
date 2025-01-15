// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Tests.Device.Adapter.Can
{
    using CagCap.Framework.Device.Adapter.Can;
    using Canable;

    public class CanIdTests
    {
        [Theory]
        [InlineData(0xffff, 0x7ff, false, false, false)]
        [InlineData(0x123 | 0x80000000, 0x123, true, false, false)]
        [InlineData(0x123 | 0x40000000, 0x123, false, true, false)]
        [InlineData(0x123 | 0x20000000, 0x123, false, false, true)]
        public void CanId(uint rawId, uint expectedId, bool exteded, bool rtr, bool error)
        {
            // Arrange
            var deviceCanId = new DeviceCanId(rawId);

            // Act
            var canId = new CanId(deviceCanId);

            // Assert
            Assert.Equal(expectedId, canId.Id);
            Assert.Equal(exteded, canId.Extended);
            Assert.Equal(rtr, canId.Rtr);
            Assert.Equal(error, canId.HasError);
            Assert.Equal(deviceCanId.ToString(), canId.ToString());
        }

        [Fact]
        public void CanId_Equality_Case0()
        {
            // Arrange
            var canId1 = new CanId(new DeviceCanId(0x123));
            var canId2 = new CanId(new DeviceCanId(0x123));
            var canId3 = new CanId(new DeviceCanId(0x456));

            // Act & Assert
            Assert.Equal(canId1, canId2);
            Assert.NotEqual(canId1, canId3);
        }

        [Fact]
        public void CanId_Equality_Case1()
        {
            // Arrange
            var canId1 = new CanId(new DeviceCanId(0x123));

            // Act & Assert
            Assert.NotEqual(canId1, new object());
        }

        [Fact]
        public void CanId_HashCode()
        {
            // Arrange
            var canId1 = new CanId(new DeviceCanId(0x123));
            var canId2 = new CanId(new DeviceCanId(0x123));
            var canId3 = new CanId(new DeviceCanId(0x456));

            // Act & Assert
            Assert.Equal(canId1.GetHashCode(), canId2.GetHashCode());
            Assert.NotEqual(canId1.GetHashCode(), canId3.GetHashCode());
        }
    }
}
