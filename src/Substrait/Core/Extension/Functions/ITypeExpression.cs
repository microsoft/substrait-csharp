// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Type expression used in the definition of functions arguments in extensions.
/// </summary>
public interface ITypeExpression
{
    /// <summary>
    /// Gets the short type name that can be used in YAML definition and anchor key construction.
    /// </summary>
    /// <returns>The short string representation of the type expression.</returns>
    /// <seealso href="https://substrait.io/extensions/#function-signature-compound-names">Function Signature Compund Names</seealso>.
    string ShortTypeName { get; }

    /// <summary>
    /// Gets the type name that Substrait type parser can handle excluding type parameters.
    /// </summary>
    /// <returns>The type name excluding type parameters (e.g., T instead of T&lt;N&gt;).</returns>
    string TypeName { get; }

    /// <summary>
    /// Converts the type expression to a type string that Substrait type parser can handle.
    /// The result is prefixed by <see cref="TypeName"/> and includes type parameters if any.
    /// </summary>
    /// <returns>The string representation of the type expression.</returns>
    string ToTypeString();
}
