// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject.Device
{
    using CagCap.DomainObject;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Timers;

    internal class GpsReceiver : IGpsReceiver, IDisposable
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
        private SatelliteView[] satellites = [];
        private int[] satelliteNumbers = [];
        private int numberOfSatellitesInView;
        private bool ggaDataIsAvailable;

        public GpsData GpsData => this.CreateGpsData();

        public event EventHandler<GpsData>? DataReceived;

        public GpsReceiver(IGpsReceiverDevice gpsReceiverDevice, IGpsDataProcessor gpsReceiverProcessor, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("GpsReceiver");

            this.timeoutTimer = new Timer(Timeout)
            {
                AutoReset = false
            };
            this.timeoutTimer.Elapsed += this.OnTimeout;

            gpsReceiverProcessor.DataReceived += this.OnDataReceived;
            gpsReceiverDevice.DataReceived += (sender, data) =>
            {
                gpsReceiverProcessor.Process(data);
            };
        }

        internal void SimulateTimeoutForTesting()
        {
            this.OnTimeout(this, null);
        }

        private GpsData CreateGpsData() => new()
        {
            Time = this.time,
            Latitude = this.latitude,
            LatitudeHemisphere = this.latitudeHemisphere,
            Longitude = this.longitude,
            LongitudeHemisphere = this.longitudeHemisphere,
            Altitude = this.altitude,
            Speed = this.speed,
            CourseTrue = this.courseTrue,
            CourseMagnetic = this.courseMagnetic,
            NumberOfSatellites = this.satellites.Length,
            Satellites = this.satellites
        };

        private void OnTimeout(object? sender, ElapsedEventArgs? e)
        {
            this.ResetData();
            this.DataReceived?.Invoke(this, this.GpsData);
        }

        private void OnDataReceived(object? sender, INmeaMessage message)
        {
            this.logger.LogDebug("Data received: {message}", message);
            this.timeoutTimer.Stop(); // Stop the timer if data is received

            switch (message)
            {
                case NmeaMessageGga gga:
                    this.UpdateGgaData(gga);
                    break;
                case NmeaMessageGsa gsa:
                    this.UpdateGsaData(gsa);
                    break;
                case NmeaMessageGsv gsv:
                    this.UpdateGsvData(gsv);
                    break;
                case NmeaMessageVtg vtg:
                    this.UpdateVtgData(vtg);
                    break;
            }

            if (this.ggaDataIsAvailable && this.satellites.Length > 1)
            {
                this.ggaDataIsAvailable = false;
                var gpsData = this.CreateGpsData();
                this.DataReceived?.Invoke(this, gpsData);
                this.logger.LogDebug("GPS data: {gpsData}", gpsData);
            }

            this.timeoutTimer.Start();
        }

        private void UpdateGgaData(NmeaMessageGga gga)
        {
            this.time = gga.Time;
            this.latitude = gga.Latitude;
            this.latitudeHemisphere = gga.LatitudeHemisphere;
            this.longitude = gga.Longitude;
            this.longitudeHemisphere = gga.LongitudeHemisphere;
            this.altitude = gga.Altitude;
            this.ggaDataIsAvailable = true;
        }

        private void UpdateGsaData(NmeaMessageGsa gsa)
        {
            this.satelliteNumbers = gsa.SatelliteNumbers;
            this.satelliteViews.Clear();
        }

        private void UpdateGsvData(NmeaMessageGsv gsv)
        {
            if (this.numberOfSatellitesInView != gsv.NumberOfSatellitesInView)
            {
                this.numberOfSatellitesInView = gsv.NumberOfSatellitesInView;
                this.satelliteViews.Clear();
            }

            foreach (var satellite in gsv.SatelliteViews)
            {
                this.satelliteViews[satellite.Id] = satellite;
            }

            if (this.satelliteViews.Count == this.numberOfSatellitesInView)
            {
                this.satellites = this.satelliteNumbers
                    .Select(id => this.satelliteViews.TryGetValue(id, out var view) ? view : null)
                    .Where(view => view != null)
                    .ToArray()!;
                this.satelliteViews.Clear();
            }
        }

        private void UpdateVtgData(NmeaMessageVtg vtg)
        {
            this.speed = vtg.SpeedOverGroundKph;
            this.courseTrue = vtg.CourseOverGroundTrue;
            this.courseMagnetic = vtg.CourseOverGroundMagnatic;
        }

        private void ResetData()
        {
            this.time = DateTime.MinValue;
            this.latitude = 0.0;
            this.latitudeHemisphere = LatitudeHemisphere.North;
            this.longitude = 0.0;
            this.longitudeHemisphere = LongitudeHemisphere.West;
            this.altitude = 0.0;
            this.speed = 0.0;
            this.courseTrue = 0.0;
            this.courseMagnetic = 0.0;
            this.satellites = [];
            this.satelliteViews.Clear();
            this.satelliteNumbers = [];
            this.numberOfSatellitesInView = 0;
            this.ggaDataIsAvailable = false;
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.timeoutTimer.Dispose();
                }
                this.disposed = true;
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}