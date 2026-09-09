namespace OpenLedger.Application.Dtos.Error
{
    public record ErrorResponseDto(int StatusCode, string ExceptionType, List<string> Errors, string TraceId)
    {
        public int StatusCode { get; private set; } = StatusCode;
        public string ExceptionType { get; private set; } = ExceptionType;
        public List<string> Errors { get; private set; } = Errors;
        public string TraceId { get; private set; } = TraceId;
        public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    }
}
