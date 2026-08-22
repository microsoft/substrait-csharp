// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Extension.Functions;
using Substrait.Core.Type;
using Substrait.Tools;
using ProtoAggregationInvocation = Substrait.Protobuf.AggregateFunction.Types.AggregationInvocation;
using ProtoAggregationPhase = Substrait.Protobuf.AggregationPhase;

namespace Substrait.Core.Expression;

/// <summary>
/// An immutable implementation of an invocation of an aggregate function.
/// </summary>
public sealed class AggregateFunctionInvocation : IEquatable<AggregateFunctionInvocation>, IFunction
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateFunctionInvocation"/> class.
    /// </summary>
    /// <param name="namespaceStr">Function namespace.</param>
    /// <param name="key">Function key.</param>
    /// <param name="args">Function arguments.</param>
    /// <param name="type">Return type of the function.</param>
    /// <param name="aggregationPhase">Aggregation phase.</param>
    /// <param name="declaration">Function declaration.</param>
    /// <param name="aggregationInvocation">Aggregation invocation.</param>
    public AggregateFunctionInvocation(string namespaceStr, string key, IEnumerable<IFunctionArg> args, IType type, AggregationPhase aggregationPhase, AggregationInvocation aggregationInvocation, AggregateFunctionImpl? declaration)
    {
        this.Namespace = namespaceStr;
        this.Key = key;
        this.Name = GetName(this.Key);
        this.Arguments = args.ToImmutableList();
        this.OutputType = type;
        this.Sort = ImmutableList<SortField>.Empty;
        this.Phase = aggregationPhase;
        this.Invocation = aggregationInvocation;
        this.Declaration = declaration;
    }

    /// <summary>
    /// Aggregation invocation.
    /// </summary>
    public enum AggregationInvocation
    {
        /// <summary>
        /// Aggregation invocation unspecified.
        /// </summary>
        Unspecified = ProtoAggregationInvocation.Unspecified,

        /// <summary>
        /// Aggregation invocation All.
        /// </summary>
        All = ProtoAggregationInvocation.All,

        /// <summary>
        /// Aggregation invocation Any.
        /// </summary>
        Distinct = ProtoAggregationInvocation.Distinct,
    }

    /// <summary>
    /// Aggregation phase.
    /// </summary>
    public enum AggregationPhase
    {
        /// <summary>
        /// Aggregation phase unspecified.
        /// </summary>
        Unspecified = ProtoAggregationPhase.Unspecified,

        /// <summary>
        /// Aggregation phase InitialToIntermediate.
        /// </summary>
        InitialToIntermediate = ProtoAggregationPhase.InitialToIntermediate,

        /// <summary>
        /// Aggregation phase IntermediateToIntermediate.
        /// </summary>
        IntermediateToIntermediate = ProtoAggregationPhase.IntermediateToIntermediate,

        /// <summary>
        /// Aggregation phase InitialToResult.
        /// </summary>
        InitialToResult = ProtoAggregationPhase.InitialToResult,

        /// <summary>
        /// Aggregation phase IntermediateToResult.
        /// </summary>
        IntermediateToResult = ProtoAggregationPhase.IntermediateToResult,
    }

    /// <summary>
    /// Gets namespace of the function.
    /// </summary>
    public string Namespace { get; }

    /// <summary>
    /// Gets key of the function.
    /// </summary>
    public string Key { get; }

    /// <inheritdoc/>
    public string Name { get; }

    /// <summary>
    /// Gets the declaration.
    /// </summary>
    public AggregateFunctionImpl? Declaration { get; }

    /// <summary>
    /// Gets the arguments.
    /// </summary>
    public IReadOnlyList<IFunctionArg> Arguments { get; }

    /// <summary>
    /// Gets the aggregation phase.
    /// </summary>
    public AggregationPhase Phase { get; }

    /// <summary>
    /// Gets the sort.
    /// </summary>
    public IReadOnlyList<SortField> Sort { get; }

    /// <summary>
    /// Gets the output type.
    /// </summary>
    public IType OutputType { get; }

    /// <summary>
    /// Gets the invocation.
    /// </summary>
    public AggregationInvocation Invocation { get; }

    /// <inheritdoc/>
    public IFunction.FunctionKind Kind => IFunction.FunctionKind.AGGREGATE;

    /// <inheritdoc/>
    AggregationInvocation? IFunction.AggregationInvocation => this.Invocation switch
    {
        AggregationInvocation.Unspecified => null,
        _ => this.Invocation,
    };

    /// <inheritdoc/>
    AggregationPhase? IFunction.AggregationPhase => this.Phase switch
    {
        AggregationPhase.Unspecified => null,
        _ => this.Phase,
    };

    /// <inheritdoc/>
    public bool Equals(AggregateFunctionInvocation? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.Namespace.Equals(other?.Namespace)
            && this.Key.Equals(other.Key)
            && this.OutputType.Equals(other.OutputType)
            && this.Phase == other.Phase
            && this.Invocation == other.Invocation
            && Enumerable.SequenceEqual(this.Sort, other.Sort)
            && Enumerable.SequenceEqual(this.Arguments, other.Arguments)
            && (this.Declaration?.Equals(other.Declaration) ?? other.Declaration is null);
    }

    /// <inheritdoc/>
    public override bool Equals(object? other)
    {
        return this.Equals(other as AggregateFunctionInvocation);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(
            this.Namespace,
            this.Key,
            this.OutputType,
            this.Phase,
            this.Invocation,
            this.Sort.CombineHashCodes(),
            this.Arguments.CombineHashCodes(),
            this.Declaration);
    }

    private static string GetName(string key)
    {
        int index = key.IndexOf(':');
        int length = index >= 0 ? index : key.Length;
        return key.AsSpan().Slice(0, length).ToString();
    }
}
