using System;
using System.Linq;
using System.Threading.Tasks;
using LinqBuilder.Core;
using LinqBuilder.EF6.Tests.Shared;
using Shouldly;
using Xunit;

namespace LinqBuilder.EF6.Tests
{
    public class EntityFrameworkExtensionsTests : IDisposable
    {
        private readonly ISpecification<Entity> _emptySpecification = Spec<Entity>.New();
        private readonly ISpecification<Entity> _value1ShouldBe1 = Spec<Entity>.New(entity => entity.Value1 == 1);
        private readonly ISpecification<Entity> _value1ShouldBe2 = Spec<Entity>.New(entity => entity.Value1 == 2);
        private readonly ISpecification<Entity> _value1ShouldBe4 = Spec<Entity>.New(entity => entity.Value1 == 4);
        private readonly ISpecification<Entity> _value2ShouldBe3 = Spec<Entity>.New(entity => entity.Value2 == 3);

        private readonly TestDb _testDb;

        public EntityFrameworkExtensionsTests()
        {
            _testDb = new TestDb();
            _testDb.AddEntity(2, 3);
            _testDb.AddEntity(1, 3);
            _testDb.AddEntity(1, 3);
            _testDb.Context.SaveChanges();
        }

        [Fact]
        public async Task AnyAsync_Specification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AnyAsync(_value1ShouldBe1);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AnyAsync_Specification_ShouldBeFalse()
        {
            var result = await _testDb.Context.Entities
                .AnyAsync(_value1ShouldBe4);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task AnyAsync_EmptySpecification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AnyAsync(_emptySpecification);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AllAsync_Specification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_value2ShouldBe3);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AllAsync_Specification_ShouldBeFalse()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_value1ShouldBe1);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task AllAsync_EmptySpecification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_emptySpecification);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task CountAsync_Specification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .CountAsync(_value1ShouldBe1);

            result.ShouldBe(2);
        }

        [Fact]
        public async Task CountAsync_EmptySpecification_ShouldBeEqualCount()
        {
            var result = await _testDb.Context.Entities
                .CountAsync(_emptySpecification);

            result.ShouldBe(_testDb.Context.Entities.Count());
        }

        [Fact]
        public async Task FirstAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstAsync(_value1ShouldBe1);

            result.ShouldBe(_testDb.Context.Entities.Find(2));
        }

        [Fact]
        public async Task FirstAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstAsync(_emptySpecification);

            result.ShouldBe(_testDb.Context.Entities.Find(1));
        }

        [Fact]
        public async Task FirstOrDefaultAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstOrDefaultAsync(_value1ShouldBe1);

            result.ShouldBe(_testDb.Context.Entities.Find(2));
        }

        [Fact]
        public async Task FirstOrDefaultAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstOrDefaultAsync(_emptySpecification);

            result.ShouldBe(_testDb.Context.Entities.Find(1));
        }

        [Fact]
        public async Task SingleAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .SingleAsync(_value1ShouldBe2);

            result.ShouldBe(_testDb.Context.Entities.Find(1));
        }

        [Fact]
        public async Task SingleAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            await Should.ThrowAsync<InvalidOperationException>(() => _testDb.Context.Entities.SingleAsync(_emptySpecification));
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .SingleOrDefaultAsync(_value1ShouldBe2);

            result.ShouldBe(_testDb.Context.Entities.Find(1));
        }

        [Fact]
        public async Task SingleOrDefaultAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            await Should.ThrowAsync<InvalidOperationException>(() => _testDb.Context.Entities.SingleOrDefaultAsync(_emptySpecification));
        }

        public void Dispose()
        {
            _testDb.Dispose();
        }
    }
}
