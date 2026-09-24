using System.Text.Json.Serialization;

namespace Darbu_records.Models
{
    public class RecordFile
    {
        [JsonPropertyName("fileName")]
        public string name { set; get; } = string.Empty;

        [JsonPropertyName("originalName")]
        public string originalName {  set; get; } = string.Empty;

        [JsonPropertyName("image")]
        public string image {  set; get; } = string.Empty;

        [JsonPropertyName("fileDirectory")]
        public string directory { set; get; }
        [JsonPropertyName("fileDate")]
        public DateTime upload_date { set; get; }






    }
}
