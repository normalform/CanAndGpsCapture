// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests
{
    using CagCap.Application;
    using CagCap.UI;

    using Moq;

    public class CagCapAppUnitTest
    {
        [Fact]
        public void Start_EnsureUiStart()
        {
            // Arrange
            var mockUserInterface = new Mock<IUserInterface>();
            var cagCapApp = new CagCapApp(mockUserInterface.Object);

            // Act
            cagCapApp.Start();

            // Assert
            mockUserInterface.Verify(ui => ui.Start(), Times.Once);
        }

        [Fact]
        public void Start_EnsureWaitForExit()
        {
            // Arrange
            var mockUserInterface = new Mock<IUserInterface>();
            var cagCapApp = new CagCapApp(mockUserInterface.Object);

            // Act
            cagCapApp.Start();

            // Assert
            mockUserInterface.Verify(ui => ui.WaitForExit(), Times.Once);
        }
    }
}