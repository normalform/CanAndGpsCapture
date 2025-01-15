// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace Canable
{
    public class DeviceCanMessage(DeviceCanId id, IReadOnlyList<byte> data, DateTime timestamp)
    {
        public DeviceCanId Id { get; } = id;
        public int Dlc { get; } = data.Count;
        public IReadOnlyList<byte> Data { get; } = data;
        public DateTime Timestamp { get; } = timestamp;

        public DeviceCanMessage(DeviceCanId id, IReadOnlyList<byte> data) : this(
            id,
            data,
            DateTime.Now)
        {
        }

        public override string ToString()
        {
            return $"ID: {this.Id}, DLC: {this.Dlc}, Data: {BitConverter.ToString([.. this.Data])}, Timestamp: {this.Timestamp}";
        }
    }
}
