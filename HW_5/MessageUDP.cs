using System.Text.Json;
namespace DBTest
{
    public enum Command { Reg, Mes, Conf }

    public class MessageUDP
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string FromName { get; set; }  = string.Empty;
        public string ToName { get; set; }  = string.Empty;
        public string Text { get; set; } = string.Empty;

        // Метод для сериализации в JSON
        public string? ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Статический метод для десериализации JSON в объект MyMessage
        public static MessageUDP? FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUDP>(json);
        }

        public override string ToString()
        {
            return $"[{DateTime.Now}] Получено сообщение: {Text} от {FromName} ";
        }
    }

}
