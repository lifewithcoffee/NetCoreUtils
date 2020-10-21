namespace NetCoreUtils.Database.InfluxDb
{
    public interface IInfluxAccess
    {
        void ChangeOrganizationBucket(string org, string bucket);
    }

    public class InfluxAccess : IInfluxAccess
    {
        protected string _org = "defaultOrg";
        protected string _bucket = "defaultBucket";

        public void ChangeOrganizationBucket(string org, string bucket)
        {
            _org = org;
            _bucket = bucket;
        }
    }
}
