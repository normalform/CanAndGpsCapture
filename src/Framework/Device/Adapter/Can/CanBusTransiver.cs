// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Can
{
    using CagCap.DomainObject.Device.Can;
    using Canable;
    using Microsoft.Extensions.Logging;
    using System;

    public class CanBusTransiver : ICanBusTransiver
    {
        private readonly ICanableDevice canableDevice;
        private readonly ILogger logger;

        public event EventHandler<CanMessageEventArgs> DataReceived = delegate { };

        public CanBusTransiver(ICanableDevice canableDevice, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(canableDevice);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            this.logger = loggerFactory.CreateLogger("CanBusTransiver");
            this.canableDevice = canableDevice;

            this.canableDevice.DataReceived += (sender, data) =>
            {
                var message = new CanMessage(data.Message);
                this.DataReceived.Invoke(this, new CanMessageEventArgs(message));

                this.logger.LogDebug("Received message: {Message}", message);
            };
        }

        public void SendMessage(ICanMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            var canId = message.Id;
            var deviceCanId = canId.Extended ? DeviceCanId.ExtendedCanId(canId.Id, canId.Rtr, canId.HasError) : DeviceCanId.StandardCanId(canId.Id, canId.Rtr, canId.HasError);
            var deviceCanMessage = new DeviceCanMessage(deviceCanId, message.Data);

            this.logger.LogDebug("Send message: {Message}", message);
            this.canableDevice.SendMessage(deviceCanMessage);
        }
    }
}
