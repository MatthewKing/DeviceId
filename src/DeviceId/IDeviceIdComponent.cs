namespace DeviceId
{
    public interface IDeviceIdComponent
    {
        string Name { get; }

        string GetValue();
    }
}
