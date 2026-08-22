// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Argument for a function.
/// </summary>
public interface IArgument
{
    /// <summary>
    /// Gets name of the argument.
    /// </summary>
    /// <returns>The name of the argument.</returns>
    string Name { get; }

    /// <summary>
    /// Gets description of the argument.
    /// </summary>
    /// <returns>The description of the argument.</returns>
    string Description { get; }

    /// <summary>
    /// Gets a value indicating whether whether the argument is required.
    /// </summary>
    /// <returns>Whether the argument is required.</returns>
    bool Required { get; }

    /// <summary>
    /// Gets the string representation of the argument type.
    /// </summary>
    /// <returns>The string representation of the argument type.</returns>
    string ToTypeString();
}
