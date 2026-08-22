// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Expression;
using Substrait.Core.Type;
using Substrait.Tools;

namespace Substrait.Core.Relation;

/// <summary>
/// An immutable implementation of the SORT relational operator representing ORDER BY semantics, <see cref="Protobuf.SortRel"/>.
/// </summary>
public sealed class Sort : SingleInput
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Sort"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="sortFields">Expressions to sort by.</param>
    public Sort(IRel input, IEnumerable<SortField> sortFields)
    {
        this.Input = input;
        this.SortFields = sortFields.ToImmutableList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Sort"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="sortFields">Expressions to sort by.</param>
    /// <param name="transmute">The remap to apply on the output.</param>
    public Sort(IRel input, IEnumerable<SortField> sortFields, Remap? transmute)
      : this(input, sortFields)
    {
        this.Transmute = transmute;
    }

    /// <inheritdoc/>
    public override IRel Input { get; }

    /// <summary>
    /// Gets sort fields.
    /// </summary>
    public IReadOnlyList<SortField> SortFields { get; }

    /// <inheritdoc/>
    public override Remap? Transmute { get; }

    /// <inheritdoc/>
    public override TOutput Accept<TContext, TOutput>(RelVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

    /// <inheritdoc/>
    public override int GetNodeHashCode()
    {
        return HashCode.Combine(nameof(Sort), this.SortFields.CombineHashCodes());
    }

    /// <inheritdoc/>
    public override bool NodeEquals(IRel other)
    {
        return other is Sort o && Enumerable.SequenceEqual(this.SortFields, o.SortFields);
    }

    /// <inheritdoc/>
    protected override ParameterizedType.Struct DeriveRecordType() => this.Input.RecordType;
}
