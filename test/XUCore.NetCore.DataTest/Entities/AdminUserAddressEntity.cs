﻿using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Ddd.Domain;
using XUCore.Serializer;

namespace XUCore.NetCore.DataTest.Entities
{
    public class AdminUserAddressEntity : Entity<long>
    {
        public long UserId { get; set; }
        public string Address { get; set; }
        public AdminUserEntity AdminUser { get; set; }
    }
}
