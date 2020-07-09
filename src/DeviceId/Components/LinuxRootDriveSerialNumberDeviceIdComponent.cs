using System.Collections.Generic;
using DeviceId.CommandExecutors;
using DeviceId.Internal;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> that uses the root drive's serial number.
    /// </summary>
    public class LinuxRootDriveSerialNumberDeviceIdComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; } = "SystemDriveSerialNumber";

        /// <summary>
        /// Command executor.
        /// </summary>
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LinuxRootDriveSerialNumberDeviceIdComponent"/> class.
        /// </summary>
        public LinuxRootDriveSerialNumberDeviceIdComponent()
            : this(CommandExecutor.Bash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinuxRootDriveSerialNumberDeviceIdComponent"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor to use.</param>
        internal LinuxRootDriveSerialNumberDeviceIdComponent(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var outputJson = _commandExecutor.Execute("lsblk -f -J");
            var output = Json.Deserialize<LsblkOutput>(outputJson);

            var device = FindRootParent(output);
            if (device == null)
            {
                return null;
            }

            var udevInfo = _commandExecutor.Execute($"udevadm info --query=all --name=/dev/{device.Name} | grep ID_SERIAL=");
            if (udevInfo != null)
            {
                var components = udevInfo.Split('=');
                if (components.Length == 2)
                {
                    return components[1];
                }
            }

            return null;
        }

        private LsblkDevice FindRootParent(LsblkOutput devices)
        {
            foreach (var device in devices.BlockDevices)
            {
                if (DeviceContainsRoot(device))
                {
                    return device;
                }
            }

            return null;
        }

        private bool DeviceContainsRoot(LsblkDevice device)
        {
            if (device.MountPoint == "/")
            {
                return true;
            }
            else if (device.Children != null && device.Children.Count > 0)
            {
                foreach (var child in device.Children)
                {
                    if (DeviceContainsRoot(child))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private sealed class LsblkOutput
        {
            public List<LsblkDevice> BlockDevices { get; set; } = new List<LsblkDevice>();
        }

        private sealed class LsblkDevice
        {
            public string Name { get; set; } = string.Empty;
            public string MountPoint { get; set; } = string.Empty;
            public List<LsblkDevice> Children { get; set; } = new List<LsblkDevice>();
        }
    }
}
