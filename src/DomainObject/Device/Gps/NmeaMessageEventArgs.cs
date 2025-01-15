﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.DomainObject.Device.Gps
{
    public class NmeaMessageEventArgs(INmeaMessage message) : EventArgs
    {
        public INmeaMessage Message { get; } = message;
    }
}
