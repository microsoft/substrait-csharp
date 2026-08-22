// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Type;
using Substrait.Tools;

namespace Substrait.Core.Relation;

/// <summary>
/// An immutable implementation of the remap operation.
/// </summary>
public sealed class Remap : IEquatable<Remap>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Remap"/> class.
    /// </summary>
    /// <param name="indices">The mapping of output fields (position in the enumerable) to input fields (value).</param>
    public Remap(IEnumerable<int> indices)
    {
        this.Indices = indices.ToImmutableList();
    }

    /// <summary>
    /// Gets indices.
    /// </summary>
    public IReadOnlyList<int> Indices { get; }

    /// <summary>
    /// Transmute.
    /// </summary>
    /// <param name="initial">The initial struct.</param>
    /// <returns>The struct after applying the remap.</returns>
    public ParameterizedType.Struct Transmute(ParameterizedType.Struct initial)
    {
        IReadOnlyList<IType> types = initial.Fields;
        return TypeFactory.Of(initial.Nullable).Struct(this.Indices.Select(i => types.ElementAt(i)).ToImmutableList());
    }

    /// <inheritdoc/>
    public bool Equals(Remap? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && Enumerable.SequenceEqual(this.Indices, other.Indices);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Remap);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.Indices.CombineHashCodes();
    }
}
