using System;
using System.Data.Entity;
using System.Threading.Tasks;
using LinqBuilder.EF6.Testing;
using LinqBuilder.EF6.Testing.Specifications;
using Shouldly;
using Xunit;

namespace LinqBuilder.EF6.Tests
{
    public sealed class IntegrationTests : IDisposable
    {
        private readonly TestDb _testDb;

        public IntegrationTests()
        {
            _testDb = new TestDb();
            _testDb.AddEntity(2, 1, 2);
            _testDb.AddEntity(1, 2, 3);
            _testDb.AddEntity(3, 1, 1);
            _testDb.Context.SaveChanges();
        }

        public void Dispose()
        {
            _testDb.Dispose();
        }

        [Fact]
        public async Task ExeSpecAsync_ChildSpecification_ShouldReturnCorrectResult()
        {
            var specification = new ChildValueSpecification(1)
                .Or(new ChildValueSpecification(2));

            var result = await _testDb.Context.Entities
                .ExeSpec(specification)
                .ToListAsync();

            result.Count.ShouldBe(2);
            result[0].Id.ShouldBe(1);
            result[1].Id.ShouldBe(3);
        }
    }
}
