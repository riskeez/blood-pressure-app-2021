using System;
using Xunit;
using BPLog.API.Extensions;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace BPLog.API.Tests.Extensions
{
    /// <summary>
    /// Tests for IQueryableExtensions methods
    /// </summary>
    public class IQueryableExtensionsTests
    {
        /// <summary>
        /// Runs bunch of tests to see if expected data are always returned
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="input"></param>
        /// <param name="expected"></param>
        [Theory]
        [InlineData(-1, -1, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(0, -1, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2, 3, 4 })]
        [InlineData(0, 2, new int[] { 1, 2, 3, 4 }, new int[] { 1, 2 })]
        [InlineData(2, 2, new int[] { 1, 2, 3, 4 }, new int[] { 3, 4 })]
        [InlineData(2, -1, new int[] { 1, 2, 3, 4 }, new int[] { 3, 4 })]
        [InlineData(10, 1, new int[] { 1, 2, 3, 4 }, new int[] { })]
        public void Take_ShouldFilterOutResults(int start, int count, int[] input, int[] expected)
        {
            var inputQueryable = input.AsQueryable();
            var expectedQueryable = expected.AsQueryable();

            var result = inputQueryable.Take(start, count);
            result.Should().BeEquivalentTo(expectedQueryable);
        }
    }
}
