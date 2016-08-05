using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.SL.Core
{
    public class SLError
    {
        enum ExceptionType
        {
            NO_ERROR = 0,
            EXCEPTION = 1,
            ARGUMENT = 2,
            ARGUMENT_NULL = 3,
            ARGUMENT_OUT_OF_RANGE = 4,
            INVALID_OPERATION = 5,
            XAML_PARSE_EXCEPTION = 6,
            UNAUTHORIZED_ACCESS = 7,
            EXECUTION_ENGINE_EXCEPTION = 8,
            GCHANDLE_EXCEPTION = 9,
            LISTEN_FAILED = 10,
            SEND_FAILED = 11,
            NOT_IMPLEMENTED_EXCEPTION = 12,
            SECURITY_EXCEPTION = 13,
            NOT_SUPPORTED_EXCEPTION = 14
        };
    }
}
