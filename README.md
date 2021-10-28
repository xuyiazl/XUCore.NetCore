# XUCore.NetCore

.NET CORE Common 扩展库

以下插件并不是架构，而是扩展插件，但在项目中使用足以支持大部分项目的开发。且能减少不少的工作量。

## 🥥 框架拓展

|																																		| 名称								|下载																																			| 版本																																								| 描述						|
| ------------------------------------------------------------------------------------------------------------------------------------  | --------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------- |
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore)							| XUCore							| [![Downloads](https://img.shields.io/nuget/dt/XUCore.svg)](https://nuget.org/packages/XUCore)													| [![nuget](https://img.shields.io/nuget/v/XUCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore)													| XUCore 基础扩展包			|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Script)					| XUCore.Script					| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Script.svg)](https://nuget.org/packages/XUCore.Script)									| [![nuget](https://img.shields.io/nuget/v/XUCore.Script.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Script)									| Script 动态运行库			|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Excel)					| XUCore.Excel					| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Excel.svg)](https://nuget.org/packages/XUCore.Excel)									| [![nuget](https://img.shields.io/nuget/v/XUCore.Excel.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Excel)									| Excel 大文件读取			|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore)					| XUCore.NetCore					| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.svg)](https://nuget.org/packages/XUCore.NetCore)									| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore)									| NetCore 中间件特性扩展		|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Data)				| XUCore.NetCore.Data				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.Data.svg)](https://nuget.org/packages/XUCore.NetCore.Data)						| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Data.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Data)							| 数据库组件					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.FreeSql)				| XUCore.NetCore.FreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.FreeSql.svg)](https://nuget.org/packages/XUCore.NetCore.FreeSql)						| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.FreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.FreeSql)							| FreeSql数据库组件					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Mongo)				| XUCore.NetCore.Mongo				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.Mongo.svg)](https://nuget.org/packages/XUCore.NetCore.Mongo)						| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Mongo.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Mongo)						| Mongo 仓储组件				|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Redis)				| XUCore.NetCore.Redis				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.Redis.svg)](https://nuget.org/packages/XUCore.NetCore.Redis)						| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.Redis.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.Redis)						| Redis 仓储组件				|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.AspectCore)		| XUCore.NetCore.AspectCore			| [![Downloads](https://img.shields.io/nuget/dt/XUCore.NetCore.AspectCore.svg)](https://nuget.org/packages/XUCore.NetCore.AspectCore)			| [![nuget](https://img.shields.io/nuget/v/XUCore.NetCore.AspectCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.NetCore.AspectCore)				| Aspect AOP扩展				|
| -					| XUCore.Drawing					| -									| -									| Drawing 绘图扩展，合并到XUCore			|
| -		| XUCore.NetCore.DynamicWebApi      | -		| -		| 动态WebApi组件，合并到NetCore				|
| -		| XUCore.NetCore.AccessControl		| -		| -		| 资源授权插件（权限），合并到NetCore		|
| -			| XUCore.NetCore.Swagger			| -					| -					| Swagger文档扩展，合并到NetCore			|
| -				| XUCore.NetCore.Jwt				| -							| -							| Jwt的实现，正式弃用					|
| -				| XUCore.Ddd.Domain					| -							| -								| Ddd Domain扩展	，合并到NetCore			|


## 🥥 框架包

如果您不想一个一个的引用，那么可以直接引用`XUCoreApp`来直接使用所有包

|																																		| 名称								|下载																																			| 版本																																								| 描述						|
| ------------------------------------------------------------------------------------------------------------------------------------  | --------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------- |
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCoreApp)							| XUCoreApp							| [![Downloads](https://img.shields.io/nuget/dt/XUCoreApp.svg)](https://nuget.org/packages/XUCoreApp)													| [![nuget](https://img.shields.io/nuget/v/XUCore.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCoreApp)													| XUCore 组件包大集合			|

## 🥥 框架描述

|																																		| 名称								| 描述																				|
| ------------------------------------------------------------------------------------------------------------------------------------  | --------------------------------- | ----------------------------------------------------------------------------------|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore)							| XUCore							| XUCore 基础扩展包、Helper库														|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Script)					| XUCore.Script					| Script 动态运行库，方便动态脚本计算以及公式的运行等处理							|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Excel)					| XUCore.Excel					| Excel 大文件读取操作							|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore)					| XUCore.NetCore					| NetCore 中间件、Oss、Razor、Sign、Quartz、Jwt、上传等，基于NetCore的特性扩展			|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Data)				| XUCore.NetCore.Data				| 数据库组件，支持MSSQL、MYSQL等常用组件												|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.FreeSql)				| XUCore.NetCore.FreeSql				| 数据库组件，FreeSql扩展，和Data二选一												|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Mongo)				| XUCore.NetCore.Mongo				| Mongo 仓储组件																		|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.Redis)				| XUCore.NetCore.Redis				| Redis 仓储组件																		|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.NetCore.AspectCore)		| XUCore.NetCore.AspectCore			| Aspect扩展，缓存拦截器、事务等，自定义AOP											|
| -					| XUCore.Drawing					| Drawing 绘图扩展，验证码、图片缩放裁剪、图像灰度等操作处理，合并到NetCore							|
| -		| XUCore.NetCore.DynamicWebApi		| 动态WebApi组件，简化开发，Restful，合并到NetCore													|
| -		| XUCore.NetCore.AccessControl		| 资源授权插件（权限），支持MVC、Razor，API端请用XUCore.NetCore内的JWT，合并到NetCore					|
| -			| XUCore.NetCore.Swagger			| Swagger文档扩展，支持jwt登录存储，合并到NetCore													|
| -				| XUCore.NetCore.Jwt				| Jwt的实现，目前【弃用】准备用微软自带的，正式弃用											|
| -				| XUCore.Ddd.Domain					| 基于MediatR的Ddd Domain扩展，合并到NetCore														|


## 🍄 框架脚手架

|																																		| 名称								| 下载																																		| 版本																																								| 描述										|
| ------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------- |
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Ddd)				| XUCore.Template.Ddd				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Ddd.svg)](https://nuget.org/packages/XUCore.Template.Ddd)					| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Ddd.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Ddd)						| Ddd 架构模板（Mvc/Api,底层相通）				|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.EasyLayer)		| XUCore.Template.EasyLayer			| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.EasyLayer.svg)](https://nuget.org/packages/XUCore.Template.EasyLayer)				| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.EasyLayer.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.EasyLayer)					| 精简分层模板(默认WebApi,底层相通)						|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.Easy)				| XUCore.Template.Easy				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.Easy.svg)](https://nuget.org/packages/XUCore.Template.Easy)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.Easy.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.Easy)				| 单层应用模板(默认WebApi,底层相通)					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| XUCore.Template.FreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.FreeSql.svg)](https://nuget.org/packages/XUCore.Template.FreeSql)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.FreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.FreeSql)				| 基于FreeSql的 Api分层应用模板(默认WebApi,底层相通)					|
| [![nuget](https://shields.io/badge/-Nuget-blue?cacheSeconds=604800)](https://www.nuget.org/packages/XUCore.Template.EasyFreeSql)				| XUCore.Template.EasyFreeSql				| [![Downloads](https://img.shields.io/nuget/dt/XUCore.Template.EasyFreeSql.svg)](https://nuget.org/packages/XUCore.Template.EasyFreeSql)		| [![nuget](https://img.shields.io/nuget/v/XUCore.Template.EasyFreeSql.svg?cacheSeconds=10800)](https://www.nuget.org/packages/XUCore.Template.EasyFreeSql)				| 基于FreeSql的 Api 单层应用模板(默认WebApi,底层相通)					|



## 如何使用脚手架 

所有脚手架在模板中默认形态是支持WebApi（因为目前都是前后端分离），但是底层是相通的，所以只需自行创建对应的web工程即可。

### [XUCore.Template.Ddd](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Ddd)

### [XUCore.Template.EasyLayer](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.EasyLayer) 

建议使用，基于EFCore的简单分层应用（动态API+业务+EFCore的数据层），缩减了Controller，独立业务，数据持久化

### [XUCore.Template.Easy](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.Easy) 

不太建议使用，毕竟单层（动态API+业务+数据）集合在一起。

### [XUCore.Template.FreeSql](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.FreeSql)

建议使用，基于FreeSql的简单分层应用（动态API+业务+FreeSql的数据层），缩减了Controller，独立业务，数据持久化（FreeSql的性能上优于EFCore，但EFCore毕竟是官方出品）


### [XUCore.Template.EasyFreeSql](https://github.com/xuyiazl/XUCore.Template/tree/main/XUCore.Template.EasyFreeSql)

建议使用，基于FreeSql的单层应用（动态API+业务+FreeSql的结合），缩减了开发时间


## XUCore 功能概览

1、基础类型的扩展以及各种Helper操作类（内容太多，主要在Base目录下）
2、缓存，IMemoryCache的管理
3、集合封装，二叉树、优先级队列等集合操作
4、Config封装，XML配置以及json配置
5、连接池
6、工具类，控制台进度条、代码性能、运行时间、流量控制、重试、单元测试等
7、绘图工具，Image的扩展包括对图片的灰度处理、图片压缩、切图缩放、图片验证码等
8、Id生成器，包括Guid（有序Guid，二进制、字符串、末位排序等）、雪花、时间戳
9、分页插件以及分页扩展，Web页码生成器、PagedList、PagedModel、PagedTools等
10、队列
11、随机数生成器
12、序列化组件。JSON、MessagePack、转换器
13、线程锁，同步锁、异步锁
14、时间扩展以及范围操作
15、WebClient、Cookie、Url构造器

## XUCore.Excel 功能概览

Excel大文件读取操作，已经针对内存进行了优化。有效控制内存溢出问题

## XUCore.Script 功能概览

动态执行JS脚本语言库，有效的执行动态公式等操作。具体自行设计程序。

## XUCore.NetCore 功能概览

1、AccessControl，资源权限管理
2、Authorization、Jwt权限
3、Ddd、Ddd Domain模式公用库
4、Dependency、生命周期扫描器（Scoped、Singleton、Transient）
5、DynamicWebApi、动态Api（减少Controller的开发）
6、EasyQuartz、Quartz封装（简化计划任务的操作）
7、Filters、过滤器封装
8、Formatter、Api输出数据规范和控制（支持MessagePack，支持由客户端决定大小写等数据格式以及输出内容）
9、HttpFactory、HttpFactory封装（非注解方式）
10、Middlewares、部分常用中间件（真实IP、跨域、IP控制）
11、Oss、Oss上传和大文件上传
12、Razors、Razors静态化处理
13、Signature、Api安全签名
14、Swagger、Swagger封装（包括Swagger登录）
15、Uploads、上传组件（图片、文件、Base64）
16、Api RestFull规范

## XUCore.NetCore.Data 功能概览

基于EFCore的数据仓储服务

## XUCore.NetCore.FreeSql 功能概览

基于FreeSql的数据仓储服务

## XUCore.NetCore.Mongo 功能概览

基于Mongo的数据仓储服务

## XUCore.NetCore.Redis 功能概览

基于Redis的数据仓储服务

## XUCore.NetCore.AspectCore 功能概览

基于Aspect的AOP插件扩展，主要包含缓存的主动和被动处理