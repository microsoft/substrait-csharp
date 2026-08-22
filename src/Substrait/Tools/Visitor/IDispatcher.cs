// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Dispatcher for traversing node trees.
/// </summary>
/// <typeparam name="TNode">The type of the nodes in the structure.</typeparam>
/// <typeparam name="TContext">The type of the context used during traversal.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the traversal.</typeparam>
public interface IDispatcher<TNode, TContext, TOutput>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Traverses the node tree starting from the given root node.
    /// </summary>
    /// <param name="root">The root node of the tree.</param>
    /// <param name="initialContext">The initial context for the traversal.</param>
    /// <returns>The output produced by the traversal.</returns>
    public TOutput Dispatch(TNode root, TContext initialContext);
}
