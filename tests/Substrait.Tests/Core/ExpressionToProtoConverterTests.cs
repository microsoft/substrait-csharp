// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Expression;
using Substrait.Core.Expression.Converters;
using Substrait.Core.Extension;
using Substrait.Core.Plan.Converters;
using Substrait.Core.Type;
using Substrait.Core.Type.Converters;

namespace Substrait.Tests.Core;

[TestClass]
public sealed class ExpressionToProtoConverterTests
{
    private readonly ExpressionToProtoConverter converter = new(new TypeToProtoConverter());

    [TestMethod]
    public void ConvertsNestedStructLiteralAndConditional()
    {
        var structure = new Literal.StructLiteral(
        [
            new Literal.I64Literal(42),
            new Literal.StructLiteral([new Literal.StrLiteral("value")]),
        ]);
        var expression = new Substrait.Core.Expression.Expression.IfThen(
            [(new Literal.BoolLiteral(true), structure)],
            new Literal.StructLiteral([new Literal.I64Literal(0), new Literal.StructLiteral([new Literal.StrLiteral("other")])]));

        Protobuf.Expression result = this.converter.From(expression);

        Assert.AreEqual(1, result.IfThen.Ifs.Count);
        Assert.AreEqual(42L, result.IfThen.Ifs[0].Then.Literal.Struct.Fields[0].I64);
        Assert.AreEqual("value", result.IfThen.Ifs[0].Then.Literal.Struct.Fields[1].Struct.Fields[0].String);
    }

    [TestMethod]
    public void CollectsScalarFunctionAnchor()
    {
        var expression = new Substrait.Core.Expression.Expression.ScalarFunctionInvocation(
            "/functions.yaml",
            "add:i64_i64",
            [new Literal.I64Literal(1), new Literal.I64Literal(2)],
            TypeFactory.REQUIRED.I64,
            null);
        var context = new PlanToProtoConverter.ConverterContext();

        Protobuf.Expression result = this.converter.From(expression, context);

        Assert.AreEqual(0U, result.ScalarFunction.FunctionReference);
        Assert.AreEqual(2, result.ScalarFunction.Arguments.Count);
        Assert.AreEqual(ExtensionsCollector.ExtensionType.Function, context.ExtensionsCollector.Extensions[0].Type);
        Assert.AreEqual("add:i64_i64", context.ExtensionsCollector.Extensions[0].Name);
    }
}
