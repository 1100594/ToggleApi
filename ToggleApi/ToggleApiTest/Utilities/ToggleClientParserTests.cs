using System;
using System.Collections.Generic;
using System.IO;
using ToggleApi.Models;
using ToggleApi.Utilities;
using Xunit;

namespace ToggleApiTest.Utilities
{
    public class ToggleClientParserTests
    {
        readonly IToggleClientParser _parser = new ToggleClientParser();

        [Fact]
        [Trait("UnitTest", "ToggleClientParser")]
        public void ExtractWithoutInputThrowsException()
        {
            ClientPermissions Extract() => _parser.Extract();

            Assert.Throws<ArgumentNullException>((Func<ClientPermissions>)Extract);
        }

        [Fact]
        [Trait("UnitTest", "ToggleClientParser")]
        public void ExtractInvalidInputThrowsException()
        {
            _parser.Input = "invalid_Sample";

            ClientPermissions Extract() => _parser.Extract();

            Assert.Throws<InvalidDataException>((Func<ClientPermissions>)Extract);
        }

        [Fact]
        [Trait("UnitTest", "ToggleClientParser")]
        public void ExtractSuccessWhitelistAndCustomValues()
        {
            _parser.Input = "{client1:*,client2:1.1.1.1}[^client3:*]";

            var validInput = _parser.IsValid();

            var extract = _parser.Extract();

            Assert.True(validInput);

            Assert.NotNull(extract.Whitelist);
            Assert.NotEmpty(extract.Whitelist);

            Assert.NotNull(extract.CustomValues);
            Assert.NotEmpty(extract.CustomValues);

            var whitelist = new List<Client>(extract.Whitelist);

            Assert.Equal(2, whitelist.Count);
            Assert.Equal(new Client("client1", "*"), whitelist[0]);
            Assert.Equal(new Client("client2", "1.1.1.1"), whitelist[1]);

            var customValues = new List<Client>(extract.CustomValues);

            Assert.Equal(1, customValues.Count);
            Assert.Equal(new Client("client3", "*"), customValues[0]);
        }

        [Fact]
        [Trait("UnitTest", "ToggleClientParser")]
        public void ExtractSuccessWhitelistOnly()
        {
            _parser.Input = "{client1:*,client2:1.1.1.1}";

            var validInput = _parser.IsValid();
            var extract = _parser.Extract();

            Assert.True(validInput);

            Assert.NotNull(extract.Whitelist);
            Assert.NotEmpty(extract.Whitelist);

            var whitelist = new List<Client>(extract.Whitelist);

            Assert.Equal(2, whitelist.Count);
            Assert.Equal(new Client("client1", "*"), whitelist[0]);
            Assert.Equal(new Client("client2", "1.1.1.1"), whitelist[1]);
        }

        [Fact]
        [Trait("UnitTest", "ToggleClientParser")]
        public void ExtractSuccessCustomValuesOnly()
        {
            _parser.Input = "[^client3:*,client1:1.1.1.1]";

            var validInput = _parser.IsValid();
            var extract = _parser.Extract();

            Assert.True(validInput);

            Assert.NotNull(extract.CustomValues);
            Assert.NotEmpty(extract.CustomValues);

            var customValues = new List<Client>(extract.CustomValues);
            Assert.Equal(2, customValues.Count);
            Assert.Equal(new Client("client3", "*"), customValues[0]);
            Assert.Equal(new Client("client1", "1.1.1.1"), customValues[1]);
        }
    }
}
