// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Expression;
using Substrait.Core.Extension.Functions;
using Substrait.Core.Extension.Types;
using Substrait.Tools.Visitor;
using ProtoNullability = Substrait.Protobuf.Type.Types.Nullability;

namespace Substrait.Core.Type;

/// <summary>
/// Interface for all relational types, <see cref="Protobuf.Type"/>.
/// </summary>
public interface IType : ITypeExpression, IFunctionArg, INavigableNode<IType>
{
    /// <summary>
    /// Enum for nullable type.
    /// </summary>
    public enum NullableType
    {
        /// <summary>
        /// Nullability Unspecified.
        /// </summary>
        Unspecified = ProtoNullability.Unspecified,

        /// <summary>
        /// Indicates that the type is required and cannot be null.
        /// </summary>
        Required = ProtoNullability.Required,

        /// <summary>
        /// Indicates that the type is nullable.
        /// </summary>
        Nullable = ProtoNullability.Nullable,
    }

    /// <summary>
    /// Gets a value indicating whether nullable.
    /// </summary>
    public NullableType Nullable { get; }

    /// <summary>
    /// Gets the type variation of this type.
    /// </summary>
    public ITypeVariation? TypeVariation { get; }

    /// <summary>
    /// Gets whether the <paramref name="other"/> is equivalent with respect to <paramref name="comparison"/>
    /// accounting only local information (i.e., not considering the nested types).
    /// </summary>
    /// <param name="other">other type to compare.</param>
    /// <param name="comparison">comparison mode.</param>
    /// <returns>true if <paramref name="other"/> is equivalent to this.</returns>
    public bool NodeEquals(IType other, ITypeComparison comparison);

    /// <summary>
    /// Accepts the node visitor.
    /// </summary>
    /// <typeparam name="TContext">Context for the visitor implementation.</typeparam>
    /// <typeparam name="TOutput">Output for the visitor implementation.</typeparam>
    /// <param name="visitor">Node visitor.</param>
    /// <param name="context">Context object.</param>
    /// <returns>Returns the output object.</returns>
    public TOutput Accept<TContext, TOutput>(TypeVisitor<TContext, TOutput> visitor, TContext context);
}
