// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Tools.Visitor;

/// <summary>
/// Auxiliary class that can be used when no output as a result of a visitation is needed.
/// </summary>
public sealed class VoidOutput
{
    /// <summary>
    /// The unique instance of the VoidOutput class.
    /// </summary>
    public static readonly VoidOutput Instance = new();

    private VoidOutput()
    {
    }
}
