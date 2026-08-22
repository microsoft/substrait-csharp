// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Expression;
using Substrait.Core.Type;

namespace Substrait.Core.Relation;

/// <summary>
/// The READ relational operator representing data scan, <see cref="Protobuf.ReadRel"/>.
/// </summary>
public abstract class Read : ZeroInput
{
    /// <summary>
    /// Gets initial schema.
    /// </summary>
    public abstract NamedStruct InitialSchema { get; }

    /// <summary>
    /// Gets filter condition.
    /// </summary>
    public abstract IExpression? Filter { get; }

    /// <inheritdoc/>
    protected override sealed ParameterizedType.Struct DeriveRecordType() => this.InitialSchema.Struct;
}
