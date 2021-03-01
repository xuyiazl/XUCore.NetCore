

StartUp 注入

```csharp

services.AddHttpMessageService("test", "http://localhost:5000");

```

在使用中

```csharp

var url = urlBuilder.Create("/api/Auth/GetRoles");

var responseMessage = await HttpRemote.MessageService.CreateClient("test") //从工厂里拿取指定的client
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

```



### HttpRemote使用样例

##

```csharp

public async Task<bool> AddAsync(MemberOrderSubmitModel model, CancellationToken cancellationToken)
{
    var url = urlBuilder.Create($"/api/MemberOrder/Add");

    //指定服务器返回数据类型
    var mediaType = HttpMediaType.Json;

    var responseMessage = await HttpRemote.MessageService.CreateClient(ApiClient.ApiMaster) //从工厂拿取指定Client
        .SetHeaderBearerToken("accesstoken") //如果服务器需要认证授权，那么写入token
        .SetHeaderAccept(mediaType) //告诉服务器需要拿取JSON数据，如果服务器支持可以选择MessagePack数据
        .PostAsync(url, model, cancellationToken); //提交数据

    //判断http状态
    if (!responseMessage.IsSuccessStatusCode)
    {
        switch (responseMessage.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                //未授权，需要传递token
                break;
            case HttpStatusCode.Forbidden:
                //没有权限访问
                break;
        }
        return false;
    }

    var res = await responseMessage.Content.ReadAsAsync<ReturnModel>(mediaType);

    return res.code == 0 && res.subCode == "1701C00";
}

```

###请求方式 一、（该方法由于扩展性不强，仍然会持续维护，但不建议使用）

```csharp
{
    var url = urlBuilder.Create(ApiClient.ApiMaster, $"/api/MemberOrder/QuerySingle")
        .Add("id", id);

    var res = await HttpRemote.Service.GetAsync<ReturnModel>(url, cancellationToken);

    if (res.code == 0 && res.subCode == "1703C00")
    {
        return res.bodyMessage.SafeString().ToObject<ESMemberOrderModel>();
    }
    return null;
}

```

###请求方式 二、（由于该方式非常灵活，推荐使用）

```csharp
{
    var url = urlBuilder.Create($"/api/MemberOrder/QuerySingle")
        .Add("id", id);

    var responseMessage = await HttpRemote.MessageService.CreateClient(ApiClient.ApiMaster)
        .SetHeaderAccept(HttpMediaType.Json)
        .GetAsync(url, cancellationToken);

    var res = await responseMessage.Content.ReadAsJsonAsync<ReturnModel>();

    if (res != null && res.subCode == "1703C00")
        return res.bodyMessage.SafeString().ToObject<ESMemberOrderModel>();
    else
        return null;
}
```

###请求方式 三、（由于该方式非常灵活，推荐使用）

```csharp
{
    var mediaType = HttpMediaType.MessagePack;

    var serializerOptions = MessagePackSerializerResolver.UnixDateTimeOptions;

    var url = urlBuilder.Create($"/api/MemberOrder/QuerySingle")
        .Add("id", id);

    var responseMessage = await HttpRemote.MessageService.CreateClient(ApiClient.ApiMaster)
        .SetHeaderAccept(mediaType)
        .GetAsync(url, cancellationToken);

    if (!responseMessage.IsSuccessStatusCode)
    {
        //Http异常错误，根据具体业务情况而定
        switch (responseMessage.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                //在新的API中一般模型参数错误提示会在400中提示
                break;
            case HttpStatusCode.InternalServerError:
                //在新的API中一般全局错误提示会在500中提示
                break;
            case HttpStatusCode.Unauthorized:
                //没有权限
                break;
            default:
                //其他Http错误
                break;
        }
        //新的API中所有全局错误异常输出结构为 ReturnModel<string>（非业务异常）
        var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, serializerOptions);

        logger.LogWarning($"服务端异常状态：\r\n{responseMessage.RequestMessage}\r\n服务端返回日志：{errorMessage?.ToJson()}");

        return null;
    }

    var res = await responseMessage.Content.ReadAsAsync<ReturnModel<ESMemberOrderModel>>(mediaType, serializerOptions);

    if (res != null && res.subCode == "1703C00")
        return res.bodyMessage;
    else
        return null;
}
```

###请求方式 四、（由于该方式非常灵活，并且代码量少，推荐使用）

```csharp
{
    var mediaType = HttpMediaType.MessagePack;

    var serializerOptions = MessagePackSerializerResolver.UnixDateTimeOptions;

    var url = urlBuilder.Create(ApiClient.ApiMaster, $"/api/MemberOrder/QuerySingle")
        .Add("id", id);

    var res = await HttpRemote.MessageService.GetAsync<ESMemberOrderModel>(url, mediaType, serializerOptions,
        clientHandler: (client) =>
        {
            client.SetHeaderBearerToken("accesstoken");
        },
        errorHandler: async (responseMessage) =>
        {
            //Http异常错误，根据具体业务情况而定
            switch (responseMessage.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    //在新的API中一般模型参数错误提示会在400中提示
                    break;
                case HttpStatusCode.InternalServerError:
                    //在新的API中一般全局错误提示会在500中提示
                    break;
                case HttpStatusCode.Unauthorized:
                    //没有权限
                    break;
                default:
                    //其他Http错误
                    break;
            }
            var errorMessage = await responseMessage.Content.ReadAsAsync<ReturnModel<string>>(mediaType, serializerOptions);

            logger.LogWarning($"服务端异常状态：\r\n{responseMessage.RequestMessage}\r\n服务端返回日志：{errorMessage?.ToJson()}");

            return null;
        });

    if (res != null && res.subCode == "1703C00")
        return res.bodyMessage;
    else
        return null;
}
```

###请求方式 五、当然如果你不需要判断Http错误可以不实现errorHandler，（由于该方式很灵活，并且代码量少，推荐使用）

```csharp
{
    var mediaType = HttpMediaType.MessagePack;

    var serializerOptions = MessagePackSerializerResolver.UnixDateTimeOptions;

    var url = urlBuilder.Create(ApiClient.ApiMaster, $"/api/MemberOrder/QuerySingle")
        .Add("id", id);

    var res = await HttpRemote.MessageService.GetAsync<ESMemberOrderModel>(url, mediaType, serializerOptions);

    if (res != null && res.subCode == "1703C00")
        return res.bodyMessage;
    else
        return null;
}
```
