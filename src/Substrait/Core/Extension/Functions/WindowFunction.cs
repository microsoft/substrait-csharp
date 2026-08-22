// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of a window function.
/// </summary>
public sealed class WindowFunction
{
    private readonly string name;
    private readonly string description;
    private readonly IReadOnlyList<WindowFunctionImpl> impls;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowFunction"/> class.
    /// </summary>
    /// <param name="name">Name of the window function.</param>
    /// <param name="description">Description of the window function.</param>
    /// <param name="impls">Different implementations of the window function.</param>
    public WindowFunction(string name, string description, IEnumerable<WindowFunctionImpl> impls)
    {
        this.name = name;
        this.description = description;
        this.impls = impls.ToImmutableList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowFunction"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public WindowFunction()
        : this(string.Empty, string.Empty, ImmutableList<WindowFunctionImpl>.Empty)
    {
    }

    /// <summary>
    /// Gets name of the window function.
    /// </summary>
    public string Name
    {
        get => this.name;
        init => this.name = value;
    }

    /// <summary>
    /// Gets description of the window function.
    /// </summary>
    public string Description
    {
        get => this.description;
        init => this.description = value;
    }

    /// <summary>
    /// Gets implementations of the window function.
    /// </summary>
    public IReadOnlyList<WindowFunctionImpl> Impls
    {
        get => this.impls;
        init => this.impls = value.ToImmutableList();
    }

    /// <summary>
    /// Resolves the window function.
    /// </summary>
    /// <param name="uri">The URI on which the window function should be resolved.</param>
    /// <returns>The window function implementations.</returns>
    public IEnumerable<WindowFunctionImpl> Resolve(string uri)
    {
        return this.Impls.Select(f => f.Resolve(uri, this.Name, this.Description));
    }
}
