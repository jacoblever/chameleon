using System;

namespace DataStore
{
    public class PersonNameNotUniqueException : Exception
    {
        public PersonNameNotUniqueException(string personName)
            : base($"The name {personName} is already taken")
        {
        }
    }
}
