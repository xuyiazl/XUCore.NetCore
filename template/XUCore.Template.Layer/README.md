# XUCore.Template.Layer

XUCore.Template.Layer

三层api代码模板。 还原简单清晰的代码。

### 使用模板

本地安装模板：可详情见 `install-template.bat` 文件内

### 本地安装模板步骤：

如果已经装过旧版，可以先执行下面命令进行卸载：

```bash

dotnet new -u XUCore.Template.Layer

```

然后安装指定版本：

```bash

dotnet new --install XUCore.Template.Layer::1.1.5

```

### 创建项目

切换到指定创建的目录。

假设我们需要在 `E:\demo` 创建，

那么先切换到该目录

```XUCore.Template.Layer

cd E:\demo

```

然后执行下面命令创建项目

```bash

dotnet new XUCore.Template.Layer -n MyTest -o .

```

这里的 `XUCore.Template.Layer` 是使用模板短名称。

`MyTest` 为新创建的项目名称。


![avatar](http://www.3624091.com/1.png)


### 本地构建项目镜像到Docker（如果使用jenkins可以直接在jenkins里配置）

前提是dockerfile需要放在项目根目录，而非启动项目的目录

不采用微软自带的dockerfile，我们需要手工打包发布。

1、切换到项目目录

```bash

cd E:\demo\MyTest.WebApi

```

2、本地编译

```bash

dotnet build -c Release

```

3、本地发布（实际发布到当前项目的 bin/Release/net5.0/publish/ 目录）

```bash

dotnet publish -c Release

```

4、打包进docker

```bash

docker build -t mytest:0.0.1 .

```

从控制台中，我们可以看到打包过程

```bash

E:\MyTest>docker build -t mytest:0.0.1 .
[+] Building 14.7s (12/20)
 => [internal] load build definition from Dockerfile                                                               0.0s
 => => transferring dockerfile: 32B                                                                                0.0s
 => [internal] load .dockerignore                                                                                  0.0s
 => => transferring context: 2B                                                                                    0.0s
 => [internal] load metadata for mcr.microsoft.com/dotnet/sdk:5.0                                                  0.0s
[+] Building 15.1s (12/20)
 => [internal] load build definition from Dockerfile                                                               0.0s
[+] Building 30.4s (12/20)
 => [internal] load build definition from Dockerfile                                                               0.0s
 => => transferring dockerfile: 32B                                                                                0.0s
 => [internal] load .dockerignore                                                                                  0.0s
 => => transferring context: 2B                                                                                    0.0s
[+] Building 56.7s (21/21) FINISHED
 => [internal] load build definition from Dockerfile                                                               0.0s
 => => transferring dockerfile: 32B                                                                                0.0s
 => [internal] load .dockerignore                                                                                  0.0s
 => => transferring context: 2B                                                                                    0.0s
 => [internal] load metadata for mcr.microsoft.com/dotnet/sdk:5.0                                                  0.0s
 => [internal] load metadata for mcr.microsoft.com/dotnet/aspnet:5.0                                               0.0s
 => [build 1/7] FROM mcr.microsoft.com/dotnet/sdk:5.0                                                              0.0s
 => [internal] load build context                                                                                  1.2s
 => => transferring context: 119.91MB                                                                              1.2s
 => [base 1/5] FROM mcr.microsoft.com/dotnet/aspnet:5.0                                                            0.0s
 => CACHED [build 2/7] WORKDIR /src                                                                                0.0s
 => CACHED [build 3/7] COPY [MyTest.WebApi/MyTest.WebApi.csproj, MyTest.WebApi/]                                   0.0s
 => CACHED [build 4/7] RUN dotnet restore "MyTest.WebApi/MyTest.WebApi.csproj"                                     0.0s
 => [build 5/7] COPY . .                                                                                           0.4s
 => [build 6/7] WORKDIR /src/MyTest.WebApi                                                                         0.0s
 => [build 7/7] RUN dotnet build "MyTest.WebApi.csproj" -c Release -o /app/build                                  43.2s
 => [publish 1/1] RUN dotnet publish "MyTest.WebApi.csproj" -c Release -o /app/publish                            11.5s
 => CACHED [base 2/5] ADD [sources.list, /etc/apt/]                                                                0.0s
 => CACHED [base 3/5] RUN rm /etc/localtime                                                                        0.0s
 => CACHED [base 4/5] RUN ln -s /usr/share/zoneinfo/Asia/Shanghai /etc/localtime                                   0.0s
 => CACHED [base 5/5] WORKDIR /app                                                                                 0.0s
 => CACHED [final 1/2] WORKDIR /app                                                                                0.0s
 => CACHED [final 2/2] COPY --from=publish /app/publish .                                                          0.0s
 => exporting to image                                                                                             0.0s
 => => exporting layers                                                                                            0.0s
 => => writing image sha256:addb11051debc4cf74c7d048d15cafdb7edbf121ae54fc55960f1b46bce94fda                       0.0s
 => => naming to docker.io/library/mytest:0.0.1                                                                    0.0s
 => => #   Determining projects to restore...
Use 'docker scan' to run Snyk tests against images to find vulnerabilities and learn how to fix them

```

结束后，我们使用 `docker images` 查看打包好的镜像

```bash

E:\MyTest>docker images
REPOSITORY                        TAG       IMAGE ID       CREATED        SIZE
mytest                            0.0.1     addb11051deb   20 hours ago   258MB
mcr.microsoft.com/dotnet/sdk      5.0       1cfcb8589c29   12 days ago    631MB
mcr.microsoft.com/dotnet/aspnet   5.0       592a912e0dcb   12 days ago    205MB

```

3、启动容器

```bash

docker run --name my-test -d -p 8090:8090 mytest:0.0.1

```

启动后我们通过 `docker ps -a` 查看运行的镜像

```bash

E:\MyTest>docker ps -a
CONTAINER ID   IMAGE          COMMAND                  CREATED         STATUS                       PORTS     NAMES
35c9010404ae   mytest:0.0.1   "dotnet MyTest.WebAp…"   5 minutes ago   Exited (139) 4 minutes ago             my-test

```

4、浏览器访问

http://127.0.0.1:8090/swagger

此时容器会报错，因为在创建项目后配置文件内需要修改数据库连接地址（进入容器后，我们的所有数据库地址以及其他相关地址都需要修改为内网访问，或者公网访问）

我们可以查看容器的日志，方便我们定位错误

```bash

docker logs 35c9010404ae

```

此时，在容器日志，我们看到

```bash

fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]
      An error occurred using the connection to database '' on server 'localhost'.
fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]
      An error occurred using the connection to database '' on server 'localhost'.
fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]
      An error occurred using the connection to database '' on server 'localhost'.
fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]
      An error occurred using the connection to database '' on server 'localhost'.
fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]
      An error occurred using the connection to database '' on server 'localhost'.
fail: Microsoft.EntityFrameworkCore.Database.Connection[20004]

```



### 客户端接入API


------------

#### API隐藏操作说明

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | `contain` or `ignore`| `contain` 的意思是指定输出字段， `ignore` 的意思是忽略指定字段|
| limit-field | 字段集合  | 指定要输出或要忽略的字段，以英文逗号分隔，如：`column1,column2,column3` |
| limit-field-rename | 要重命名的字段  | 字段以输出为准，比如：`code=c,subCode=sub,data=data,nickname=userNickName` |
| limit-resolver  | `camelcase` or `default`| `camelcase` 的意思是指定输出小驼峰字段， `default` 的意思是默认输出大小写字段|
| limit-date-unix  | `true` or `false`| 当设置为`true`的时候 则是所有`DateTime`时间全部返回时间戳，当为`false`的时候不启用   |
| limit-date-format | 日期格式化字符串  | 比如：`yyyy-MM-dd'T'HH:mm:ss'Z'` 返回的数据如：`2021-01-06T10:03:38Z` |


#### 1、如何改变DateTime的格式？

我们需要在http请求的时候，在head里加入配置

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-date-unix  | `true` or `false`| 当设置为`true`的时候 则是所有`DateTime`时间全部返回时间戳，当为`false`的时候不启用   |
| limit-date-format | 日期格式化字符串  | 比如：`yyyy-MM-dd'T'HH:mm:ss'Z'` 返回的数据如：`2021-01-06T10:03:38Z` |

注意： `limit-date-unix`的优先级要大于`limit-date-format`

------------

#### 2、如何重命名和指定输出需要的字段？

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | `contain` or `ignore`| `contain` 的意思是指定输出字段， `ignore` 的意思是忽略指定字段|
| limit-field | 字段集合  | 指定要输出或要忽略的字段，以英文逗号分隔，如：`column1,column2,column3` |
| limit-field-rename | 要重命名的字段  | 字段以输出为准，比如：`code=c,subCode=sub,data=data,nickname=userNickName` |

注意：当你使用重命名`limit-field-rename`字段后，指定输出的字段`limit-field`要以重命名后的字段名为准，大小写也请依照你重命名后的格式。

> 任何指定输出，均不影响原始定义的结构。

#### 3、如何指定输出小驼峰字段？

| head  | value  | 说明  |
| ------------ | ------------ | ------------ |
| limit-resolver  | `camelcase` or `default`| `camelcase` 的意思是指定输出小驼峰字段， `default` 的意思是默认输出大小写字段|

> 任何指定输出，均不影响原始定义的结构。

### 示例一

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | contain  |  指定匹配输出模式 |
| limit-field  |  code,subCode,data,userId,userNickName,entName |   设置需要的字段集合，英文逗号分隔|

### 示例二

如下表设置：

|  设置 |  值 | 说明  |
| ------------ | ------------ | ------------ |
| limit-mode  | contain  |  指定匹配输出模式 |
| limit-field  |  code,sub,data,totalPages,totalRecords,pageDatas,createTime,nickName,entId,entName |   设置需要的字段集合，英文逗号分隔，并以重命名后的字段为准设置输出字段|
| limit-field-rename  | subcode=sub,data=data,items=pageDatas,userNickName=nickName  |  重命名字段 |

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
| Accept  | application/json |  指定输出json格式的json字符串格式|

# 适用范围定义

在一定程度上使API接入变得稍微复杂了一点点，但是能优化网络传输，或许我们可以考虑牺牲一点复杂度，按需索取来优化传输问题。

| 客户端  |  适合程度 |
| ------------ | ------------ |
| 移动端  | 非常适合  |
| web端  | 适合  |
| 服务端  | 跨语言，在不同规范的情况下适合接入，能解决`模型不一致`的问题，k8s内走内网地址不需要考虑这个问题  |