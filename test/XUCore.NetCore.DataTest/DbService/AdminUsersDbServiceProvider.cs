﻿using System;
using System.Collections.Generic;
using System.Text;
using XUCore.NetCore.Data.DbService.ServiceProvider;
using XUCore.NetCore.DataTest.DbRepository;
using XUCore.NetCore.DataTest.Entities;

namespace XUCore.NetCore.DataTest.DbService
{

    public class AdminUsersDbServiceProvider : DbServiceBaseProvider<AdminUsersEntity>, IAdminUsersDbServiceProvider
    {
        public AdminUsersDbServiceProvider(INigelDbReadRepository<AdminUsersEntity> readRepository, INigelDbWriteRepository<AdminUsersEntity> writeRepository)
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
