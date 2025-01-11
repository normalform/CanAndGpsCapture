// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Canable
{
    internal interface IUsbAccess
    {
        void StartReceive();
        void UsbControlMessageSet<T>(CanRequest request, ushort value, ushort index, T structure) where T : struct;
        T UsbControlMessageGet<T>(CanRequest request, ushort value, ushort index) where T : struct;

        void SendFrame(int channel, CandleDataStructure.CandleDataFrame frame);
        public event EventHandler<CanMessage>? DataReceived;
    }
}
