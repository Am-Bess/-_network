using System.Text.Json;

namespace HW_4
{
    internal class Message
    {
        public string FromName { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime STime { get; set; }

        public Message(string name, string text)
        {
            FromName = name;
            Text = text;
        }

       public Message()
        {
            FromName = string.Empty;
            Text = string.Empty;
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
            return $"Получено сообщение от {FromName} ({STime}): {Text}";
        }
    }
}