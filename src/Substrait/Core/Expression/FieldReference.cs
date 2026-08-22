// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Type;

namespace Substrait.Core.Expression;

/// <summary>
/// An immutable implementation of the field reference expression, <see cref="Protobuf.Expression.Types.FieldReference"/>.
/// </summary>
public sealed class FieldReference : IExpression, IEquatable<FieldReference>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FieldReference"/> class.
    /// </summary>
    /// <param name="type">Type of the field.</param>
    /// <param name="fieldIndex">Index of the field.</param>
    /// <param name="subqueryLevels">The number of subquery boundaries to traverse upward for this field's reference.</param>
    public FieldReference(IType type, int fieldIndex, int subqueryLevels = 0)
    {
        if (subqueryLevels < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subqueryLevels), subqueryLevels, ">= 0");
        }

        this.Type = type;
        this.FieldIndex = fieldIndex;
        this.SubqueryLevels = subqueryLevels;
    }

    /// <inheritdoc/>
    public IType Type { get; }

    /// <inheritdoc/>
    public IEnumerable<IExpression> InputNodes => ImmutableList<IExpression>.Empty;

    /// <summary>
    /// Gets field index.
    /// </summary>
    public int FieldIndex { get; }

    /// <summary>
    /// Gets the number of subquery boundaries to traverse upward for this field's reference.
    /// 0 means that this reference is *not* an outer reference.
    /// </summary>
    /// <remarks>This is only relevant for nested subqueries.</remarks>
    public int SubqueryLevels { get; }

    /// <inheritdoc/>
    public TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

    /// <inheritdoc/>
    public bool Equals(FieldReference? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.FieldIndex == other?.FieldIndex && this.SubqueryLevels == other.SubqueryLevels && this.Type.Equals(other.Type);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as FieldReference);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Type, this.FieldIndex, this.SubqueryLevels);
    }
}
