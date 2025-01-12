// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.UbloxGps
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class NullGpsReceiverDevice : IGpsReceiverDevice
    {
        public event EventHandler<string>? DataReceived;

        public async Task WriteAsync(string data)
        {
            await Task.CompletedTask;
        }
    }
}
