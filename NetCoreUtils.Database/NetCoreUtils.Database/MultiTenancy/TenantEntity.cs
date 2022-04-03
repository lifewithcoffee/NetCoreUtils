namespace NetCoreUtils.Database.MultiTenancy
{
    /// <summary>
    /// Used as the based class for entities for a multi-tenant system
    /// </summary>
    public class TenantEntity
    {
        public string TenantId { get; set; }
    }
}
