// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.UbloxGps
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO.Ports;
    using System.Threading.Tasks;

    [ExcludeFromCodeCoverage]
    public class UbloxGpsReceiverDevice : IUbloxGpsReceiverDevice, IDisposable
    {
        private readonly SerialPort serialPort;
        private readonly ILogger logger;
        private bool disposed = false;

        public event EventHandler<string>? DataReceived;

        public UbloxGpsReceiverDevice(string portName, int baudRate, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("GpsMessage");
            serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error opening COM port: {message}", ex.Message);
                Console.WriteLine($"Error opening COM port: {ex.Message}");
            }
        }

        public async Task WriteAsync(string data)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    await Task.Run(() => serialPort.WriteLine(data));
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"Error writing to COM port: {ex.Message}");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string data = sp.ReadExisting();
                OnDataReceived(data);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error reading COM port: {message}", ex.Message);
                Console.WriteLine($"Error reading from COM port: {ex.Message}");
            }
        }

        private void OnDataReceived(string data)
        {
            this.logger.LogInformation("Data received: {data}", data);
            DataReceived?.Invoke(this, data);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (serialPort != null)
                    {
                        if (serialPort.IsOpen)
                        {
                            serialPort.Close();
                        }
                        serialPort.Dispose();
                    }
                }

                disposed = true;
            }
        }
    }
}