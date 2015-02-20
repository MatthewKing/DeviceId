namespace DeviceId
{
    using System;
    using System.Collections.Generic;

    internal sealed class DeviceIdComponentEqualityComparer : IEqualityComparer<IDeviceIdComponent>
    {
        public bool Equals(IDeviceIdComponent x, IDeviceIdComponent y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name);
        }

        public int GetHashCode(IDeviceIdComponent obj)
        {
            return StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
        }
    }
}
