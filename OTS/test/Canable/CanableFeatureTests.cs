// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable.Tests
{
    using Canable;

    public class CanableFeatureTests
    {
        [Fact]
        public void CanableFeature_AllFeaturesSet()
        {
            // Arrange
            uint feature = CanableFeature.ListenOnlyBit |
                           CanableFeature.LoopbackBit |
                           CanableFeature.TripleSampleBit |
                           CanableFeature.OneShotBit |
                           CanableFeature.HwTimeStampBit |
                           CanableFeature.IdentityBit |
                           CanableFeature.UserIdBit |
                           CanableFeature.PadPacketsToMaxPacketSizeBit |
                           CanableFeature.FdBit |
                           CanableFeature.RequestUsbQuirkLpc546XXBit |
                           CanableFeature.BtConstExtBit |
                           CanableFeature.TerminationBit |
                           CanableFeature.BerrReportingBit |
                           CanableFeature.GetStateBit;

            // Act
            var canableFeature = new CanableFeature(feature);

            // Assert
            Assert.True(canableFeature.ListenOnly);
            Assert.True(canableFeature.Loopback);
            Assert.True(canableFeature.TripleSample);
            Assert.True(canableFeature.OneShot);
            Assert.True(canableFeature.HwTimeStamp);
            Assert.True(canableFeature.Identity);
            Assert.True(canableFeature.UserId);
            Assert.True(canableFeature.PadPacketsToMaxPacketSize);
            Assert.True(canableFeature.Fd);
            Assert.True(canableFeature.RequestUsbQuirkLpc546XX);
            Assert.True(canableFeature.BtConstExt);
            Assert.True(canableFeature.Termination);
            Assert.True(canableFeature.BerrReporting);
            Assert.True(canableFeature.GetState);
        }

        [Fact]
        public void CanableFeature_NoFeaturesSet()
        {
            // Arrange
            uint feature = 0x00;

            // Act
            var canableFeature = new CanableFeature(feature);

            // Assert
            Assert.False(canableFeature.ListenOnly);
            Assert.False(canableFeature.Loopback);
            Assert.False(canableFeature.HwTimeStamp);
            Assert.False(canableFeature.Identity);
            Assert.False(canableFeature.UserId);
            Assert.False(canableFeature.PadPacketsToMaxPacketSize);
        }

        [Fact]
        public void CanableFeature_SomeFeaturesSet()
        {
            // Arrange
            uint feature = CanableFeature.ListenOnlyBit | CanableFeature.HwTimeStampBit;

            // Act
            var canableFeature = new CanableFeature(feature);

            // Assert
            Assert.True(canableFeature.ListenOnly);
            Assert.False(canableFeature.Loopback);
            Assert.True(canableFeature.HwTimeStamp);
            Assert.False(canableFeature.Identity);
            Assert.False(canableFeature.UserId);
            Assert.False(canableFeature.PadPacketsToMaxPacketSize);
        }

        [Fact]
        public void ToString_ReturnString()
        {
            // Arrange
            uint feature = CanableFeature.ListenOnlyBit | CanableFeature.HwTimeStampBit;

            // Act
            var canableFeature = new CanableFeature(feature);

            // Assert
            Assert.Equal("ListenOnly: True, Loopback: False, TripleSample: False, OneShot: False, HwTimeStamp: True, Identity: False, UserId: False, PadPacketsToMaxPacketSize: False, Fd: False, RequestUsbQuirkLpc546XX: False, BtConstExt: False, Termination: False, BerrReporting: False, GetState: False", canableFeature.ToString());
        }
    }
}
