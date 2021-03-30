
### 缓存拦截器

注册：

```cshar

public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               //......其他代码省略
               //注入缓存任务，这里的作用是让其AOP起作用
                .UseCacheHostBuilder();

```

```csharp

public void ConfigureServices(IServiceCollection services)
{
    //注册缓存服务，暂时只提供内存缓存，后面会增加Redis，需要视情况而定
    services.AddCacheService<MemoryCacheService>();
}

```

```csharp

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //*******启用缓存服务*******
    //此方法适合web和api
    app.UseCacheService();
}

```

使用

主动缓存方式：

该方式没有找到缓存的时候会请求方法拿数据。
适用于缓存文章详情、交易商详情等。因为我们并不知道用户会看哪个数据，所以不适合把所有详情数据都自动同步到web里。

```csharp

[CacheMethod(Key = "AdsService_GetAdsByPositionIdsList", ParamterKey = "{0}", Seconds = CacheTime.Min5)]
public async Task<List<AdsAdsInationOutPutModel>> GetAdsByPositionIdsList(int[] ids, CancellationToken cancellationToken)
{
    //....
}

```

被动缓存方式：

由于后置刷新缓存需要调度方法，所以在使用该方式的时候必须要注意，服务一定得是单例模式，不然请求后服务会被释放掉，任务无法访问一个已经被释放的资源。

```csharp

[CacheTigger(Key = "ReputationService_GetMonthRank", ParamterKey = "{0}_{1}_{2}_{3}", Seconds = CacheTime.Min2)]

```

该方式适用于一次性拿数据的请求，不适合根据参数拿数据的请求
比如：
交易商分页？文章分页？不适合，分页类的都不适合
获取相对实时的数据？适合，为了避免前端的js疯狂请求拿取数据导致API服务器扛不住？那么这个时候我们可以通过这种方式阻断前端请求。被动更新数据。


最后特别提醒：
不管是哪一种方式，我们需要斟酌具体的使用场景来选择哪个方式合适。过度使用都会出现不可预测的问题。

如果是整站使用，则会导致内存不足的情况，所以选择性的优化使用。如果整站使用还不如全站静态化。