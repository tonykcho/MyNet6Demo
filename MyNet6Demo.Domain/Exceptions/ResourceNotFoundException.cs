namespace MyNet6Demo.Domain.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string name) : base($"Entity ({name}) was not found.")
        {
        }
    }
}