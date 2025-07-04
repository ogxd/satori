// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using Xunit;

namespace System.Linq.Tests
{
    public class SingleTests : EnumerableTests
    {
        [Fact]
        public void SameResultsRepeatCallsIntQuery()
        {
            var q = from x in new[] { 999.9m }
                    select x;

            Assert.Equal(q.Single(), q.Single());
        }

        [Fact]
        public void SameResultsRepeatCallsStringQuery()
        {
            var q = from x in new[] { "!@#$%^" }
                    where !string.IsNullOrEmpty(x)
                    select x;

            Assert.Equal(q.Single(), q.Single());
        }

        [Fact]
        public void SameResultsRepeatCallsIntQueryWithZero()
        {
            var q = from x in new[] { 0 }
                    select x;

            Assert.Equal(q.Single(), q.Single());
        }

        [Fact]
        public void Empty()
        {
            foreach (IEnumerable<int> source in CreateSources<int>([]))
            {
                Assert.Throws<InvalidOperationException>(() => source.Single());
            }
        }

        [Fact]
        public void SingleElement()
        {
            int expected = 4;
            foreach (IEnumerable<int> source in CreateSources([4]))
            {
                Assert.Equal(expected, source.Single());
            }
        }

        [Fact]
        public void ManyElement()
        {
            foreach (IEnumerable<int> source in CreateSources([4, 4, 4, 4, 4]))
            {
                Assert.Throws<InvalidOperationException>(() => source.Single());
            }
        }

        [Fact]
        public void EmptySourceWithPredicate()
        {
            int[] source = { };

            Assert.All(CreateSources(source), source =>
            {
                Assert.Throws<InvalidOperationException>(() => source.Single(i => i % 2 == 0));
            });
        }

        [Fact]
        public void SingleElementPredicateTrue()
        {
            int[] source = { 4 };
            int expected = 4;

            Assert.All(CreateSources(source), source =>
            {
                Assert.Equal(expected, source.Single(i => i % 2 == 0));
            });
        }

        [Fact]
        public void SingleElementPredicateFalse()
        {
            int[] source = { 3 };

            Assert.All(CreateSources(source), source =>
            {
                Assert.Throws<InvalidOperationException>(() => source.Single(i => i % 2 == 0));
            });
        }

        [Fact]
        public void ManyElementsPredicateFalseForAll()
        {
            int[] source = { 3, 1, 7, 9, 13, 19 };

            Assert.All(CreateSources(source), source =>
            {
                Assert.Throws<InvalidOperationException>(() => source.Single(i => i % 2 == 0));
            });
        }

        [Fact]
        public void ManyElementsPredicateTrueForLast()
        {
            int[] source = { 3, 1, 7, 9, 13, 19, 20 };
            int expected = 20;

            Assert.All(CreateSources(source), source =>
            {
                Assert.Equal(expected, source.Single(i => i % 2 == 0));
            });
        }

        [Fact]
        public void ManyElementsPredicateTrueForFirstAndLast()
        {
            int[] source = { 2, 3, 1, 7, 9, 13, 19, 10 };

            Assert.All(CreateSources(source), source =>
            {
                Assert.Throws<InvalidOperationException>(() => source.Single(i => i % 2 == 0));
            });
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void FindSingleMatch(int target, int range)
        {
            Assert.All(CreateSources(Enumerable.Range(0, range)), source =>
            {
                Assert.Equal(target, source.Single(i => i == target));
            });
        }

        [Theory]
        [InlineData(1, 100)]
        [InlineData(42, 100)]
        public void RunOnce(int target, int range)
        {
            Assert.All(CreateSources(Enumerable.Range(0, range)), source =>
            {
                Assert.Equal(target, source.RunOnce().Single(i => i == target));
            });
        }

        [Fact]
        public void ThrowsOnNullSource()
        {
            int[] source = null;
            AssertExtensions.Throws<ArgumentNullException>("source", () => source.Single());
            AssertExtensions.Throws<ArgumentNullException>("source", () => source.Single(i => i % 2 == 0));
        }

        [Fact]
        public void ThrowsOnNullPredicate()
        {
            int[] source = { };
            Func<int, bool> nullPredicate = null;
            AssertExtensions.Throws<ArgumentNullException>("predicate", () => source.Single(nullPredicate));
        }
    }
}
