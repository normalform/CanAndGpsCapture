// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData.Nmea
{
    using CagCap.DomainObject;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// GNSS Satellites in View
    /// </summary>
    internal class NmeaMessageGsv(string[] dataVector, ILogger logger) : INmeaMessage
    {
        internal int NumberOfMessages { get; } = NmeaMessageUtil.ParseToInt(dataVector[0], logger);
        internal int MessageNumber { get; } = NmeaMessageUtil.ParseToInt(dataVector[1], logger);
        internal int NumberOfSatellitesInView { get; } = NmeaMessageUtil.ParseToInt(dataVector[2], logger);
        internal SatelliteView[] SatelliteViews { get; } = ParseSatelliteView(dataVector[3..], logger);

        internal static SatelliteView[] ParseSatelliteView(string[] dataVector, ILogger logger)
        {
            const int numberOfFields = 4;

            var satelliteView = new SatelliteView[dataVector.Length / numberOfFields];
            for (int i = 0; i < satelliteView.Length; i++)
            {
                satelliteView[i] = new SatelliteView(
                    NmeaMessageUtil.ParseToInt(dataVector[i * numberOfFields], logger),
                    NmeaMessageUtil.ParseToInt(dataVector[i * numberOfFields + 1], logger),
                    NmeaMessageUtil.ParseToInt(dataVector[i * numberOfFields + 2], logger),
                    NmeaMessageUtil.ParseToInt(dataVector[i * numberOfFields + 3], logger)
                );
            }

            return satelliteView;
        }

        public override string ToString()
        {
            var satelliteViewStrList = new List<string>();
            foreach (var satelliteView in this.SatelliteViews)
            {
                var satelliteViewString = $"[Id: {satelliteView.Id}, Elevation: {satelliteView.Elevation}, Azimuth: {satelliteView.Azimuth}, SignalToNoiseRatio: {satelliteView.SignalToNoiseRatio}]";
                satelliteViewStrList.Add(satelliteViewString);
            }

            var satelliteViewsStr = string.Join(", ", satelliteViewStrList);

            return $"GSV: NumberOfMessages: {this.NumberOfMessages}, MessageNumber: {this.MessageNumber}, NumberOfSatellitesInView: {this.NumberOfSatellitesInView}, SatelliteViews: [{satelliteViewsStr}]";
        }
    }
}
