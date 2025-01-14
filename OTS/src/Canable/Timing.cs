// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    public record Timing(
                uint BaseClk,
                uint BitRate,
                uint SamplePoint,
                uint Brp,
                uint PhaseSeg1,
                uint PhaseSeg2)
    {
        public CandleDataStructure.BitTiming BitTiming => new(
            PropSeg: 1,
            PhaseSeg1: PhaseSeg1,
            PhaseSeg2: PhaseSeg2,
            Sjw: 1,
            Brp: Brp);

        public static List<Timing> Timings { get; } =
            [
                // sample point: 50.0%
                new Timing(48000000,   10000, 500, 300, 6, 8),
                new Timing(48000000,   20000, 500, 150, 6, 8),
                new Timing(48000000,   50000, 500,  60, 6, 8),
                new Timing(48000000,   83333, 500,  36, 6, 8),
                new Timing(48000000,  100000, 500,  30, 6, 8),
                new Timing(48000000,  125000, 500,  24, 6, 8),
                new Timing(48000000,  250000, 500,  12, 6, 8),
                new Timing(48000000,  500000, 500,   6, 6, 8),
                new Timing(48000000,  800000, 500,   3, 8, 9),
                new Timing(48000000, 1000000, 500,   3, 6, 8),

                // sample point: 62.5%
                new Timing(48000000,   10000, 625, 300, 8, 6),
                new Timing(48000000,   20000, 625, 150, 8, 6),
                new Timing(48000000,   50000, 625,  60, 8, 6),
                new Timing(48000000,   83333, 625,  36, 8, 6),
                new Timing(48000000,  100000, 625,  30, 8, 6),
                new Timing(48000000,  125000, 625,  24, 8, 6),
                new Timing(48000000,  250000, 625,  12, 8, 6),
                new Timing(48000000,  500000, 625,   6, 8, 6),
                new Timing(48000000,  800000, 600,   4, 7, 6),
                new Timing(48000000, 1000000, 625,   3, 8, 6),

                // sample point: 75.0%
                new Timing(48000000,   10000, 750, 300, 10, 4),
                new Timing(48000000,   20000, 750, 150, 10, 4),
                new Timing(48000000,   50000, 750,  60, 10, 4),
                new Timing(48000000,   83333, 750,  36, 10, 4),
                new Timing(48000000,  100000, 750,  30, 10, 4),
                new Timing(48000000,  125000, 750,  24, 10, 4),
                new Timing(48000000,  250000, 750,  12, 10, 4),
                new Timing(48000000,  500000, 750,   6, 10, 4),
                new Timing(48000000,  800000, 750,   3, 13, 5),
                new Timing(48000000, 1000000, 750,   3, 10, 4),

                // sample point: 87.5%
                new Timing(48000000,   10000, 875, 300, 12, 2),
                new Timing(48000000,   20000, 875, 150, 12, 2),
                new Timing(48000000,   50000, 875,  60, 12, 2),
                new Timing(48000000,   83333, 875,  36, 12, 2),
                new Timing(48000000,  100000, 875,  30, 12, 2),
                new Timing(48000000,  125000, 875,  24, 12, 2),
                new Timing(48000000,  250000, 875,  12, 12, 2),
                new Timing(48000000,  500000, 875,   6, 12, 2),
                new Timing(48000000,  800000, 867,   4, 11, 2),
                new Timing(48000000, 1000000, 875,   3, 12, 2)
            ];
    }
}
