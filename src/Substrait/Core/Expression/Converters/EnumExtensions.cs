// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Tools;
using static Substrait.Core.Expression.AggregateFunctionInvocation;
using static Substrait.Core.Expression.Expression.Cast;
using static Substrait.Core.Expression.Expression.SetComparisonSubquery;
using static Substrait.Core.Expression.Expression.SetPredicateSubquery;
using static Substrait.Core.Expression.SortField;
using ProtoAggregationInvocation = Substrait.Protobuf.AggregateFunction.Types.AggregationInvocation;
using ProtoAggregationPhase = Substrait.Protobuf.AggregationPhase;
using ProtoComparisonOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetComparison.Types.ComparisonOp;
using ProtoFailureBehavior = Substrait.Protobuf.Expression.Types.Cast.Types.FailureBehavior;
using ProtoPredicateOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetPredicate.Types.PredicateOp;
using ProtoReductionOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetComparison.Types.ReductionOp;
using ProtoSortDirection = Substrait.Protobuf.SortField.Types.SortDirection;

namespace Substrait.Core.Expression.Converters;

/// <summary>
/// Conversion methods for expression enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts an aggregation invocation to protobuf.
    /// </summary>
    /// <param name="op">The invocation.</param>
    /// <returns>The protobuf invocation.</returns>
    public static ProtoAggregationInvocation ToProto(this AggregationInvocation op) =>
        EnumUtils.Cast<AggregationInvocation, ProtoAggregationInvocation>(op);

    /// <summary>
    /// Converts an aggregation invocation from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf invocation.</param>
    /// <returns>The internal invocation.</returns>
    public static AggregationInvocation FromProto(this ProtoAggregationInvocation proto) =>
        EnumUtils.Cast<ProtoAggregationInvocation, AggregationInvocation>(proto);

    /// <summary>
    /// Converts an aggregation phase to protobuf.
    /// </summary>
    /// <param name="op">The phase.</param>
    /// <returns>The protobuf phase.</returns>
    public static ProtoAggregationPhase ToProto(this AggregationPhase op) =>
        EnumUtils.Cast<AggregationPhase, ProtoAggregationPhase>(op);

    /// <summary>
    /// Converts an aggregation phase from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf phase.</param>
    /// <returns>The internal phase.</returns>
    public static AggregationPhase FromProto(this ProtoAggregationPhase proto) =>
        EnumUtils.Cast<ProtoAggregationPhase, AggregationPhase>(proto);

    /// <summary>
    /// Converts cast failure behavior to protobuf.
    /// </summary>
    /// <param name="op">The failure behavior.</param>
    /// <returns>The protobuf failure behavior.</returns>
    public static ProtoFailureBehavior ToProto(this FailureBehavior op) =>
        EnumUtils.Cast<FailureBehavior, ProtoFailureBehavior>(op);

    /// <summary>
    /// Converts cast failure behavior from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf failure behavior.</param>
    /// <returns>The internal failure behavior.</returns>
    public static FailureBehavior FromProto(this ProtoFailureBehavior proto) =>
        EnumUtils.Cast<ProtoFailureBehavior, FailureBehavior>(proto);

    /// <summary>
    /// Converts a sort direction to protobuf.
    /// </summary>
    /// <param name="op">The sort direction.</param>
    /// <returns>The protobuf sort direction.</returns>
    public static ProtoSortDirection ToProto(this SortDirection op) =>
        EnumUtils.Cast<SortDirection, ProtoSortDirection>(op);

    /// <summary>
    /// Converts a sort direction from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf sort direction.</param>
    /// <returns>The internal sort direction.</returns>
    public static SortDirection FromProto(this ProtoSortDirection proto) =>
        EnumUtils.Cast<ProtoSortDirection, SortDirection>(proto);

    /// <summary>
    /// Converts a set predicate operation to protobuf.
    /// </summary>
    /// <param name="op">The predicate operation.</param>
    /// <returns>The protobuf predicate operation.</returns>
    public static ProtoPredicateOp ToProto(this PredicateOp op) =>
        EnumUtils.Cast<PredicateOp, ProtoPredicateOp>(op);

    /// <summary>
    /// Converts a set predicate operation from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf predicate operation.</param>
    /// <returns>The internal predicate operation.</returns>
    public static PredicateOp FromProto(this ProtoPredicateOp proto) =>
        EnumUtils.Cast<ProtoPredicateOp, PredicateOp>(proto);

    /// <summary>
    /// Converts a set comparison operation to protobuf.
    /// </summary>
    /// <param name="op">The comparison operation.</param>
    /// <returns>The protobuf comparison operation.</returns>
    public static ProtoComparisonOp ToProto(this ComparisonOp op) =>
        EnumUtils.Cast<ComparisonOp, ProtoComparisonOp>(op);

    /// <summary>
    /// Converts a set comparison operation from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf comparison operation.</param>
    /// <returns>The internal comparison operation.</returns>
    public static ComparisonOp FromProto(this ProtoComparisonOp proto) =>
        EnumUtils.Cast<ProtoComparisonOp, ComparisonOp>(proto);

    /// <summary>
    /// Converts a set reduction operation to protobuf.
    /// </summary>
    /// <param name="op">The reduction operation.</param>
    /// <returns>The protobuf reduction operation.</returns>
    public static ProtoReductionOp ToProto(this ReductionOp op) =>
        EnumUtils.Cast<ReductionOp, ProtoReductionOp>(op);

    /// <summary>
    /// Converts a set reduction operation from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf reduction operation.</param>
    /// <returns>The internal reduction operation.</returns>
    public static ReductionOp FromProto(this ProtoReductionOp proto) =>
        EnumUtils.Cast<ProtoReductionOp, ReductionOp>(proto);
}
