namespace Darbu_records.Models
{
    public class DepartmentTopic
    {
        public int Id { get; set; }               
        public int DepartmentId { get; set; }     
        public int CreatedBy { get; set; }
        public string? Title { get; set; }      
        public string? Description { get; set; }
        public string Status { get; set; }= string.Empty;
        public int CommentCount { get; set; } = 0; 

        public string? Avatar {  get; set; }

        public int CommentRead {  get; set; } = 0;  
        public DateTime CreatedAt { get; set; }   








        public int GetCommentNotification()
        {
            return CommentCount - CommentRead;
        }
    }
}
