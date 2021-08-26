using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.NetCore.Data;
using XUCore.Template.FreeSql.Core.Enums;

namespace XUCore.Template.FreeSql.DbService.Auth.Menu
{
    /// <summary>
    /// 菜单查询命令
    /// </summary>
    public class MenuQueryPagedCommand : PageCommand
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

        public class Validator : CommandPageValidator<MenuQueryPagedCommand, bool>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
    }
}
