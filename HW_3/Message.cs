using System.Text.Json;

namespace HW_3
{
    public class Message
    {
        public string? NickName { get; set; }
        public DateTime DateMessage { get; set; }
        public string? TextMessage { get; set; }
        
        public string MessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? MessageFromJson(string text)
        {
            return JsonSerializer.Deserialize<Message>(text);
        }
        public override string ToString()
        {
            return $"Получено от {NickName} ({DateMessage}): {TextMessage}";
        }
    }
}