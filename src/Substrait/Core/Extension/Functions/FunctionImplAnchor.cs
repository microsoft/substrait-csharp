// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Immutable implementation of a function anchor used to identify a function implementation.
/// </summary>
public sealed class FunctionImplAnchor : IAnchor, IEquatable<FunctionImplAnchor>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionImplAnchor"/> class.
    /// </summary>
    /// <param name="namespaceStr">Namespace of the function anchor.</param>
    /// <param name="key">Key of the function anchor.</param>
    public FunctionImplAnchor(string namespaceStr, string key)
    {
        this.Namespace = namespaceStr;
        this.Key = key;
    }

    /// <inheritdoc/>
    public string Namespace { get; }

    /// <inheritdoc/>
    public string Key { get; }

    /// <inheritdoc/>
    public bool Equals(FunctionImplAnchor? other)
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
        return this.Equals(obj as FunctionImplAnchor);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Namespace, this.Key);
    }
}
