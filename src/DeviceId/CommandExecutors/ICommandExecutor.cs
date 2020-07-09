namespace DeviceId.CommandExecutors
{
    /// <summary>
    /// Provides functionality to execute a command.
    /// </summary>
    public interface ICommandExecutor
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The command output.</returns>
        string Execute(string command);
    }
}
