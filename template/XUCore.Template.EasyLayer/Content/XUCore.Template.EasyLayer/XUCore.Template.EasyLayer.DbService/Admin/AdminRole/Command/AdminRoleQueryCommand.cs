using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;
using XUCore.Template.EasyLayer.Core.Enums;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminRole
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class AdminRoleQueryCommand : ListCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandLimitValidator<AdminRoleQueryCommand, bool>
        {
            public Validator()
            {

            }
        }
    }
}
