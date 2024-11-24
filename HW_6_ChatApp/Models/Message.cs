namespace HW_6_ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        public string? Text { get; set; }
        
        public DateTime DateMessage { get; set; }
        
        public bool IsReceived { get; set; }

        public int? ToUserId { get; set; }

        public virtual User? ToUser { get; set; }

        public virtual User? FromUser { get; set; }


        public int? FromUserId { get; set; }
    }
}
