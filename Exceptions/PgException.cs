using System;
using System.Net;
using System.Runtime.Serialization;

namespace PgBackup.Exceptions
{

    [Serializable]
    public class PgException : Exception
    {
        public string Error { get; set; }

        public PgException(string errorCode, string message, Exception inner = null) : base(message, inner)
        {
            Error = errorCode;
        }
        protected PgException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {

        }

    }
}
