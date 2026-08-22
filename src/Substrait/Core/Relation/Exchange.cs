// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Expression;
using Substrait.Core.Type;

namespace Substrait.Core.Relation;

/// <summary>
/// The EXCHANGE relational operator representing data exchange, <see cref="Protobuf.ExchangeRel"/>.
/// </summary>
public abstract class Exchange : SingleInput
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Exchange"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="partitionCount">Number of partitions targeted for output.</param>
    public Exchange(IRel input, int partitionCount)
    {
        this.Input = input;
        this.PartitionCount = partitionCount;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Exchange"/> class.
    /// </summary>
    /// <param name="input">Input relation.</param>
    /// <param name="partitionCount">Number of partitions targeted for output.</param>
    /// <param name="transmute">Remap to apply on the output.</param>
    public Exchange(IRel input, int partitionCount, Remap? transmute)
      : this(input, partitionCount)
    {
        this.Transmute = transmute;
    }

    /// <inheritdoc/>
    public override IRel Input { get; }

    /// <summary>
    /// Gets the number of partitions targeted for output.
    /// </summary>
    public int PartitionCount { get; }

    /// <inheritdoc/>
    public override Remap? Transmute { get; }

    /// <inheritdoc/>
    protected override ParameterizedType.Struct DeriveRecordType() => this.Input.RecordType;
}
