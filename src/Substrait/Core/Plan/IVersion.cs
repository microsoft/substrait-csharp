// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Plan;

/// <summary>
/// Interface for plan version.
/// </summary>
public interface IVersion
{
    /// <summary>
    /// Gets the major version number.
    /// </summary>
    public uint MajorNumber { get; }

    /// <summary>
    /// Gets the minor version number.
    /// </summary>
    public uint MinorNumber { get; }

    /// <summary>
    /// Gets the patch version number.
    /// </summary>
    public uint PatchNumber { get; }

    /// <summary>
    /// Gets the git hash of the version (if custom version was used).
    /// </summary>
    public string GitHash { get; }

    /// <summary>
    /// Gets the identifying information for the producer that created this plan.
    /// </summary>
    public string Producer { get; }
}
