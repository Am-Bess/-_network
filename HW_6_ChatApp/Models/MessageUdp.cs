using System.Text.Json;

namespace HW_6_ChatApp.Models
{
    public class MessageUdp
    {
        public int Id { get; set; }
        public string? FromName { get; set; }
        public string? ToName { get; set; }
        public string? Text { get; set; }
        public Command Command { get; set; }

        public string MessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static MessageUdp? MessageFromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUdp>(json);
        }

    }

    public enum Command { Message, Register, Confirmation }
}
