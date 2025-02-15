﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Gps
{
    using System;

    public interface IGpsReceiverDevice
    {
        void Write(string data);
        event EventHandler<GpsDataReceivedEventArgs> DataReceived;
    }
}
