﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Template.EasyLayer.Core.Enums;

namespace XUCore.Template.EasyLayer.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 管理员查询
    /// </summary>
    public class AdminUserQueryCommand : ListCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 排序方式 exp：“Id asc or Id desc”
        /// </summary>
        public string Orderby { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<AdminUserQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}