// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject
{
    internal record GpsData
    {
        public DateTime Time { get; init; }
        public double Latitude { get; init; }
        public LatitudeHemisphere LatitudeHemisphere { get; init; }
        public double Longitude { get; init; }
        public LongitudeHemisphere LongitudeHemisphere { get; init; }
        public double Altitude { get; init; }
        public double Speed { get; init; }
        public double CourseTrue { get; init; }
        public double CourseMagnetic { get; init; }
        public int NumberOfSatellites { get; init; }
        public SatelliteView[] Satellites { get; init; } = [];

        public override string ToString()
        {
            var satellites = string.Join(", ", Satellites.Select(s => $"[Id: {s.Id}, E: {s.Elevation}, A: {s.Azimuth}, SNR: {s.SignalToNoiseRatio}]"));
            return $"Latitude: {Latitude} {LatitudeHemisphere}, Longitude: {Longitude} {LongitudeHemisphere}, Altitude: {Altitude}m, Speed: {Speed}km/h, Course (True): {CourseTrue}°, satellites: {satellites}";
        }
    }
}
