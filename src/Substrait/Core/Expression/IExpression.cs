// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Type;
using Substrait.Tools.Visitor;

namespace Substrait.Core.Expression;

/// <summary>
/// Base type for all relational expressions, <see cref="Protobuf.Expression"/>.
/// </summary>
public interface IExpression : IFunctionArg, INavigableNode<IExpression>
{
    /// <summary>
    /// Gets the type of the expression.
    /// </summary>
    public IType Type { get; }

    /// <summary>
    /// Accepts the node visitor.
    /// </summary>
    /// <typeparam name="TContext">Context for the visitor implementation.</typeparam>
    /// <typeparam name="TOutput">Output for the visitor implementation.</typeparam>
    /// <param name="visitor">Node visitor.</param>
    /// <param name="context">Context object.</param>
    /// <returns>Returns the output object.</returns>
    public TOutput Accept<TContext, TOutput>(ExpressionVisitor<TContext, TOutput> visitor, TContext context);
}
