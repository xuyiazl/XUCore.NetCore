﻿using System.Linq;
using XUCore.Template.EasyLayer.Persistence;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminUser
{
    internal static class View
    {
        /// <summary>
        /// linq 关联查询，关联账号信息
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<AdminUserLoginRecordViewModel> Create(DefaultDbContext db)
        {
            return
                from record in db.AdminLoginRecord
                    //关联账号，获取发布人信息
                join __admin__ in db.AdminUser on record.AdminId equals __admin__.Id into __tmp__admin__
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
