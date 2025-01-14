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
    internal class UbloxGpsReceiverDevice : IUbloxGpsReceiverDevice, IDisposable
    {
        private readonly SerialPort serialPort;
        private readonly ILogger logger;
        private bool disposed;

        public event EventHandler<string>? DataReceived;

        public UbloxGpsReceiverDevice(string portName, int baudRate, ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("GpsDevice");
            this.serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                ReadTimeout = 500,
                WriteTimeout = 500
            };

            this.serialPort.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandler);

            try
            {
                if (!this.serialPort.IsOpen)
                {
                    this.serialPort.Open();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                this.logger.LogError(ex, "Unauthorized access error opening COM port: {message}", ex.Message);
            }
            catch (IOException ex)
            {
                this.logger.LogError(ex, "I/O error opening COM port: {message}", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Invalid operation error opening COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error opening COM port: {ex.Message}");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                this.logger.LogError(ex, "Argument error opening COM port: {message}", ex.Message);
                Console.WriteLine($"Argument error opening COM port: {ex.Message}");
            }
        }

        public async Task WriteAsync(string data)
        {
            try
            {
                if (this.serialPort.IsOpen)
                {
                    await Task.Run(() => this.serialPort.WriteLine(data)).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Invalid operation error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error writing to COM port: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                this.logger.LogError(ex, "Timeout error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"Timeout error writing to COM port: {ex.Message}");
            }
            catch (IOException ex)
            {
                this.logger.LogError(ex, "I/O error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"I/O error writing to COM port: {ex.Message}");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var sp = (SerialPort)sender;
                var data = sp.ReadExisting();
                this.logger.LogDebug("Data received: {data}", data);
                this.DataReceived?.Invoke(this, data);
            }
            catch (InvalidOperationException ex)
            {
                this.logger.LogError(ex, "Invalid operation error reading COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error reading from COM port: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                this.logger.LogError(ex, "Timeout error reading COM port: {message}", ex.Message);
                Console.WriteLine($"Timeout error reading from COM port: {ex.Message}");
            }
            catch (IOException ex)
            {
                this.logger.LogError(ex, "I/O error reading COM port: {message}", ex.Message);
                Console.WriteLine($"I/O error reading from COM port: {ex.Message}");
            }
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
                if (disposing && this.serialPort != null)
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