// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Type;

/// <summary>
/// Type comaprison mode.
/// </summary>
[Flags]
public enum ITypeComparison
{
    /// <summary>
    /// Compare type nullability (e.g., decimal vs. decimal? == false).
    /// </summary>
    Nullability = 1,

    /// <summary>
    /// Compare type parameters (e.g., decimal&lt;10,5&gt; vs. decimal&lt;5,10&gt; == false).
    /// </summary>
    TypeParameter = 2,

    /// <summary>
    /// Compare type variantions (e.g., decimal[0]&lt;10,5&gt; vs. decimal[1]&lt;10,5&gt; == false).
    /// </summary>
    TypeVariation = 4,

    /// <summary>
    /// Strictly compare types.
    /// </summary>
    Strict = Nullability | TypeParameter | TypeVariation,

    /// <summary>
    /// Ignore nullability, type parameters, and type variation.
    /// </summary>
    IgnoreNullability = 0,

    /// <summary>
    /// Ignore type parameters and type variation.
    /// </summary>
    IgnoreTypeParameters = Nullability,

    /// <summary>
    /// Ignore type variation.
    /// </summary>
    IgnoreTypeVariation = Nullability | TypeParameter,
}
