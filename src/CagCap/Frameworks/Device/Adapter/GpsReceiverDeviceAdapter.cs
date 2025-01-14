// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Adapter
{
    using CagCap.DomainObject.Device;
    using CagCap.Frameworks.Device.UbloxGps;
    using System;
    using System.Threading.Tasks;

    internal class GpsReceiverDeviceAdapter : IGpsReceiverDevice
    {
        public event EventHandler<string> DataReceived = delegate { };

        private readonly IUbloxGpsReceiverDevice ubloxGpsReceiverDevice;

        public GpsReceiverDeviceAdapter(IUbloxGpsReceiverDevice ubloxGpsReceiverDevice)
        {
            this.ubloxGpsReceiverDevice = ubloxGpsReceiverDevice;

            this.ubloxGpsReceiverDevice.DataReceived += (sender, data) =>
            {
                this.DataReceived?.Invoke(sender, data);
            };
        }

        public async Task WriteAsync(string data)
        {
            await ubloxGpsReceiverDevice.WriteAsync(data).ConfigureAwait(false);
        }
    }
}
