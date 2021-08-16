#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base

#更换阿里源
ADD ["sources.list", "/etc/apt/"]

#修改时区
RUN rm /etc/localtime 
RUN ln -s /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
CMD rm /etc/localtime && ln -s /usr/share/zoneinfo/Asia/Shanghai /etc/localtime

#添加libgdiplus库（视情况可选）
#RUN apt-get update -y && apt-get install -y libgdiplus && apt-get clean && ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
#RUN apt-get install -y ghostscript


#不采用微软自带的dockerfile，我们需要手工打包发布。

#1、切换到当前待发布的项目目录
#cd D:\demo\XUCore.Template.Layer.WebApi\XUCore.Template.Layer.WebApi
#
#2、本地编译
#
#dotnet build -c Release
#
#3、本地发布（实际发布到当前项目的 bin/Release/net5.0/publish/ 目录）
#
#dotnet publish -c Release
#
#4、打包进docker
#
#docker build -t mytest:0.0.1 .

EXPOSE 80

WORKDIR app
#项目设置里面将所有需要输出的xml文件输出到同一个doc目录下面
ADD ./bin/Release/net5.0/publish/ /app/
ENTRYPOINT ["dotnet","/app/XUCore.Template.Layer.WebApi.dll","--environment","Development"]

#以下部分为NetCore自动生成，【弃用】

#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#
#WORKDIR /src
#COPY ["XUCore.Template.Layer.WebApi/XUCore.Template.Layer.WebApi.csproj", "XUCore.Template.Layer.WebApi/"]
#RUN dotnet restore "XUCore.Template.Layer.WebApi/XUCore.Template.Layer.WebApi.csproj"
#COPY . .
#WORKDIR "/src/XUCore.Template.Layer.WebApi"
#RUN dotnet build "XUCore.Template.Layer.WebApi.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "XUCore.Template.Layer.WebApi.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet","XUCore.Template.Layer.WebApi.dll","--environment","Development"]
#
##配置启动端口
#ENV ASPNETCORE_URLS http://+:80