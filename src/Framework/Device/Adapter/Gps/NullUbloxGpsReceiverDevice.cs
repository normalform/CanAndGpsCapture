// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Gps
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UbloxGpsReceiver;

    [ExcludeFromCodeCoverage]
    public class NullUbloxGpsReceiverDevice : IUbloxGpsReceiverDevice
    {
#pragma warning disable CS0067
        public event EventHandler<DataReceivedEventArgs> DataReceived = delegate { };
#pragma warning restore CS0067

        public void Write(string data)
        {
        }
    }
}
