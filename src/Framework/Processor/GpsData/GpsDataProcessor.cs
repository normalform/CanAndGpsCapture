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

        public event EventHandler<INmeaMessage>? DataReceived;

        public GpsDataProcessor(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("GpsDataProcessor");

            dataQueue = new Queue<char>();
            nemaProtocol = new NmeaProtocol(logger);
        }

        public void Process(string data)
        {
            if (data.Length == 0)
            {
                return;
            }

            foreach (var ch in data)
            {
                dataQueue.Enqueue(ch);
            }
            Process();
        }

        private void Process()
        {
            while (dataQueue.Count > 0)
            {
                var data = dataQueue.Dequeue();
                var nmeaMessage = nemaProtocol.Process(data);
                if (nmeaMessage != null)
                {
                    DataReceived?.Invoke(this, nmeaMessage);
                    logger.LogDebug("NMEA message {message}", nmeaMessage);
                }
            }
        }
    }
}
