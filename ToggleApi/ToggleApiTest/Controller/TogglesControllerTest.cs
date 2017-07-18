using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ToggleApi.Commands;
using ToggleApi.Controllers;
using ToggleApi.Models;
using ToggleApi.Queries;
using ToggleApi.Utilities;
using Xunit;

public class TogglesControllerTest
{
    private readonly Mock<IQueryHandler> _queryHandler;
    private readonly Mock<ICommandHandler> _commandHandler;
    private readonly Mock<ILogger<TogglesController>> _logger;
    private readonly TogglesController _controller;
    private readonly Toggle _toggle = new Toggle("toggle1", true);

    public TogglesControllerTest()
    {
        _toggle.AddOrUpdateCustomValue(new Client("Id2", "*"), false);
        _toggle.AddToWhitelist(new[] { new Client("id1", "1.1.1.1") });
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

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByToggleName()
    {
        const string toggleName = "toggle1";

        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchToggleByName>(f => f.ToggleName.Equals(toggleName))))
            .Returns(_toggle);

        var result = _controller.Get(toggleName);

        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.IsType<Toggle>(okResult.Value);

        var toggle = okResult.Value as Toggle;
        Assert.NotNull(toggle);
        Assert.Equal(toggleName, toggle.Name);
        Assert.True(toggle.DefaultValue);
        Assert.NotNull(toggle.Whitelist);
        Assert.NotEmpty(toggle.Whitelist);
        Assert.NotNull(toggle.CustomValues);
        Assert.NotEmpty(toggle.CustomValues);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByToggleNameNotFound()
    {
        const string toggleName = "toggle2";

        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchToggleByName>(f => f.ToggleName.Equals(toggleName))))
            .Returns<Toggle>(null);

        var result = _controller.Get(toggleName);

        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByToggleNameInternalError()
    {
        const string toggleName = "toggle2";

        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchToggleByName>(f => f.ToggleName.Equals(toggleName))))
            .Throws<Exception>();

        var result = _controller.Get(toggleName);

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByClientIdAndVersion()
    {
        const string clientId = "id1";
        const string clientVersion = "1.1.1.1";
        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchTogglesForClient>(f => f.ClientId.Equals(clientId)
           && f.ClientVersion.Equals(clientVersion))))
            .Returns(new Dictionary<string, bool>
            {
                { _toggle.Name, _toggle.GetValueFor(new Client("id1", "1.1.1.1")) }
            });

        var result = _controller.Get("id1", "1.1.1.1");

        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);

        var resultValue = okResult.Value as IEnumerable<KeyValuePair<string, bool>>;
        Assert.NotNull(resultValue);
        Assert.NotEmpty(resultValue);
        Assert.Equal("toggle1", resultValue.First().Key);
        Assert.True(resultValue.First().Value);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByClientIdAndVersionNotFound()
    {
        const string clientId = "id1";
        const string clientVersion = "*";
        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchTogglesForClient>(f => f.ClientId.Equals(clientId)
             && f.ClientVersion.Equals(clientVersion))))
            .Returns(new Dictionary<string, bool>());

        var result = _controller.Get(clientId, clientVersion);

        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void GetByClientIdAndVersionInternalError()
    {
        const string clientId = "id1";
        const string clientVersion = "*";
        _queryHandler
            .Setup(handler => handler
            .Execute(It.Is<FetchTogglesForClient>(f => f.ClientId.Equals(clientId)
            && f.ClientVersion.Equals(clientVersion))))
            .Throws<Exception>();

        var result = _controller.Get(clientId, clientVersion);

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PostNewToggle()
    {
        const string toggleName = "toggle2";
        const bool toggleValue = true;

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<CreateToggle>(c => c.ToggleName.Equals(toggleName)
           && c.ToggleValue.Equals(toggleValue))))
            .Verifiable();

        var result = _controller.Post(toggleName, toggleValue, @"{id1:*}[^Id2:*]");

        _commandHandler.Verify(handler => handler.Execute(It.Is<CreateToggle>(c =>
        c.ToggleName.Equals(toggleName) && c.ToggleValue.Equals(toggleValue))));

        Assert.IsType<OkResult>(result);

        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PostNewToggleMethodNotAllowed()
    {
        const string toggleName = "toggle2";
        const bool toggleValue = true;

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<CreateToggle>(c => c.ToggleName.Equals(toggleName)
           && c.ToggleValue.Equals(toggleValue))))
            .Verifiable();

        var result = _controller.Post(toggleName, toggleValue, "");

        Assert.IsType<ObjectResult>(result);

        var notAllowedResult = result as ObjectResult;
        Assert.NotNull(notAllowedResult);
        Assert.Equal(405, notAllowedResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PostNewToggleInternalError()
    {
        const string toggleName = "toggle2";
        const bool toggleValue = true;

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<CreateToggle>(c => c.ToggleName.Equals(toggleName)
             && c.ToggleValue.Equals(toggleValue))))
            .Throws<Exception>();

        var result = _controller.Post(toggleName, toggleValue, @"{id1:*}[^Id2:*]");

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleValue()
    {
        const string toggleName = "toggle2";
        const bool toggleValue = true;

        _commandHandler
            .Setup(handler => handler
            .Execute(It.Is<UpdateToggleValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue))))
            .Verifiable();

        IActionResult result = _controller.Put(toggleName, toggleValue);
        _commandHandler
            .Verify(handler => handler
            .Execute(It.Is<UpdateToggleValue>(c => c.ToggleName.Equals(toggleName)
            && c.ToggleValue.Equals(toggleValue))));

        Assert.IsType<OkResult>(result);

        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleValueNotAllowed()
    {
        const string toggleName = "toggle1";
        const bool toggleValue = false;

        _commandHandler
            .Setup(handler => handler
            .Execute(It.Is<UpdateToggleValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue))))
            .Throws<NotSupportedException>();

        var result = _controller.Put(toggleName, toggleValue);

        Assert.IsType<ObjectResult>(result);

        var notAllowedResult = result as ObjectResult;
        Assert.NotNull(notAllowedResult);
        Assert.Equal(405, notAllowedResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleValueNotFound()
    {
        const string toggleName = "toggle1";
        const bool toggleValue = false;

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<UpdateToggleValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue))))
            .Throws<ArgumentException>();

        IActionResult result = _controller.Put(toggleName, toggleValue);

        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleValueInternalError()
    {
        const string toggleName = "toggle1";
        const bool toggleValue = false;

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<UpdateToggleValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue))))
            .Throws<Exception>();

        var result = _controller.Put(toggleName, toggleValue);

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleCustomValue()
    {
        const string toggleName = "toggle1";
        const bool toggleValue = false;
        const string clientId = "client1";
        const string clientVersion = "*";

        _commandHandler
            .Setup(handler => handler.Execute(It.Is<UpdateToggleCustomValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue) 
            && c.ClientId.Equals(clientId) && c.ClientVersion.Equals(clientVersion))))
            .Verifiable();

        var result = _controller.Put(toggleName, toggleValue, clientId, clientVersion);

        _commandHandler
            .Verify(handler => handler.Execute(It.Is<UpdateToggleCustomValue>(c => c.ToggleName.Equals(toggleName) 
            && c.ToggleValue.Equals(toggleValue) 
            && c.ClientId.Equals(clientId)
            && c.ClientVersion.Equals(clientVersion))));

        Assert.IsType<OkResult>(result);

        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleCustomValueNotAllowed()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateToggleCustomValue>()))
            .Throws<NotSupportedException>();

        IActionResult result = _controller.Put("toggle1", false, "client1", "*");

        Assert.IsType<ObjectResult>(result);

        var castResult = result as ObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(405, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleCustomValueNotFound()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateToggleCustomValue>()))
            .Throws<ArgumentException>();

        IActionResult result = _controller.Put("toggle1", false, "client1", "*");

        Assert.IsType<NotFoundObjectResult>(result);

        var castResult = result as NotFoundObjectResult;
        Assert.Equal(404, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutToggleCustomValueInternalError()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateToggleCustomValue>()))
            .Throws<Exception>();

        IActionResult result = _controller.Put("toggle1", false, "client1", "*");

        Assert.IsType<ObjectResult>(result);

        var castResult = result as ObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(500, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutClientInToggleWhitelist()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateWhitelist>()))
            .Verifiable();

        IActionResult result = _controller.Put("toggle1", "client1", "*");
        _commandHandler
            .Verify(handler => handler.Execute(It.IsAny<UpdateWhitelist>()));

        Assert.IsType<OkResult>(result);

        var castResult = result as OkResult;
        Assert.NotNull(castResult);
        Assert.Equal(200, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutClientInToggleWhitelistNotAllowed()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateWhitelist>()))
            .Throws<NotSupportedException>();

        IActionResult result = _controller.Put("toggle1", "client1", "*");

        Assert.IsType<ObjectResult>(result);

        var castResult = result as ObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(405, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutClientInToggleWhitelistNotFound()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateWhitelist>()))
            .Throws<ArgumentException>();

        IActionResult result = _controller.Put("toggle1", "client1", "*");

        Assert.IsType<NotFoundObjectResult>(result);

        var castResult = result as NotFoundObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(404, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void PutClientInToggleWhitelistInternalError()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<UpdateWhitelist>()))
            .Throws<Exception>();

        IActionResult result = _controller.Put("toggle1", "client1", "*");

        Assert.IsType<ObjectResult>(result);

        var castResult = result as ObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(500, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void DeleteClientInToggle()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteClientToggle>()))
            .Verifiable();

        IActionResult result = _controller.Delete("toggle1", "client1", "*");
        _commandHandler
            .Verify(handler => handler.Execute(It.IsAny<DeleteClientToggle>()));

        Assert.IsType<OkResult>(result);

        var castResult = result as OkResult;
        Assert.NotNull(castResult);
        Assert.Equal(200, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void DeleteClientInToggleNotFound()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteClientToggle>()))
            .Throws<ArgumentException>();

        IActionResult result = _controller.Delete("toggle1", "client1", "*");

        Assert.IsType<NotFoundObjectResult>(result);

        var castResult = result as NotFoundObjectResult;
        Assert.NotNull(castResult);
        Assert.Equal(404, castResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void DeleteClientInToggleInternalError()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteClientToggle>()))
            .Throws<Exception>();

        IActionResult result = _controller.Delete("toggle1", "client1", "*");

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void Delete()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteToggle>()))
            .Verifiable();

        IActionResult result = _controller.Delete("toggle1");
        _commandHandler
            .Verify(handler => handler.Execute(It.IsAny<DeleteToggle>()));

        Assert.IsType<OkResult>(result);

        var okResult = result as OkResult;
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void DeleteNotFound()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteToggle>()))
            .Throws<ArgumentException>();

        IActionResult result = _controller.Delete("toggle1");

        Assert.IsType<NotFoundObjectResult>(result);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    [Trait("UnitTest", "TogglesController")]
    public void DeleteInternalError()
    {
        _commandHandler
            .Setup(handler => handler.Execute(It.IsAny<DeleteToggle>()))
            .Throws<Exception>();

        IActionResult result = _controller.Delete("toggle1");

        Assert.IsType<ObjectResult>(result);

        var internalErrorResult = result as ObjectResult;
        Assert.NotNull(internalErrorResult);
        Assert.Equal(500, internalErrorResult.StatusCode);
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

