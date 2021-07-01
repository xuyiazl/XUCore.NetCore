﻿# XUCore.NetCore

.NET CORE Common 扩展库


#### 原则：只使用插件化的框架，不使用集成式框架，方便自由插拔。


## 🥥 框架拓展

|                                                                     包类型                                                                      | 名称                                       |                                                                                          版本                                                                                           | 描述                       |
| :---------------------------------------------------------------------------------------------------------------------------------------------: | ------------------------------------------ | :-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: | -------------------------- |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore)                   | XUCore                                     |                                     [![nuget](https://img.shields.io/nuget/v/XUCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore)                                     | XUCore 基础扩展包、Helper库              |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Drawing) | XUCore.Drawing | [![nuget](https://img.shields.io/nuget/v/XUCore.Drawing.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Drawing) | Drawing 绘图扩展，验证码、图片缩放裁剪、图像灰度等操作处理 |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore)   | XUCore.NetCore     |     [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore)     | NetCore 中间件、Oss、Razor、Sign、Quartz、Jwt、上传等，基于NetCore的特性扩展          |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Data)        | XUCore.NetCore.Data              |              [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Data.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Data)              | 数据库组件，支持MSSQL、MYSQL等常用组件  |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Mongo)     | XUCore.NetCore.Mongo         |         [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Mongo.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Mongo)         | Mongo 仓储组件      |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Redis)   | XUCore.NetCore.Redis    |    [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Redis.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Redis)    | Redis 仓储组件 |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.DynamicWebApi)    | XUCore.NetCore.DynamicWebApi      |      [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.DynamicWebApi.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.DynamicWebApi)      | 动态WebApi组件，简化开发，Restful       |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.AspectCore)   | XUCore.NetCore.AspectCore     |     [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.AspectCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.AspectCore)     | Aspect扩展，缓存拦截器、事务等，自定义AOP      |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.AccessControl)        | XUCore.NetCore.AccessControl              |              [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.AccessControl.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.AccessControl)              | 资源授权插件（权限），支持MVC、Razor，API端请用XUCore.NetCore内的JWT      |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Swagger)        | XUCore.NetCore.Swagger              |              [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Swagger.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Swagger)              | Swagger文档扩展，支持jwt登录存储      |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Jwt)        | XUCore.NetCore.Jwt              |              [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Jwt.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Jwt)              | Jwt的实现，目前【弃用】准备用微软自带的      |
|[![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Ddd.Domain)        | XUCore.Ddd.Domain              |              [![nuget](https://img.shields.io/nuget/v/XUCore.Ddd.Domain.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Ddd.Domain)              | 基于MediatR的Ddd Domain扩展      |

[![nuget](https://img.shields.io/badge/anything-youlike-brightgreen.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore)

[![Version](https://img.shields.io/nuget/v/XUCore.svg)](https://nuget.org/packages/XUCore)
[![Downloads](https://img.shields.io/nuget/dt/XUCore.svg)](https://nuget.org/packages/XUCore)
[![Donate](https://img.shields.io/badge/donate-XUCore-purple.svg)](https://tyrrrz.me/donate)
[![Extra Services](https://img.shields.io/badge/XUCore:code-blue.svg)](https://xscode.com/Tyrrrz/YoutubeExplode)

## 🍄 框架脚手架

|                                                                 模板类型                                                                 | 名称                             |                                                                                 版本                                                                                 | 描述                   |
| :--------------------------------------------------------------------------------------------------------------------------------------: | -------------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------: | ---------------------- |
|[![nuget](https://shields.io/badge/-Nuget-yellow?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Net5.Template)        | XUCore.Net5.Template              |              [![nuget](https://img.shields.io/nuget/v/XUCore.Net5.Template.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Net5.Template)              | Mvc/WebApi 模板（Ddd架构模式）               |
|[![nuget](https://shields.io/badge/-Nuget-yellow?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.WebApi.Template)        | XUCore.WebApi.Template              |              [![nuget](https://img.shields.io/nuget/v/XUCore.WebApi.Template.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.WebApi.Template)              | WebApi 模板（三层模式）           |
|[![nuget](https://shields.io/badge/-Nuget-yellow?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.SimpleApi.Template)        | XUCore.SimpleApi.Template              |              [![nuget](https://img.shields.io/nuget/v/XUCore.SimpleApi.Template.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.SimpleApi.Template)              | WebApi 模板（单层快速开发）        |

[如何使用脚手架 XUCore.Net5.Template](https://github.com/xuyiazl/XUCore.NetCore/tree/master/template/XUCore.Net5.Template)

[如何使用脚手架 XUCore.WebApi.Template](https://github.com/xuyiazl/XUCore.NetCore/tree/master/template/XUCore.WebApi.Template)

[如何使用脚手架 XUCore.SimpleApi.Template](https://github.com/xuyiazl/XUCore.NetCore/tree/master/template/XUCore.SimpleApi.Template)