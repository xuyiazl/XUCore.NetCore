using XUCore.SimpleApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.SimpleApi.Template.Applaction.Login
{
    public class LoginRecordViewModel : LoginRecordEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}
