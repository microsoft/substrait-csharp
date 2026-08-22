// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a base implementation of a context used during the traversal of a node tree.
/// </summary>
/// <typeparam name="TNode">The type of the nodes to be visited.</typeparam>
/// <typeparam name="TOutput">The type of the output produced by the visitation.</typeparam>
public class Context<TNode, TOutput> : IContext<TNode, TOutput>
    where TNode : class, INavigableNode<TNode>
{
    private readonly Dictionary<TNode, (TOutput Output, int RefCount)> outputs = new(ReferenceEqualityComparer.Instance);

    /// <inheritdoc/>
    public TOutput GetOutput(TNode node)
    {
        return this.outputs.TryGetValue(node, out var data)
            ? data.Output
            : throw new KeyNotFoundException($"Output for node {node} not found in context.");
    }

    /// <inheritdoc/>
    public void AddOutput(TNode node, TOutput output)
    {
        this.outputs[node] = this.outputs.TryGetValue(node, out var data)
            ? (output, ++data.RefCount)
            : (output, 1);
    }

    /// <inheritdoc/>
    public void RemoveOutput(TNode node)
    {
        if (this.outputs.TryGetValue(node, out var data) && --data.RefCount == 0)
        {
            this.outputs.Remove(node);
        }
        else if (data.RefCount > 0)
        {
            this.outputs[node] = (data.Output, data.RefCount);
        }
    }

#if !NET5_0_OR_GREATER
    private sealed class ReferenceEqualityComparer : IEqualityComparer<object?>, System.Collections.IEqualityComparer
    {
        public static ReferenceEqualityComparer Instance { get; } = new ReferenceEqualityComparer();

        public new bool Equals(object? x, object? y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(object? obj)
        {
            return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj!);
        }
    }
#endif
}
