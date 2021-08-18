﻿using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Template.Easy.Core.Enums;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 菜单查询命令
    /// </summary>
    public class AdminMenuQueryCommand : ListCommand
    {
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<AdminMenuQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
