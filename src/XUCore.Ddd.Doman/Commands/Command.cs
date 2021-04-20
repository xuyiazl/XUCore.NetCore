using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using XUCore.Ddd.Doman.Events;

namespace XUCore.Ddd.Doman.Commands
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class Command<TResponse> : Message<TResponse>
    {
        //时间戳
        public DateTime Timestamp { get; private set; }
        protected ValidationResult ValidationResult
        {
            get;
            set;
        }

        protected Command()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public virtual IList<ValidationFailure> GetErrors() => ValidationResult.Errors;

        public virtual string GetErrors(string separator) => ValidationResult.ToString(separator);

        public virtual bool IsVaild()
        {
            return ValidationResult.IsValid;
        }
    }
}
