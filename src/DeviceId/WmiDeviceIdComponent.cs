namespace DeviceId
{
    using System;
    using System.Collections.Generic;
    using System.Management;

    public sealed class WmiDeviceIdComponent : IDeviceIdComponent
    {
        private readonly string name;
        private readonly string wmiClass;
        private readonly string wmiProperty;

        public WmiDeviceIdComponent(string name, string wmiClass, string wmiProperty)
        {
            this.name = name;
            this.wmiClass = wmiClass;
            this.wmiProperty = wmiProperty;
        }

        public string Name { get { return this.name; } }

        public string GetValue()
        {
            List<string> values = new List<string>();

            using (ManagementClass mc = new ManagementClass(this.wmiClass))
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    try
                    {
                        string value = mo[this.wmiProperty] as string;
                        if (value != null)
                        {
                            values.Add(value);
                        }
                    }
                    finally
                    {
                        mo.Dispose();
                    }
                }
            }

            return String.Join(",", values);
        }
    }
}
