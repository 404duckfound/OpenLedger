using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLedger.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = "Bad request") : base(message) { }
    }
}
