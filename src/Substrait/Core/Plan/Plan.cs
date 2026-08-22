// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Core.Relation;
using Substrait.Tools;
using static Substrait.Core.Plan.IPlan;

namespace Substrait.Core.Plan;

/// <summary>
/// An immutable implementation of plan defined in <see cref="IPlan"/>.
/// </summary>
public sealed class Plan : IPlan, IEquatable<Plan>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Plan"/> class.
    /// </summary>
    /// <param name="roots">The roots of the plan.</param>
    /// <param name="version">The version of the plan.</param>
    public Plan(IEnumerable<IRoot> roots, IVersion version)
    {
        this.Roots = roots.ToImmutableList();
        this.Version = version;
    }

    /// <inheritdoc/>
    public IReadOnlyList<IRoot> Roots { get; }

    /// <inheritdoc/>
    public IVersion Version { get; }

    /// <inheritdoc/>
    public bool Equals(Plan? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && Enumerable.SequenceEqual(this.Roots, other.Roots) && this.Version.Equals(other.Version);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Plan);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.Roots.CombineHashCodes(), this.Version);
    }

    /// <summary>
    /// An immutable implementation of root defined in <see cref="IRoot"/>.
    /// </summary>
    public sealed class Root : IRoot, IEquatable<Root>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Root"/> class.
        /// </summary>
        /// <param name="input">The input relation.</param>
        /// <param name="names">The output names for the relation.</param>
        public Root(IRel input, IEnumerable<string> names)
        {
            this.Input = input;
            this.Names = names.ToImmutableList();
        }

        /// <inheritdoc />
        public IRel Input { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> Names { get; }

        /// <inheritdoc />
        public bool Equals(Root? other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other is not null && Enumerable.SequenceEqual(this.Names, other.Names) && this.Input.Equals(other.Input);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return this.Equals(obj as Root);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(this.Names.CombineHashCodes(), this.Input);
        }
    }
}
