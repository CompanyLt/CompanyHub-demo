namespace Darbu_records.Models
{
    public class Comment
    {

        public long id { get; set; }

        public int topic_id { get; set; }

        public string title { get; set; }

        public string text { get; set; }

        public int created_by { get; set; }

        public DateTime created_at { get; set; }

        public int categoryId { set; get; }

        public int groupId { set; get; }


        public FileForm fileForm { get; set; } = new FileForm();



    }
}
