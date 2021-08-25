using XUCore.Template.FreeSql.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.FreeSql.DbService.Sys.Admin.AdminUser
{
    public class AdminUserLoginRecordViewModel : AdminUserLoginRecordEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}
