// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Extension.Functions;
using Substrait.Core.Type;

namespace Substrait.Tests.Core;

[TestClass]
public sealed class ExtensionFunctionTests
{
    [TestMethod]
    public void TypeExpressionParserCreatesScalarTypes()
    {
        ITypeExpression parsed = TypeExpressionParser.Parse("i64");

        Assert.AreEqual(TypeFactory.REQUIRED.I64, parsed);
    }

    [TestMethod]
    public void FunctionKeyUsesParsedArgumentSignatures()
    {
        IArgument[] arguments =
        [
            new ValueArgument("i64", "left", "Left operand.", required: true),
            new ValueArgument("string", "right", "Right operand.", required: true),
        ];

        ScalarFunctionImpl function = new(
            "https://example.test/functions",
            "compare",
            "Compares two values.",
            FunctionImpl.NullabilityMode.Mirror,
            arguments,
            ImmutableDictionary<string, IOption>.Empty,
            ordered: null,
            variadic: null,
            returnType: "boolean");

        Assert.AreEqual("compare:i64_str", function.Key);
        Assert.AreEqual(function.Uri, function.Anchor.Namespace);
        Assert.AreEqual(function.Key, function.Anchor.Key);
    }

    [TestMethod]
    public void FunctionRangeUsesDeclaredArgumentCountForNonVariadicFunction()
    {
        IArgument[] arguments =
        [
            new ValueArgument("i64", "required", "Required operand.", required: true),
            new ValueArgument("i64", "optional", "Optional operand.", required: false),
        ];
        ScalarFunctionImpl function = CreateFunction(arguments, variadic: null);

        Assert.AreEqual(new Tuple<int, int>(1, 2), function.GetRange());
    }

    [TestMethod]
    public void FunctionRangeUsesVariadicOccurrenceBounds()
    {
        IArgument[] arguments =
        [
            new ValueArgument("i64", "fixed", "Fixed operand.", required: true),
            new ValueArgument("i64", "repeated", "Repeated operand.", required: true),
        ];

        ScalarFunctionImpl bounded = CreateFunction(arguments, new VariadicBehavior(min: 2, max: 4));
        ScalarFunctionImpl unbounded = CreateFunction(arguments, new VariadicBehavior(min: 0));

        Assert.AreEqual(new Tuple<int, int>(3, 5), bounded.GetRange());
        Assert.AreEqual(new Tuple<int, int>(1, int.MaxValue), unbounded.GetRange());
    }

    private static ScalarFunctionImpl CreateFunction(IEnumerable<IArgument> arguments, IVariadicBehavior? variadic)
    {
        return new ScalarFunctionImpl(
            "https://example.test/functions",
            "function",
            "Test function.",
            FunctionImpl.NullabilityMode.Mirror,
            arguments,
            ImmutableDictionary<string, IOption>.Empty,
            ordered: null,
            variadic,
            returnType: "boolean");
    }
}
