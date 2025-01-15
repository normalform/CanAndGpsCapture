// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject.Device.Gps
{
    using System.Collections.ObjectModel;

    public record GpsData
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
        public ReadOnlyCollection<SatelliteView> Satellites { get; init; } = new List<SatelliteView>().AsReadOnly();

        public override string ToString()
        {
            var satellites = string.Join(", ", Satellites.Select(s => $"[Id: {s.Id}, Elevation: {s.Elevation}, Azimuth: {s.Azimuth}, SNR: {s.SignalToNoiseRatio}]"));
            return $"Latitude: {Latitude} {LatitudeHemisphere}, Longitude: {Longitude} {LongitudeHemisphere}, Altitude: {Altitude}m, Speed: {Speed}km/h, Course (True): {CourseTrue}°, satellites: {satellites}";
        }
    }
}
