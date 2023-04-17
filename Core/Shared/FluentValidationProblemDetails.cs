using Microsoft.AspNetCore.Mvc;

namespace Core.Shared
{
    public class FluentValidationProblemDetails : ProblemDetails
    {
        public IEnumerable<Error>? ValidationErrors { get; set; }
    }
}
