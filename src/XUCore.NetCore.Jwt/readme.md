
# jwt源代码出处 目前主要用于WebApi


源码出处 [![NuGet](https://img.shields.io/nuget/v/JWT.svg)](https://www.nuget.org/packages/JWT)

- 重写了中间件 JwtAuthenticationMiddleware

- 增加JwtAuthorizeAttribute过滤器验证权限

- 增加JwtAllowAnonymousAttribute


#### 注册jwt

```c#
 services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var appSection = Configuration.GetSection("JwtOptions");

var jwtSettings = appSection.Get<JwtOptions>();

services.AddJwtOptions(options => appSection.Bind(options));

services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    })
.AddJwt(JwtAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Keys = new[] { jwtSettings.Secret };
        options.VerifySignature = true;
    });
```

#### Controller 使用方式
```
[JwtAuthorize]
public class TokenController : ApiControllerBase
{
    JwtOptions _jwtOptions;
    public TokenController(ILogger<TokenController> logger, JwtOptions jwtOptions)
        : base(logger)
    {
        _jwtOptions = jwtOptions;
    }

    [Route("Create")]
    [HttpGet]
    [JwtAllowAnonymous]
    public IActionResult Create()
    {
        var token = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(_jwtOptions.Secret)
            .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds())
            .AddClaim("claim2", "claim2-value")
            .Build();
        return Success("000001", token);
    }

    [Route("Verify")]
    [HttpGet]
    public IActionResult Verify()
    {
        var users = RouteData.Values.GetValueOrDefault("identity");

        return Success("000002", message: "验证成功");
    }
}
```
#### appsettings.json配置
```

"JwtOptions": {
    "Secret": "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk"
  }

```

#### 如果想用中间件全局验证

```c#
app.UseJwtMiddleware();
```

