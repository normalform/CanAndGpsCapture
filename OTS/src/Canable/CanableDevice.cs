// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    using Canable.Config;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class CanableDevice : ICanableDevice
    {
        private readonly IUsbAccess usbAccess;
        private readonly CanBusConfig canBusConfig;
        private readonly ILogger logger;

        public event EventHandler<DeviceCanMessage>? DataReceived;

        private readonly CandleDataStructure.CandleCapability candleCapability;

        public CanableDevice(IUsbAccess usbAccess, CanBusConfig canBusConfig, ILoggerFactory loggerFactory)
        {
            this.usbAccess = usbAccess;
            this.canBusConfig = canBusConfig;
            logger = loggerFactory.CreateLogger("CanMessage");

            this.usbAccess.DataReceived += (sender, message) => OnMessageReceived(message);

            SetDeviceMode(start: false);

            candleCapability = this.usbAccess.UsbControlMessageGet<CandleDataStructure.CandleCapability>(CanRequest.BitTimingConstants, 0, 0);

            int samplePoint = GetSamplePoint(this.canBusConfig.SamplePoint, logger);
            this.SetBitTiming(canBusConfig.BitRate, samplePoint);

            SetDeviceMode(start: true);

            this.usbAccess.StartReceive();
        }

        public void SendMessage(DeviceCanMessage message)
        {
            LogCanMessage("Send CAN message: {CanMessage}", message);

            var frame = new CandleDataStructure.CandleDataFrame
            {
                EchoId = 0,
                CanId = message.Id.Id,
                CanDlc = (byte)message.Dlc,
                Channel = 0,
                Flags = 0,
                TimestampUs = 0
            };

            // Set Data0 to Data7 based on the size of message.Data
            if (message.Data.Count > 0) frame.Data0 = message.Data[0];
            if (message.Data.Count > 1) frame.Data1 = message.Data[1];
            if (message.Data.Count > 2) frame.Data2 = message.Data[2];
            if (message.Data.Count > 3) frame.Data3 = message.Data[3];
            if (message.Data.Count > 4) frame.Data4 = message.Data[4];
            if (message.Data.Count > 5) frame.Data5 = message.Data[5];
            if (message.Data.Count > 6) frame.Data6 = message.Data[6];
            if (message.Data.Count > 7) frame.Data7 = message.Data[7];

            usbAccess.SendFrame(0, frame);
        }

        public static int GetSamplePoint(string samplePointConfig, ILogger logger)
        {
            int samplePointInt;
            var samplePointString = samplePointConfig.TrimEnd('%');
            if (double.TryParse(samplePointString, out double samplePoint))
            {
                if (samplePoint < 0 || samplePoint > 100)
                {
                    var exception = new ArgumentOutOfRangeException(nameof(samplePointConfig), "The input sample point format is out of range.");
                    logger.LogError(exception, "The input sample point format is out of range. {samplePoint}", samplePoint);
                    throw exception;
                }

                samplePointInt = (int)(samplePoint * 10);
            }
            else
            {
                var exception = new FormatException("Invalid sample point format.");
                logger.LogError(exception, "Invalid sample point format. {samplePointString}", samplePointString);
                throw exception;
            }

            return samplePointInt;
        }

        private void OnMessageReceived(DeviceCanMessage message)
        {
            LogCanMessage("Received CAN message: {CanMessage}", message);
        }

        [ExcludeFromCodeCoverage]
        private void LogCanMessage(string message, DeviceCanMessage canMessage)
        {
            var dataString = string.Join(",", canMessage.Data.Select(b => $"0x{b:X2}"));
            logger.LogDebug("CAN message: {Message}, Id: {Id}, Extended: {Extended}, Rtr: {Rtr}, Error: {Error}, Dlc: {Dlc}, Data: {Data}, Timestamp: {Timestamp}",
                message,
                canMessage.Id.Id,
                canMessage.Id.Extended,
                canMessage.Id.Rtr,
                canMessage.Id.HasError,
                canMessage.Dlc,
                $"[{dataString}]",
                canMessage.Timestamp.Ticks / 10);
        }

        private void SetDeviceMode(bool start)
        {
            var avaliableFeature = new CanableFeature(candleCapability.Feature);

            uint flags = 0;
            if (avaliableFeature.ListenOnly && canBusConfig.EnableListenOnly)
            {
                flags |= CanableFeature.ListenOnlyBit;
            }

            if (avaliableFeature.Loopback && canBusConfig.EnableLoopback)
            {
                flags |= CanableFeature.LoopbackBit;
            }

            if (avaliableFeature.HwTimeStamp && canBusConfig.EnableHwTimestamp)
            {
                flags |= CanableFeature.HwTimeStampBit;
            }

            if (avaliableFeature.Identity && canBusConfig.EnableIdentity)
            {
                flags |= CanableFeature.IdentityBit;
            }

            if (avaliableFeature.UserId && canBusConfig.EnableUserId)
            {
                flags |= CanableFeature.UserIdBit;
            }

            if (avaliableFeature.PadPacketsToMaxPacketSize && canBusConfig.EnablePadPacketsToMaxPacketSize)
            {
                flags |= CanableFeature.PadPacketsToMaxPacketSizeBit;
            }

            var candleDeviceMode = new CandleDataStructure.CandleDeviceMode
            {
                Mode = start ? 1u : 0,
                Flags = flags
            };

            usbAccess.UsbControlMessageSet(CanRequest.Mode, 0, 0, candleDeviceMode);
        }

        private void SetBitTiming(int bitRate, int samplePoint)
        {
            foreach (var timing in Timing.Timings)
            {
                if (timing.BaseClk == candleCapability.FclkCan && timing.BitRate == bitRate && timing.SamplePoint == samplePoint)
                {
                    var bitTiming = timing.BitTiming;

                    var bitTimingStruct = new CandleDataStructure.BitTimingStruct
                    {
                        PropSeg = bitTiming.PropSeg,
                        PhaseSeg1 = bitTiming.PhaseSeg1,
                        PhaseSeg2 = bitTiming.PhaseSeg2,
                        Sjw = bitTiming.Sjw,
                        Brp = bitTiming.Brp
                    };

                    usbAccess.UsbControlMessageSet(CanRequest.BitTiming, 0, 0, bitTimingStruct);
                }
            }
        }
    }
}
