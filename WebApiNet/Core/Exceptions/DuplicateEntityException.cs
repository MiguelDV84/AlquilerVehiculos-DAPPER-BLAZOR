namespace WebApiNet.Core.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message) : base(message) { }
    }
}
