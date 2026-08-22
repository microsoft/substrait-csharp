// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// An immutable implementation of <see cref="IVariadicBehavior"/>.
/// </summary>
public sealed class VariadicBehavior : IVariadicBehavior
{
    private readonly int min;
    private readonly int? max;
    private readonly IVariadicBehavior.ParameterConsistency? parameterConsistency;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariadicBehavior"/> class.
    /// </summary>
    /// <param name="min">Minimum number of arguments.</param>
    public VariadicBehavior(int min)
    {
        this.min = min;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariadicBehavior"/> class.
    /// </summary>
    /// <param name="min">Minimum number of arguments.</param>
    /// <param name="parameterConsistency">Parameter consistency in the variadic behavior.</param>
    public VariadicBehavior(int min, IVariadicBehavior.ParameterConsistency parameterConsistency)
        : this(min)
    {
        this.parameterConsistency = parameterConsistency;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariadicBehavior"/> class.
    /// </summary>
    /// <param name="min">Minimum number of arguments.</param>
    /// <param name="max">Maximum number of arguments.</param>
    public VariadicBehavior(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariadicBehavior"/> class.
    /// </summary>
    /// <param name="min">Minimum number of arguments.</param>
    /// <param name="max">Maximum number of arguments.</param>
    /// <param name="parameterConsistency">Parameter consistency in the variadic behavior.</param>
    public VariadicBehavior(int min, int max, IVariadicBehavior.ParameterConsistency parameterConsistency)
        : this(min, max)
    {
        this.parameterConsistency = parameterConsistency;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VariadicBehavior"/> class.
    /// This is only used for deserialization purposes from YAML files.
    /// </summary>
    public VariadicBehavior()
        : this(0)
    {
    }

    /// <summary>
    /// Gets minimum number of arguments.
    /// </summary>
    public int Min
    {
        get => this.min;
        init => this.min = value;
    }

    /// <summary>
    /// Gets maximum number of arguments.
    /// </summary>
    public int? Max
    {
        get => this.max;
        init => this.max = value;
    }

    /// <summary>
    /// Gets the parameter consistency in the variadic behavior.
    /// </summary>
    /// <returns>The parameter consistency.</returns>
    public IVariadicBehavior.ParameterConsistency? ParameterConsistency
    {
        get => this.parameterConsistency;
        init => this.parameterConsistency = value;
    }

    /// <inheritdoc/>
    public IVariadicBehavior.ParameterConsistency BParameterConsistency => this.parameterConsistency ?? IVariadicBehavior.ParameterConsistency.Consistent;
}
