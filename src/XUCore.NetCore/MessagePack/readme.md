###MessagePack API输出

Startup注入MessagePackFormatters

```csharp

services.AddControllers(options =>
{
    options.MaxModelValidationErrors = 50;
})
//注册API MessagePack输出格式。 输入JSON/MessagePack  输出 JSON/MessagePack/MessagePack-Jackson
.AddMessagePackFormatters(options =>
{
    options.JsonSerializerSettings = new JsonSerializerSettings()
    {
        //统一设置JSON格式输出为utc
        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        //统一设置JSON为小驼峰格式
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };
    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
});

```

在控制器头部添加MessagePackProduces标签

```csharp

[Route("api/[controller]/[Action]")]
[ApiController]
[MessagePackRequestContentType("application/json")]
[MessagePackResponseContentType]
public class MessagePackController : ControllerBase
{
    [HttpGet]
    public User Get()
    {
        return new User { Id = 1, Name = "test", CreateTime = DateTime.Now };
    }


    [HttpPost]
    public User Add([FromBody] User user)
    {
        user.Name = "哈哈";
        user.CreateTime = DateTime.Now;

        return user;
    }
}

```

请求API
通过使用HttpService请求自动解析

StartUp 注入 HttpService

```csharp

services.AddHttpMessageService("msgpack", "http://localhost:57802");

```

在构造函数中引入 IHttpService

```csharp

private readonly IHttpMessageService _httpService;

public HomeController(IHttpMessageService httpService)
{
    _httpService = httpService;
}

```

实体类

```csharp

[MessagePackObject]
public class User
{
    [Key(0)]
    public int Id { get; set; }
    [Key(1)]
    public string Name { get; set; }
    [Key(2)]
    public DateTime CreateTime { get; set; }
}

```

使用并请求相应API

```csharp

var url = UrlArguments.Create("/api/Auth/GetRoles");

var responseMessage = await _httpService.CreateClient("test") //从工厂里拿取指定的client
    .SetHeaderAccept(HttpMediaType.MessagePack) //告诉client需要拿取什么样的数据格式  json or messagepack
    .SetHeaderBearerToken(tokenResponse.AccessToken) //写入token
    .PostAsync(url, new List<string>() { "0001" }); //请求服务端

//判断http状态
if (!responseMessage.IsSuccessStatusCode)
{
    Console.WriteLine(responseMessage.StatusCode);
}
else
{
    //拿取指定的MessagePack数据
    var content = await responseMessage.Content.ReadAsMessagePackAsync<ReturnModel<UserAuthTreeModel>>();

    Console.WriteLine(content);
}

//或者

var url = UrlArguments.Create(ApiClient.ApiLive, $"/api/UserVirtual/GetRandomUser")
    .Add("aid", aid);

//指定服务器返回数据类型
var mediaType = HttpMediaType.MessagePack;

var res = await HttpRemote.MessageService.GetAsync<ReturnModel<string>>(url, mediaType, cancellationToken: cancellationToken,
    errorHandler: responseMessage =>
    {
        //记录日志 或者 其他异常逻辑处理
        return null;
    });

if (res != null && res.subCode == "B400900")
    return res.bodyMessage.ToObject<UserVirtualModel>();
return null;

//或者


var mediaType = HttpMediaType.MessagePack;

var serializerOptions = MessagePackSerializerResolver.UnixDateTimeOptions;

var url = UrlArguments.Create(ApiClient.ApiMaster, $"/api/MemberOrder/QuerySingle")
    .Add("id", id);

var res = await HttpRemote.MessageService.GetAsync<ESMemberOrderModel>(url, mediaType, serializerOptions);

if (res != null && res.subCode == "1703C00")
    return res.bodyMessage;
else
    return null;
```