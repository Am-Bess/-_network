using System.Text.Json;

namespace HW_2
{
    public class Message
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public DateTime STime { get; set; }

        public Message(string name, string text)
        {
            Name = name;
            Text = text;
            STime = DateTime.Now;
        }

        public string ToJson()
        {
           return JsonSerializer.Serialize(this);
        }

        public static Message? FromJson(string text)
        {
            return JsonSerializer.Deserialize<Message>(text);
        }

        public override string ToString()
        {
            return $"Получено от {Name} ({STime}): {Text}";
        }
    }
}