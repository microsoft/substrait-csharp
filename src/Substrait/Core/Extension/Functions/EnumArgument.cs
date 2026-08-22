// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of an enum argument for a function.
/// </summary>
public sealed class EnumArgument : IArgument
{
    private readonly IReadOnlyList<string> options;
    private readonly string name;
    private readonly string description;
    private readonly bool required;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumArgument"/> class.
    /// </summary>
    /// <param name="options">Options of the enum argument.</param>
    /// <param name="name">Name of the enum argument.</param>
    /// <param name="description">Description of the enum argument.</param>
    /// <param name="required">Whether the argument is required.</param>
    public EnumArgument(IEnumerable<string> options, string name, string description, bool required)
    {
        this.options = options.ToImmutableList();
        this.name = name;
        this.description = description;
        this.required = required;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumArgument"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public EnumArgument()
        : this([], string.Empty, string.Empty, true)
    {
    }

    /// <summary>
    /// Gets the options.
    /// </summary>
    public IReadOnlyList<string> Options
    {
        get => this.options;
        init => this.options = value.ToImmutableList();
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
    public string ToTypeString() => "req";
}
