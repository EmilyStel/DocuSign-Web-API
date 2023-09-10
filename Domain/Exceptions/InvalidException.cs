namespace Domain.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string entity)
            : base($"{entity} {Messages.IS_INVALID}")
        {
        }
    }
}
