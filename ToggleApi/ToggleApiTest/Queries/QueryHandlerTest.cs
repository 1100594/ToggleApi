using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using ToggleApi.Models;
using ToggleApi.Queries;
using ToggleApi.Repository;
using Xunit;

namespace ToggleApiTest.Queries
{
    public class QueryHandlerTest
    {
        private readonly IQueryHandler _handler;

        public QueryHandlerTest()
        {
            var toggleClientRepoMock = new Mock<IToggleClientRepository>();

            toggleClientRepoMock.Setup(repo => 
             repo.GetTogglesForClient(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new[]
                {
                    new KeyValuePair<string, bool>("toggleA", true )
                });

            toggleClientRepoMock.Setup(repo => repo.GetToggleByName(It.IsAny<string>()))
                .Returns(new Toggle("toggleA", true));

            _handler = new QueryHandler(toggleClientRepoMock.Object);
        }

        [Fact]
        [Trait("UnitTest", "QueryHandler")]
        public void GetTogglesForClient()
        {
            var result = _handler.Execute(new FetchTogglesForClient("client1", "*"))
                .ToList();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(1, result.Count);
            Assert.Equal(result[0].Key, "toggleA");
            Assert.True(result[0].Value);
        }

        [Fact]
        [Trait("UnitTest", "QueryHandler")]
        public void GetToggleByName()
        {
            var result = _handler.Execute(new FetchToggleByName("toggleA"));

            Assert.NotNull(result);
            Assert.Equal(new Toggle("toggleA", true), result);
        }

    }
}
