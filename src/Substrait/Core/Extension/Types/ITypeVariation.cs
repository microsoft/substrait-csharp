// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Extension.Functions;
using Substrait.Core.Type;

namespace Substrait.Core.Extension.Types;

/// <summary>
/// Interface for type variation.
/// </summary>
public interface ITypeVariation
{
    /// <summary>
    /// Gets the namespace where this type variation is declared.
    /// </summary>
    public string Namespace { get; }

    /// <summary>
    /// Gets the name of this type variation.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the description of this type variation.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the base type name of this type variation.
    /// In YAML definition, this corresponds to *parent* property.
    /// </summary>
    /// <remarks>Type parameters are not specified in the YAML thus we may not
    /// be able to instantiate a type.</remarks>
    public string BaseTypeName { get; }

    /// <summary>
    /// Gets the function behavior of this type variation.
    /// </summary>
    public FunctionBehavior FunctionBehavior { get; }

    /// <summary>
    /// Tests whether the <paramref name="type"/> is compatible with this type variation.
    /// </summary>
    /// <param name="type">type to test.</param>
    /// <returns>true if the type variation is compatible.</returns>
    public bool IsCompatible(IType type);
}
