namespace DeviceId
{
    using System;

    public sealed class DeviceIdComponent : IDeviceIdComponent
    {
        private readonly string name;
        private readonly Func<string> value;

        public DeviceIdComponent(string name, string value)
        {
            this.name = name;
            this.value = () => value;
        }

        public DeviceIdComponent(string name, Func<string> value)
        {
            this.name = name;
            this.value = value;
        }

        public string Name { get { return this.name; } }

        public string GetValue()
        {
            return this.value();
        }
    }
}
