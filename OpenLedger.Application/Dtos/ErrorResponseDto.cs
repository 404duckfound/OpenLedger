namespace OpenLedger.Application.Dtos
{
    public record ErrorResponseDto(int StatusCode, string ExceptionType, List<string> Errors, string TraceId)
    {
        public int StatusCode { get; set; } = StatusCode;
        public string ExceptionType { get; set; } = ExceptionType;
        public List<string> Errors { get; set; } = Errors;
        public string TraceId { get; set; } = TraceId;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
