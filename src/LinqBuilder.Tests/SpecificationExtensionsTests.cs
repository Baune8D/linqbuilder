﻿using LinqBuilder.Testing;
using Shouldly;
using Xunit;

namespace LinqBuilder.Tests
{
    public class SpecificationExtensionsTests
    {
        private readonly ISpecification<Entity> _emptySpecification = Spec<Entity>.New();
        private readonly ISpecification<Entity> _value1ShouldBe3 = Spec<Entity>.New(entity => entity.Value1 == 3);
        private readonly ISpecification<Entity> _value1ShouldBe5 = Spec<Entity>.New(entity => entity.Value1 == 5);
        private readonly ISpecification<Entity> _value2ShouldBe5 = Spec<Entity>.New(entity => entity.Value2 == 5);

        [Theory]
        [ClassData(typeof(TestData))]
        public void IsSatisfiedBy_Theory(Entity entity, bool expected)
        {
            _value1ShouldBe3
                .IsSatisfiedBy(entity)
                .ShouldBe(expected);
        }

        [Theory]
        [ClassData(typeof(AndTestData))]
        public void And_IsSatisfiedBy_Theory(Entity entity, bool expected)
        {
            _value1ShouldBe3
                .And(_value2ShouldBe5)
                .IsSatisfiedBy(entity)
                .ShouldBe(expected);
        }

        [Fact]
        public void AndNew_IsSatisfiedBy_ShouldBeTrue()
        {
            _value1ShouldBe3
                .And(_emptySpecification)
                .IsSatisfiedBy(new Entity { Value1 = 3 })
                .ShouldBe(true);
        }

        [Fact]
        public void NewAnd_IsSatisfiedBy_ShouldBeTrue()
        {
            _emptySpecification
                .And(_value2ShouldBe5)
                .IsSatisfiedBy(new Entity { Value2 = 5 })
                .ShouldBe(true);
        }

        [Fact]
        public void AndEmpty_IsSatisfiedBy_ShouldBeTrue()
        {
            _emptySpecification
                .And(_emptySpecification)
                .IsSatisfiedBy(new Entity())
                .ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(OrTestData))]
        public void Or_IsSatisfiedBy_Theory(Entity entity, bool expected)
        {
            _value1ShouldBe3
                .Or(_value1ShouldBe5)
                .IsSatisfiedBy(entity)
                .ShouldBe(expected);
        }

        [Fact]
        public void OrNew_IsSatisfiedBy_ShouldBeTrue()
        {
            _value1ShouldBe3
                .Or(_emptySpecification)
                .IsSatisfiedBy(new Entity { Value1 = 3 })
                .ShouldBe(true);
        }

        [Fact]
        public void NewOr_IsSatisfiedBy_ShouldBeTrue()
        {
            _emptySpecification
                .Or(_value1ShouldBe5)
                .IsSatisfiedBy(new Entity { Value1 = 5 })
                .ShouldBe(true);
        }

        [Fact]
        public void OrEmpty_IsSatisfiedBy_ShouldBeTrue()
        {
            _emptySpecification
                .Or(_emptySpecification)
                .IsSatisfiedBy(new Entity())
                .ShouldBe(true);
        }

        [Theory]
        [ClassData(typeof(NotTestData))]
        public void Not_IsSatisfiedBy_Theory(Entity entity, bool expected)
        {
            _value1ShouldBe5
                .Not()
                .IsSatisfiedBy(entity)
                .ShouldBe(expected);
        }

        [Fact]
        public void NewNot_IsSatisfiedBy_ShouldBeTrue()
        {
            _emptySpecification
                .Not()
                .IsSatisfiedBy(new Entity())
                .ShouldBe(true);
        }

        [Fact]
        public void Clone_Specifications_ShouldNotBeEqual()
        {
            var spec1 = _value1ShouldBe3;
            var spec2 = spec1.Clone();
            spec1.ShouldNotBe(spec2);
            spec1.Internal.QuerySpecification.ShouldBe(spec2.Internal.QuerySpecification);
            spec1.Internal.OrderSpecifications.ShouldBe(spec2.Internal.OrderSpecifications);
            spec1.Internal.Skip.ShouldBe(spec2.Internal.Skip);
            spec1.Internal.Take.ShouldBe(spec2.Internal.Take);
        }

        private class TestData : EntityTheoryData
        {
            public TestData()
            {
                AddEntity(3, 1, true);
                AddEntity(4, 1, false);
            }
        }

        private class AndTestData : EntityTheoryData
        {
            public AndTestData()
            {
                AddEntity(3, 5, true);
                AddEntity(3, 4, false);
            }
        }

        private class OrTestData : EntityTheoryData
        {
            public OrTestData()
            {
                AddEntity(3, 1, true);
                AddEntity(5, 1, true);
                AddEntity(4, 1, false);
            }
        }

        private class NotTestData : EntityTheoryData
        {
            public NotTestData()
            {
                AddEntity(3, 1, true);
                AddEntity(5, 1, false);
            }
        }
    }
}
