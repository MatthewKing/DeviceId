using System.Diagnostics;

namespace DeviceId.Internal
{
    internal class CommandExecutor : ICommandExecutor
    {
        public static CommandExecutor Default { get; } = new CommandExecutor();

        public string Execute(string command, string arguments)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = command;
            psi.Arguments = arguments;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using var process = Process.Start(psi);

            var output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }
    }
}
