// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData
{
    using CagCap.DomainObject.Device.Gps;
    using CagCap.Framework.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;

    public class GpsDataProcessor : IGpsDataProcessor
    {
        private readonly ILogger logger;

        private readonly Queue<char> dataQueue;
        private readonly NmeaProtocol nemaProtocol;

        public event EventHandler<NmeaMessageEventArgs> DataReceived = delegate { };

        public GpsDataProcessor(ILoggerFactory loggerFactory)
        {
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
            while (this.dataQueue.Count > 0)
            {
                var data = this.dataQueue.Dequeue();
                var nmeaMessage = this.nemaProtocol.Process(data);
                if (nmeaMessage != null)
                {
                    this.DataReceived.Invoke(this, new NmeaMessageEventArgs(nmeaMessage));
                    this.logger.LogDebug("NMEA message {message}", nmeaMessage);
                }
            }
        }
    }
}
