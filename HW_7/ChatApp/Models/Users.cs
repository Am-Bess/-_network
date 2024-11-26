namespace ChatApp.Models
{
    public class Users
    {
        public int Id { get; set; }

        public string ?Name { get; set; }

        public virtual ICollection<Messages> ?ToMessages { get; set; }

        public virtual ICollection<Messages>?FromMessages { get; set; }
    }
}
