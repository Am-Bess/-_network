using HW_6_ChatApp.Abstraction;
using HW_6_ChatApp.Model;
using HW_6_ChatApp;
using System.Net;


namespace HW_6_NUnitTests
{
    public class MockMessageSource : IMessageSource
    {
        private Queue<MessageUDP> messages = new(); // Очередь сообщений для имитации приёма сообщений
        private ServerUDP? server; // Ссылка на сервер для управления его работой
        private IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0); // Конечная точка для имитации

        public MockMessageSource()
        {
            messages.Enqueue(new MessageUDP { Command = Command.Reg, FromName = "Вася" });
            messages.Enqueue(new MessageUDP { Command = Command.Reg, FromName = "Юля" });
            messages.Enqueue(new MessageUDP { Command = Command.Mes, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
            messages.Enqueue(new MessageUDP { Command = Command.Mes, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
        }

        // Метод для имитации приёма сообщения
        public MessageUDP ReceiveMessage(ref IPEndPoint ep)
        {
            ep = endPoint; // Устанавливаем конечную точку
            if (messages.Count == 0)
            {
                return null!;
            }
            var msg = messages.Dequeue(); // Извлекаем сообщение из очереди
            return msg;
        }
        public void SendMessage(MessageUDP message, IPEndPoint ep)
        {
            // Реализация этого метода не требуется в данном контексте
        }

        public void AddServer(ServerUDP srv)
        {
            server = srv;
        }
    }

    public class ServerTest
    {
        [NUnit.Framework.SetUp]
        public void Setup()
        {
            using (var ctx = new ContextDB())
            {
                ctx.Messages?.RemoveRange(ctx.Messages);
                ctx.Users?.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }

        [NUnit.Framework.TearDown]
        public void TeatDown()
        {
            using (var ctx = new ContextDB())
            {
                ctx.Messages?.RemoveRange(ctx.Messages);
                ctx.Users?.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }

        [NUnit.Framework.Test]
        public void Test1()
        {
            var mock = new MockMessageSource(); // Создаем объект Мока для тестирования
            var srv = new ServerUDP(mock); // Создаем сервер и передаем ему Мок
            mock.AddServer(srv); // Связываем Мок с сервером
            srv.Work(); // Запускаем работу сервера

            using (var ctx = new ContextDB())
            {
                // Проверяем, что пользователи созданы
                Assert.IsTrue(ctx.Users?.Count() == 2, "Пользователи не созданы");
                var user1 = ctx.Users.FirstOrDefault(x => x.Name == "Вася");
                var user2 = ctx.Users.FirstOrDefault(x => x.Name == "Юля");
                Assert.IsNotNull(user1, "Пользователь не создан");
                Assert.IsNotNull(user2, "Пользователь не создан");

                // Проверяем, что отправлены и получены сообщения
                Assert.IsTrue(user1?.FromMessages?.Count == 1);
                Assert.IsTrue(user2?.FromMessages?.Count == 1);
                Assert.IsTrue(user1?.ToMessages?.Count == 1);
                Assert.IsTrue(user2?.ToMessages?.Count == 1);

                // Проверяем тексты сообщений
                var msg1 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                var msg2 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                Assert.AreEqual("От Юли", msg2?.Text);
                Assert.AreEqual("От Васи", msg1?.Text);
            }
        }
    }
}