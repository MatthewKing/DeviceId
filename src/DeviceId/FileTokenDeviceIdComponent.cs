namespace DeviceId
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// An implementation of IDeviceIdComponent that retrieves its value from a file.
    /// </summary>
    /// <remarks>
    /// If the file exists, the contents of that file will be used as the component value.
    /// If the file does not exist, a new file will be created and populated with a new GUID,
    /// which will be used as the component value.
    /// </remarks>
    public sealed class FileTokenDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// The name of the component.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The path where the token will be stored.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// Initializes a new instance of the FileTokenDeviceIdComponent class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="path">The path where the component will be stored.</param>
        public FileTokenDeviceIdComponent(string name, string path)
        {
            this.name = name;
            this.path = path;
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get { return this.name; } }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
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
