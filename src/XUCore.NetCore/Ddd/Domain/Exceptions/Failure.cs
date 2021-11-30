using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using XUCore.Extensions;

namespace XUCore.Ddd.Domain
{
    public static class Failure
    {
        public static void Error(List<ValidationFailure> failures)
        {
            throw new ValidationException(failures);
        }

        public static void Error(string propertyName, string error)
        {
            throw new ValidationException(new List<ValidationFailure> { new ValidationFailure(propertyName, error) });
        }

        public static void Error(string error) => Error("", error);

        public static bool IsFailure(this Exception ex)
        {
            return ex is ValidationException;
        }

        public static bool ThrowValidation(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
                Error(validationResult.Errors);

            return true;
        }

        public static string GetError(this ValidationException ex)
        {
            return ex.Failures.Select(c => c.Value.Join("")).Join("");
        }
    }
}
