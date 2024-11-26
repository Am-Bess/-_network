using System.Text.Json;

namespace ChatCommLib
{

    public enum Command { Reg, Mes, Conf, All }
    public class MessageUdp
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string? FromName { get; set; }
        public string? ToName { get; set; }
        public string? Text { get; set; }

        //To Json serialization
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        //From Json deserialization
        public static MessageUdp? FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUdp>(json);
        }

        public override string ToString()
        {
            return $" FromName: {FromName}, ToName: {ToName}, Text: {Text}";
        }
    }
}
