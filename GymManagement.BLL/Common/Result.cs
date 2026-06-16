using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public record Result(bool success, string? error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result Ok() => new (true);
        public static Result fail(string message, ResultKind kind = ResultKind.Conflict) => new (false, message, kind);
        public static Result NotFound(string message = "Not Found") => new (false, message, ResultKind.NotFound);
        public static Result Validation(string message) => new (false, message, ResultKind.ValidationFailed);
    }
    public record Result<T>(bool success, T? Value, string? error = null, ResultKind Kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new (true, value);
        public static Result<T> fail(string message, ResultKind kind = ResultKind.Conflict) => new (false, default, message, kind);
        public static Result<T> NotFound(string message = "Not Found") => new (false, default, message, ResultKind.NotFound);
        public static Result<T> Validation(string message) => new (false, default, message, ResultKind.ValidationFailed);
    }
}
