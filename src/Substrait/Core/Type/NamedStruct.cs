// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Tools;

namespace Substrait.Core.Type;

/// <summary>
/// Base class for named structs, <see cref="Protobuf.NamedStruct"/>.
/// </summary>
public class NamedStruct : IEquatable<NamedStruct>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NamedStruct"/> class.
    /// </summary>
    /// <param name="names">The names of the struct fields.</param>
    /// <param name="type">The struct type.</param>
    public NamedStruct(IEnumerable<string> names, ParameterizedType.Struct type)
    {
        this.Names = names.ToImmutableList();
        this.Struct = type;
    }

    /// <summary>
    /// Gets struct type.
    /// </summary>
    public ParameterizedType.Struct Struct { get; }

    /// <summary>
    /// Gets names of the struct fields.
    /// </summary>
    public IReadOnlyList<string> Names { get; }

    /// <inheritdoc/>
    public bool Equals(NamedStruct? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && Enumerable.SequenceEqual(this.Names, other.Names) && this.Struct.Equals(other.Struct);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as NamedStruct);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Names.CombineHashCodes(), this.Struct);
    }
}
