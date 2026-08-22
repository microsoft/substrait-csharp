// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Types;

/// <summary>
/// Type variation.
/// </summary>
/// <see href="https://substrait.io/types/type_variations/">Substrait type Variation documentation.</see>
public sealed class TypeVariation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeVariation"/> class. Only for YAML deserialization.
    /// </summary>
    public TypeVariation()
        : this(string.Empty, string.Empty, string.Empty, FunctionBehavior.INHERITS)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeVariation"/> class.
    /// </summary>
    /// <param name="parent">Parent type.</param>
    /// <param name="name">name.</param>
    /// <param name="description">description.</param>
    /// <param name="functionBehavior">function behavior.</param>
    public TypeVariation(string parent, string name, string description, FunctionBehavior functionBehavior)
    {
        this.Parent = parent;
        this.Name = name;
        this.Description = description;
        this.Functions = functionBehavior;
    }

    /// <summary>
    /// Gets the base type.
    /// </summary>
    public string Parent { get; init; }

    /// <summary>
    /// Gets the name of type variation.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets the function behavior of this type variation.
    /// </summary>
    public FunctionBehavior Functions { get; init; }

    /// <summary>
    /// Gets the description of the type variation.
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Resolves the type variation.
    /// </summary>
    /// <param name="uri">The URI on which the type variation shoudl be resolved.</param>
    /// <returns>The type variation implementation.</returns>
    public TypeVariationImpl Resolve(string uri)
    {
        return new TypeVariationImpl(uri, this.Parent, this.Name, this.Description, this.Functions);
    }
}
