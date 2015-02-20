namespace DeviceId
{
    using System;
    using System.IO;
    using System.Text;

    public sealed class FileTokenDeviceIdComponent : IDeviceIdComponent
    {
        private readonly string name;
        private readonly string path;

        public string Name { get { return this.name; } }

        public FileTokenDeviceIdComponent(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        public string GetValue()
        {
            string value = Guid.NewGuid().ToString().ToUpper();

            if (File.Exists(this.path))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(this.path);
                    value = Encoding.ASCII.GetString(bytes);
                }
                catch { }
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(value);
                    File.WriteAllBytes(this.path, bytes);
                }
                catch { }
            }

            return value;
        }
    }
}
