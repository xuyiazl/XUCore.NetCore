using System.Linq;
using XUCore.NetCore.Data;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminUser
{
    internal static class View
    {
        /// <summary>
        /// linq 关联查询，关联账号信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<AdminUserLoginRecordViewModel> Create(IDbContext db)
        {
            return
                from record in db.Set<AdminUserLoginRecordEntity>()
                    //关联账号，获取发布人信息
                join __admin__ in db.Set<AdminUserEntity>() on record.AdminId equals __admin__.Id into __tmp__admin__
                from admin in __tmp__admin__.DefaultIfEmpty()

                select new AdminUserLoginRecordViewModel
                {
                    AdminId = record.AdminId,
                    Id = record.Id,
                    LoginIp = record.LoginIp,
                    LoginTime = record.LoginTime,
                    LoginWay = record.LoginWay,
                    Mobile = admin.Mobile,
                    Name = admin.Name,
                    Picture = admin.Picture,
                    UserName = admin.UserName
                };
        }
    }
}
