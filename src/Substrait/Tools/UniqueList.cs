// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections;

namespace Substrait.Tools;

/// <summary>
/// Unique append-only list that preserves insertion order.
/// </summary>
/// <typeparam name="T">The element type.</typeparam>
public class UniqueList<T>(IEqualityComparer<T> comparer) : IReadOnlyList<T>, IList<T>
    where T : notnull
{
    private readonly Dictionary<T, int> valueToIndex = new(comparer);
    private readonly List<T> values = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="UniqueList{T}"/> class.
    /// </summary>
    public UniqueList()
        : this(EqualityComparer<T>.Default)
    {
    }

    /// <inheritdoc/>
    public int Count => this.values.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    T IList<T>.this[int index]
    {
        get => ((IList<T>)this.values)[index];
        set => throw new NotSupportedException("Does not support update.");
    }

    /// <inheritdoc/>
    public T this[int index] => ((IReadOnlyList<T>)this.values)[index];

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => this.values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.values.GetEnumerator();

    /// <inheritdoc/>
    public int IndexOf(T item) => this.valueToIndex.TryGetValue(item, out int index) ? index : -1;

    /// <inheritdoc/>
    public void Insert(int index, T item) => throw new NotSupportedException("Use Add() instead.");

    /// <inheritdoc/>
    public void RemoveAt(int index) => throw new NotSupportedException("Does not support remove.");

    /// <summary>
    /// Adds an item if it is not already present.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns><see langword="true"/> when the item was added.</returns>
    public bool TryAdd(T item)
    {
#if NET5_0_OR_GREATER
        if (!this.valueToIndex.TryAdd(item, this.values.Count))
        {
            return false;
        }
#else
        if (this.valueToIndex.ContainsKey(item))
        {
            return false;
        }

        this.valueToIndex.Add(item, this.values.Count);
#endif
        this.values.Add(item);
        return true;
    }

    /// <inheritdoc/>
    public void Add(T item) => this.TryAdd(item);

    /// <summary>
    /// Adds an item if needed and returns its index.
    /// </summary>
    /// <param name="item">The item to locate or add.</param>
    /// <returns>The item's index.</returns>
    public int TryAddAndIndexOf(T item)
    {
        if (this.valueToIndex.TryGetValue(item, out int index))
        {
            return index;
        }

        index = this.values.Count;
        this.valueToIndex.Add(item, index);
        this.values.Add(item);
        return index;
    }

    /// <summary>
    /// Adds each value that is not already present.
    /// </summary>
    /// <param name="items">The values to add.</param>
    public void AddRange(IEnumerable<T> items) => this.TryAddRange(items);

    /// <summary>
    /// Adds each value that is not already present.
    /// </summary>
    /// <param name="items">The values to add.</param>
    /// <returns>The number of added values.</returns>
    public int TryAddRange(IEnumerable<T> items)
    {
        int added = 0;
        foreach (T item in items)
        {
            if (this.TryAdd(item))
            {
                added++;
            }
        }

        return added;
    }

    /// <inheritdoc/>
    public void Clear()
    {
        this.values.Clear();
        this.valueToIndex.Clear();
    }

    /// <inheritdoc/>
    public bool Contains(T item) => this.valueToIndex.ContainsKey(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => ((ICollection<T>)this.values).CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public bool Remove(T item) => throw new NotSupportedException("Does not support remove.");
}
