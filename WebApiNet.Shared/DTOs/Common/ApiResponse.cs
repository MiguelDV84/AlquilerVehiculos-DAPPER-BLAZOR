namespace WebApiNet.Application.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateOnly Timestamp { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
        public T? Data { get; set; }
    }
}
