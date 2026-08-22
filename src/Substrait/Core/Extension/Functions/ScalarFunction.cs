// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of a scalar function.
/// </summary>
public sealed class ScalarFunction
{
    private readonly string name;
    private readonly string description;
    private readonly IReadOnlyList<ScalarFunctionImpl> impls;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScalarFunction"/> class.
    /// </summary>
    /// <param name="name">Name of the scalar function.</param>
    /// <param name="description">Description of the scalar function.</param>
    /// <param name="impls">Different implementations of the scalar function.</param>
    public ScalarFunction(string name, string description, IEnumerable<ScalarFunctionImpl> impls)
    {
        this.name = name;
        this.description = description;
        this.impls = impls.ToImmutableList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ScalarFunction"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public ScalarFunction()
        : this(string.Empty, string.Empty, ImmutableList<ScalarFunctionImpl>.Empty)
    {
    }

    /// <summary>
    /// Gets name of the scalar function.
    /// </summary>
    public string Name
    {
        get => this.name;
        init => this.name = value;
    }

    /// <summary>
    /// Gets description of the scalar function.
    /// </summary>
    public string Description
    {
        get => this.description;
        init => this.description = value;
    }

    /// <summary>
    /// Gets implementations of the scalar function.
    /// </summary>
    public IReadOnlyList<ScalarFunctionImpl> Impls
    {
        get => this.impls;
        init => this.impls = value.ToImmutableList();
    }

    /// <summary>
    /// Resolves the scalar function.
    /// </summary>
    /// <param name="uri">The URI on which the scalar function should be resolved.</param>
    /// <returns>The scalar function implementations.</returns>
    public IEnumerable<ScalarFunctionImpl> Resolve(string uri)
    {
        return this.Impls.Select(f => f.Resolve(uri, this.Name, this.Description));
    }
}
