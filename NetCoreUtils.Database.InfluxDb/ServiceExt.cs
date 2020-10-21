using Microsoft.Extensions.DependencyInjection;

namespace NetCoreUtils.Database.InfluxDb
{
    static public class ServiceExt
    {
        static public void AddInfluxDb(this IServiceCollection services, InfluxDbSetting setting)
        {
            services.AddSingleton<IInfluxDbConnection>(x => new InfluxDbConnection(setting));
            services.AddTransient<IInfluxWriter, InfluxWriter>();
            services.AddTransient<IInfluxReader, InfluxReader>();
        }
    }
}
