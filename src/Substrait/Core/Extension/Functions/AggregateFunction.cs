// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of an aggregate function.
/// </summary>
public sealed class AggregateFunction
{
    private readonly string name;
    private readonly string description;
    private readonly IReadOnlyList<AggregateFunctionImpl> impls;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateFunction"/> class.
    /// </summary>
    /// <param name="name">Name of the aggregate function.</param>
    /// <param name="description">Description of the aggregate function.</param>
    /// <param name="impls">Different implementations of the aggregate function.</param>
    public AggregateFunction(string name, string description, IEnumerable<AggregateFunctionImpl> impls)
    {
        this.name = name;
        this.description = description;
        this.impls = impls.ToImmutableList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateFunction"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public AggregateFunction()
        : this(string.Empty, string.Empty, ImmutableList<AggregateFunctionImpl>.Empty)
    {
    }

    /// <summary>
    /// Gets name of the aggregate function.
    /// </summary>
    public string Name
    {
        get => this.name;
        init => this.name = value;
    }

    /// <summary>
    /// Gets description of the aggregate function.
    /// </summary>
    public string Description
    {
        get => this.description;
        init => this.description = value;
    }

    /// <summary>
    /// Gets implementations of the aggregate function.
    /// </summary>
    public IReadOnlyList<AggregateFunctionImpl> Impls
    {
        get => this.impls;
        init => this.impls = value.ToImmutableList();
    }

    /// <summary>
    /// Resolves the aggregate function.
    /// </summary>
    /// <param name="uri">The URI on which the aggregate function should be resolved.</param>
    /// <returns>The aggregate function implementations.</returns>
    public IEnumerable<AggregateFunctionImpl> Resolve(string uri)
    {
        return this.Impls.Select(f => f.Resolve(uri, this.Name, this.Description));
    }
}
