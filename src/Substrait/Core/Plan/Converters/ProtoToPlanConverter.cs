// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using System.Runtime.Serialization;
using Substrait.Core.Extension;
using Substrait.Core.Relation.Converters;
using Substrait.Tools;

namespace Substrait.Core.Plan.Converters;

/// <summary>
/// Converts protobuf plans to the internal representation.
/// </summary>
public class ProtoToPlanConverter
{
    private readonly ExtensionsCollection extensions;

    /// <summary>
    /// Initializes a converter using standard extensions.
    /// </summary>
    public ProtoToPlanConverter()
        : this(ExtensionUtils.LoadDefaults())
    {
    }

    /// <summary>
    /// Initializes a converter.
    /// </summary>
    /// <param name="extensions">The available extensions.</param>
    public ProtoToPlanConverter(ExtensionsCollection extensions)
    {
        this.extensions = extensions;
    }

    /// <summary>
    /// Converts a protobuf plan.
    /// </summary>
    /// <param name="plan">The protobuf plan.</param>
    /// <param name="strictMode">The extension resolution mode.</param>
    /// <returns>The internal plan.</returns>
    public IPlan From(
        Protobuf.Plan plan,
        ExtensionsDictionary.StrictMode strictMode = ExtensionsDictionary.StrictMode.STRICT)
    {
        if (plan.Relations.Count > 1)
        {
            throw new NotImplementedException("Plans with more than one relation are not supported yet.");
        }

        var relationConverter = this.GetProtoRelConverter(new ExtensionsDictionary.Builder(plan).Build(), strictMode);
        var roots = plan.Relations.Select(planRelation =>
        {
            if (planRelation.RelTypeCase != Protobuf.PlanRel.RelTypeOneofCase.Root)
            {
                throw new SerializationException("Deserialization error: plan relations must be roots.");
            }

            return (IPlan.IRoot)new Plan.Root(
                relationConverter.ToRel(planRelation.Root.Input),
                planRelation.Root.Names.ToImmutableList());
        });

        Protobuf.Version version = plan.Version;
        return new Plan(
            roots,
            new Version(
                version.MajorNumber,
                version.MinorNumber,
                version.PatchNumber,
                version.GitHash,
                version.Producer));
    }

    /// <summary>
    /// Creates the relation converter used by this plan converter.
    /// </summary>
    /// <param name="lookup">The plan extension lookup.</param>
    /// <param name="strictMode">The extension resolution mode.</param>
    /// <returns>The relation converter.</returns>
    protected ProtoToRelConverter GetProtoRelConverter(
        ExtensionsDictionary lookup,
        ExtensionsDictionary.StrictMode strictMode) =>
        new(lookup, this.extensions, strictMode);
}
