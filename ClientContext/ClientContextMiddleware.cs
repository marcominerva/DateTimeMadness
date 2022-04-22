using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using TimeZoneConverter;

namespace ClientContext;

internal class ClientContextMiddleware
{
    private readonly RequestDelegate next;
    private readonly IClientContextAccessor clientContextAccessor;
    private readonly ClientContextOptions clientContextOptions;

    public ClientContextMiddleware(RequestDelegate next, IClientContextAccessor clientContextAccessor, ClientContextOptions clientContextOptions)
    {
        this.next = next;
        this.clientContextAccessor = clientContextAccessor;
        this.clientContextOptions = clientContextOptions;
    }

    public async Task Invoke(HttpContext context)
    {
        clientContextAccessor.ClientContext = new DefaultClientContext(clientContextOptions.DefaultTimeZone);

        try
        {
            var hasTimeZoneHeader = context.Request.Headers.TryGetValue(clientContextOptions.TimeZoneHeader, out var timeZone) && !StringValues.IsNullOrEmpty(timeZone);
            if (hasTimeZoneHeader)
            {
                var isValidTimeZone = TZConvert.TryGetTimeZoneInfo(timeZone.ToString(), out var timeZoneInfo);
                if (isValidTimeZone)
                {
                    clientContextAccessor.ClientContext.TimeZone = timeZoneInfo;
                }
            }
        }
        catch
        {
        }

        await next(context);
    }
}
