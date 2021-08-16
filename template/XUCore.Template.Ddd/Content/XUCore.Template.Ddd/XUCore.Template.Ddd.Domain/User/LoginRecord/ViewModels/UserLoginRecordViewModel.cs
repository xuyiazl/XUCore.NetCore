using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    public class UserLoginRecordViewModel : UserLoginRecordEntity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string UserName { get; set; }
        public string Picture { get; set; }
    }
}
