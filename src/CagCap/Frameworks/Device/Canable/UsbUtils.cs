// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Canable
{
    using LibUsbDotNet;
    using LibUsbDotNet.Main;
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class UsbUtils : IUsbUtils
    {
        public Guid[] GetDeviceSymbolicName(int vendtorId, int productId)
        {
            var finder = new UsbDeviceFinder(vendtorId, productId);
            var registry = UsbDevice.AllDevices.Find(finder);
            if (registry == null)
            {
                return [];
            }

            return registry.DeviceInterfaceGuids;
        }
    }
}
