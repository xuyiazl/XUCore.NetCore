﻿using Sample.Easy.Persistence.Entities.Sys.Admin;

namespace Sample.Easy.Applaction.Admin
{
    public class AdminUserLoginRecordViewModel : AdminUserLoginRecordEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}