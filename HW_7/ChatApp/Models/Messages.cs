namespace ChatApp.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public bool IsReceived { get; set; }

        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }

        public virtual Users ?ToUser { get; set; } 

        public virtual Users ?FromUser { get; set; }
    }
}
