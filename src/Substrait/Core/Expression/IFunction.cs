// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Extension.Functions;
using Substrait.Core.Type;
using static Substrait.Core.Expression.AggregateFunctionInvocation;

namespace Substrait.Core.Expression;

/// <summary>
/// Captures high-level of function invocation to produce succinct C# pattern match over expression trees.
/// </summary>
public interface IFunction
{
    /// <summary>
    /// Kinds of functions.
    /// </summary>
    [Flags]
    public enum FunctionKind
    {
        /// <summary>
        /// Scalar function.
        /// </summary>
        SCALAR = 1,

        /// <summary>
        /// Aggregate functions.
        /// </summary>
        AGGREGATE = 2,

        /// <summary>
        /// Window functions.
        /// </summary>
        WINDOW = 4,
    }

    /// <summary>
    /// Gets the name of the invoked function.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the kind of function.
    /// </summary>
    public FunctionKind Kind { get; }

    /// <summary>
    /// Gets aggregation invocation if the function is aggregate.
    /// </summary>
    public AggregationInvocation? AggregationInvocation { get; }

    /// <summary>
    /// Gets aggregation invocation if the function is aggregate.
    /// </summary>
    public AggregationPhase? AggregationPhase { get; }

    /// <summary>
    /// Gets the output type of the function.
    /// </summary>
    public IType OutputType { get; }

    /// <summary>
    /// Gets the funtion arguments.
    /// </summary>
    public IReadOnlyList<IFunctionArg> Arguments { get; }
}
