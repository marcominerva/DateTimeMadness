namespace ClientContext;

public interface IClientContextAccessor
{
    public IClientContext ClientContext { get; internal set; }
}
