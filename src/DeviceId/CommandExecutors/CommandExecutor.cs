namespace DeviceId.CommandExecutors;

/// <summary>
/// Enumerate the various command executors that are available.
/// </summary>
public static class CommandExecutor
{
    /// <summary>
    /// Gets a command executor that uses /bin/bash to execute commands.
    /// </summary>
    public static ICommandExecutor Bash { get; } = new BashCommandExecutor();

    /// <summary>
    /// Gets a command executor that uses /bin/sh to execute commands.
    /// </summary>
    public static ICommandExecutor Sh { get; } = new ShCommandExecutor();
}
