using Xunit;
using FluentAssertions;
using BPLog.API.Helpers;
using BPLog.API.Services;

namespace BPLog.API.Tests.Helpers
{
    /// <summary>
    /// Tests for QueryHelper methods
    /// </summary>
    public class QueryHelperTests
    {
        /// <summary>
        /// Runs bunch of tests to verify that "sort" parameter is parsed and converted to SortOrder value properly
        /// </summary>
        /// <param name="input">Query param with sort data</param>
        /// <param name="expected">Expected sort param result</param>
        [Theory]
        [InlineData("", SortOrder.Descending)]
        [InlineData(null, SortOrder.Descending)]
        [InlineData(" ", SortOrder.Descending)]
        [InlineData("desc", SortOrder.Descending)]
        [InlineData("Hey, hope you have a good day :)", SortOrder.Descending)]
        [InlineData("ASC", SortOrder.Ascending)]
        [InlineData("asc", SortOrder.Ascending)]
        [InlineData("num ASC", SortOrder.Ascending)]
        [InlineData("num asc   ", SortOrder.Ascending)]
        public void GetParamSortOrder_ShouldReturnCorrectSortOrder(string input, SortOrder expected)
        {
            SortOrder result = QueryHelper.GetParamSortOrder(input);
            result.Should().Be(expected);
        }
    }
}
