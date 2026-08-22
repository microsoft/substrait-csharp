// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Option for a function.
/// </summary>
public interface IOption
{
    /// <summary>
    /// Gets description of the option.
    /// </summary>
    /// <returns>The description of the option.</returns>
    string? Description { get; }

    /// <summary>
    /// Gets values of the option.
    /// </summary>
    /// <returns>The values of the option.</returns>
    IReadOnlyList<string> Values { get; }
}
