// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsData
{
    using CagCap.Frameworks.Device.UbloxGps;
    using CagCap.Frameworks.Processor.GpsData.Nmea;
    using Microsoft.Extensions.Logging;

    internal class GpsDataProcessor : IGpsDataProcessor
    {
        private readonly IGpsReceiverDevice gpsReceiver;
        private readonly ILogger logger;

        private readonly Queue<char> dataQueue;

        private NmeaProtocol nemaProtocol;

        public GpsDataProcessor(IGpsReceiverDevice gpsReceiver, ILoggerFactory loggerFactory)
        {
            this.gpsReceiver = gpsReceiver;
            logger = loggerFactory.CreateLogger("GpsDataProcessor");

            dataQueue = new Queue<char>();
            nemaProtocol = new NmeaProtocol(logger);

            this.gpsReceiver.DataReceived += OnDataReceived;
        }

        public void Process()
        {
            if (dataQueue.Count == 0)
            {
                return;
            }

            while (dataQueue.Count > 0)
            {
                var data = dataQueue.Dequeue();
                var nmeaMessage = nemaProtocol.Process(data);
                if (nmeaMessage != null)
                {
                    logger.LogDebug("Nmea message: {nmeaMessage}", nmeaMessage);
                }
            }
        }

        private void OnDataReceived(object? sender, string data)
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
    }
}
