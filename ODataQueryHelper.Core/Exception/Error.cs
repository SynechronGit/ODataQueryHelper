using ODataQueryHelper.Core.Exception;
using System;

namespace ODataQueryHelper.Core
{
    public static class Error
    {
        public static void Null(string parameter)
        {
            throw new ArgumentNullException(parameter);
        }

        public static void NotImplemented(string message)
        {
            throw new NotImplementedException(message);
        }

        public static void PropertyNotFound(string message)
        {
            throw new PropertyNotFoundException(message);
        }

        public static void InvalidSkipValueException(string message)
        {
            throw new InvalidSkipValueException(message);
        }

        public static void InvalidTopValueException(string message)
        {
            throw new InvalidTopValueException(message);
        }
    }
}
