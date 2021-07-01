﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;

namespace XUCore.SimpleApi.Template.Applaction.Login
{
    /// <summary>
    /// 查询命令
    /// </summary>
    public class LoginRecordQueryPagedCommand : Command<bool>
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
        /// 查询字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式 exp:"asc or desc"
        /// </summary>
        public string Order { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandValidator<LoginRecordQueryPagedCommand>
        {
            public Validator()
            {

                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("CurrentPage");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(100).WithName("PageSize");
            }
        }
    }
}