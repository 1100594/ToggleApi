using System;
using System.Collections.Generic;
using ToggleApi.Models;
using Xunit;

namespace ToggleApiTest.Models
{
    public class ClientTest
    {

        [Fact]
        [Trait("UnitTest", "Client")]
        public void CreateProperIdAndVersionSuccess()
        {
            string id = Guid.NewGuid().ToString();
            string version = "1.1.1.1";

            var client = new Client(id, version);

            Assert.NotNull(client.Id);
            Assert.NotNull(client.Version);

            Assert.Equal(id, client.Id);
            Assert.Equal(version, client.Version);
        }

        [Theory]
        [InlineData("myClient", null)]
        [InlineData(null, "*")]
        [InlineData(null, null)]
        [Trait("UnitTest", "Client")]
        public void CreateNullArgumentsThrowsException(string id, string version)
        {
            void Client() => new Client(id, version);

            Assert.Throws<ArgumentNullException>((Action)Client);
        }

        [Theory]
        [MemberData(nameof(ClientTestDataForEqualityComparer))]
        [Trait("UnitTest", "Client")]
        public void ClientEqualsTrue(Client c1, Client c2)
        {
            Assert.True(c1.Equals(c2));
        }

        [Fact]
        [Trait("UnitTest", "Client")]
        public void ClientEqualsFalse()
        {
            //Test the two equals
            var c1 = new Client("myClient", "*");
            object c2 = new Client("myClient2", "*");

            var isEqual = c1.Equals(c2);

            Assert.False(isEqual);
        }

        [Theory]
        [MemberData(nameof(ClientDataForHashCodeCalculation))]
        [Trait("UnitTest", "Client")]
        public void GetHashCodeEqualObjectsSameHashCode(Client c1, Client c2)
        {
            Assert.Equal(c1, c2);
            Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(ClientDataForDivergentHashCodeCalculation))]
        [Trait("UnitTest", "Client")]
        public void GetHashCode_DifferentHashCode(Client left, Client right)
        {
            Assert.NotEqual(left.GetHashCode(), right.GetHashCode());
        }

        private static IEnumerable<object[]> ClientTestDataForEqualityComparer()
        {
            yield return new object[] { new Client("myClient", "*"), new Client("myClient", "*"), };
            yield return new object[] { new Client("myClient", "*"), new Client("myClient", "1.1.1.1"), };
            yield return new object[] { new Client("myClient", "1.1.1.1"), new Client("myClient", "*") };
            yield return new object[] { new Client("myClient", "1.1.*"), new Client("myClient", "*") };
            yield return new object[] { new Client("myClient", "1.1.*"), new Client("myClient", "1.1.1.1") };
            yield return new object[] { new Client("myClient", "1.1.*"), new Client("myClient", "1.1.1.*") };
        }

        private static IEnumerable<object[]> ClientDataForHashCodeCalculation()
        {
            yield return new object[] { new Client("myClient", "*"), new Client("myClient", "*") };
            yield return new object[] { new Client("myClient2", "1.1.1.1"), new Client("myClient2", "1.1.1.1") };
        }

        private static IEnumerable<object[]> ClientDataForDivergentHashCodeCalculation()
        {
            yield return new object[] { new Client("myClient", "*"), new Client("myClient2", "1.1.1.1") };
            yield return new object[] { new Client("myClient2", "1.1.1.1"), new Client("myClient", "*") };
        }

    }
}
