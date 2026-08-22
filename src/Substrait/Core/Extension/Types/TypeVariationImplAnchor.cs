// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Types;

/// <summary>
/// Type variant anchor used to identify a type variant.
/// </summary>
public sealed class TypeVariationImplAnchor : IAnchor, IEquatable<TypeVariationImplAnchor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeVariationImplAnchor"/> class.
    /// </summary>
    /// <param name="namespaceStr">Namespace of the function anchor.</param>
    /// <param name="key">Key of the function anchor.</param>
    public TypeVariationImplAnchor(string namespaceStr, string key)
    {
        this.Namespace = namespaceStr;
        this.Key = key;
    }

    /// <inheritdoc/>
    public string Namespace { get; }

    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public bool Equals(TypeVariationImplAnchor? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && this.Namespace.Equals(other.Namespace) && this.Key.Equals(other.Key);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as TypeVariationImplAnchor);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Namespace, this.Key);
    }
}
