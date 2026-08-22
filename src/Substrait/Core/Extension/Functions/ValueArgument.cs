// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Type;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of a value argument for a function.
/// </summary>
public sealed class ValueArgument : IArgument
{
    private readonly string value;
    private readonly string name;
    private readonly string description;
    private readonly bool required;
    private readonly bool? constant;
    private readonly Lazy<ITypeExpression> typeExpression;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueArgument"/> class.
    /// </summary>
    /// <param name="value">Type of the value argument.</param>
    /// <param name="name">Name of the value argument.</param>
    /// <param name="description">Description of the value argument.</param>
    /// <param name="required">Whether the argument is required.</param>
    public ValueArgument(string value, string name, string description, bool required)
    {
        this.value = value;
        this.name = name;
        this.description = description;
        this.required = required;
        this.typeExpression = new Lazy<ITypeExpression>(() =>
        {
            return TypeExpressionParser.Parse(this.value);
        });
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueArgument"/> class.
    /// </summary>
    /// <param name="value">Type of the value argument.</param>
    /// <param name="name">Name of the value argument.</param>
    /// <param name="description">Description of the value argument.</param>
    /// <param name="required">Whether the argument is required.</param>
    /// <param name="constant">Whether the argument is constant.</param>
    public ValueArgument(string value, string name, string description, bool required, bool? constant)
      : this(value, name, description, required)
    {
        this.constant = constant;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueArgument"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public ValueArgument()
        : this(string.Empty, string.Empty, string.Empty, true, false)
    {
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    public string Value
    {
        get => this.value;
        init => this.value = value;
    }

    /// <summary>
    /// Gets whether the argument is constant.
    /// </summary>
    public bool? Constant
    {
        get => this.constant;
        init => this.constant = value;
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

    /// <summary>
    /// Gets the type expression for this argument.
    /// </summary>
    /// <returns>The type expression for this argument.</returns>
    public ITypeExpression GetTypeExpression()
    {
        return this.typeExpression.Value;
    }

    /// <inheritdoc/>
    public string ToTypeString()
    {
        return this.typeExpression.Value.ShortTypeName;
    }
}
