// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Type;
using static Substrait.Core.Type.IType;

namespace Substrait.Core.Extension.Functions;

/// <summary>
/// Type Expression Factory.
/// </summary>
public class TypeExpressionFactory : TypeFactory
{
    /// <summary>
    /// Creator for non-nullable types.
    /// </summary>
    public static new readonly TypeExpressionFactory REQUIRED = new(NullableType.Required);

    /// <summary>
    /// Creator for nullable types.
    /// </summary>
    public static new readonly TypeExpressionFactory NULLABLE = new(NullableType.Nullable);

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeExpressionFactory"/> class.
    /// </summary>
    /// <param name="nullable">Whether the created type is nullable.</param>
    protected TypeExpressionFactory(NullableType nullable)
        : base(nullable)
    {
    }

    /// <summary>
    /// Returns the type creator for the specified nullability.
    /// </summary>
    /// <param name="nullable">Whether the created types should be nullable.</param>
    /// <returns>The type creator.</returns>
    public static new TypeExpressionFactory Of(NullableType nullable)
    {
        return nullable switch
        {
            NullableType.Required => REQUIRED,
            NullableType.Nullable => NULLABLE,
            _ => throw new NotImplementedException(nullable.ToString()),
        };
    }

    /// <summary>
    /// VarChar type expression.
    /// </summary>
    /// <param name="length">The length of the varchar type expression.</param>
    /// <returns>The varchar type expression.</returns>
    public ParameterizedTypeExpression.VarChar VarChar(ParameterizedTypeExpression length)
    {
        return new ParameterizedTypeExpression.VarChar(length, this.Nullable);
    }

    /// <summary>
    /// String literal.
    /// </summary>
    /// <param name="value">The value for the string literal.</param>
    /// <returns>The string literal type.</returns>
    public ParameterizedTypeExpression.StringLiteral StringLiteral(string value)
    {
        return new ParameterizedTypeExpression.StringLiteral(value, this.Nullable);
    }
}
