using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqBuilder.Core;
using LinqBuilder.EF6.AutoMapper.Tests.TestHelpers;
using LinqBuilder.EF6.Tests.Shared;
using Shouldly;
using Xunit;

namespace LinqBuilder.EF6.AutoMapper.Tests
{
    public class EntityFrameworkCoreExtensionsTests : IClassFixture<AutoMapperFixture>, IDisposable
    {
        private readonly ISpecification<ProjectedEntity> _emptySpecification = Spec<ProjectedEntity>.New();
        private readonly ISpecification<ProjectedEntity> _value1ShouldBe1 = Spec<ProjectedEntity>.New(entity => entity.Value1 == 1);
        private readonly ISpecification<ProjectedEntity> _value1ShouldBe2 = Spec<ProjectedEntity>.New(entity => entity.Value1 == 2);
        private readonly ISpecification<ProjectedEntity> _value1ShouldBe4 = Spec<ProjectedEntity>.New(entity => entity.Value1 == 4);
        private readonly ISpecification<ProjectedEntity> _value2ShouldBe3 = Spec<ProjectedEntity>.New(entity => entity.Value2 == 3);

        private readonly IConfigurationProvider _mapperConfig;
        private readonly TestDb _testDb;

        public EntityFrameworkCoreExtensionsTests(AutoMapperFixture autoMapperFixture)
        {
            _mapperConfig = autoMapperFixture.MapperConfig;
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
                .AnyAsync(_value1ShouldBe1, _mapperConfig);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AnyAsync_Specification_ShouldBeFalse()
        {
            var result = await _testDb.Context.Entities
                .AnyAsync(_value1ShouldBe4, _mapperConfig);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task AnyAsync_EmptySpecification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AnyAsync(_emptySpecification, _mapperConfig);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AllAsync_Specification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_value2ShouldBe3, _mapperConfig);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task AllAsync_Specification_ShouldBeFalse()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_value1ShouldBe1, _mapperConfig);

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task AllAsync_EmptySpecification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .AllAsync(_emptySpecification, _mapperConfig);

            result.ShouldBeTrue();
        }

        [Fact]
        public async Task CountAsync_Specification_ShouldBeTrue()
        {
            var result = await _testDb.Context.Entities
                .CountAsync(_value1ShouldBe1, _mapperConfig);

            result.ShouldBe(2);
        }

        [Fact]
        public async Task CountAsync_EmptySpecification_ShouldBeEqualCount()
        {
            var result = await _testDb.Context.Entities
                .CountAsync(_emptySpecification, _mapperConfig);

            result.ShouldBe(_testDb.Context.Entities.Count());
        }

        [Fact]
        public async Task FirstAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstAsync(_value1ShouldBe1, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(2);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task FirstAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstAsync(_emptySpecification, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(1);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstOrDefaultAsync(_value1ShouldBe1, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(2);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .FirstOrDefaultAsync(_emptySpecification, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(1);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task SingleAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .SingleAsync(_value1ShouldBe2, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(1);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task SingleAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            await Should.ThrowAsync<InvalidOperationException>(() => _testDb.Context.Entities.SingleAsync(_emptySpecification, _mapperConfig));
        }

        [Fact]
        public async Task SingleOrDefaultAsync_Specification_ShouldReturnCorrectResult()
        {
            var result = await _testDb.Context.Entities
                .SingleOrDefaultAsync(_value1ShouldBe2, _mapperConfig);

            var entity = _testDb.Context.Entities.Find(1);

            ProjectedShouldBeSameAsEntity(result, entity);
        }

        [Fact]
        public async Task SingleOrDefaultAsync_EmptySpecification_ShouldReturnCorrectResult()
        {
            await Should.ThrowAsync<InvalidOperationException>(() => _testDb.Context.Entities.SingleOrDefaultAsync(_emptySpecification, _mapperConfig));
        }

        public void Dispose()
        {
            _testDb.Dispose();
        }

        private static void ProjectedShouldBeSameAsEntity(ProjectedEntity projectedEntity, Entity entity)
        {
            projectedEntity.Id.ShouldBe(entity.Id);
            projectedEntity.Value1.ShouldBe(entity.Value1);
            projectedEntity.Value2.ShouldBe(entity.Value2);
        }
    }
}