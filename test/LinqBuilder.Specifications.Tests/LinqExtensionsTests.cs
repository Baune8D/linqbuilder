﻿using System;
using System.Collections.Generic;
using System.Linq;
using LinqBuilder.Specifications.Tests.TestHelpers;
using Shouldly;
using Xunit;

namespace LinqBuilder.Specifications.Tests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void Where_IQueryable_ShouldReturn1Result()
        {
            var query = GetTestList().AsQueryable();
            var result = query.Where(new Value1Specification(5)).ToList();
            result.Count.ShouldBe(1);
            result[0].Value1.ShouldBe(5);
        }

        [Fact]
        public void Where_IEnumerable_ShouldReturn1Result()
        {
            var query = GetTestList();
            var result = query.Where(new Value1Specification(5)).ToList();
            result.Count.ShouldBe(1);
            result[0].Value1.ShouldBe(5);
        }

        [Fact]
        public void Where_IQueryable_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => GetTestList().AsQueryable().Where(null));
        }

        [Fact]
        public void Where_IEnumerable_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => GetTestList().Where(null));
        }

        private static IEnumerable<TestEntity> GetTestList()
        {
            return new List<TestEntity>
            {
                new TestEntity { Value1 = 4 },
                new TestEntity { Value1 = 5 },
                new TestEntity { Value1 = 4 }
            };
        }
    }
}