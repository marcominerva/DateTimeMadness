namespace ClientContext;

public class ClientContextOptions
{
    public const string DefaultTimeZoneHeader = "X-Timezone";

    public string TimeZoneHeader { get; set; } = DefaultTimeZoneHeader;

    public TimeZoneInfo DefaultTimeZone { get; set; } = TimeZoneInfo.Utc;
}
