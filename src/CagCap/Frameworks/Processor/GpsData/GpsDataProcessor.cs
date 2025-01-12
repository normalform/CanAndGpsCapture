// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Processor.GpsDataProcessor
{
    using CagCap.Frameworks.Device.UbloxGps;
    using CagCap.Frameworks.Processor.GpsData;
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
            this.logger = loggerFactory.CreateLogger("GpsDataProcessor");
            
            this.dataQueue = new Queue<char>();
            this.nemaProtocol = new NmeaProtocol(this.logger);

            this.gpsReceiver.DataReceived += this.OnDataReceived;
        }

        public void Process()
        {
            if (this.dataQueue.Count == 0)
            {
                return;
            }

            while(this.dataQueue.Count > 0)
            {
                var data = this.dataQueue.Dequeue();
                var nmeaMessage = this.nemaProtocol.Process(data);
                if (nmeaMessage != null)
                {
                    Console.WriteLine(nmeaMessage);
                }
            }
        }

        private void OnDataReceived(object? sender, string data)
        {
            if (data.Length == 0)
            {
                return;
            }
            var stripedData = data.Trim();

            foreach (var ch in data)
            {
                this.dataQueue.Enqueue(ch);
            }

            this.Process();
        }
    }
}
