// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Can
{
    using Canable;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class NullCanableDevice : ICanableDevice
    {
#pragma warning disable CS0067
        public event EventHandler<DeviceCanMessage>? DataReceived;
#pragma warning restore CS0067

        public void SendMessage(DeviceCanMessage message)
        {
        }
    }
}
