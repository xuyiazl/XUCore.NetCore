using XUCore.Template.Layer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Layer.DbService.Sys.Admin.LoginRecord
{
    public class LoginRecordViewModel : LoginRecordEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}
