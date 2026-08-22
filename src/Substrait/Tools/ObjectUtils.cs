// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics.CodeAnalysis;
using Substrait.Tools.Visitor;

namespace Substrait.Tools;

/// <summary>
/// Various utiltiy functions for C# objects.
/// </summary>
public static class ObjectUtils
{
    /// <summary>
    /// Combines hashcode of values.
    /// </summary>
    /// <typeparam name="T">element type.</typeparam>
    /// <param name="values">values to calculate hash.</param>
    /// <param name="seed">optional seed to combine.</param>
    /// <returns>combined hash code of values. returns seed as-is for empty values.</returns>
    public static int CombineHashCodes<T>(this IEnumerable<T> values, int seed = 0)
    {
        foreach (var value in values)
        {
            seed = HashCode.Combine(seed, value);
        }

        return seed;
    }

    /// <summary>
    /// Compare equality of <paramref name="value"/> and <paramref name="other"/> with accounting nulls.
    /// </summary>
    /// <typeparam name="T">type of the values.</typeparam>
    /// <param name="value">value.</param>
    /// <param name="other">other.</param>
    /// <returns>true when value and other are both nulls otherwise Equals return true.</returns>
    public static bool EqualsWithNull<T>(this T? value, T? other)
    {
        return (value is null && other is null) || (value is not null && other is not null && EqualityComparer<T>.Default.Equals(value, other));
    }

    /// <summary>
    /// Generic implementation of node equals.
    /// </summary>
    /// <typeparam name="TNode">node type.</typeparam>
    /// <typeparam name="TDispatcher">dispatcher with default constructor.</typeparam>
    /// <param name="node">comparing node or this.</param>
    /// <param name="other">compared node.</param>
    /// <returns>true if <paramref name="node"/> and <paramref name="other"/> are equivalent.</returns>
    public static bool NodeEqualsImpl<TNode, TDispatcher>(this TNode node, TNode? other)
        where TNode : INavigableNode<TNode>
        where TDispatcher : IDispatcher<TNode, IEnumerator<TNode>, bool>, new()
    {
        return NodeEqualsImpl(node, other, new TDispatcher());
    }

    /// <summary>
    /// Generic implementation of node equals.
    /// </summary>
    /// <typeparam name="TNode">node type.</typeparam>
    /// <typeparam name="TDispatcher">dispatcher.</typeparam>
    /// <param name="node">comparing node or this.</param>
    /// <param name="other">compared node.</param>
    /// <param name="dispatcher">dispatcher to use.</param>
    /// <returns>true if <paramref name="node"/> and <paramref name="other"/> are equivalent.</returns>
    public static bool NodeEqualsImpl<TNode, TDispatcher>(this TNode node, TNode? other, TDispatcher dispatcher)
        where TNode : INavigableNode<TNode>
        where TDispatcher : IDispatcher<TNode, IEnumerator<TNode>, bool>, new()
    {
        if (ReferenceEquals(node, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        using var otherNodes = new TopDownTraversal<TNode>().Traverse(other).GetEnumerator();
        var result = dispatcher.Dispatch(node, otherNodes) && !otherNodes.MoveNext();
        return result;
    }

    /// <summary>
    /// Equality comparison for tuple type.
    /// </summary>
    /// <typeparam name="TKey">the first element type. considered as key.</typeparam>
    /// <typeparam name="TValue">the value.</typeparam>
    public class FirstValueComparer<TKey, TValue> : IEqualityComparer<(TKey, TValue)>
        where TKey : notnull, IEquatable<TKey>
    {
        /// <inheritdoc/>
        public bool Equals((TKey, TValue) x, (TKey, TValue) y)
        {
            return x.Item1.Equals(y.Item1);
        }

        /// <inheritdoc/>
#if NET5_0_OR_GREATER
        public int GetHashCode([DisallowNull] (TKey, TValue) obj)
#else
        public int GetHashCode((TKey, TValue) obj)
#endif
        {
            return obj.Item1.GetHashCode();
        }
    }
}
