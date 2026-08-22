// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a visitor for processing nodes in a tree-like structure.
/// </summary>
/// <typeparam name="TNode">The type of the nodes to be visited.</typeparam>
/// <typeparam name="TContext">The type of the context used during visitation.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the visitation.</typeparam>
public interface IVisitor<TNode, TContext, TOutput>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Processes a generic node, serving as a catch-all method for node types
    /// that do not have a specialized visitor method.
    /// </summary>
    /// <param name="any">The node to be processed.</param>
    /// <param name="context">The initial context for the visitation.</param>
    /// <returns>The output produced after processing the node.</returns>
    TOutput Visit(TNode any, TContext context);
}
