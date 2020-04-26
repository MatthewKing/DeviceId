namespace DeviceId.Internal
{
    internal static class CommandExecutorExtensions
    {
        public static string Bash(this ICommandExecutor commandExecutor, string arguments)
        {
            var escapedArguments = arguments.Replace("\"", "\\\"");

            return commandExecutor.Execute("/bin/bash", $"-c \"{escapedArguments}\"");
        }
    }
}
