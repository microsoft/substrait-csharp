// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Expression;
using Substrait.Core.Type;
using Substrait.Tools;

namespace Substrait.Core.Relation;

/// <summary>
/// An immutable implementation of the READ relational operator for a named table, <see cref="Protobuf.ReadRel.NamedTable"/>.
/// </summary>
public sealed class NamedTableRead : Read
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedTableRead"/> class.
    /// </summary>
    /// <param name="initialSchema">The schema for the named table read.</param>
    /// <param name="names">List of names that make the qualified name of the table.</param>
    /// <param name="filter">Filter expression to apply.</param>
    public NamedTableRead(NamedStruct initialSchema, IEnumerable<string> names, IExpression? filter)
    {
        this.InitialSchema = initialSchema;
        this.Names = names.ToImmutableList();
        this.Filter = filter;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedTableRead"/> class.
    /// </summary>
    /// <param name="initialSchema">The schema for the named table read.</param>
    /// <param name="names">List of names that make the qualified name of the table.</param>
    /// <param name="filter">Filter expression to apply.</param>
    /// <param name="transmute">Remap to apply on the output.</param>
    public NamedTableRead(NamedStruct initialSchema, IEnumerable<string> names, IExpression? filter, Remap? transmute)
        : this(initialSchema, names, filter)
    {
        this.Transmute = transmute;
    }

    /// <inheritdoc/>
    public override NamedStruct InitialSchema { get; }

    /// <summary>
    /// Gets qualified name of the table.
    /// </summary>
    public IReadOnlyList<string> Names { get; }

    /// <inheritdoc/>
    public override IExpression? Filter { get; }

    /// <inheritdoc/>
    public override Remap? Transmute { get; }

    /// <inheritdoc/>
    public override TOutput Accept<TContext, TOutput>(RelVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

    /// <inheritdoc/>
    public override int GetNodeHashCode()
    {
        return HashCode.Combine(nameof(NamedTableRead), this.Names.CombineHashCodes(), this.InitialSchema, this.Filter);
    }

    /// <inheritdoc/>
    public override bool NodeEquals(IRel other)
    {
        return other is NamedTableRead o
            && Enumerable.SequenceEqual(this.Names, o.Names)
            && this.InitialSchema.Equals(o.InitialSchema)
            && this.Filter.EqualsWithNull(o.Filter);
    }
}
