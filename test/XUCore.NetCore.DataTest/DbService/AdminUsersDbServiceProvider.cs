using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbService
{

    public class AdminUsersDbServiceProvider : DbServiceProvider<AdminUserEntity>, IAdminUsersDbServiceProvider
    {
        public AdminUsersDbServiceProvider(INigelReadDbRepository<AdminUserEntity> readRepository, INigelWriteDbRepository<AdminUserEntity> writeRepository)
            : base(readRepository, writeRepository)
        {

        }
    }


    //public class AdminUsersDbServiceProvider : NigelDbRepository<AdminUsersEntity>, IAdminUsersDbServiceProvider
    //{
    //    public AdminUsersDbServiceProvider(INigelDbEntityContext context)
    //        : base(context)
    //    {

    //    }
    //}

    //public interface IAdminUsersDbServiceProvider : INigelDbRepository<AdminUsersEntity>, IDbServiceProvider
    //{

    //}
}
