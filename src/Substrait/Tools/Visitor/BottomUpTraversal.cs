// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a bottom-up traversal strategy for tree-like structures.
/// </summary>
/// <typeparam name="TNode">The type of the nodes in the structure.</typeparam>
public class BottomUpTraversal<TNode> : ITraversalStrategy<TNode>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Traverses the tree using a bottom-up (post-order) strategy.
    /// </summary>
    /// <param name="root">The root node of the tree.</param>
    /// <returns>An iterator producing nodes in bottom-up order.</returns>
    public IEnumerable<TNode> Traverse(TNode root)
    {
        // If we don't need to visit root, we are done.
        if (!this.ShouldVisit(root))
        {
            yield break;
        }

        var stack = new Stack<(TNode Node, IEnumerator<TNode> Inputs)>(128);
        stack.Push((root, root.InputNodes.GetEnumerator()));

        while (stack.Count > 0)
        {
            var (currentNode, enumerator) = stack.Peek();

            // Traverse inputs first
            if (enumerator.MoveNext())
            {
                var input = enumerator.Current;

                // If we do not have to visit the subject child, we are done.
                if (this.ShouldVisit(input))
                {
                    stack.Push((input, input.InputNodes.GetEnumerator()));
                }
            }
            else
            {
                // No more inputs to process, yield the current node
                stack.Pop(); // Remove from stack
                yield return currentNode;

                // Dispose the enumerator to free resources
                enumerator.Dispose();
            }
        }
    }

    /// <summary>
    /// Tests whether we should traverse the <paramref name="node"/> and its descendants.
    /// </summary>
    /// <param name="node">a node to be tested.</param>
    /// <returns>true if we should visit the node and its subtree. Otherwise, false.</returns>
    /// <remarks>on skipping, the node is being skipped thus post-order traversal may not see all the nodes.
    /// It is consumers responsibility to handle such skipped traversal.</remarks>
    protected virtual bool ShouldVisit(TNode node)
    {
        return true;
    }
}
