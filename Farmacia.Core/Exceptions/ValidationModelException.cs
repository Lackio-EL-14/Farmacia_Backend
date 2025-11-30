using GenerativeAI.Exceptions;

namespace Farmacia.Core.Exceptions
{
    public class ValidationModelException : ApiException
    {
        public IEnumerable<string> Errors { get; }

        public ValidationModelException(IEnumerable<string> errors)
            : base("Validation error")
        {
            Errors = errors;
        }
    }
}
