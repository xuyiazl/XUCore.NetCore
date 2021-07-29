using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using WebApi.Core.Enums;

namespace WebApi.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 管理员查询分页
    /// </summary>
    public class AdminUserQueryPagedCommand : Command<bool>
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        [Required]
        public int CurrentPage { get; set; } = 1;
        /// <summary>
        /// 分页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; } = 10;
        /// <summary>
        /// 搜索字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式 exp：“asc or desc”
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }


        public class Validator : CommandValidator<AdminUserQueryPagedCommand>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("页码");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("分页大小");
            }
        }
    }
}
