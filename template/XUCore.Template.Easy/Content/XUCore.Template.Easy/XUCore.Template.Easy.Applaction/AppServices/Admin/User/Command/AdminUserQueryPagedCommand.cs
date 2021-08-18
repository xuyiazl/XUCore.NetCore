﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Template.Easy.Core.Enums;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 管理员查询分页
    /// </summary>
    public class AdminUserQueryPagedCommand : PageCommand
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


        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandPageValidator<AdminUserQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}