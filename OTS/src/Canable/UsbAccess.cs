// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    using LibUsbDotNet;
    using LibUsbDotNet.Info;
    using LibUsbDotNet.Main;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    public enum CanRequest : byte
    {
        HostFormat = 0,
        BitTiming = 1,
        Mode = 2,
        Error = 3,
        BitTimingConstants = 4,
        DeviceConfig = 5,
        TimeStamp = 6,
    }

    public class UsbAccess : IUsbAccess, IDisposable
    {
        private const int VendorId = 0x1d50;
        private const int ProductId = 0x606f;

        private const byte UsbDirMask = 0x80;
        private const byte UsbDirOut = 0x0;     // to device
        private const byte UsbDirIn = 0x80;     // to host
        private const byte UsbTypeVendor = 0x40;
        private const byte UsbRecipInterface = 0x01;

        private bool disposed;

        private readonly ILogger logger;

        private readonly UsbDevice? usbDevice;
        private UsbEndpointReader? reader;
        private UsbEndpointWriter? writer;

        private readonly CancellationTokenSource cancellationTokenSource = new();
        public event EventHandler<DeviceCanMessageEventArgs> DataReceived = delegate { };

        [ExcludeFromCodeCoverage]
        public UsbAccess(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger("UsbAccess");
            logger.LogDebug("Creating UsbAccess");

            var finder = new UsbDeviceFinder(VendorId, ProductId);
            usbDevice = UsbDevice.OpenUsbDevice(finder);
            if (usbDevice == null)
            {
                var exception = new InvalidOperationException("Canable device not found.");
                logger.LogError(exception, "Canable device not found.");
                throw exception;
            }

            try
            {
                // Iterate through configurations and interfaces to find bulk input and output endpoints
                foreach (UsbConfigInfo configInfo in usbDevice.Configs)
                {
                    FindBulkEndpoints(configInfo);
                }

                ResetEndPoint(reader);
                ResetEndPoint(writer);
            }
            catch
            {
                Close();
                throw;
            }
        }

        public static Guid[] GetDeviceGuid(IUsbUtils usbUtils)
        {
            return usbUtils.GetDeviceSymbolicName(VendorId, ProductId);
        }

        [ExcludeFromCodeCoverage]
        public void SendFrame(int channel, CandleDataStructure.CandleDataFrame frame)
        {
            frame.EchoId = 0;
            frame.Channel = (byte)channel;
            var errorCode = writer?.Write(frame, 2000, out _);
            if (errorCode != ErrorCode.None)
            {
                var exception = new InvalidOperationException(UsbDevice.LastErrorString);
                logger.LogError(exception, "Failed to send frame: {errorCode}", errorCode);
                throw exception;
            }
        }

        [ExcludeFromCodeCoverage]
        public void StartReceive()
        {
            Task.Run(() => ReadDataAsync(cancellationTokenSource.Token));
        }

        [ExcludeFromCodeCoverage]
        private void FindBulkEndpoints(UsbConfigInfo configInfo)
        {
            foreach (UsbInterfaceInfo interfaceInfo in configInfo.InterfaceInfoList)
            {
                var bulkEndpoints = interfaceInfo.EndpointInfoList
                    .Where(endpointInfo => endpointInfo.Descriptor.Attributes == (byte)EndpointType.Bulk);

                foreach (var endpointInfo in bulkEndpoints)
                {
                    AssignEndpoint(endpointInfo);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private void AssignEndpoint(UsbEndpointInfo endpointInfo)
        {
            switch (endpointInfo.Descriptor.EndpointID)
            {
                case (byte)ReadEndpointID.Ep01:
                    reader = usbDevice?.OpenEndpointReader((ReadEndpointID)endpointInfo.Descriptor.EndpointID);
                    break;
                case (byte)WriteEndpointID.Ep02:
                    writer = usbDevice?.OpenEndpointWriter((WriteEndpointID)endpointInfo.Descriptor.EndpointID);
                    break;
                default:
                    break;
            }
        }

        [ExcludeFromCodeCoverage]
        private void Close()
        {
            logger.LogDebug("Closing UsbAccess");

            if (usbDevice != null)
            {
                if (usbDevice is IUsbDevice wholeUsbDevice)
                {
                    wholeUsbDevice.ReleaseInterface(0);
                }
                usbDevice.Close();
            }
        }

        [ExcludeFromCodeCoverage]
        private async Task ReadDataAsync(CancellationToken cancellationToken)
        {
            if (reader != null)
            {
                int bufferSize = Marshal.SizeOf<CandleDataStructure.CandleDataFrame>();
                byte[] buffer = new byte[bufferSize];
                while (!cancellationToken.IsCancellationRequested)
                {
                    var ec = reader.Read(buffer, 2000, out int bytesRead);
                    if (ec == ErrorCode.None && bytesRead > 0)
                    {
                        ProcessReceivedData(buffer);
                    }

                    await Task.Delay(1, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private void ProcessReceivedData(byte[] buffer)
        {
            var frame = CandleDataStructure.FromByteArray<CandleDataStructure.CandleDataFrame>(buffer);
            var canId = new DeviceCanId(frame.CanId);
            var data = ExtractData(frame);
            var timeStamp = new DateTime(frame.TimestampUs, DateTimeKind.Utc);

            var message = new DeviceCanMessage(canId, data, timeStamp);
            DataReceived.Invoke(this, new DeviceCanMessageEventArgs(message));
        }

        [ExcludeFromCodeCoverage]
        private static byte[] ExtractData(CandleDataStructure.CandleDataFrame frame)
        {
            var data = new byte[Math.Min(frame.CanDlc, (byte)8)];
            if (frame.CanDlc > 0) data[0] = frame.Data0;
            if (frame.CanDlc > 1) data[1] = frame.Data1;
            if (frame.CanDlc > 2) data[2] = frame.Data2;
            if (frame.CanDlc > 3) data[3] = frame.Data3;
            if (frame.CanDlc > 4) data[4] = frame.Data4;
            if (frame.CanDlc > 5) data[5] = frame.Data5;
            if (frame.CanDlc > 6) data[6] = frame.Data6;
            if (frame.CanDlc > 7) data[7] = frame.Data7;
            return data;
        }

        [ExcludeFromCodeCoverage]
        private static void ResetEndPoint(UsbEndpointBase? endpoint)
        {
            if (endpoint != null)
            {
                endpoint.Reset();
                endpoint.Flush();
            }
        }

        [ExcludeFromCodeCoverage]
        public void UsbControlMessageSet<T>(CanRequest request, ushort value, ushort index, T type) where T : struct
        {
            byte requestType = UsbDirOut | UsbTypeVendor | UsbRecipInterface;
            UsbControlMessage(request, requestType, value, index, ref type);
        }

        [ExcludeFromCodeCoverage]
        public T UsbControlMessageGet<T>(CanRequest request, ushort value, ushort index) where T : struct
        {
            T type = default;
            byte requestType = UsbDirIn | UsbTypeVendor | UsbRecipInterface;
            UsbControlMessage(request, requestType, value, index, ref type);
            return type;
        }

        [ExcludeFromCodeCoverage]
        private void UsbControlMessage<T>(CanRequest request, byte requestType, ushort value, ushort index, ref T type) where T : struct
        {
            if (usbDevice == null)
            {
                var exception = new InvalidOperationException("Device not opened.");
                logger.LogError(exception, "Device not opened.");
                throw exception;
            }

            int size = Marshal.SizeOf<T>();
            byte[] data = new byte[size];

            if ((requestType & UsbDirMask) == UsbDirOut)
            {
                // Marshal the input structure to the byte array
                var ptr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(type, ptr, true);
                    Marshal.Copy(ptr, data, 0, size);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }

            var setupPacket = new UsbSetupPacket
            {
                RequestType = requestType,
                Request = (byte)request,
                Value = (short)value,
                Index = (short)index,
                Length = (short)data.Length
            };
            bool success = usbDevice.ControlTransfer(ref setupPacket, data, data.Length, out _);
            if (!success)
            {
                var exception = new InvalidOperationException($"Control transfer failed: {UsbDevice.LastErrorString}");
                logger.LogError(exception, "Control transfer failed: {error}", UsbDevice.LastErrorString);
                throw exception;
            }

            // Convert the byte array to the specified struct
            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            try
            {
                type = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }

        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                // Cancel the token to stop the reading task
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();

                if (disposing)
                {
                    Close();
                }

                disposed = true;
            }
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
