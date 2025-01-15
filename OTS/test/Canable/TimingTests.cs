// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable.Tests
{
    using Canable;

    public class TimingTests
    {
        [Fact]
        public void Timing_Constructor_SetsProperties()
        {
            // Arrange
            uint baseClk = 48000000;
            uint bitRate = 1000000;
            uint samplePoint = 750;
            uint brp = 3;
            uint phaseSeg1 = 10;
            uint phaseSeg2 = 4;

            // Act
            var timing = new Timing(baseClk, bitRate, samplePoint, brp, phaseSeg1, phaseSeg2);

            // Assert
            Assert.Equal(baseClk, timing.BaseClk);
            Assert.Equal(bitRate, timing.BitRate);
            Assert.Equal(samplePoint, timing.SamplePoint);
            Assert.Equal(brp, timing.Brp);
            Assert.Equal(phaseSeg1, timing.PhaseSeg1);
            Assert.Equal(phaseSeg2, timing.PhaseSeg2);
        }

        [Fact]
        public void Timing_BitTiming_ReturnsCorrectBitTiming()
        {
            // Arrange
            uint baseClk = 48000000;
            uint bitRate = 1000000;
            uint samplePoint = 750;
            uint brp = 3;
            uint phaseSeg1 = 10;
            uint phaseSeg2 = 4;

            var timing = new Timing(baseClk, bitRate, samplePoint, brp, phaseSeg1, phaseSeg2);

            // Act
            var bitTiming = timing.BitTiming;

            // Assert
            Assert.Equal(1u, bitTiming.PropSeg);
            Assert.Equal(phaseSeg1, bitTiming.PhaseSeg1);
            Assert.Equal(phaseSeg2, bitTiming.PhaseSeg2);
            Assert.Equal(1u, bitTiming.Sjw);
            Assert.Equal(brp, bitTiming.Brp);
        }

        [Fact]
        public void Timing_Timings_ContainsExpectedTimings()
        {
            // Act
            var actualTimings = Timing.Timings;

            // Assert
            Assert.Equal(40, actualTimings.Count);

            Assert.Equal(48000000u, actualTimings[0].BaseClk);
            Assert.Equal(10000u, actualTimings[0].BitRate);
            Assert.Equal(500u, actualTimings[0].SamplePoint);
            Assert.Equal(300u, actualTimings[0].Brp);
            Assert.Equal(6u, actualTimings[0].PhaseSeg1);
            Assert.Equal(8u, actualTimings[0].PhaseSeg2);

            Assert.Equal(48000000u, actualTimings[10].BaseClk);
            Assert.Equal(10000u, actualTimings[10].BitRate);
            Assert.Equal(625u, actualTimings[10].SamplePoint);
            Assert.Equal(300u, actualTimings[10].Brp);
            Assert.Equal(8u, actualTimings[10].PhaseSeg1);
            Assert.Equal(6u, actualTimings[10].PhaseSeg2);

            Assert.Equal(48000000u, actualTimings[20].BaseClk);
            Assert.Equal(10000u, actualTimings[20].BitRate);
            Assert.Equal(750u, actualTimings[20].SamplePoint);
            Assert.Equal(300u, actualTimings[20].Brp);
            Assert.Equal(10u, actualTimings[20].PhaseSeg1);
            Assert.Equal(4u, actualTimings[20].PhaseSeg2);

            Assert.Equal(48000000u, actualTimings[30].BaseClk);
            Assert.Equal(10000u, actualTimings[30].BitRate);
            Assert.Equal(875u, actualTimings[30].SamplePoint);
            Assert.Equal(300u, actualTimings[30].Brp);
            Assert.Equal(12u, actualTimings[30].PhaseSeg1);
            Assert.Equal(2u, actualTimings[30].PhaseSeg2);
        }
    }
}
