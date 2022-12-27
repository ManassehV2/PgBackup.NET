using System;
using System.Runtime.Serialization;

namespace PgBackup.Exceptions
{
    [Serializable]
    public class DependencyPgToolsNotFoundException : PgException
    {
        public DependencyPgToolsNotFoundException(string toolName)
          : base("DependencyPgToolsNotFoundException", $"PostgreSQL client tool {toolName} does not appear to be installed on this linux system according to which command; go to https://www.postgresql.org/download/", null)
        {

        }
        protected DependencyPgToolsNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}