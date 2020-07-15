
Data使用说明

一、建立表实体对象

二、需要建立Mapping做映射，将实体对象和表做映射

三、建立DbRepository数据仓储

其中包含：

####数据库集群，读写分离

需要建立和配置 `ReadRepository` 和 `WriteRepository`

负责读写分离操作，在dbservices中为每个实体建立一个数据访问类，并继承`DbServiceBaseProvider`

dbservices层实现

切记继承 `IDbServiceProvider` 接口，用于扫描注入用

如下代码，一般情况下不需要写任何对数据库的操作，如有特殊情况，请自行新增查询方法（如需要用到SQL查询等等）。

```csharp
public class AdminUsersDbServiceProvider : DbServiceBaseProvider<AdminUsersEntity>, IAdminUsersDbServiceProvider
{
    public AdminUsersDbServiceProvider(INigelDbReadRepository<AdminUsersEntity> readRepository, INigelDbWriteRepository<AdminUsersEntity> writeRepository)
        : base(readRepository, writeRepository)
    {

    }

    /* todo : 如果默认提供的方法无法满足你的需求，那么可以在这里进行特殊数据操作，请勿在此处双向依赖其他表，容易出现循环依赖造成死循环 */

}
```

####单库操作（快速开发）

只需要建立和配置一个 `Repository`

在dbservices中为每个实体建立一个数据访问类，并继承 `NigelDbRepository`

dbservices层实现

切记继承 `IDbServiceProvider` 接口，用于扫描注入用

如下代码，一般情况下不需要写任何对数据库的操作，如有特殊情况，请自行新增查询方法（如需要用到SQL查询等等）。

```csharp
public class AdminUsersDbServiceProvider : NigelDbRepository<AdminUsersEntity>, IAdminUsersDbServiceProvider
{
    public AdminUsersDbServiceProvider1(INigelDbEntityContext context)
        : base(context)
    {
            
    }
    
    /* todo : 如果默认提供的方法无法满足你的需求，那么可以在这里进行特殊数据操作，请勿在此处双向依赖其他表，容易出现循环依赖造成死循环 */
}

public interface IAdminUsersDbServiceProvider : INigelDbRepository<AdminUsersEntity>, IDbServiceProvider
{

}
```

**或许我们不需要dbservices。直接在业务层引入 `NigelDbRepository<AdminUsersEntity>` 即可操作数据库。**

具体的实现demo请查看 `XUCore.NetCore.DataTest`

###目前支持

1、MsSql
2、MySql
3、Sqlite

无缝切换数据库只需要将 `DbRepository` 内修改成对应的实现即可