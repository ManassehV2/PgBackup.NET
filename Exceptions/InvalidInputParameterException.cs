using System;
using System.Runtime.Serialization;

namespace PgBackup.Exceptions
{
    [Serializable]
    public class InvalidInputParameterException : PgException
    {
        public InvalidInputParameterException(string command, string args, Exception inner)
           : base("InvalidInputParameterException", $"Invalid pg client tool command:  {command} {args}.", inner)
        {

        }
        public InvalidInputParameterException(string command, string args)
          : base("InvalidInputParameterException", $"Invalid pg client tool command:  {command} {args}.", null)
        {

        }
        protected InvalidInputParameterException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {

        }

    }
}