using System;
using System.Security.Cryptography;
using System.Text;
using DeviceId.Internal.CommandExecutors;

namespace DeviceId.Linux.Components;

/// <summary>
/// An implementation of <see cref="IDeviceIdComponent"/>
/// </summary>
public class CpuInfoIdComponent : IDeviceIdComponent
{
    /// <summary>
    /// Command executor.
    /// </summary>
    private readonly ICommandExecutor _commandExecutor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CpuInfoIdComponent"/> class.
    /// </summary>
    public CpuInfoIdComponent() : this(CommandExecutor.Bash) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CpuInfoIdComponent"/> class.
    /// </summary>
    /// <param name="commandExecutor">The command executor to use.</param>
    internal CpuInfoIdComponent(ICommandExecutor commandExecutor)
    {
        _commandExecutor = commandExecutor;
    }

    /// <summary>
    /// Gets the component value.
    /// </summary>
    /// <returns>The component value.</returns>
    public string GetValue()
    {
        try
        {
            string content = _commandExecutor.Execute("cat /proc/cpuinfo | grep -v \"cpu MHz\"");
            using var hasher = MD5.Create();
            var hash = hasher.ComputeHash(Encoding.ASCII.GetBytes(content));
            return BitConverter.ToString(hash).Replace("-", "").ToUpper();
        }
        catch
        {
            // Can fail if we have no permissions to access the file.
        }
        return null;
    }
}
