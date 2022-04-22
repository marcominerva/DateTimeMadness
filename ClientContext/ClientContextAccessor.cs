namespace ClientContext;

internal class ClientContextAccessor : IClientContextAccessor
{
    private static readonly AsyncLocal<IClientContext> clientContext = new();

    public IClientContext ClientContext
    {
        get => clientContext.Value;
        set => clientContext.Value = value;
    }
}
