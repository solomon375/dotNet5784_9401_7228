using DO;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    public class DalNotExistException : Exception
    {
        public DalNotExistException(string? message) : base(message)
        {
        }

        public DalNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DalNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class DalAlreadyExistException : Exception
    {
        public DalAlreadyExistException(string? message) : base(message)
        {
        }

        public DalAlreadyExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DalAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class DalXMLFileLoadCreateException : Exception
    {
        public DalXMLFileLoadCreateException(string? message) : base(message)
        {
        }

        public DalXMLFileLoadCreateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DalXMLFileLoadCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
