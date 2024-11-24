using HW_6_ChatApp;
using HW_6_ChatApp.Abstraction;
using HW_6_ChatApp.Model;
using System.Net;

namespace HW_6_NUnitTests
{
    public class MockMessageSourceClient : IMessageSource
    {
        private ClientUDP? cl;
        private Queue<MessageUDP>? receivedMessages = new();
        public Queue<MessageUDP>? sentMessages = new();

        public MockMessageSourceClient()
        {
            receivedMessages.Enqueue(new MessageUDP { Command = Command.Mes, FromName = "Юля", ToName = "Петя", Text = "От Юли" });
            receivedMessages.Enqueue(new MessageUDP { Command = Command.Mes, FromName = "Петя", ToName = "Юля", Text = "От Пети" });
        }

        public MessageUDP ReceiveMessage(ref IPEndPoint ep)
        {
            if (receivedMessages?.Count == 0)
            {
                return null!;
            }
            MessageUDP? msg = receivedMessages?.Dequeue();
            return msg!;
        }

        public void SendMessage(MessageUDP message, IPEndPoint ep)
        {
            sentMessages?.Enqueue(message);
        }
        public void AddClient(ClientUDP client)
        {
            cl = client;
        }
    }
    public class ClientTest
    {
        [NUnit.Framework.SetUp]
        public void SetUp()
        {
            using (var ctx = new ContextDB())
            {
                ctx.Messages?.RemoveRange(ctx.Messages);
                ctx.Users?.RemoveRange(ctx.Users);
                ctx.SaveChanges();
            }
        }

        [NUnit.Framework.TearDown]
        public void TearDown()
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
            var mock = new MockMessageSourceClient();

            var cln = new ClientUDP(mock, "127.0.0.1", 12345, "Вася");

            cln.Start();

            Assert.IsTrue(mock.sentMessages?.Count > 0, "Сообщения не отправляются!");

            for (int i = 0; i < mock.sentMessages?.Count; i++)
            {
                var msg = mock.sentMessages.Dequeue();
                if (i == 0)
                {
                    Assert.IsTrue(msg.Command == Command.Reg, "Сообщение о регистрации не отправлено!");
                }
                else
                {
                    Assert.IsTrue(msg.Command == Command.Conf, "Подтверждение получения сообщения не отправлено!");
                }
            }
        }

        [NUnit.Framework.Test]
        public void Test2()
        {

            var mock2 = new MockMessageSourceClient();

            ClientUDP client1 = new ClientUDP(mock2, "127.0.0.1", 12345, "Вася");
            ClientUDP client2 = new ClientUDP(mock2, "127.0.0.1", 12345, "Гена");

            mock2.AddClient(client1);
            mock2.AddClient(client2);

            client1.Start();
            client2.Start();

            using (ContextDB ctx = new ContextDB())
            {
                Assert.IsTrue(ctx.Users?.Count() == 2, "Юзеры созданы");

                var user1 = ctx.Users?.FirstOrDefault(x => x.Name == "Юля");
                var user2 = ctx.Users?.FirstOrDefault(x => x.Name == "Петя");

                Assert.IsNotNull(user1, "Юзер есть");
                Assert.IsNotNull(user2, "Юзер есть");

                Assert.IsTrue(user1?.FromMessages?.Count == 1);
                Assert.IsTrue(user2?.FromMessages?.Count == 1);

                Assert.IsTrue(user1?.ToMessages?.Count == 1);
                Assert.IsTrue(user2?.ToMessages?.Count == 1);

                var msg1 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                var msg2 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                Assert.AreEqual("От Юли", msg2!.Text);
                Assert.AreEqual("От Пети", msg1!.Text);
            }
        }
    }
}