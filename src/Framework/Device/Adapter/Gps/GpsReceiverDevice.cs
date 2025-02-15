﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Gps
{
    using CagCap.Framework.Device.Gps;
    using System;
    using UbloxGpsReceiver;

    public class GpsReceiverDevice : IGpsReceiverDevice
    {
        public event EventHandler<GpsDataReceivedEventArgs> DataReceived = delegate { };

        private readonly IUbloxGpsReceiverDevice ubloxGpsReceiverDevice;

        public GpsReceiverDevice(IUbloxGpsReceiverDevice ubloxGpsReceiverDevice)
        {
            this.ubloxGpsReceiverDevice = ubloxGpsReceiverDevice;

            this.ubloxGpsReceiverDevice.DataReceived += (sender, data) =>
            {
                this.DataReceived.Invoke(sender, new GpsDataReceivedEventArgs(data.Data));
            };
        }

        public void Write(string data)
        {
            this.ubloxGpsReceiverDevice.Write(data);
        }
    }
}
