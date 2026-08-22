// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a strategy for traversing a tree-like structure.
/// </summary>
/// <typeparam name="TNode">The type of the nodes in the structure.</typeparam>
public interface ITraversalStrategy<TNode>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Produces an iterator that follows the defined traversal strategy
    /// for the tree rooted at the specified node.
    /// </summary>
    /// <param name="root">The root node of the tree to traverse.</param>
    /// <returns>An iterator that yields nodes in the traversal order.</returns>
    public IEnumerable<TNode> Traverse(TNode root);
}
