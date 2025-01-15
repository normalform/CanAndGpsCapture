// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable.Tests
{
    using Canable;
    using Moq;

    public class UsbAccessTests
    {
        [Fact]
        public void GetDeviceGuid_ReturnsExpectedGuids()
        {
            // Arrange
            const int VendorId = 0x1d50;
            const int ProductId = 0x606f;

            var deviceGuid = Guid.NewGuid();
            var expectedGuids = new[] { deviceGuid };
            var usbUtilsMock = new Mock<IUsbUtils>();
            usbUtilsMock.Setup(uu => uu.GetDeviceSymbolicName(VendorId, ProductId)).Returns(expectedGuids);

            // Act
            var result = UsbAccess.GetDeviceGuid(usbUtilsMock.Object);

            // Assert
            Assert.Equal(expectedGuids, result);
            usbUtilsMock.Verify(uu => uu.GetDeviceSymbolicName(VendorId, ProductId), Times.Once);
        }
    }
}
