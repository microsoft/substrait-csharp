// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Type;
using static Substrait.Core.Type.IType;
using static Substrait.Tools.TypeUtils;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Base class for parameterized type expressions.
/// These are expressions that can only be used in the definition of functions arguments in extensions.
/// </summary>
public abstract class ParameterizedTypeExpression : ParameterizedType
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterizedTypeExpression"/> class.
    /// </summary>
    /// <param name="nullable">Whether it is nullable.</param>
    protected ParameterizedTypeExpression(NullableType nullable)
        : base(nullable)
    {
    }

    /// <summary>
    /// Immutable implementation of VarChar type expression.
    /// </summary>
    public sealed new class VarChar : ParameterizedTypeExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VarChar"/> class.
        /// </summary>
        /// <param name="length">The expression representing the length of the varchar.</param>
        /// <param name="nullable">Whether it is nullable.</param>
        public VarChar(ParameterizedTypeExpression length, NullableType nullable)
            : base(nullable)
        {
            this.Length = length;
        }

        /// <inheritdoc/>
        public override string ShortTypeName => "vchar";

        /// <inheritdoc/>
        public override string TypeName => "varchar";

        /// <summary>
        /// Gets the length of the varchar.
        /// </summary>
        public ParameterizedTypeExpression Length { get; }

        /// <inheritdoc/>
        public override string ToTypeString() => $"varchar<{this.Length.ToTypeString()}>";

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(TypeVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        protected override bool NodeEqualTypeParameters(IType other)
        {
            return other is VarChar o && this.Length.NodeEquals(o.Length, ITypeComparison.Strict);
        }
    }

    /// <summary>
    /// Immutable implementation of StringLiteral type.
    /// </summary>
    public sealed class StringLiteral : ParameterizedTypeExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringLiteral"/> class.
        /// </summary>
        /// <param name="value">The value of the string literal.</param>
        /// <param name="nullable">Whether it is nullable.</param>
        public StringLiteral(string value, NullableType nullable)
            : base(nullable)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets value.
        /// </summary>
        public string Value { get; }

        /// <inheritdoc/>
        public override string ShortTypeName
        {
            get
            {
                if (this.IsWildcard)
                {
                    return "any";
                }

                throw new ArgumentException("Unexpected string literal: " + this.Value);
            }
        }

        /// <inheritdoc/>
        public override bool IsWildcard
        {
            get => this.Value.StartsWith("any");
        }

        /// <inheritdoc/>
        public override string TypeName => this.ShortTypeName;

        /// <inheritdoc/>
        public override string ToTypeString() => this.Value;

        /// <inheritdoc/>
        public override TOutput Accept<TContext, TOutput>(TypeVisitor<TContext, TOutput> visitor, TContext context) => visitor.Visit(this, context);

        /// <inheritdoc/>
        protected override bool NodeEqualTypeParameters(IType other)
        {
            return other is StringLiteral o && this.Value == o.Value;
        }
    }
}
