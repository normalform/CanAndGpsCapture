// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace UbloxGpsReceiver
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO.Ports;

    public class UbloxGpsReceiverDevice : IUbloxGpsReceiverDevice, IDisposable
    {
        private readonly ISerialPort serialPort;
        private readonly ILogger logger;
        private bool disposed;

        public event EventHandler<DataReceivedEventArgs> DataReceived = delegate { };

        public UbloxGpsReceiverDevice(string portName, int baudRate, ILoggerFactory loggerFactory)
            : this(CreateSerialPortAdapter(portName, baudRate), loggerFactory)
        {
        }

        private static SerialPortAdapter CreateSerialPortAdapter(string portName, int baudRate)
        {
            return new SerialPortAdapter(portName, baudRate, Parity.None, 8, StopBits.One);
        }

        internal UbloxGpsReceiverDevice(ISerialPort serialPort, ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(serialPort);
            ArgumentNullException.ThrowIfNull(loggerFactory);

            this.serialPort = serialPort;
            this.logger = loggerFactory.CreateLogger("GpsDevice");

            this.serialPort.DataReceived += this.DataReceivedHandler;

            if (!this.serialPort.IsOpen)
            {
                this.serialPort.Open();
            }
        }

        public void Write(string data)
        {
            if (this.serialPort.IsOpen)
            {
                this.serialPort.Write(data);
            }
        }

        private void DataReceivedHandler(object? sender, DataReceivedEventArgs e)
        {
            var sp = (ISerialPort)sender!;
            var data = sp.ReadExisting();
            this.logger.LogDebug("Data received: {data}", data);
            this.DataReceived.Invoke(this, new DataReceivedEventArgs(data));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.serialPort.IsOpen)
                    {
                        this.serialPort.Close();
                    }
                    this.serialPort.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}