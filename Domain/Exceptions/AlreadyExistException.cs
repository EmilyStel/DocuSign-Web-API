using System;
using Domain.Exceptions;

namespace Domain.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException(string entity)
            : base($"{entity} already exists")
        {
        }
    }
}

