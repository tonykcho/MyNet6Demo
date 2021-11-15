namespace MyNet6Demo.Domain.Exceptions
{
    public class ResourceAlreadyExistException : Exception
    {
        public ResourceAlreadyExistException(string message) : base(message)
        {

        }
    }
}