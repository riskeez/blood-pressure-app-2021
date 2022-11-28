using System;
using Xunit;
using BPLog.API.Extensions;
using FluentAssertions;

namespace BPLog.API.Tests.Extensions
{
    /// <summary>
    /// Tests for StringExtensions methods
    /// </summary>
    public class StringExtensionsTests
    {
        /// <summary>
        /// Runs a test to verify that SHA256 hash is generated and then converted to Base64
        /// </summary>
        [Fact]
        public void ToSha256_IfStringHasValue_ShouldReturnSHA256Base64()
        {
            string inputValue = "Test value";
            string expectedValue = "WKgNXZ3cs9Jp9NXRhdhuCVZOpZl7PxxPbBnRCFHcgek=";

            string result = inputValue.ToSha256();
            result.Should().Be(expectedValue);
        }

        /// <summary>
        /// Runs bunch of tests to verify that empty string is returned when provided string can't be used for SHA256 hash calculation
        /// </summary>
        /// <param name="inputValue"></param>
        /// <param name="expectedValue"></param>
        [Theory]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void ToSha256_IfStringNullOrEmpty_ShouldReturnEmptyString(string inputValue, string expectedValue)
        {
            string result = inputValue.ToSha256();
            result.Should().Be(expectedValue);
        }
    }
}
