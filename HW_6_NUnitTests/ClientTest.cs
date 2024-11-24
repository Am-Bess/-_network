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

            var client1 = new Client(mock, "127.0.0.1", 5522, "Anna");
            var client2 = new Client(mock, "127.0.0.1", 5533, "Ivan");

            mock.AddClient(client1);
            mock.AddClient(client2);

            client1.ClientStart();
            client2.ClientStart();

            using (var ctx = new Context())
            {
                Assert.IsTrue(ctx.Users?.Count() == 2, "Пользователи не созданы");

                var user1 = ctx.Users.FirstOrDefault(x => x.Name == "Anna");
                var user2 = ctx.Users.FirstOrDefault(x => x.Name == "Ivann");

                Assert.IsNotNull(user1, "Пользователь не создан");
                Assert.IsNotNull(user2, "Пользователь не создан");

                Assert.IsTrue(user1.FromMessages?.Count == 1);
                Assert.IsTrue(user2.FromMessages?.Count == 1);

                Assert.IsTrue(user1.ToMessage?.Count == 1);
                Assert.IsTrue(user2.ToMessage?.Count == 1);

                var msg1 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user1 && x.ToUser == user2);
                var msg2 = ctx.Messages?.FirstOrDefault(x => x.FromUser == user2 && x.ToUser == user1);

                Assert.AreEqual("от Anna", msg2?.Text);
                Assert.AreEqual("от Ivan", msg1?.Text);

            }
        }
    }
}