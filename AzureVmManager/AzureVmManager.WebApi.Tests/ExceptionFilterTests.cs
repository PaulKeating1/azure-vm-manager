using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Routing;
using AzureVmManager.WebApi.Filters;
using FluentAssertions;

namespace AzureVmManager.WebApi.Tests
{
    internal class ExceptionFilterTests
    {
        [Test]
        public void Should_HandleUnknownException()
        {
            // Arrange
            var httpContextMock = new Mock<HttpContext>();
            var actionContext = new ActionContext(
                httpContextMock.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                new ModelStateDictionary()
            );

            var exceptionContext = new ExceptionContext(
                actionContext,
                new List<IFilterMetadata>()
            );
            exceptionContext.Exception = new Exception("Test exception");

            var exceptionFilter = new ExceptionFilter();

            // Act
            exceptionFilter.OnException(exceptionContext);

            // Assert
            exceptionContext.Result.Should().BeOfType<ObjectResult>();
            var objectResult = exceptionContext.Result as ObjectResult;
            objectResult?.Value.Should().Be("An unexpected error has occurred.");
            objectResult?.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
