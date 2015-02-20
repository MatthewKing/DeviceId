namespace DeviceId
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class DeviceIdBuilder
    {
        private readonly HashSet<IDeviceIdComponent> components;

        public DeviceIdBuilder()
        {
            this.components = new HashSet<IDeviceIdComponent>(new DeviceIdComponentEqualityComparer());
        }

        public DeviceIdBuilder AddComponent(IDeviceIdComponent component)
        {
            this.components.Add(component);
            return this;
        }

        public string GetDeviceId()
        {
            return this.GetDeviceId("SHA256");
        }

        public string GetDeviceId(string hashName)
        {
            if (this.components.Count == 0) return null;

            IEnumerable<string> orderedValues = this.components
                .OrderBy(o => o.Name)
                .Select(o => String.Concat(o.Name, ":", o.GetValue()));
            string combinedValue = String.Join(",", orderedValues) ?? String.Empty;

            byte[] data, hash;
            using (HashAlgorithm algorithm = HashAlgorithm.Create(hashName))
            {
                data = Encoding.UTF8.GetBytes(combinedValue);
                hash = algorithm.ComputeHash(data);
            }

            return Convert.ToBase64String(hash);
        }
    }
}
