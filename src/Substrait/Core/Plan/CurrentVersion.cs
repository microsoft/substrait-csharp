// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Plan;

/// <summary>
/// Provides information about the current version of the Substrait library.
/// This can be used to populate the Version field in a plan if it is created
/// by this library.
/// </summary>
public static class CurrentVersion
{
    /// <summary>
    /// The current major version number information for Substrait.
    /// </summary>
    public static readonly uint MajorNumber = 0;

    /// <summary>
    /// The current minor version number information for Substrait.
    /// </summary>
    public static readonly uint MinorNumber = 73;

    /// <summary>
    /// The current patch version number information Substrait.
    /// </summary>
    public static readonly uint PatchNumber = 0;

    /// <summary>
    /// The current Git commit hash from the Substrait repository.
    /// </summary>
    public static readonly string GitHash = "d430e521f203aec6a4e06731d4bfd68cdf61f443";

    /// <summary>
    /// The producer information for the Substrait plan (if any).
    /// </summary>
    public static readonly string Producer = string.Empty;
}
