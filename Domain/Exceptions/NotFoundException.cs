namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity)
        : base($"{entity} {Messages.NOT_FOUND}")
        {
        }
    }
}

