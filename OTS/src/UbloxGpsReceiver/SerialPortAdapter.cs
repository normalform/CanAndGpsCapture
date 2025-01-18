// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace UbloxGpsReceiver
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Ports;

    [ExcludeFromCodeCoverage]
    internal class SerialPortAdapter : ISerialPort
    {
        private readonly SerialPort serialPort;
        private bool disposed;

        public SerialPortAdapter(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            this.serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);
        }

        public bool IsOpen => this.serialPort.IsOpen;

        public int ReadTimeout
        {
            get => this.serialPort.ReadTimeout;
            set => this.serialPort.ReadTimeout = value;
        }

        public int WriteTimeout
        {
            get => this.serialPort.WriteTimeout;
            set => this.serialPort.WriteTimeout = value;
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived = delegate { };

        public void Close()
        {
            this.serialPort.Close();
        }

        public void Open()
        {
            this.serialPort.Open();
        }

        public string ReadExisting()
        {
            return this.serialPort.ReadExisting();
        }

        public void Write(string data)
        {
            this.serialPort.Write(data);
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var dummyArg = new DataReceivedEventArgs(string.Empty);
            this.DataReceived?.Invoke(this, dummyArg);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.serialPort.Dispose();
                }

                this.disposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SerialPortAdapter()
        {
            this.Dispose(false);
        }
    }
}
