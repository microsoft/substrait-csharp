// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Expression;
using Substrait.Core.Type;

namespace Substrait.Core.Relation;

/// <summary>
/// An immutable implementation of the FETCH operator, <see cref="Protobuf.FetchRel"/>.
/// </summary>
public sealed class Fetch : SingleInput
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Fetch"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="count">Expression representing the number of rows to fetch. -1 represents that all rows should be fetched.</param>
    /// <param name="offset">Expression representing the offset to start fetching from.</param>
    public Fetch(IRel input, IExpression count, IExpression offset)
    {
        this.Input = input;
        this.Count = count;
        this.Offset = offset;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Fetch"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="count">Expression representing the number of rows to fetch. -1 represents that all rows should be fetched.</param>
    /// <param name="offset">Expression representing the offset to start fetching from.</param>
    /// <param name="transmute">The remap to apply on the output.</param>
    public Fetch(IRel input, IExpression count, IExpression offset, Remap? transmute)
      : this(input, count, offset)
    {
        this.Transmute = transmute;
    }

    /// <inheritdoc/>
    public override IRel Input { get; }

    /// <summary>
    /// Gets the expression representing the number of rows to fetch.
    /// -1 represents that all rows should be fetched.
    /// </summary>
    public IExpression Count { get; }

    /// <summary>
    /// Gets the expression representing the offset to start fetching from.
    /// </summary>
    public IExpression Offset { get; }

    /// <inheritdoc/>
    public override Remap? Transmute { get; }

    /// <inheritdoc/>
    public override TOutput Accept<TContext, TOutput>(RelVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

    /// <inheritdoc/>
    public override int GetNodeHashCode()
    {
        return HashCode.Combine(nameof(Fetch), this.Count, this.Offset);
    }

    /// <inheritdoc/>
    public override bool NodeEquals(IRel other)
    {
        return (other is Fetch o) && this.Count.Equals(o.Count) && this.Offset.Equals(o.Offset);
    }

    /// <inheritdoc/>
    protected override ParameterizedType.Struct DeriveRecordType() => this.Input.RecordType;
}
