namespace OpenLedger.API.Middlewares
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Not found.") : base(message) { }
    }
}