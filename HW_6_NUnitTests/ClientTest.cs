using HW_6_ChatApp.Models;
using HW_6_ChatApp.Services;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestClient
{
    
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            using (var ctx = new Context())
            {
                ctx.Messages?.RemoveRange(ctx.Messages);
                ctx.Users?.RemoveRange(ctx.Users);

                ctx.SaveChanges();
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var ctx = new Context())
            {
                ctx.Messages?.RemoveRange(ctx.Messages);
                ctx.Users?.RemoveRange(ctx.Users);

                ctx.SaveChanges();
            }
        }

        [Test]
        public void ClientTest()
        {

            var mock = new MockMessageSource();

            var client1 = new Client("Вася", "127.0.0.1", 5050);
            var client2 = new Client("Юля", "127.0.0.2", 6060);

            mock.AddClient(client1);
            mock.AddClient(client2);

            client1.StartClient();
            client2.StartClient();

            using (var ctx = new Context())
            {
                Assert.IsTrue(ctx.Users?.Count() == 2, "Нет Юзеров");

                var user1 = ctx.Users.FirstOrDefault(x => x.Name == "Вася");
                var user2 = ctx.Users.FirstOrDefault(x => x.Name == "Юля");

                Assert.IsNotNull(user1, "Юзер не создан");
                Assert.IsNotNull(user2, "Юзер не создан");

                Assert.IsTrue(user1.FromMessages?.Count == 1);
                Assert.IsTrue(user2.FromMessages?.Count == 1);

                Assert.IsTrue(user1.ToMessage?.Count == 1);
                Assert.IsTrue(user2.ToMessage?.Count == 1);

                var msg1 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                var msg2 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                Assert.AreEqual("От Васи", msg2?.Text);
                Assert.AreEqual("От Юли", msg1?.Text);
            }
        }
    }
}