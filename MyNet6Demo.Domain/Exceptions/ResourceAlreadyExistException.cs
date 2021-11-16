namespace MyNet6Demo.Domain.Exceptions
{
    public class ResourceAlreadyExistException : Exception
    {
        public ResourceAlreadyExistException(string name) : base($"Entity ({name}) was already exist.")
        {

        }
    }
}