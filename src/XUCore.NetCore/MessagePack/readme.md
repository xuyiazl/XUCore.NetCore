

[TOC]


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

###  :tada: 该功能仅支持 `JSON` 格式的输出，对MessagePack无效 :tada:

起因：因在对接APP的时候我们一直在围绕着几个反复的问题在纠结：

1. 新迁移的NetCore API 对原来的PageModel的字段变更不支持以前老的格式（当然事已至此已经无法再改回老的模型了）

2. 在与APP对接的时候一直围绕DateTime的格式问题纠结，APP对于处理日期足够的麻烦。所以客户端一直要求需要时间戳。

> 故此在API脚手架新增加隐藏功能。


------------

:smirk::smirk::smirk::smirk::smirk::smirk::smirk::smirk::smirk::smirk::smirk:

#### 现在API可支持：

##### 1、自定义输出字段

> 决模型不一致的问题，直接改善了客户端对接各种API导致模型不一致的问题，也可以按需索取，减少不需可解决模型不一致的问题，直接改善了客户端对接各种API导致模型不一致的问题，也可以按需索取，减少不需要的字段输出，避免加大网络的传输

##### 2、对输出的字段重命名

> 在新老API切换过程中难免字段的命名变化，或者由于服务端的字段命名过长的问题导致增加网络传输压力等一些问题

##### 3、自定义DateTime格式的输出

> 因为日期格式的问题，按照老的方式，可能需要额外创建其他格式的字段输出，造成没必要的开销。

## 那么怎么使用新功能 :interrobang:

### :heavy_check_mark: 服务端接入


当然我们需要引用：

```csharp
<PackageReference Include="Bailun.NetCore.Common" Version="1.1.0" />
```

然后需要在注册`MessagePack`的时候配置`ContractResolver`即可`LimitPropsCamelCaseContractResolver`

如下配置代码：:arrow_down:

```csharp
//注册API MessagePack输出格式。 输入JSON/MessagePack  输出 JSON/MessagePack/MessagePack-Jackson
.AddMessagePackFormatters(options =>
{
    options.JsonSerializerSettings = new JsonSerializerSettings()
    {
        DateTimeZoneHandling = DateTimeZoneHandling.Local,
		//自定义JSON输出
        ContractResolver = new LimitPropsCamelCaseContractResolver()
    };
    //默认设置MessageagePack的日期序列化格式为时间戳，对外输出一致为时间戳的日期，不需要我们自己去序列化，自动操作。
    //C#实体内仍旧保持DateTime。跨语言MessageagePack没有DateTime类型。
    options.FormatterResolver = MessagePackSerializerResolver.UnixDateTimeFormatter;
    options.Options = MessagePackSerializerResolver.UnixDateTimeOptions;
});
```

------------



### :heavy_check_mark: 客户端接入

------------


#### 1、如何改变DateTime的格式？

我们需要在http请求的时候，在head里加入配置

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-date-unix  | `true` or `false`| 当设置为`true`的时候 则是所有`DateTime`时间全部返回时间戳，当为`false`的时候不启用   |
|  limit-date-format | 日期格式化字符串  | 比如：`yyyy-MM-dd'T'HH:mm:ss'Z'` 返回的数据如：`2021-01-06T10:03:38Z` |

注意： `limit-date-unix`的优先级要大于`limit-date-format`

------------


#### 2、如何重命名和指定输出需要的字段？

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | `contain` or `ignore`| `contain` 的意思是指定输出字段， `ignore` 的意思是忽略指定字段|
| limit-field | 字段集合  | 指定要输出或要忽略的字段，以英文逗号分隔，如：`column1,column2,column3` |
| limit-field-rename | 要重命名的字段  | 字段以输出为准，比如：`code=c,subCode=sub,bodyMessage=data,nickname=userNickName` |

注意：当你使用重命名`limit-field-rename`字段后，指定输出的字段`limit-field`要以重命名后的字段名为准，大小写也请依照你重命名后的格式。

> 任何指定输出，均不影响原始定义的结构。

### 示例一

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | contain  |  指定匹配输出模式 |
| limit-field  |  code,subCode,bodyMessage,userId,userNickName,entName |   设置需要的字段集合，英文逗号分隔|


### 示例二

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | contain  |  指定匹配输出模式 |
| limit-field  |  code,sub,data,totalPages,totalRecords,pageDatas,createTime,nickName,entId,entName |   设置需要的字段集合，英文逗号分隔，并以重命名后的字段为准设置输出字段|
| limit-field-rename  | subcode=sub,bodyMessage=data,items=pageDatas,userNickName=nickName  |  重命名字段 |

### 示例三

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | ignore  |  指定忽略输出模式 |
| limit-field  | code,subCode,message,userId,userNickName,entName,productType,userHeadImg_48 |   设置要忽略的字段集合，英文逗号分隔|

### 示例四

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | ignore  |  指定忽略输出模式 |
| limit-field  | code,subCode,message,userId,userNickName,entName,productType,userHeadImg_48 |   设置要忽略的字段集合，英文逗号分隔|
| limit-date-unix  | true |   设置DateTime输出时间戳|
| Accept  | application/bailun-json |  指定输出bailun-json格式的json字符串格式|

# 适用范围定义

在一定程度上使API接入变得稍微复杂了一点点，但是能优化网络传输，或许我们可以考虑牺牲一点复杂度，按需索取来优化传输问题。

| 客户端  |  适合程度 |
| ------------ | ------------ |
| 移动端  | 非常适合  |
| web端  | 适合  |
| 服务端  | 跨语言，在不同规范的情况下适合接入，能解决`模型不一致`的问题，k8s内走内网地址不需要考虑这个问题  |