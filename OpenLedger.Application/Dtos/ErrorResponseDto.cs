namespace OpenLedger.Application.Dtos
{
    public class ErrorResponseDto(int StatusCode, string Message, string Exception, List<string> Errors, string TraceId)
    {
        public int StatusCode { get; set; } = StatusCode;
        public string Message { get; set; } = Message;
        public string Exception { get; set; } = Exception;
        public List<string> Errors { get; set; } = Errors;
        public string TraceId { get; set; } = TraceId;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
    }
}
