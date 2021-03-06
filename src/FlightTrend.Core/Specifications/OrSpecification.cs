﻿using JetBrains.Annotations;

namespace FlightTrend.Core.Specifications
{

    [UsedImplicitly]
    public sealed class OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _left;
        private readonly ISpecification<T> _right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public bool IsSatisfiedBy(T value)
        {
            return _left.IsSatisfiedBy(value) || _right.IsSatisfiedBy(value);
        }
    }
}