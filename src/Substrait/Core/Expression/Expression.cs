// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Extension.Functions;
using Substrait.Core.Relation;
using Substrait.Core.Type;
using Substrait.Tools;
using Substrait.Tools.Visitor;
using ProtoComparisonOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetComparison.Types.ComparisonOp;
using ProtoFailureBehavior = Substrait.Protobuf.Expression.Types.Cast.Types.FailureBehavior;
using ProtoPredicateOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetPredicate.Types.PredicateOp;
using ProtoReductionOp = Substrait.Protobuf.Expression.Types.Subquery.Types.SetComparison.Types.ReductionOp;

namespace Substrait.Core.Expression;

/// <summary>
/// Base class for expressions, <see cref="Protobuf.Expression"/>.
/// </summary>
public abstract class Expression : IExpression, IEquatable<Expression>, INodeEquatable<IExpression>
{
    private Lazy<int> hashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Expression"/> class.
    /// </summary>
    /// <param name="type">Type of the expression.</param>
    protected Expression(IType type)
    {
        this.Type = type;
        this.hashCode = new Lazy<int>(() => { return new GetNodeHashCodeDispatcher().Dispatch(this, NoOpContext<IExpression, int>.DEFAULT); });
    }

    /// <inheritdoc/>
    public IType Type { get; }

    /// <inheritdoc/>
    public abstract IEnumerable<IExpression> InputNodes { get; }

    /// <inheritdoc/>
    public bool HasHashCode => this.hashCode.IsValueCreated;

    /// <inheritdoc/>
    public abstract TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context);

    /// <inheritdoc/>
    public bool Equals(Expression? other)
    {
        return this.NodeEqualsImpl<IExpression, NodeEqualsDispatcher>(other);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Expression);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.hashCode.Value;
    }

    /// <inheritdoc/>
    public abstract int GetNodeHashCode();

    /// <inheritdoc/>
    public abstract bool NodeEquals(IExpression other);

    /// <summary>
    /// Dispatch NodeEquals for equality comparison.
    /// </summary>
    public sealed class NodeEqualsDispatcher : ExpressionTopDownDispatcher<IEnumerator<IExpression>, bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeEqualsDispatcher"/> class.
        /// </summary>
        public NodeEqualsDispatcher()
            : base(NodeEqualsVisitor.DEFAULT)
        {
        }

        /// <inheritdoc/>
        protected override bool ShouldBailOut(bool result, IEnumerator<IExpression> context)
        {
            return !result;
        }

        /// <summary>
        /// NodeEquality visitor.
        /// </summary>
        private sealed class NodeEqualsVisitor : DefaultExpressionVisitor<IEnumerator<IExpression>, bool>
        {
            /// <summary>
            /// Default instance.
            /// </summary>
            internal static readonly NodeEqualsVisitor DEFAULT = new();

            /// <inheritdoc/>
            protected override bool DefaultVisit(IExpression expr, IEnumerator<IExpression> context)
            {
                return context.MoveNext()
                    && context.Current is not null
                    && expr.Type.Equals(context.Current.Type)
                    && expr switch
                    {
                        INodeEquatable<IExpression> e => e.NodeEquals(context.Current),
                        _ => expr.Equals(context.Current),
                    };
            }
        }
    }

    /// <summary>
    /// Dispatch NodeHashes for equality comparison.
    /// </summary>
    public sealed class GetNodeHashCodeDispatcher : ExpressionBottomUpDispatcher<NoOpContext<IExpression, int>, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetNodeHashCodeDispatcher"/> class.
        /// </summary>
        public GetNodeHashCodeDispatcher()
            : base(GetNodeHashCodeVisitor.DEFAULT, new GetNodeHashCodeTraversal())
        {
        }

        private sealed class GetNodeHashCodeTraversal : BottomUpTraversal<IExpression>
        {
            protected override bool ShouldVisit(IExpression node)
            {
                return node is INodeEquatable<IExpression> n && !n.HasHashCode;
            }
        }

        /// <summary>
        /// GetNodeHashCode visitor.
        /// </summary>
        private sealed class GetNodeHashCodeVisitor : DefaultExpressionVisitor<NoOpContext<IExpression, int>, int>
        {
            /// <summary>
            /// Default instance.
            /// </summary>
            internal static readonly GetNodeHashCodeVisitor DEFAULT = new();

            /// <inheritdoc/>
            protected override int DefaultVisit(IExpression expr, NoOpContext<IExpression, int> noContext)
            {
                return expr switch
                {
                    // The hash codes of input nodes have been generated (or ensured the hash code is already generated)
                    // by the bottom up traversal thus it won't trigger a deep recursive call.
                    INodeEquatable<IExpression> e => HashCode.Combine(e.GetNodeHashCode(), expr.Type, expr.InputNodes.CombineHashCodes()),
                    _ => expr.GetHashCode(),
                };
            }
        }
    }

    /// <summary>
    /// Cast expression.
    /// </summary>
    public sealed class Cast : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cast"/> class.
        /// </summary>
        /// <param name="type">Return type of the cast.</param>
        /// <param name="input">Input of the cast.</param>
        /// <param name="failureBehavior">Behavior on failure.</param>
        public Cast(IType type, IExpression input, FailureBehavior failureBehavior)
            : base(type)
        {
            this.Input = input;
            this.Behavior = failureBehavior;
        }

        /// <summary>
        /// Failure behavior.
        /// </summary>
        public enum FailureBehavior
        {
            /// <summary>
            /// Failure behavior unspecified.
            /// </summary>
            Unspecified = ProtoFailureBehavior.Unspecified,

            /// <summary>
            /// Failure behavior ReturnNull.
            /// </summary>
            ReturnNull = ProtoFailureBehavior.ReturnNull,

            /// <summary>
            /// Failure behavior ThrowException.
            /// </summary>
            ThrowException = ProtoFailureBehavior.ThrowException,
        }

        /// <summary>
        /// Gets case clauses property.
        /// </summary>
        public IExpression Input { get; }

        /// <summary>
        /// Gets failure behavior property.
        /// </summary>
        public FailureBehavior Behavior { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes
        {
            get { yield return this.Input; }
        }

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(nameof(Cast), this.Behavior);
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is Cast o && this.Behavior == o.Behavior;
        }
    }

    /// <summary>
    /// Scalar function invocation.
    /// </summary>
    public sealed class ScalarFunctionInvocation : Expression, IFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarFunctionInvocation"/> class.
        /// </summary>
        /// <param name="namespaceStr">Function namespace.</param>
        /// <param name="key">Function key.</param>
        /// <param name="args">Arguments of the function.</param>
        /// <param name="type">Return type of the function.</param>
        /// <param name="declaration">Declaration of the function.</param>
        public ScalarFunctionInvocation(string namespaceStr, string key, IEnumerable<IFunctionArg> args, IType type, ScalarFunctionImpl? declaration)
            : base(type)
        {
            this.Namespace = namespaceStr;
            this.Key = key;
            this.Name = GetName(this.Key);
            this.Arguments = args.ToImmutableList();
            this.Declaration = declaration;
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
        /// Gets declaration property.
        /// </summary>
        public ScalarFunctionImpl? Declaration { get; }

        /// <summary>
        /// Gets arguments property.
        /// </summary>
        public IReadOnlyList<IFunctionArg> Arguments { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => this.Arguments.OfType<IExpression>();

        /// <inheritdoc/>
        public IType OutputType => this.Type;

        /// <inheritdoc/>
        public IFunction.FunctionKind Kind => IFunction.FunctionKind.SCALAR;

        /// <inheritdoc/>
        public AggregateFunctionInvocation.AggregationInvocation? AggregationInvocation => null;

        /// <inheritdoc/>
        public AggregateFunctionInvocation.AggregationPhase? AggregationPhase => null;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(
                nameof(ScalarFunctionInvocation),
                this.Namespace,
                this.Key,
                this.Type,
                this.Declaration,
                this.Arguments.Where(x => x is not IExpression).CombineHashCodes());
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is ScalarFunctionInvocation o
                && this.Namespace.Equals(o.Namespace)
                && this.Key.Equals(o.Key)
                && (this.Declaration?.Equals(o.Declaration) ?? o.Declaration is null)
                && Enumerable.SequenceEqual(this.Arguments.Where(x => x is not IExpression), o.Arguments.Where(x => x is not IExpression));
        }

        private static string GetName(string key)
        {
            int index = key.IndexOf(':');
            int length = index >= 0 ? index : key.Length;
            return key.AsSpan().Slice(0, length).ToString();
        }
    }

    /// <summary>
    /// If-then expression.
    /// </summary>
    public sealed class IfThen : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IfThen"/> class.
        /// </summary>
        /// <param name="ifClauses">If-then clauses.</param>
        /// <param name="elseClause">Else clause.</param>
        public IfThen(IEnumerable<(IExpression Condition, IExpression Then)> ifClauses, IExpression elseClause)
            : base(DeduceType(ifClauses, elseClause))
        {
            this.IfClauses = ifClauses.Select(x => new IfClause(x.Condition, x.Then)).ToImmutableList();
            this.ElseClause = elseClause;
        }

        /// <summary>
        /// Gets if clauses property.
        /// </summary>
        public IReadOnlyList<IfClause> IfClauses { get; }

        /// <summary>
        /// Gets else clause property.
        /// </summary>
        public IExpression ElseClause { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => this.IfClauses.SelectMany(c => new[] { c.Condition, c.Then }).Append(this.ElseClause);

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(nameof(IfThen));
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is IfThen;
        }

        private static IType DeduceType(IEnumerable<(IExpression Condition, IExpression Then)> ifClauses, IExpression elseClause)
        {
            // If-then expression is nullable if any of the then clauses are nullable or the else clause is nullable.
            // Otherwise, it is not nullable.
            IType type = elseClause.Type;

            if (ifClauses.Any(c => c.Then.Type.Nullable == IType.NullableType.Nullable))
            {
                return TypeFactory.NULLABLE.ResolveTypeWithNullability(type);
            }

            return type;
        }

        /// <summary>
        /// If clause.
        /// </summary>
        /// <param name="condition">condition of the If clause.</param>
        /// <param name="then">then of the If clause.</param>
        public struct IfClause(IExpression condition, IExpression then)
        {
            /// <summary>
            /// Gets conditional expression of the If clause.
            /// </summary>
            public readonly IExpression Condition => condition;

            /// <summary>
            /// Gets then expression of the If clause.
            /// </summary>
            public readonly IExpression Then => then;

            /// <summary>
            /// Deconstructor.
            /// </summary>
            /// <param name="condition">condition part of the If clause.</param>
            /// <param name="then">then part of the If clause.</param>
            public readonly void Deconstruct(out IExpression condition, out IExpression then)
            {
                condition = this.Condition;
                then = this.Then;
            }
        }
    }

    /// <summary>
    /// Scalar subquery.
    /// </summary>
    public sealed class ScalarSubquery : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarSubquery"/> class.
        /// </summary>
        /// <param name="subquery">Relation for the subquery expression.</param>
        /// <param name="type">Return type of the expression.</param>
        public ScalarSubquery(IRel subquery, IType type)
            : base(type)
        {
            this.Subquery = subquery;
        }

        /// <summary>
        /// Gets subquery relation property.
        /// </summary>
        public IRel Subquery { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => ImmutableList<IExpression>.Empty;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(nameof(ScalarSubquery), this.Subquery);
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is ScalarSubquery o && this.Subquery.Equals(o.Subquery);
        }
    }

    /// <summary>
    /// In predicate subquery.
    /// </summary>
    public sealed class InPredicateSubquery : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InPredicateSubquery"/> class.
        /// </summary>
        /// <param name="subquery">Relation for the subquery expression.</param>
        /// <param name="values">Values to compare with in the subquery result.</param>
        public InPredicateSubquery(IRel subquery, IEnumerable<IExpression> values)
            : base(PrimitiveTypeFactory.REQUIRED.BOOL)
        {
            this.Subquery = subquery;
            this.Values = values.ToImmutableList();
        }

        /// <summary>
        /// Gets subquery relation property.
        /// </summary>
        public IRel Subquery { get; }

        /// <summary>
        /// Gets the values to compare with in the subquery result.
        /// </summary>
        public IReadOnlyList<IExpression> Values { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => this.Values;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(nameof(InPredicateSubquery), this.Subquery);
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is InPredicateSubquery o && this.Subquery.Equals(o.Subquery);
        }
    }

    /// <summary>
    /// Set predicate subquery.
    /// </summary>
    public sealed class SetPredicateSubquery : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetPredicateSubquery"/> class.
        /// </summary>
        /// <param name="subquery">Relation for the subquery expression.</param>
        /// <param name="operation">Operation to perform over the set.</param>
        public SetPredicateSubquery(IRel subquery, PredicateOp operation)
            : base(PrimitiveTypeFactory.REQUIRED.BOOL)
        {
            this.Subquery = subquery;
            this.Operation = operation;
        }

        /// <summary>
        /// Predicate operation.
        /// </summary>
        public enum PredicateOp
        {
            /// <summary>
            /// Predicate operation unspecified.
            /// </summary>
            Unspecified = ProtoPredicateOp.Unspecified,

            /// <summary>
            /// Predicate operation Exists.
            /// </summary>
            Exists = ProtoPredicateOp.Exists,

            /// <summary>
            /// Predicate operation Unique.
            /// </summary>
            Unique = ProtoPredicateOp.Unique,
        }

        /// <summary>
        /// Gets subquery relation property.
        /// </summary>
        public IRel Subquery { get; }

        /// <summary>
        /// Gets the operation to perform over the set.
        /// </summary>
        public PredicateOp Operation { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => ImmutableList<IExpression>.Empty;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(nameof(SetPredicateSubquery), this.Operation, this.Subquery);
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is SetPredicateSubquery o && this.Operation == o.Operation && this.Subquery.Equals(o.Subquery);
        }
    }

    /// <summary>
    /// Set comparison subquery.
    /// </summary>
    public sealed class SetComparisonSubquery : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetComparisonSubquery"/> class.
        /// </summary>
        /// <param name="expr">Expression to compare against subquery.</param>
        /// <param name="comparison">Comparison operation.</param>
        /// <param name="reduction">Reduction operation.</param>
        /// <param name="subquery">Relation for the subquery expression.</param>
        public SetComparisonSubquery(IExpression expr, ComparisonOp comparison, ReductionOp reduction, IRel subquery)
            : base(PrimitiveTypeFactory.REQUIRED.BOOL)
        {
            this.Expression = expr;
            this.Comparison = comparison;
            this.Reduction = reduction;
            this.Subquery = subquery;
        }

        /// <summary>
        /// Comparison operation.
        /// </summary>
        public enum ComparisonOp
        {
            /// <summary>
            /// Comparison operation unspecified.
            /// </summary>
            Unspecified = ProtoComparisonOp.Unspecified,

            /// <summary>
            /// Equal comparison.
            /// </summary>
            Equal = ProtoComparisonOp.Eq,

            /// <summary>
            /// Not equal comparison.
            /// </summary>
            NotEqual = ProtoComparisonOp.Ne,

            /// <summary>
            /// Less than comparison.
            /// </summary>
            LessThan = ProtoComparisonOp.Lt,

            /// <summary>
            /// Greater than comparison.
            /// </summary>
            GreaterThan = ProtoComparisonOp.Gt,

            /// <summary>
            /// Less than equal comparison.
            /// </summary>
            LessThanEqual = ProtoComparisonOp.Le,

            /// <summary>
            /// Greater than equal comparison.
            /// </summary>
            GreaterThanEqual = ProtoComparisonOp.Ge,
        }

        /// <summary>
        /// Reduction operation.
        /// </summary>
        public enum ReductionOp
        {
            /// <summary>
            /// Reduction operation unspecified.
            /// </summary>
            Unspecified = ProtoReductionOp.Unspecified,

            /// <summary>
            /// ANY reduction.
            /// </summary>
            Any = ProtoReductionOp.Any,

            /// <summary>
            /// ALL reduction.
            /// </summary>
            All = ProtoReductionOp.All,
        }

        /// <summary>
        /// Gets expression to compare.
        /// </summary>
        public IExpression Expression { get; }

        /// <summary>
        /// Gets the comparison to perform over the set.
        /// </summary>
        public ComparisonOp Comparison { get; }

        /// <summary>
        /// Gets the reduction to perform over the set.
        /// </summary>
        public ReductionOp Reduction { get; }

        /// <summary>
        /// Gets subquery relation property.
        /// </summary>
        public IRel Subquery { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes
        {
            get { yield return this.Expression; }
        }

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return HashCode.Combine(
                nameof(SetComparisonSubquery),
                this.Comparison,
                this.Reduction,
                this.Subquery);
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is SetComparisonSubquery o
                && this.Comparison == o.Comparison
                && this.Reduction == o.Reduction
                && this.Subquery.Equals(o.Subquery);
        }
    }

    /// <summary>
    /// Struct expression.
    /// </summary>
    public sealed class Struct : Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Struct"/> class.
        /// </summary>
        /// <param name="fields">Fields in the struct.</param>
        public Struct(IEnumerable<IExpression> fields)
          : this(fields, IType.NullableType.Required)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Struct"/> class.
        /// </summary>
        /// <param name="fields">Fields in the struct.</param>
        /// <param name="nullable">Whether the struct expression is nullable.</param>
        public Struct(IEnumerable<IExpression> fields, IType.NullableType nullable)
            : base(TypeFactory.Of(nullable).Struct(fields.Select(_ => _.Type).ToImmutableList()))
        {
            this.Fields = fields.ToImmutableList();
        }

        /// <summary>
        /// Gets struct fields.
        /// </summary>
        public IReadOnlyList<IExpression> Fields { get; }

        /// <inheritdoc/>
        public override IEnumerable<IExpression> InputNodes => this.Fields;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        public override int GetNodeHashCode()
        {
            return nameof(Struct).GetHashCode();
        }

        /// <inheritdoc/>
        public override bool NodeEquals(IExpression other)
        {
            return other is Struct;
        }
    }
}
