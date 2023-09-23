using System.Text.Json.Serialization;

namespace api.ViewModels
{
    public class Result
    {
        public Result(string message, object? data = null)
        {
            Message = message;
            Data = data;
        }

        public Result(IDictionary<string, string[]>? errors)
        {
            Errors = errors; 
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Data { get; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, string[]>? Errors { get; }
    }
}
