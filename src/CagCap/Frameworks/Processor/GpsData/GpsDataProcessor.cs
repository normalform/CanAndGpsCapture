// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData
{
    using CagCap.DomainObject;
    using CagCap.DomainObject.Device;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;

    internal class GpsDataProcessor : IGpsDataProcessor
    {
        private readonly IGpsReceiverDevice gpsReceiverDevice;
        private readonly ILogger logger;

        private readonly Queue<char> dataQueue;
        private readonly NmeaProtocol nemaProtocol;

        public event EventHandler<INmeaMessage>? DataReceived;

        public GpsDataProcessor(IGpsReceiverDevice gpsReceiver, ILoggerFactory loggerFactory)
        {
            this.gpsReceiverDevice = gpsReceiver;
            this.logger = loggerFactory.CreateLogger("GpsDataProcessor");

            this.dataQueue = new Queue<char>();
            this.nemaProtocol = new NmeaProtocol(logger);
        }

        public void Process(string data)
        {
            if (data.Length == 0)
            {
                return;
            }

            foreach (var ch in data)
            {
                this.dataQueue.Enqueue(ch);
            }
            this.Process();
        }

        private void Process()
        {
            if (this.dataQueue.Count == 0)
            {
                return;
            }

            while (this.dataQueue.Count > 0)
            {
                var data = this.dataQueue.Dequeue();
                var nmeaMessage = this.nemaProtocol.Process(data);
                if (nmeaMessage != null)
                {
                    this.DataReceived?.Invoke(this, nmeaMessage);
                    this.logger.LogDebug("NMEA message {message}", nmeaMessage);
                }
            }
        }
    }
}
