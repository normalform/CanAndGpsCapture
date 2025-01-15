// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Can
{
    using CagCap.DomainObject.Device.Can;
    using Canable;
    using System;
    using System.Collections.Generic;

    internal class CanMessage(DeviceCanMessage deviceCanMessage) : ICanMessage
    {
        public ICanId Id => new CanId(deviceCanMessage.Id);

        public int Dlc => deviceCanMessage.Dlc;

        public IReadOnlyList<byte> Data => deviceCanMessage.Data;

        public DateTime Timestamp => deviceCanMessage.Timestamp;

        public override string ToString()
        {
            return deviceCanMessage.ToString();
        }
    }
}
