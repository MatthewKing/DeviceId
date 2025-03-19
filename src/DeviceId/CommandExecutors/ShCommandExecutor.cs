namespace DeviceId.CommandExecutors;

/// <summary>
/// An implementation of <see cref="ICommandExecutor"/> that uses /bin/sh to execute commands.
/// </summary>
public class ShCommandExecutor : CommandExecutorBase
{
    /// <summary>
    /// Executes the specified command.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <returns>The command output.</returns>
    public override string Execute(string command)
    {
        return RunWithShell("/bin/sh", $"-c \"{command.Replace("\"", "\\\"")}\"").Trim('\r').Trim('\n').TrimEnd().TrimStart();
    }
}
