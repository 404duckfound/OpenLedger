namespace OpenLedger.Application.Exceptions
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message = "Method not allowed.") : base(message) { }
    }
}
