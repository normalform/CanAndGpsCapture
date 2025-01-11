// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Device.Canable
{
    using CagCap.Frameworks.Device.Canable;

    public class CanIdTests
    {
        [Theory]
        [InlineData(0xffff, 0x7ff, false, false, false)]
        [InlineData(0x123 | 0x80000000, 0x123, true, false, false)]
        [InlineData(0x123 | 0x40000000, 0x123, false, true, false)]
        [InlineData(0x123 | 0x20000000, 0x123, false, false, true)]
        public void CanId_Constructor_SetsRawId(uint rawId, uint expectedId, bool exteded, bool rtr, bool error)
        {
            // Act
            var canId = new CanId(rawId);

            // Assert
            Assert.Equal(expectedId, canId.Id);
            Assert.Equal(exteded, canId.Extended);
            Assert.Equal(rtr, canId.Rtr);
            Assert.Equal(error, canId.Error);
        }

        [Fact]
        public void CanId_StandardCanId_CreatesStandardId()
        {
            // Arrange
            uint id = 0x123;

            // Act
            var canId = CanId.StandardCanId(id);

            // Assert
            Assert.Equal(id, canId.Id);
            Assert.False(canId.Extended);
        }

        [Fact]
        public void CanId_ExtendedCanId_CreatesExtendedId()
        {
            // Arrange
            uint id = 0x123;

            // Act
            var canId = CanId.ExtendedCanId(id);

            // Assert
            Assert.Equal(id, canId.Id);
            Assert.True(canId.Extended);
        }

        [Fact]
        public void CanId_StandardCanId_WithRtrAndError()
        {
            // Arrange
            uint id = 0x123;

            // Act
            var canId = CanId.StandardCanId(id, rtr: true, error: true);

            // Assert
            Assert.Equal(id, canId.Id);
            Assert.False(canId.Extended);
            Assert.True(canId.Rtr);
            Assert.True(canId.Error);
        }

        [Fact]
        public void CanId_ExtendedCanId_WithRtrAndError()
        {
            // Arrange
            uint id = 0x123;

            // Act
            var canId = CanId.ExtendedCanId(id, rtr: true, error: true);

            // Assert
            Assert.Equal(id, canId.Id);
            Assert.True(canId.Extended);
            Assert.True(canId.Rtr);
            Assert.True(canId.Error);
        }

        [Fact]
        public void CanId_ToStringCase0_ReturnsCorrectString()
        {
            // Arrange
            uint id = 0x123;
            var canId = CanId.ExtendedCanId(id, rtr: true, error: true);

            // Act
            var result = canId.ToString();

            // Assert
            Assert.Equal("Id: 291(0x123), RTR, Extended, Error, raw ID: 0xe0000123", result);
        }

        [Fact]
        public void CanId_ToStringCase1_ReturnsCorrectString()
        {
            // Arrange
            uint id = 0x123;
            var canId = CanId.StandardCanId(id);

            // Act
            var result = canId.ToString();

            // Assert
            Assert.Equal("Id: 291(0x123), raw ID: 0x123", result);
        }
    }
}
