// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Substrait.Tools;

namespace Substrait.Core.Extension;

/// <summary>
/// Collects extension declarations while translating the internal representation to protobuf.
/// </summary>
public class ExtensionsCollector
{
    private ExtensionsCollector(
        IEnumerable<string> extensionUris,
        IEnumerable<(ExtensionType Type, string Name, int ExtensionUriReference)> extensions)
    {
        this.ExtensionUris = extensionUris.ToImmutableList();
        this.Extensions = extensions.ToImmutableList();
    }

    /// <summary>
    /// The extension declaration type.
    /// </summary>
    public enum ExtensionType
    {
        /// <summary>
        /// A user-defined type.
        /// </summary>
        Type,

        /// <summary>
        /// A type variation.
        /// </summary>
        TypeVariation,

        /// <summary>
        /// A function.
        /// </summary>
        Function,
    }

    /// <summary>
    /// Gets extension URIs in anchor order.
    /// </summary>
    public IReadOnlyList<string> ExtensionUris { get; init; }

    /// <summary>
    /// Gets extension declarations in anchor order within each extension type.
    /// </summary>
    public IReadOnlyList<(ExtensionType Type, string Name, int ExtensionUriReference)> Extensions { get; init; }

    /// <summary>
    /// Builds an extension collection and assigns stable anchors.
    /// </summary>
    public class Builder
    {
        private readonly UniqueList<string> extensionUris = new();
        private readonly Dictionary<ExtensionType, UniqueList<(string Name, int ExtensionUriReference)>> extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="Builder"/> class.
        /// </summary>
        public Builder()
        {
#if NET5_0_OR_GREATER
            this.extensions = Enum.GetValues<ExtensionType>()
#else
            this.extensions = Enum.GetValues(typeof(ExtensionType)).Cast<ExtensionType>()
#endif
                .ToDictionary(type => type, _ => new UniqueList<(string Name, int ExtensionUriReference)>());
        }

        /// <summary>
        /// Collects an extension and returns its plan anchor.
        /// </summary>
        /// <param name="type">The extension type.</param>
        /// <param name="uri">The extension URI.</param>
        /// <param name="name">The extension name.</param>
        /// <returns>The extension anchor.</returns>
        public int Collect(ExtensionType type, string uri, string name)
        {
            int extensionUriAnchor = this.extensionUris.TryAddAndIndexOf(uri);
            int anchor = this.extensions[type].TryAddAndIndexOf((name, extensionUriAnchor));
            return anchor + (type is ExtensionType.TypeVariation ? 1 : 0);
        }

        /// <summary>
        /// Builds the collected extension declarations.
        /// </summary>
        /// <returns>The collected extensions.</returns>
        public ExtensionsCollector Build()
        {
            return new ExtensionsCollector(
                this.extensionUris,
                this.extensions.SelectMany(pair =>
                    pair.Value.Select(extension => (pair.Key, extension.Name, extension.ExtensionUriReference))));
        }
    }
}
