namespace ClientContext;

internal class DefaultClientContext : IClientContext
{
    public TimeZoneInfo TimeZone { get; set; }

    public DefaultClientContext(TimeZoneInfo timeZone)
    {
        TimeZone = timeZone;
    }
}
