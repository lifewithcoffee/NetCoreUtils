﻿# NetCoreUtils.Database.InfluxDb

## Dependency Injection Configuration

``` csharp
public void ConfigureServices(IServiceCollection services)
{
	services.AddInfluxDb(new InfluxDbSetting
	{
		Token = "4R1aL7t1hZolnMQezXQxkhhMGlqYUBy7g5Ue8RQAQ9wHn_XIHJN_2EpFqaYcD9F2wv_lt-kHqP8Ym99c7Gv5pw=="
	});
}
```

## Release Notes

## v1.4.0-working

- Upgrade to .net7

### v1.3.0.3

- Add non-POCO query method: `Task<PivotData> IInfluxReader.QueryAsync`

### v1.2.0.2

- Add PointModel class for json serialization
- Add InfluxWrite methods for PointModel

### v1.1.0.1

- Add writing by point, including point list and array
- Add writing POCO list and array

### v1.0.0.0

Initial release with the following implementation:

- InfluxReader, InfluxWriter, for database access
- MeasurementBase, for besing used as the base class of a data POCO class
- Extension method ServiceExt.AddInfluxDb(), for dependency injection configuration
