using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that retrieves its value from a file.
    /// </summary>
    public class FileDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The paths of file we should look at.
        /// </summary>
        private readonly string[] _paths;

        /// <summary>
        /// Should the contents of the file be hashed? (Relevant for sources such as /proc/cpuinfo)
        /// </summary>
        private readonly bool _shouldHashContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTokenDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="path">The path of the file holding the component ID.</param>
        /// <param name="shouldHashContents">Whether the file contents should be hashed.</param>
        public FileDeviceIdComponent(string name, string path, bool shouldHashContents = false)
            : this(name, new string[] { path }, shouldHashContents) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTokenDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="paths">The paths of the files holding the component ID.</param>
        /// <param name="shouldHashContents">Whether the file contents should be hashed.</param>
        public FileDeviceIdComponent(string name, IEnumerable<string> paths, bool shouldHashContents = false)
        {
            Name = name;
            _paths = paths.ToArray();
            _shouldHashContents = shouldHashContents;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            foreach (var path in _paths)
            {
                if (!File.Exists(path))
                {
                    continue;
                }

                try
                {
                    var contents = default(string);

                    using (var file = File.OpenText(path))
                    {
                        contents = file.ReadToEnd(); // File.ReadAllBytes() fails for special files such as /sys/class/dmi/id/product_uuid
                    }

                    contents = contents.Trim();

                    if (!_shouldHashContents)
                    {
                        return contents;
                    }

                    using var hasher = MD5.Create();
                    var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(contents));
                    return BitConverter.ToString(hash).Replace("-", "").ToUpper();
                }
                catch (UnauthorizedAccessException)
                {
                    // Can fail if we have no permissions to access the file.
                }
            }

            return null;
        }
    }
}
