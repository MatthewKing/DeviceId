using DeviceId.CommandExecutors;
using DeviceId.Components;
using DeviceId.Linux.Components;

namespace DeviceId;

/// <summary>
/// Extension methods for <see cref="LinuxDeviceIdBuilder"/>.
/// </summary>
public static class LinuxDeviceIdBuilderExtensions
{
    /// <summary>
    /// Adds the system drive serial number to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddSystemDriveSerialNumber(this LinuxDeviceIdBuilder builder)
    {
        return AddSystemDriveSerialNumber(builder, CommandExecutor.Bash);
    }

    /// <summary>
    /// Adds the system drive serial number to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <param name="commandExecutor">The command executor to use.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddSystemDriveSerialNumber(this LinuxDeviceIdBuilder builder, ICommandExecutor commandExecutor)
    {
        return builder.AddComponent("SystemDriveSerialNumber", new LinuxRootDriveSerialNumberDeviceIdComponent(commandExecutor));
    }

    /// <summary>
    /// Adds the docker container id to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddDockerContainerId(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("DockerContainerId", new DockerContainerIdComponent("/proc/1/cgroup"));
    }

    /// <summary>
    /// Adds the machine ID (from /var/lib/dbus/machine-id or /etc/machine-id) to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddMachineId(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("MachineID", new FileContentsDeviceIdComponent(new[] { "/var/lib/dbus/machine-id", "/etc/machine-id" }, false));
    }

    /// <summary>
    /// Adds the product UUID (from /sys/class/dmi/id/product_uuid) to the device identifier.
    /// On ARM systems where DMI is not available, it falls back to the Device Tree serial number.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddProductUuid(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("ProductUUID", new FileContentsDeviceIdComponent(new[]
        {
            "/sys/class/dmi/id/product_uuid",
            "/sys/firmware/devicetree/base/serial-number",
            "/proc/device-tree/serial-number"
        }, false));
    }

    /// <summary>
    /// Adds the CPU info (from /proc/cpuinfo) to the device identifier.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddCpuInfo(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("CPUInfo", new CpuInfoIdComponent());
    }

    /// <summary>
    /// Adds the motherboard serial number (from /sys/class/dmi/id/board_serial) to the device identifier.
    /// On ARM systems where DMI is not available, it falls back to the Device Tree model.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddMotherboardSerialNumber(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("MotherboardSerialNumber", new FileContentsDeviceIdComponent(new[]
        {
            "/sys/class/dmi/id/board_serial",
            "/sys/firmware/devicetree/base/model",
            "/proc/device-tree/model"
        }, false));
    }

    /// <summary>
    /// Adds the Device Tree serial number to the device identifier.
    /// This is typically available on ARM-based Linux systems.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddDeviceTreeSerialNumber(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("DeviceTreeSerialNumber", new FileContentsDeviceIdComponent(new[]
        {
            "/sys/firmware/devicetree/base/serial-number",
            "/proc/device-tree/serial-number"
        }, false));
    }

    /// <summary>
    /// Adds the Device Tree model to the device identifier.
    /// This is typically available on ARM-based Linux systems.
    /// </summary>
    /// <param name="builder">The <see cref="LinuxDeviceIdBuilder"/> to add the component to.</param>
    /// <returns>The <see cref="LinuxDeviceIdBuilder"/> instance.</returns>
    public static LinuxDeviceIdBuilder AddDeviceTreeModel(this LinuxDeviceIdBuilder builder)
    {
        return builder.AddComponent("DeviceTreeModel", new FileContentsDeviceIdComponent(new[]
        {
            "/sys/firmware/devicetree/base/model",
            "/proc/device-tree/model"
        }, false));
    }
}
