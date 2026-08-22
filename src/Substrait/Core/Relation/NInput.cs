// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Relation;

/// <summary>
/// Abstract class for N-ary relational operation where N > 2.
/// </summary>
public abstract class NInput : Rel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NInput"/> class.
    /// </summary>
    protected NInput()
    {
    }
}
