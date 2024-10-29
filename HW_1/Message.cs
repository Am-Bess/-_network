using System.Text.Json;

namespace HW_1
{
    public class Message
    {
        public string ?NikName { get; set; }
        public string ?Text { get; set; }
        public DateTime STime { get; set; }

        public Message(string name, string text)
        {
            this.NikName = name;
            this.Text = text;
            this.STime = DateTime.Now;
        }

        public Message(){}

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
            return $"Получено сообщение от {NikName} ({STime.ToShortTimeString()}): {Text}";
        }
    }
}