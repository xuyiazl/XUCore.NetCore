﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using Sample.Mini.Core.Enums;

namespace Sample.Mini.Applaction.Admin
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
        public int CurrentPage { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        [Required]
        public int PageSize { get; set; }
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


        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<AdminUserQueryPagedCommand>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
            }
        }
    }
}
