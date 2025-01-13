// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagcapTests.Frameworks.Device.Canable
{
    using CagCap;
    using CagCap.Frameworks.Device.Canable;
    using Microsoft.Extensions.Logging;
    using Moq;

    public class CanableDeviceTests
    {
        private readonly CanBusConfig canBusConfig;
        private readonly ILogger logger;
        private readonly ILoggerFactory loggerFactory;

        public CanableDeviceTests()
        {
            this.canBusConfig = new CanBusConfig
            {
                Enable = true,
                BitRate = 500000,
                SamplePoint = "50.0"
            };
            this.logger = new Mock<ILogger>().Object;
            var loggerFactoryMock = new Mock<ILoggerFactory>();
            loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(this.logger);
            this.loggerFactory = loggerFactoryMock.Object;
        }

        [Fact]
        public void CanableDevice_Constructor_Success()
        {
            // Arrange
            var usbAccessMock = new Mock<IUsbAccess>(MockBehavior.Strict);
            var mockSequence = new MockSequence();

            var candleDeviceModeStop = new CandleDataStructure.CandleDeviceMode
            {
                Mode = 0,
                Flags = 0
            };

            var candleDeviceModeStart = new CandleDataStructure.CandleDeviceMode
            {
                Mode = 1,
                Flags = (
                    CanableFeature.ListenOnlyBit |
                    CanableFeature.LoopbackBit |
                    CanableFeature.HwTimeStampBit |
                    CanableFeature.IdentityBit |
                    CanableFeature.UserIdBit |
                    CanableFeature.PadPacketsToMaxPacketSizeBit)
            };

            var bitTiming = new CandleDataStructure.BitTimingStruct
            {
                PropSeg = 1,
                PhaseSeg1 = 6,
                PhaseSeg2 = 8,
                Sjw = 1,
                Brp = 6
            };

            var candleCapability = new CandleDataStructure.CandleCapability
            {
                Feature = (
                    CanableFeature.ListenOnlyBit |
                    CanableFeature.LoopbackBit |
                    CanableFeature.HwTimeStampBit |
                    CanableFeature.IdentityBit |
                    CanableFeature.UserIdBit |
                    CanableFeature.PadPacketsToMaxPacketSizeBit),
                FclkCan = 48000000
            };

            var candleDeviceConfig = new CandleDataStructure.CandleDeviceConfig();
            canBusConfig.EnableListenOnly = true;
            canBusConfig.EnableLoopback = true;
            canBusConfig.EnableHwTimestamp = true;
            canBusConfig.EnableIdentity = true;
            canBusConfig.EnableUserId = true;
            canBusConfig.EnablePadPacketsToMaxPacketSize = true;

            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.UsbControlMessageSet(CanRequest.Mode, 0, 0, It.Is<CandleDataStructure.CandleDeviceMode>(m =>
                m.Mode == candleDeviceModeStop.Mode &&
                m.Flags == candleDeviceModeStop.Flags
            )));
            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.UsbControlMessageGet<CandleDataStructure.CandleCapability>(CanRequest.BitTimingConstants, 0, 0))
                .Returns(candleCapability);
            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.UsbControlMessageGet<CandleDataStructure.CandleDeviceConfig>(CanRequest.DeviceConfig, 0, 0))
                .Returns(candleDeviceConfig);
            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.UsbControlMessageSet(CanRequest.BitTiming, 0, 0, It.Is<CandleDataStructure.BitTimingStruct>(m =>
                m.PropSeg == bitTiming.PropSeg &&
                m.PhaseSeg1 == bitTiming.PhaseSeg1 &&
                m.PhaseSeg2 == bitTiming.PhaseSeg2 &&
                m.Sjw == bitTiming.Sjw &&
                m.Brp == bitTiming.Brp
            )));
            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.UsbControlMessageSet(CanRequest.Mode, 0, 0, It.Is<CandleDataStructure.CandleDeviceMode>(m =>
                m.Mode == candleDeviceModeStart.Mode &&
                m.Flags == candleDeviceModeStart.Flags
            )));
            usbAccessMock.InSequence(mockSequence).Setup(ua => ua.StartReceive());

            // Act
            var canableDevice = new CanableDevice(usbAccessMock.Object, this.canBusConfig, this.loggerFactory);

            // Assert
            Assert.NotNull(canableDevice);
            usbAccessMock.Verify(ua => ua.UsbControlMessageSet(CanRequest.Mode, 0, 0, It.Is<CandleDataStructure.CandleDeviceMode>(m =>
                m.Mode == candleDeviceModeStop.Mode &&
                m.Flags == candleDeviceModeStop.Flags
            )), Times.Once);
            usbAccessMock.Verify(ua => ua.UsbControlMessageGet<CandleDataStructure.CandleCapability>(CanRequest.BitTimingConstants, 0, 0), Times.Once);
            usbAccessMock.Verify(ua => ua.UsbControlMessageGet<CandleDataStructure.CandleDeviceConfig>(CanRequest.DeviceConfig, 0, 0), Times.Once);
            usbAccessMock.Verify(ua => ua.UsbControlMessageSet(CanRequest.BitTiming, 0, 0, It.Is<CandleDataStructure.BitTimingStruct>(m =>
                m.PropSeg == bitTiming.PropSeg &&
                m.PhaseSeg1 == bitTiming.PhaseSeg1 &&
                m.PhaseSeg2 == bitTiming.PhaseSeg2 &&
                m.Sjw == bitTiming.Sjw &&
                m.Brp == bitTiming.Brp
            )), Times.Once);
            usbAccessMock.Verify(ua => ua.UsbControlMessageSet(CanRequest.Mode, 0, 0, It.Is<CandleDataStructure.CandleDeviceMode>(m =>
                m.Mode == candleDeviceModeStart.Mode &&
                m.Flags == candleDeviceModeStart.Flags
            )), Times.Once);
            usbAccessMock.Verify(ua => ua.StartReceive(), Times.Once);
        }

        [Fact]
        public void CanableDevice_SendMessage()
        {
            // Arrange
            var usbAccessMock = new Mock<IUsbAccess>();
            var canableDevice = new CanableDevice(usbAccessMock.Object, this.canBusConfig, this.loggerFactory);
            var canMessage = new CanMessage(new CanId(0x123), [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08]);

            // Act
            canableDevice.SendMessage(canMessage);

            // Assert
            var expectedFrame = new CandleDataStructure.CandleDataFrame
            {
                EchoId = 0,
                CanId = 0x123,
                CanDlc = 8,
                Channel = 0,
                Flags = 0,
                Data0 = 0x01,
                Data1 = 0x02,
                Data2 = 0x03,
                Data3 = 0x04,
                Data4 = 0x05,
                Data5 = 0x06,
                Data6 = 0x07,
                Data7 = 0x08,
                TimestampUs = 0
            };

            usbAccessMock.Verify(ua => ua.SendFrame(0, It.Is<CandleDataStructure.CandleDataFrame>(frame =>
                frame.EchoId == expectedFrame.EchoId &&
                frame.CanId == expectedFrame.CanId &&
                frame.CanDlc == expectedFrame.CanDlc &&
                frame.Channel == expectedFrame.Channel &&
                frame.Flags == expectedFrame.Flags &&
                frame.Data0 == expectedFrame.Data0 &&
                frame.Data1 == expectedFrame.Data1 &&
                frame.Data2 == expectedFrame.Data2 &&
                frame.Data3 == expectedFrame.Data3 &&
                frame.Data4 == expectedFrame.Data4 &&
                frame.Data5 == expectedFrame.Data5 &&
                frame.Data6 == expectedFrame.Data6 &&
                frame.Data7 == expectedFrame.Data7 &&
                frame.TimestampUs == expectedFrame.TimestampUs
            )), Times.Once);
        }

        [Theory]
        [InlineData("50.0", 500)]
        [InlineData("50.0%", 500)]
        [InlineData("75.0%", 750)]
        public void GetSamplePoint_ValidInput_Success(string samplePointString, int expectedSamplePoint)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            // Act
            var samplePoint = CanableDevice.GetSamplePoint(samplePointString, this.logger);

            // Assert
            Assert.Equal(expectedSamplePoint, samplePoint);
        }

        [Theory]
        [InlineData("-50", typeof(FormatException))]
        [InlineData("101", typeof(FormatException))]
        [InlineData("invalid", typeof(FormatException))]
        [InlineData("", typeof(FormatException))]
        [InlineData(null, typeof(NullReferenceException))]
        public void GetSamplePoint_InValidInput_Throw(string samplePointString, Type expectedExceptionType)
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();

            // Act & Assert
            Assert.Throws(expectedExceptionType, () => CanableDevice.GetSamplePoint(samplePointString, this.logger));
        }
    }
}
