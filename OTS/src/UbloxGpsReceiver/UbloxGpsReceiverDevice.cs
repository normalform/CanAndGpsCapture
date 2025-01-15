// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace UbloxGpsReceiver
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
        private bool disposed;

        public event EventHandler<DataReceivedEventArgs> DataReceived = delegate { };

        public UbloxGpsReceiverDevice(string portName, int baudRate, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("GpsDevice");
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
            catch (UnauthorizedAccessException ex)
            {
                logger.LogError(ex, "Unauthorized access error opening COM port: {message}", ex.Message);
            }
            catch (IOException ex)
            {
                logger.LogError(ex, "I/O error opening COM port: {message}", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Invalid operation error opening COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error opening COM port: {ex.Message}");
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                logger.LogError(ex, "Argument error opening COM port: {message}", ex.Message);
                Console.WriteLine($"Argument error opening COM port: {ex.Message}");
            }
        }

        public async Task WriteAsync(string data)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    await Task.Run(() => serialPort.WriteLine(data)).ConfigureAwait(false);
                }
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Invalid operation error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error writing to COM port: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                logger.LogError(ex, "Timeout error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"Timeout error writing to COM port: {ex.Message}");
            }
            catch (IOException ex)
            {
                logger.LogError(ex, "I/O error writing to COM port: {message}", ex.Message);
                Console.WriteLine($"I/O error writing to COM port: {ex.Message}");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var sp = (SerialPort)sender;
                var data = sp.ReadExisting();
                logger.LogDebug("Data received: {data}", data);
                this.DataReceived.Invoke(this, new DataReceivedEventArgs(data));
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError(ex, "Invalid operation error reading COM port: {message}", ex.Message);
                Console.WriteLine($"Invalid operation error reading from COM port: {ex.Message}");
            }
            catch (TimeoutException ex)
            {
                logger.LogError(ex, "Timeout error reading COM port: {message}", ex.Message);
                Console.WriteLine($"Timeout error reading from COM port: {ex.Message}");
            }
            catch (IOException ex)
            {
                logger.LogError(ex, "I/O error reading COM port: {message}", ex.Message);
                Console.WriteLine($"I/O error reading from COM port: {ex.Message}");
            }
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
                if (disposing && serialPort != null)
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Close();
                    }
                    serialPort.Dispose();
                }

                disposed = true;
            }
        }
    }
}