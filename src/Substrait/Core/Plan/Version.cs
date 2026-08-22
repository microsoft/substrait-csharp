// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

namespace Substrait.Core.Plan;

/// <summary>
/// Interface for plan version.
/// </summary>
public sealed class Version : IVersion, IEquatable<Version>
{
    /// <summary>
    /// Gets the current version.
    /// </summary>
    public static readonly Version Current = new Version(
        CurrentVersion.MajorNumber,
        CurrentVersion.MinorNumber,
        CurrentVersion.PatchNumber,
        CurrentVersion.GitHash,
        CurrentVersion.Producer);

    /// <summary>
    /// Initializes a new instance of the <see cref="Version"/> class.
    /// </summary>
    /// <param name="majorNumber">The major version number.</param>
    /// <param name="minorNumber">The minor version number.</param>
    /// <param name="patchNumber">The patch version number.</param>
    /// <param name="gitHash">The git hash of the version (if custom version was used).</param>
    /// <param name="producer">The identifying information for the producer that created this plan.</param>
    public Version(uint majorNumber, uint minorNumber, uint patchNumber, string gitHash, string producer)
    {
        this.MajorNumber = majorNumber;
        this.MinorNumber = minorNumber;
        this.PatchNumber = patchNumber;
        this.GitHash = gitHash;
        this.Producer = producer;
    }

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

    /// <inheritdoc/>
    public bool Equals(Version? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.MajorNumber == other?.MajorNumber
            && this.MinorNumber == other.MinorNumber
            && this.PatchNumber == other.PatchNumber
            && this.GitHash.Equals(other.GitHash)
            && this.Producer.Equals(other.Producer);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Version);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(
            this.MajorNumber,
            this.MinorNumber,
            this.PatchNumber,
            this.GitHash,
            this.Producer);
    }
}
