﻿// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Processor.GpsData.Nmea
{
    using CagCap.DomainObject.Device.Gps;
    using Microsoft.Extensions.Logging;
    using System.Text;

    internal enum NmeaState
    {
        Start,
        Address,
        Data,
        Checksum,
        Done
    }

    internal class NmeaProtocol(ILogger logger)
    {
        private const char NmeaPrefix = '$';
        private const int AddressLength = 5;
        private const int ChecksumLength = 2;
        private const char NmeaDelimitor = ',';
        private const char NmeaChecksum = '*';
        private const char NmeaSuffix0 = '\r';
        private const char NmeaSuffix1 = '\n';

        private readonly char[] addressBuffer = new char[AddressLength];
        private readonly char[] checksumBuffer = new char[ChecksumLength];
        private readonly StringBuilder currentDataStringBuilder = new();
        private readonly List<string> dataVector = [];
        private int index;
        private byte checksum;

        private NmeaState currentState = NmeaState.Start;
        private readonly ILogger logger = logger;

        public INmeaMessage? Process(char dataIn)
        {
            switch (currentState)
            {
                case NmeaState.Start:
                    HandleStartState(dataIn);
                    break;
                case NmeaState.Address:
                    HandleAddressState(dataIn);
                    break;
                case NmeaState.Data:
                    HandleDataState(dataIn);
                    break;
                case NmeaState.Checksum:
                    HandleChecksumState(dataIn);
                    break;
                case NmeaState.Done:
                    return HandleDoneState(dataIn);
            }

            return null;
        }

        private void HandleStartState(char dataIn)
        {
            if (dataIn == NmeaPrefix)
            {
                currentState = NmeaState.Address;
            }
        }

        private void HandleAddressState(char dataIn)
        {
            addressBuffer[index++] = dataIn;
            if (index == AddressLength)
            {
                currentState = NmeaState.Data;
                index = 0;
            }
            checksum ^= (byte)dataIn;
        }

        private void HandleDataState(char dataIn)
        {
            if (dataIn == NmeaChecksum)
            {
                currentState = NmeaState.Checksum;
                index = 0;
                return;
            }

            if (dataIn == NmeaDelimitor)
            {
                if (index != 0)
                {
                    dataVector.Add(currentDataStringBuilder.ToString());
                    currentDataStringBuilder.Clear();
                }
            }
            else
            {
                currentDataStringBuilder.Append(dataIn);
            }

            index++;

            checksum ^= (byte)dataIn;
        }

        private void HandleChecksumState(char dataIn)
        {
            if (dataIn == NmeaSuffix0)
            {
                index = 0;
                var checksumStr = new string(checksumBuffer);
                var localChecksum = Convert.ToInt32(checksumStr, 16);
                if (localChecksum == checksum)
                {
                    dataVector.Add(currentDataStringBuilder.ToString());
                    currentState = NmeaState.Done;
                }
                else
                {
                    logger.LogError("Checksum error: {checksumStr} != {checksum}", localChecksum, checksum);
                    ResetState();
                }
            }
            else
            {
                if (index >= ChecksumLength)
                {
                    ResetState();
                }
                else
                {
                    checksumBuffer[index++] = dataIn;
                }
            }
        }

        private INmeaMessage? HandleDoneState(char dataIn)
        {
            if (dataIn == NmeaSuffix1)
            {
                var address = new string(addressBuffer);
                var message = NmeaMessageFactory.Create(address, [.. dataVector], logger);
                ResetState();

                return message;
            }

            return null;
        }

        private void ResetState()
        {
            currentState = NmeaState.Start;
            index = 0;
            checksum = 0;
            dataVector.Clear();
            currentDataStringBuilder.Clear();
        }
    }
}
