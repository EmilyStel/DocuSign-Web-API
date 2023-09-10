using System;
using Domain.Exceptions;

namespace Domain.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string entity)
            : base($"{entity} is invalid")
        {
        }
    }
}
