// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of <see cref="IOption"/>.
/// </summary>
public sealed class Option : IOption
{
    private readonly IReadOnlyList<string> values;
    private readonly string? description;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option"/> class.
    /// </summary>
    /// <param name="values">Values of the option.</param>
    /// <param name="description">Description of the option.</param>
    public Option(IEnumerable<string> values, string? description)
    {
        this.values = values.ToImmutableList();
        this.description = description;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Option"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public Option()
        : this([], null)
    {
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> Values
    {
        get => this.values;
        init => this.values = value.ToImmutableList();
    }

    /// <inheritdoc/>
    public string? Description
    {
        get => this.description;
        init => this.description = value;
    }
}
