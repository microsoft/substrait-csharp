// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Defines a top-down traversal strategy for tree-like structures.
/// </summary>
/// <typeparam name="TNode">The type of the nodes in the structure.</typeparam>
public class TopDownTraversal<TNode> : ITraversalStrategy<TNode>
    where TNode : INavigableNode<TNode>
{
    /// <summary>
    /// Traverses the tree using a top-down (pre-order) strategy.
    /// </summary>
    /// <param name="root">The root node of the tree.</param>
    /// <returns>An iterator producing nodes in top-down order.</returns>
    public IEnumerable<TNode> Traverse(TNode root)
    {
        var stack = new Stack<IEnumerator<TNode>>(128);
        stack.Push(Enumerable.Repeat(root, 1).GetEnumerator());

        while (stack.Count > 0)
        {
            var enumerator = stack.Peek();

            if (enumerator.MoveNext())
            {
                var currentNode = enumerator.Current;
                if (this.ShouldVisit(currentNode))
                {
                    yield return currentNode;

                    // Push inputs enumerator onto the stack to maintain natural order
                    stack.Push(currentNode.InputNodes.GetEnumerator());
                }
            }
            else
            {
                // No more nodes in this enumerator, clean up and pop
                enumerator.Dispose();
                stack.Pop();
            }
        }
    }

    /// <summary>
    /// Tests whether we should traverse the <paramref name="node"/> and its descendants.
    /// </summary>
    /// <param name="node">a node to be tested.</param>
    /// <returns>true if we should visit the node and its subtree. Otherwise, false.</returns>
    /// <remarks>on skipping, the node is being skipped thus pre-order traversal may not see all the nodes.
    /// It is consumers responsibility to handle such skipped traversal.</remarks>
    protected virtual bool ShouldVisit(TNode node)
    {
        return true;
    }
}
