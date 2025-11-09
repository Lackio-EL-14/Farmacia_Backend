
namespace Farmacia.Core.Exceptions
{
    public class BusinessException : ApiException
    {
        public BusinessException(string message)
            : base(message, 400) { }
    }
}
