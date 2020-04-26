namespace DeviceId.Internal
{
    internal interface ICommandExecutor
    {
        string Execute(string command, string arguments);
    }
}
