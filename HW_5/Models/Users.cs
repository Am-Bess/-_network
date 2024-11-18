namespace DBTest.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public virtual ICollection<Messages> ToMessages { get; set; } = new List<Messages>();
        public virtual ICollection<Messages> FromMessages { get; set; } = new List<Messages>();

    }
}
