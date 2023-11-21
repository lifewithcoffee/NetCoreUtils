namespace NetCoreUtils.Database.MultiTenant
{
    /// <summary>
    /// Used as the based class for entities for a multi-tenant system
    /// </summary>
    public class TenantEntity
    {
        public string TenantId { get; set; }  // _todo_ need?
    }
}
