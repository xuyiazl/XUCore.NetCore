using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;
using XUCore.Template.FreeSql.Core.Enums;

namespace XUCore.Template.FreeSql.DbService.Auth.Role
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class RoleQueryPagedCommand : PageCommand
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 启用
        /// </summary>s
		public bool Enabled { get; set; } = true;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public class Validator : CommandPageValidator<RoleQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
