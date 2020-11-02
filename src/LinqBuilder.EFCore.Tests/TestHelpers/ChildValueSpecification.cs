using System;
using System.Linq;
using System.Linq.Expressions;
using LinqBuilder.EFCore.Tests.Shared;

namespace LinqBuilder.EFCore.Tests.TestHelpers
{
    public class ChildValueSpecification : Specification<Entity>
    {
        private readonly int _value;

        public ChildValueSpecification(int value)
        {
            _value = value;
        }

        public override Expression<Func<Entity, bool>> AsExpression()
        {
            return entity => entity.ChildEntities.Any(x => x.Value == _value);
        }
    }
}