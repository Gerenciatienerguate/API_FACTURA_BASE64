using System.Text.Json;
using System.Text.Json.Serialization;

namespace API_CHATBOT_PDF.Models
{
    public class ResponseDto
    {
        //public bool Success { get; set; }
        //public string Message { get; set; }
        //public InvoinceFile Data { get; set; }
        //public int StatusCode { get; set; }
        [JsonPropertyName("Base64")]
        public string Base64 { get; set; }
        [JsonPropertyName("Error")]
        public string Error { get; set; }

    }

    public class InvoinceFile
    {
        public string Base64 { get; set; }

    }
}
