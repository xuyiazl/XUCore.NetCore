using FluentValidation;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System.Collections.Generic;
using System.Linq;

namespace XUCore.Net5.Template.Domain.Sys.LoginRecord
{
    internal static class View
    {
        /// <summary>
        /// linq 关联查询，关联账号信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<LoginRecordViewModel> Create(INigelDbContext db)
        {
            return
                from record in db.AdminLoginRecord
                    //关联账号，获取发布人信息
                join __admin__ in db.AdminUser on record.AdminId equals __admin__.Id into __tmp__admin__
                from admin in __tmp__admin__.DefaultIfEmpty()

                select new LoginRecordViewModel
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
