public interface ITenantProvider
{
    string GetTenantId();
}

public class EmptyTenantProvider : ITenantProvider
{
    public string GetTenantId()
    {
        return null;    // multi-tenant is not enabled
    }
}
