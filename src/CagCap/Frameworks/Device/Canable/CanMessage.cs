// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Frameworks.Device.Canable
{
    internal class CanMessage(CanId id, byte[] data, DateTime timestamp)
    {
        public CanId Id { get; } = id;
        public int Dlc { get; } = data.Length;
        public byte[] Data { get; } = data;
        public DateTime Timestamp { get; } = timestamp;

        public CanMessage(CanId id, byte[] data) : this(
            id,
            data,
            DateTime.Now)
        {
        }
    }
}
