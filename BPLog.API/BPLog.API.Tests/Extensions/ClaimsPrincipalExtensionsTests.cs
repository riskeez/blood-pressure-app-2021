using System;
using System.Security.Claims;
using Xunit;
using BPLog.API.Extensions;
using FluentAssertions;

namespace BPLog.API.Tests.Extensions
{
    /// <summary>
    /// Tests for ClaimsPrincipalExtensions methods
    /// </summary>
    public class ClaimsPrincipalExtensionsTests
    {
        /// <summary>
        /// Runs a test to make sure that user Id (int) can be read from principal's claim using this extension
        /// </summary>
        [Fact]
        public void GetUserId_IfInteger_ShouldReturnInteger()
        {
            int id = 101;

            var testPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, id.ToString()) }));
            int? resultId = testPrincipal.GetUserId();

            resultId.Should().Be(id);
        }

        /// <summary>
        /// Runs a test to verify that null is returned if principal's claim contain not integer value of user Id
        /// </summary>
        [Fact]
        public void GetUserId_IfNotInteger_ShouldReturnNull()
        {
            string testValue = "my random text";

            var testPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, testValue) }));
            int? resultId = testPrincipal.GetUserId();

            resultId.Should().BeNull();
        }

        /// <summary>
        /// Runs a test to verify that null is returned if principal doesn't have a claim that contains user Id
        /// </summary>
        [Fact]
        public void GetUserId_IfNoClaimFound_ShouldReturnNull()
        {
            var emptyPrincipal = new ClaimsPrincipal(new ClaimsIdentity(Array.Empty<Claim>()));
            int? resultId = emptyPrincipal.GetUserId();

            resultId.Should().BeNull();
        }
    }
}
