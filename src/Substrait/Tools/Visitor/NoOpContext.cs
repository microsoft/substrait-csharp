// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a base implementation of a context that does not keep track of any outputs.
/// </summary>
/// <typeparam name="TNode">The type of the nodes to be visited.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the visitation.</typeparam>
public class NoOpContext<TNode, TOutput> : IContext<TNode, TOutput>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Default instance.
    /// </summary>
    public static readonly NoOpContext<TNode, TOutput> DEFAULT = new();

    /// <inheritdoc/>
    public TOutput GetOutput(TNode node)
    {
        return default!;
    }

    /// <inheritdoc/>
    public void AddOutput(TNode node, TOutput output)
    {
        // Do nothing.
    }

    /// <inheritdoc/>
    public void RemoveOutput(TNode node)
    {
        // Do nothing.
    }
}
