﻿using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Ddd.Domain;
using XUCore.Serializer;

namespace XUCore.NetCore.DataTest.Entities
{
    public class AdminUserEntity : Entity<long>
    {
        public AdminUserEntity()
        {
            AdminUserAddress = new List<AdminUserAddressEntity>();
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public DateTime Deleted_At { get; set; }
        public int Status { get; set; }
        public ICollection<AdminUserAddressEntity> AdminUserAddress { get; private set; }
    }

    public class AdminUserSimpleEntity : Entity<long>
    {
        public AdminUserSimpleEntity()
        {
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public long IdCount { get; set; }
    }
}
