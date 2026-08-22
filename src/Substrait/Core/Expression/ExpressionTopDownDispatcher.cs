// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Tools.Visitor;

namespace Substrait.Core.Expression;

/// <summary>
/// Dispatcher for traversing expression trees following a top-down traversal strategy.
/// </summary>
/// <typeparam name="TContext">The type of the context used during traversal.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the traversal.</typeparam>
public class ExpressionTopDownDispatcher<TContext, TOutput> : IDispatcher<IExpression, TContext, TOutput>
{
    private readonly ExpressionVisitor<TContext, TOutput> visitor;

    private readonly ITraversalStrategy<IExpression> traversalStrategy;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTopDownDispatcher{TContext, TOutput}"/> class.
    /// </summary>
    /// <param name="visitor">The visitor used to process nodes during traversal.</param>
    public ExpressionTopDownDispatcher(ExpressionVisitor<TContext, TOutput> visitor)
    {
        this.visitor = visitor;
        this.traversalStrategy = new TopDownTraversal<IExpression>();
    }

    /// <summary>
    /// Traverses the expression tree starting from the given root node.
    /// </summary>
    /// <param name="root">The root node of the tree.</param>
    /// <param name="context">The initial context for the traversal.</param>
    /// <returns>The output produced by the traversal.</returns>
    public TOutput Dispatch(IExpression root, TContext context)
    {
        TOutput result = default!;

        foreach (var node in this.traversalStrategy.Traverse(root))
        {
            result = node.Accept(this.visitor, context);
            if (this.ShouldBailOut(result, context))
            {
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether the traversal should bail out early.
    /// </summary>
    /// <param name="result">The result of the current visit.</param>
    /// <param name="context">The current context.</param>
    /// <returns>True if the traversal should bail out, false otherwise.</returns>
    protected virtual bool ShouldBailOut(TOutput result, TContext context)
    {
        // Default implementation: do not bail out
        return false;
    }
}
