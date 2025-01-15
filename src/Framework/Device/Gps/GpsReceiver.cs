// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Gps
{
    using CagCap.DomainObject.Device.Gps;
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Timers;

    public class GpsReceiver : IGpsReceiver, IDisposable
    {
        private const int Timeout = 3000; // 3 seconds

        private readonly ILogger logger;
        private readonly Timer timeoutTimer;
        private readonly Dictionary<int, SatelliteView> satelliteViews = [];
        private bool disposed;

        private DateTime time = DateTime.MinValue;
        private double latitude;
        private LatitudeHemisphere latitudeHemisphere = LatitudeHemisphere.North;
        private double longitude;
        private LongitudeHemisphere longitudeHemisphere = LongitudeHemisphere.West;
        private double altitude;
        private double speed;
        private double courseTrue;
        private double courseMagnetic;
        private ReadOnlyCollection<SatelliteView> satellites = new List<SatelliteView>().AsReadOnly();
        private int[] satelliteNumbers = [];
        private int numberOfSatellitesInView;
        private bool ggaDataIsAvailable;

        public GpsData GpsData => CreateGpsData();

        public event EventHandler<GpsDataEventArgs> DataReceived = delegate { };

        public GpsReceiver(IGpsReceiverDevice gpsReceiverDevice, IGpsDataProcessor gpsReceiverProcessor, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("GpsReceiver");

            timeoutTimer = new Timer(Timeout)
            {
                AutoReset = false
            };
            timeoutTimer.Elapsed += OnTimeout;

            gpsReceiverProcessor.DataReceived += OnDataReceived;
            gpsReceiverDevice.DataReceived += (sender, data) =>
            {
                gpsReceiverProcessor.Process(data.Data);
            };
        }

        internal void SimulateTimeoutForTesting()
        {
            OnTimeout(this, null);
        }

        private GpsData CreateGpsData() => new()
        {
            Time = time,
            Latitude = latitude,
            LatitudeHemisphere = latitudeHemisphere,
            Longitude = longitude,
            LongitudeHemisphere = longitudeHemisphere,
            Altitude = altitude,
            Speed = speed,
            CourseTrue = courseTrue,
            CourseMagnetic = courseMagnetic,
            NumberOfSatellites = satellites.Count,
            Satellites = satellites
        };

        private void OnTimeout(object? sender, ElapsedEventArgs? e)
        {
            ResetData();
            this.DataReceived.Invoke(this, new GpsDataEventArgs(GpsData));
        }

        private void OnDataReceived(object? sender, NmeaMessageEventArgs message)
        {
            logger.LogDebug("Data received: {message}", message);
            timeoutTimer.Stop(); // Stop the timer if data is received

            switch (message.Message)
            {
                case NmeaMessageGga gga:
                    UpdateGgaData(gga);
                    break;
                case NmeaMessageGsa gsa:
                    UpdateGsaData(gsa);
                    break;
                case NmeaMessageGsv gsv:
                    UpdateGsvData(gsv);
                    break;
                case NmeaMessageVtg vtg:
                    UpdateVtgData(vtg);
                    break;
            }

            if (ggaDataIsAvailable && satellites.Count > 1)
            {
                ggaDataIsAvailable = false;
                var gpsData = CreateGpsData();
                DataReceived.Invoke(this, new GpsDataEventArgs(gpsData));
                logger.LogDebug("GPS data: {gpsData}", gpsData);
            }

            timeoutTimer.Start();
        }

        private void UpdateGgaData(NmeaMessageGga gga)
        {
            time = gga.Time;
            latitude = gga.Latitude;
            latitudeHemisphere = gga.LatitudeHemisphere;
            longitude = gga.Longitude;
            longitudeHemisphere = gga.LongitudeHemisphere;
            altitude = gga.Altitude;
            ggaDataIsAvailable = true;
        }

        private void UpdateGsaData(NmeaMessageGsa gsa)
        {
            satelliteNumbers = gsa.SatelliteNumbers;
            satelliteViews.Clear();
        }

        private void UpdateGsvData(NmeaMessageGsv gsv)
        {
            if (numberOfSatellitesInView != gsv.NumberOfSatellitesInView)
            {
                numberOfSatellitesInView = gsv.NumberOfSatellitesInView;
                satelliteViews.Clear();
            }

            foreach (var satellite in gsv.SatelliteViews)
            {
                satelliteViews[satellite.Id] = satellite;
            }

            if (satelliteViews.Count == numberOfSatellitesInView)
            {
                satellites = satelliteNumbers
                    .Select(id => satelliteViews.TryGetValue(id, out var view) ? view : null)
                    .Where(view => view != null)
                    .Cast<SatelliteView>()
                    .ToList()
                    .AsReadOnly();
                satelliteViews.Clear();
            }
        }

        private void UpdateVtgData(NmeaMessageVtg vtg)
        {
            speed = vtg.SpeedOverGroundKph;
            courseTrue = vtg.CourseOverGroundTrue;
            courseMagnetic = vtg.CourseOverGroundMagnatic;
        }

        // ... other code ...

        private void ResetData()
        {
            time = DateTime.MinValue;
            latitude = 0.0;
            latitudeHemisphere = LatitudeHemisphere.North;
            longitude = 0.0;
            longitudeHemisphere = LongitudeHemisphere.West;
            altitude = 0.0;
            speed = 0.0;
            courseTrue = 0.0;
            courseMagnetic = 0.0;
            satellites = new List<SatelliteView>().AsReadOnly();
            satelliteViews.Clear();
            satelliteNumbers = Array.Empty<int>();
            numberOfSatellitesInView = 0;
            ggaDataIsAvailable = false;
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    timeoutTimer.Dispose();
                }
                disposed = true;
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}