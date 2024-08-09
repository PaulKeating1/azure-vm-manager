using AzureVmManager.WebApi.Controllers;
using FluentAssertions;
using MailMe8.Server.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Security.Claims;

namespace AzureVmManager.WebApi.Tests
{
    internal class UserActionFilterTests
    {
        [Test]
        public async Task Should_GetUser()
        {
            // Arrange
            const string objectId = "test-object-id";
            const string displayName = "test-display-name";
            const string givenName = "test-given-name";
            const string surname = "test-surname";

            var baseControllerMock = new Mock<BaseController>();

            var httpContextMock = new Mock<HttpContext>();
            var claims = new[]
            { 
                new Claim("oid", objectId),
                new Claim(ClaimsIdentity.DefaultNameClaimType, displayName),
                new Claim(ClaimTypes.GivenName, givenName),
                new Claim(ClaimTypes.Surname, surname)
            };
            var claimsPrincipal = new ClaimsPrincipal([new ClaimsIdentity(claims)]);
            httpContextMock.Setup(x => x.User).Returns(claimsPrincipal);

            var actionContext = new ActionContext(
                httpContextMock.Object,
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                new ModelStateDictionary()
            );

            var actionExecutingContext = new ActionExecutingContext(
                    actionContext,
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object?>(),
                    baseControllerMock.Object
                );

            var actionExecutionDelegateMock = new Mock<ActionExecutionDelegate>();
            var userActionFilter = new UserActionFilter();

            // Act
            await userActionFilter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegateMock.Object);

            // Assert
            baseControllerMock.Object.UserDataObject.Should().NotBeNull();
            baseControllerMock.Object.UserDataObject?.ObjectId.Should().Be(objectId);
            baseControllerMock.Object.UserDataObject?.DisplayName.Should().Be(displayName);
            baseControllerMock.Object.UserDataObject?.GivenName.Should().Be(givenName);
            baseControllerMock.Object.UserDataObject?.Surname.Should().Be(surname);
        }
    }
}
