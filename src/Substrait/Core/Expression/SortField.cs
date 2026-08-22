// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using ProtoSortDirection = Substrait.Protobuf.SortField.Types.SortDirection;

namespace Substrait.Core.Expression;

/// <summary>
/// An immutable implementation of a sort field.
/// </summary>
public sealed class SortField : IEquatable<SortField>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SortField"/> class.
    /// </summary>
    /// <param name="expr">Expression to sort on.</param>
    /// <param name="sortDirection">Sort direction.</param>
    public SortField(IExpression expr, SortDirection sortDirection)
    {
        this.Expr = expr;
        this.Direction = sortDirection;
    }

    /// <summary>
    /// Sort direction.
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sort Direction Unspecified.
        /// </summary>
        Unspecified = ProtoSortDirection.Unspecified,

        /// <summary>
        /// Set Direction Asc Nulls First.
        /// </summary>
        SortDirectionAscNullsFirst = ProtoSortDirection.AscNullsFirst,

        /// <summary>
        /// Set Direction Asc Nulls Last.
        /// </summary>
        SortDirectionAscNullsLast = ProtoSortDirection.AscNullsLast,

        /// <summary>
        /// Set Direction Desc Nulls First.
        /// </summary>
        SortDirectionDescNullsFirst = ProtoSortDirection.DescNullsFirst,

        /// <summary>
        /// Set Direction Desc Nulls Last.
        /// </summary>
        SortDirectionDescNullsLast = ProtoSortDirection.DescNullsLast,

        /// <summary>
        /// Set Direction Clustered.
        /// </summary>
        SortDirectionClustered = ProtoSortDirection.Clustered,
    }

    /// <summary>
    /// Gets expression property.
    /// </summary>
    public IExpression Expr { get; }

    /// <summary>
    /// Gets direction property.
    /// </summary>
    public SortDirection Direction { get; }

    /// <inheritdoc/>
    public bool Equals(SortField? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Direction == other?.Direction && this.Expr.Equals(other.Expr);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as SortField);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Expr, this.Direction);
    }
}
