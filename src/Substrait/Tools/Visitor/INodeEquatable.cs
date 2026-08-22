// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Node level equalities (i.e., do not consider equalities of subtree).
/// </summary>
/// <typeparam name="TNode">Node type.</typeparam>
public interface INodeEquatable<TNode>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Gets a value indicating whether hash code has been calculated.
    /// </summary>
    public bool HasHashCode { get; }

    /// <summary>
    /// Checks equality of node local information between the two nodes.
    /// </summary>
    /// <param name="other">node.</param>
    /// <returns>true if this node has the equivalent node local information as <paramref name="other"/>.</returns>
    public bool NodeEquals(TNode other);

    /// <summary>
    /// Gets the hash code of node local information.
    /// </summary>
    /// <returns>hash code of node local information.</returns>
    public int GetNodeHashCode();
}
