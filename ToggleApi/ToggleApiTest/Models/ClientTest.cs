using System.Collections.Generic;
using ToggleApi.Models;
using Xunit;

namespace ToggleApiTest.Models
{
    public class ClientTest
    {
        [Theory]
        [MemberData(nameof(ClientTestData))]
        public void ClientEqualsTest(Client clientToCompare)
        {
            var client = new Client("myClient","*");
            Assert.True(client.Equals(clientToCompare));
        }

        public static IEnumerable<object[]> ClientTestData()
        {
            yield return new object[] { new Client("myClient", "1.1.1.1"), };
            yield return new object[] { new Client("myClient", "*")};
           //TODO new Client("myClient", "1.1.1.*");
        }
    }
}
