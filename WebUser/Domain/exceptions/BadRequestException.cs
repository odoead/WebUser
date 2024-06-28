namespace WebUser.Domain.exceptions
{
    public abstract class BadRequestException : Exception
    {
        protected BadRequestException(string message)
            : base(message) { }
    }
}
