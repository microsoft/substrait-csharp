// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Function variadic behavior.
/// </summary>
public interface IVariadicBehavior
{
    /// <summary>
    /// Enum representing parameter consistency.
    /// </summary>
    public enum ParameterConsistency
    {
        /// <summary>
        /// Consistent.
        /// </summary>
        Consistent,

        /// <summary>
        /// Inconsistent.
        /// </summary>
        Inconsistent,
    }

    /// <summary>
    /// Gets minimum number of arguments.
    /// </summary>
    int Min { get; }

    /// <summary>
    /// Gets maximum number of arguments.
    /// </summary>
    int? Max { get; }

    /// <summary>
    /// Gets the parameter consistency in the variadic behavior.
    /// </summary>
    /// <returns>The parameter consistency.</returns>
    abstract ParameterConsistency BParameterConsistency { get; }
}
