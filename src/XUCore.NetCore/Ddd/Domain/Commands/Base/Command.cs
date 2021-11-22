using FluentValidation.Results;
using System.Collections.Generic;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// 命令验证
        /// </summary>
        protected ValidationResult ValidationResult
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        protected Command()
        {
            ValidationResult = new ValidationResult();
        }
        /// <summary>
        /// 获取验证的错误消息
        /// </summary>
        /// <returns></returns>
        public virtual IList<ValidationFailure> GetErrors() => ValidationResult.Errors;
        /// <summary>
        /// 获取验证的错误消息
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public virtual string GetErrors(string separator) => ValidationResult.ToString(separator);
        /// <summary>
        /// 是否验证通过
        /// </summary>
        /// <returns></returns>
        public virtual bool IsVaild() => ValidationResult.IsValid;
    }
}
