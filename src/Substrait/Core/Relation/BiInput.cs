// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Relation;

/// <summary>
/// Relational operator with a two inputs.
/// </summary>
public abstract class BiInput : Rel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiInput"/> class.
    /// </summary>
    protected BiInput()
    {
    }

    /// <summary>
    /// Gets left input.
    /// </summary>
    public abstract IRel Left { get; }

    /// <summary>
    /// Gets right input.
    /// </summary>
    public abstract IRel Right { get; }

    /// <inheritdoc/>
    public override sealed IReadOnlyList<IRel> Inputs => ImmutableList.Create(this.Left, this.Right);
}
