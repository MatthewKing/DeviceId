using System;
using System.IO;
using System.Text;

namespace DeviceId
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves its value from a file.
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
        private readonly string _name;

        /// <summary>
        /// The path where the token will be stored.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTokenDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="path">The path where the component will be stored.</param>
        public FileTokenDeviceIdComponent(string name, string path)
        {
            _name = name;
            _path = path;
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            string value = Guid.NewGuid().ToString().ToUpper();

            if (File.Exists(_path))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(_path);
                    value = Encoding.ASCII.GetString(bytes);
                }
                catch { }
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(value);
                    File.WriteAllBytes(_path, bytes);
                }
                catch { }
            }

            return value;
        }
    }
}
