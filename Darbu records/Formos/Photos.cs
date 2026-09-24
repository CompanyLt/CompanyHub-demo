using System.Text.Json.Serialization;

namespace Darbu_records.Formos
{
    public class Photos
    {
        [JsonPropertyName("photoName")]
        public string? name { set; get; }

        [JsonPropertyName("photoDirectory")]
        public string directory { set; get; }
        [JsonPropertyName("photoDate")]
        public DateTime? upload_date { set; get; }




      
    }
}
