// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Type;
using static Substrait.Core.Type.IType;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of a type argument for a function.
/// </summary>
public sealed class TypeArgument : IArgument
{
    private readonly ParameterizedType type;
    private readonly string name;
    private readonly string description;
    private readonly bool required;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgument"/> class.
    /// </summary>
    /// <param name="type">Type of the type argument.</param>
    /// <param name="name">Name of the type argument.</param>
    /// <param name="description">Description of the type argument.</param>
    /// <param name="required">Whether the argument is required.</param>
    public TypeArgument(ParameterizedType type, string name, string description, bool required)
    {
        this.type = type;
        this.name = name;
        this.description = description;
        this.required = required;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeArgument"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public TypeArgument()
        : this(new ParameterizedType.Struct([], NullableType.Required), string.Empty, string.Empty, true)
    {
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public ParameterizedType Type
    {
        get => this.type;
        init => this.type = value;
    }

    /// <inheritdoc/>
    public string Name
    {
        get => this.name;
        init => this.name = value;
    }

    /// <inheritdoc/>
    public string Description
    {
        get => this.description;
        init => this.description = value;
    }

    /// <inheritdoc/>
    public bool Required
    {
        get => this.required;
        init => this.required = value;
    }

    /// <inheritdoc/>
    public string ToTypeString() => "type";
}
