// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject
{
    internal enum LatitudeHemisphere
    {
        North,
        South
    };

    internal enum LongitudeHemisphere
    {
        East,
        West
    };

    internal record SatelliteView(
        int Id,
        int Elevation,
        int Azimuth,
        int SignalToNoiseRatio
    );
}
