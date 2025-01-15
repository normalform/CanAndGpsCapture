// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Framework.Device.Adapter.Can
{
    using CagCap.DomainObject.Device.Can;
    using Canable;

    internal class CanId(DeviceCanId deviceCanId) : ICanId
    {
        public uint Id => deviceCanId.Id;

        public bool Extended => deviceCanId.Extended;

        public bool Rtr => deviceCanId.Rtr;

        public bool HasError => deviceCanId.HasError;

        public override string ToString()
        {
            return deviceCanId.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj is CanId other)
            {
                return Id == other.Id &&
                       Extended == other.Extended &&
                       Rtr == other.Rtr &&
                       HasError == other.HasError;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Extended, Rtr, HasError);
        }
    }
}
