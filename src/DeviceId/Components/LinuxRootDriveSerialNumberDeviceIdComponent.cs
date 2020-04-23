using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

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
        /// Initializes a new instance of the <see cref="LinuxRootDriveSerialNumberDeviceIdComponent"/> class.
        /// </summary>
        public LinuxRootDriveSerialNumberDeviceIdComponent() { }

        /// <summary>
        /// Gets the component value.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            string json = "lsblk -f -J".Bash();
            lsblkOutput output = JsonConvert.DeserializeObject<lsblkOutput>(json);

            lsblkDevice device = findRootParent(output);
            if (device == null)
            {
                return null;
            }

            string udevInfo = string.Format("udevadm info --query=all --name=/dev/{0} | grep ID_SERIAL=", device.name).Bash();
            string[] components = udevInfo.Split('=');
            if (components.Length == 2)
            {
                return components[1];
            }

            return null;
        }

        private lsblkDevice findRootParent(lsblkOutput devices)
        {
            foreach (lsblkDevice device in devices.blockdevices)
            {
                if (deviceContainsRoot(device))
                {
                    return device;
                }
            }

            return null;
        }

        private bool deviceContainsRoot(lsblkDevice device)
        {
            if (device.mountpoint == "/")
            {
                return true;
            } else if (device.children != null && device.children.Count > 0)
            {
                foreach (lsblkDevice child in device.children)
                {
                    if(deviceContainsRoot(child))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private class lsblkOutput
        {
            public List<lsblkDevice> blockdevices { get; set; } = new List<lsblkDevice>();
        }

        private class lsblkDevice
        {
            public string name { get; set; } = string.Empty;
            public string fstype { get; set; } = string.Empty;
            public string label { get; set; } = string.Empty;
            public string uuid { get; set; } = string.Empty;
            public string mountpoint { get; set; } = string.Empty;
            public List<lsblkDevice> children { get; set; } = new List<lsblkDevice>();
        }
    }

    /// <summary>
    /// Extension method for running Bash scripts courtesy https://loune.net/2017/06/running-shell-bash-commands-in-net-core/
    /// </summary>
    public static class ShellHelper
    {
        /// <summary>
        /// Execute a string as a Bash command
        /// </summary>
        /// <param name="cmd">The Bash command to run</param>
        /// <returns>The result of the Bash command</returns>
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
