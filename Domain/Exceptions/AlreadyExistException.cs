namespace Domain.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string entity)
            : base($"{entity} {Messages.ALREADY_EXISTS}")
        {
        }
    }
}

