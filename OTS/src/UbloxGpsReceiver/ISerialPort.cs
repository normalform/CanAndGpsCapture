// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace UbloxGpsReceiver
{
    using System;

    internal interface ISerialPort : IDisposable
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        bool IsOpen { get; }
        int ReadTimeout { get; set; }
        int WriteTimeout { get; set; }

        void Open();
        void Close();

        void Write(string data);
        string ReadExisting();
    }
}
