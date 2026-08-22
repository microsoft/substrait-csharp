// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a context used during the traversal of a node tree.
/// </summary>
/// <typeparam name="TNode">The type of the nodes to be visited.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the visitation.</typeparam>
public interface IContext<TNode, TOutput>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Retrieves the output associated with the given node.
    /// </summary>
    /// <param name="node">The node for which to retrieve the output.</param>
    /// <returns>The output associated with the node.</returns>
    public TOutput GetOutput(TNode node);

    /// <summary>
    /// Adds an output produced by the visitor.
    /// </summary>
    /// <param name="node">The input node that resulted in the output.</param>
    /// <param name="output">The output produced by the node.</param>
    public void AddOutput(TNode node, TOutput output);

    /// <summary>
    /// Removes the output produced by the visitor.
    /// </summary>
    /// <param name="node">The input node that resulted in the output.</param>
    public void RemoveOutput(TNode node);
}
