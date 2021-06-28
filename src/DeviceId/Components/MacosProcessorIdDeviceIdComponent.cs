using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using DeviceId.CommandExecutors;
using DeviceId.Internal;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that uses the root drive's serial number.
    /// </summary>
    public class MacosProcessorIdDeviceIdComponent
        : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; } = "ProcessorId";

        /// <summary>
        /// Command executor.
        /// </summary>
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Should the contents of the file be hashed? (Relevant for sources such as /proc/cpuinfo)
        /// </summary>
        private readonly bool _shouldHashContents;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacosProcessorIdDeviceIdComponent"/> class.
        /// </summary>
        public MacosProcessorIdDeviceIdComponent(bool shouldHashContents = false)
            : this(CommandExecutor.Bash, shouldHashContents) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacosProcessorIdDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor to use.</param>
        /// <param name="shouldHashContents">Whether the result contents should be hashed</param>
        internal MacosProcessorIdDeviceIdComponent(ICommandExecutor commandExecutor, bool shouldHashContents = false)
        {
            _commandExecutor = commandExecutor;
            _shouldHashContents = shouldHashContents;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var contents = _commandExecutor.Execute("sysctl -a | grep machdep.cpu");

            if (!string.IsNullOrEmpty(contents))
            {
                if (!_shouldHashContents)
                {
                    return contents;
                }

                using var hasher = MD5.Create();
                var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(contents));
                return BitConverter.ToString(hash).Replace("-", "").ToUpper();
            }

            return null;
        }

    }
}
