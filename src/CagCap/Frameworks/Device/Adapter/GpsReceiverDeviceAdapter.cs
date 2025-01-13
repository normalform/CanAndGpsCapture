// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Adapter
{
    using CagCap.DomainObject.Device;
    using CagCap.Frameworks.Device.UbloxGps;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    internal class GpsReceiverDeviceAdapter : IGpsReceiverDevice, IDisposable
    {
        public event EventHandler<string> DataReceived = delegate { };

        private readonly UbloxGpsReceiverDevice ubloxGpsReceiverDevice;
        private bool disposed;

        public GpsReceiverDeviceAdapter(string portName, int baudRate, ILoggerFactory loggerFactory)
        {
            this.ubloxGpsReceiverDevice = new UbloxGpsReceiverDevice(portName, baudRate, loggerFactory);

            this.ubloxGpsReceiverDevice.DataReceived += (sender, data) =>
            {
                this.DataReceived?.Invoke(sender, data);
            };
        }

        public async Task WriteAsync(string data)
        {
            await ubloxGpsReceiverDevice.WriteAsync(data).ConfigureAwait(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    ubloxGpsReceiverDevice?.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
