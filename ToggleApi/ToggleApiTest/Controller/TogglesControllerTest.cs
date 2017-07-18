using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using ToggleApi.Commands;
using ToggleApi.Controllers;
using ToggleApi.Queries;
using ToggleApi.Utilities;
using Xunit;

public class TogglesControllerTest
{
    private readonly Mock<IQueryHandler> _queryHandler;
    private readonly Mock<ICommandHandler> _commandHandler;
    private readonly Mock<ILogger<TogglesController>> _logger;
    private readonly TogglesController _controller;

    public TogglesControllerTest()
    {
        _queryHandler = new Mock<IQueryHandler>();
        _commandHandler = new Mock<ICommandHandler>();
        _logger = new Mock<ILogger<TogglesController>>();
        _controller = new TogglesController(
            new ToggleClientParser(),
            _queryHandler.Object,
            _commandHandler.Object,
            _logger.Object);
    }

    [Theory]
    [MemberData(nameof(InvalidCtorData))]
    [Trait("UnitTest", "TogglesController")]
    public void InvalidControllerCtor(
        IToggleClientParser parser,
        IQueryHandler queryHandler,
        ICommandHandler cmdHandler,
        ILogger<TogglesController> logger)
    {
        void Create() => new TogglesController(parser, queryHandler, cmdHandler, logger);

        Assert.Throws<ArgumentNullException>((Action)Create);
    }

    public void GetByToggleName()
    {

    }

    private static IEnumerable<object[]> InvalidCtorData()
    {
        yield return new object[]
        {
            null,
            new Mock<IQueryHandler>().Object,
            new Mock<ICommandHandler>().Object ,
            new Mock<ILogger<TogglesController>>().Object
        };
        yield return new object[]
        {
            new ToggleClientParser(),
            null,
            new Mock<ICommandHandler>().Object ,
            new Mock<ILogger<TogglesController>>().Object
        };
        yield return new object[]
        {
            new ToggleClientParser(),
            new Mock<IQueryHandler>().Object,
            null,
            new Mock<ILogger<TogglesController>>().Object
        };
        yield return new object[]
        {
            new ToggleClientParser(),
            new Mock<IQueryHandler>().Object,
            new Mock<ICommandHandler>().Object ,
            null
        };
        yield return new object[]
        {
            null,
            null,
            null,
            null,
        };
    }
}
