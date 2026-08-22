// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Relation;

namespace Substrait.Core.Plan;

/// <summary>
/// Interface for plan.
/// </summary>
public interface IPlan
{
    /// <summary>
    /// Interface for plan root.
    /// </summary>
    public interface IRoot
    {
        /// <summary>
        /// Gets the input relation.
        /// </summary>
        public IRel Input { get; }

        /// <summary>
        /// Gets the names.
        /// </summary>
        public IReadOnlyList<string> Names { get; }
    }

    /// <summary>
    /// Gets the roots.
    /// </summary>
    public IReadOnlyList<IRoot> Roots { get; }

    /// <summary>
    /// Gets the version associated with the plan.
    /// </summary>
    public IVersion Version { get; }
}
