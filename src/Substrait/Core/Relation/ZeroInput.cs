// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;

namespace Substrait.Core.Relation;

/// <summary>
/// Relational operator with no inputs.
/// </summary>
public abstract class ZeroInput : Rel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ZeroInput"/> class.
    /// </summary>
    protected ZeroInput()
    {
    }

    /// <inheritdoc/>
    public override sealed IReadOnlyList<IRel> Inputs { get; } = ImmutableList<IRel>.Empty;
}
