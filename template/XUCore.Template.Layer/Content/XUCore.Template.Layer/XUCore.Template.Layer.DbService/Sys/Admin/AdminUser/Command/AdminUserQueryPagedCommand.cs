using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Layer.Core.Enums;

namespace XUCore.Template.Layer.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 管理员查询分页
    /// </summary>
    public class AdminUserQueryPagedCommand : CommandPage<bool>
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序方式 exp：“Id asc or Id desc”
        /// </summary>
        public string OrderBy { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }


        public class Validator : CommandPageValidator<AdminUserQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
