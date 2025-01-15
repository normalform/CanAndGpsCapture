// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable.Tests
{
    using Canable;

    public class CandleDataStructureTests
    {
        [Fact]
        public void FromByteArray_CandleDeviceConfig_Success()
        {
            // Arrange
            var data = new byte[]
            {
                1, 2, 3, 4, // reserved1, reserved2, reserved3, icount
                5, 0, 0, 0, // sw_version
                6, 0, 0, 0  // hw_version
            };

            // Act
            var result = CandleDataStructure.FromByteArray<CandleDataStructure.CandleDeviceConfig>(data);

            // Assert
            Assert.Equal(1, result.reserved1);
            Assert.Equal(2, result.reserved2);
            Assert.Equal(3, result.reserved3);
            Assert.Equal(4, result.icount);
            Assert.Equal(5u, result.sw_version);
            Assert.Equal(6u, result.hw_version);
        }

        [Fact]
        public void FromByteArray_CandleCapability_Success()
        {
            // Arrange
            var data = new byte[]
            {
                1, 0, 0, 0, // Feature
                2, 0, 0, 0, // FclkCan
                3, 0, 0, 0, // Tseg1Min
                4, 0, 0, 0, // Tseg1Max
                5, 0, 0, 0, // Tseg2Min
                6, 0, 0, 0, // Tseg2Max
                7, 0, 0, 0, // SjwMax
                8, 0, 0, 0, // BrpMin
                9, 0, 0, 0, // BrpMax
                10, 0, 0, 0 // BrpInc
            };

            // Act
            var result = CandleDataStructure.FromByteArray<CandleDataStructure.CandleCapability>(data);

            // Assert
            Assert.Equal(1u, result.Feature);
            Assert.Equal(2u, result.FclkCan);
            Assert.Equal(3u, result.Tseg1Min);
            Assert.Equal(4u, result.Tseg1Max);
            Assert.Equal(5u, result.Tseg2Min);
            Assert.Equal(6u, result.Tseg2Max);
            Assert.Equal(7u, result.SjwMax);
            Assert.Equal(8u, result.BrpMin);
            Assert.Equal(9u, result.BrpMax);
            Assert.Equal(10u, result.BrpInc);
        }

        [Fact]
        public void FromByteArray_CandleDeviceMode_Success()
        {
            // Arrange
            var data = new byte[]
            {
                1, 0, 0, 0, // Mode
                2, 0, 0, 0  // Flags
            };

            // Act
            var result = CandleDataStructure.FromByteArray<CandleDataStructure.CandleDeviceMode>(data);

            // Assert
            Assert.Equal(1u, result.Mode);
            Assert.Equal(2u, result.Flags);
        }

        [Fact]
        public void FromByteArray_CandleDataFrame_Success()
        {
            // Arrange
            var data = new byte[]
            {
                1, 0, 0, 0, // EchoId
                2, 0, 0, 0, // CanId
                3,          // CanDlc
                4,          // Channel
                5,          // Flags
                6,          // Reserved
                7, 8, 9, 10, 11, 12, 13, 14, // Data0 to Data7
                15, 0, 0, 0 // TimestampUs
            };

            // Act
            var result = CandleDataStructure.FromByteArray<CandleDataStructure.CandleDataFrame>(data);

            // Assert
            Assert.Equal(1u, result.EchoId);
            Assert.Equal(2u, result.CanId);
            Assert.Equal(3, result.CanDlc);
            Assert.Equal(4, result.Channel);
            Assert.Equal(5, result.Flags);
            Assert.Equal(6, result.Reserved);
            Assert.Equal(7, result.Data0);
            Assert.Equal(8, result.Data1);
            Assert.Equal(9, result.Data2);
            Assert.Equal(10, result.Data3);
            Assert.Equal(11, result.Data4);
            Assert.Equal(12, result.Data5);
            Assert.Equal(13, result.Data6);
            Assert.Equal(14, result.Data7);
            Assert.Equal(15u, result.TimestampUs);
        }

        [Fact]
        public void FromByteArray_BitTimingStruct_Success()
        {
            // Arrange
            var data = new byte[]
            {
                1, 0, 0, 0, // PropSeg
                2, 0, 0, 0, // PhaseSeg1
                3, 0, 0, 0, // PhaseSeg2
                4, 0, 0, 0, // Sjw
                5, 0, 0, 0, // Brp
            };

            // Act
            var result = CandleDataStructure.FromByteArray<CandleDataStructure.BitTimingStruct>(data);

            // Assert
            Assert.Equal(1u, result.PropSeg);
            Assert.Equal(2u, result.PhaseSeg1);
            Assert.Equal(3u, result.PhaseSeg2);
            Assert.Equal(4u, result.Sjw);
            Assert.Equal(5u, result.Brp);
        }

        [Fact]
        public void UseSmallBuffer_Throw()
        {
            // Arrange
            var smallDataBuffer = new byte[]
            {
                0,
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CandleDataStructure.FromByteArray<CandleDataStructure.BitTimingStruct>(smallDataBuffer));
        }
    }
}
