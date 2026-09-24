namespace Darbu_records.Models
{
    public class FileForm
    {

       

        public List<RecordFile> filesCollection { set; get; } = new List<RecordFile>();
        public List<RecordFile> pdfCollection { set; get; } = new List<RecordFile>();

        public List<RecordFile> photosCollection { set; get; } = new();



    }
}
