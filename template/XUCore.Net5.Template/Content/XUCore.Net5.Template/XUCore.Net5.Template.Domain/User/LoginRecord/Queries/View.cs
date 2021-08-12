using FluentValidation;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using System.Collections.Generic;
using System.Linq;

namespace XUCore.Net5.Template.Domain.User.LoginRecord
{
    internal static class View
    {
        /// <summary>
        /// linq 关联查询，关联账号信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<UserLoginRecordViewModel> Create(ITaxDbContext db)
        {
            return
                from record in db.UserLoginRecord
                    //关联账号，获取发布人信息
                join __user__ in db.User on record.UserId equals __user__.Id into __tmp__user__
                from user in __tmp__user__.DefaultIfEmpty()

                select new UserLoginRecordViewModel
                {
                    UserId = record.UserId,
                    Id = record.Id,
                    LoginIp = record.LoginIp,
                    LoginTime = record.LoginTime,
                    LoginWay = record.LoginWay,
                    Mobile = user.Mobile,
                    Name = user.Name,
                    Picture = user.Picture,
                    UserName = user.UserName
                };
        }
    }
}
