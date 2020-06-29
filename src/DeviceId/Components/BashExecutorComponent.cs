using System.Collections.Generic;
using DeviceId.Internal;

namespace DeviceId.Components
{
    /// <summary>
    /// An implementation of <see cref="IDeviceIdComponent"/> to execute bash commands
    /// </summary>
    public class BashExecutorComponent : IDeviceIdComponent
    {
        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; } = "BashExecutorComponent";
        public string Command { get; } = "echo 'set command'";

        /// <summary>
        /// Command executor.
        /// </summary>
        private readonly ICommandExecutor _commandExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="BashExecutorComponent"/> class.
        /// </summary>
        public BashExecutorComponent(string name, string command)
            : this(CommandExecutor.Default)
        {
            Name = name;
            Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BashExecutorComponent"/> class.
        /// </summary>
        /// <param name="commandExecutor">Command executor.</param>
        internal BashExecutorComponent(ICommandExecutor commandExecutor)
        {
            _commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Gets the component value by executing given bash command.
        /// </summary>
        /// <returns>The component value.</returns>
        public string GetValue()
        {
            var result = _commandExecutor.Bash(Command);
            /// remove newline chars \n, whitespaces at start/end if exists
            return result.Trim('\n').TrimStart().TrimEnd();
        }
    }
}
