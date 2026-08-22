// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Antlr4.Runtime;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Antlr.Type;
using Substrait.Protobuf;

namespace Substrait.Tests;

[TestClass]
public sealed class ToolchainValidationTests
{
    [TestMethod]
    public void PlanRoundTripsThroughBinaryAndJson()
    {
        Plan expected = new()
        {
            Version = new Substrait.Protobuf.Version
            {
                MajorNumber = 0,
                MinorNumber = 73,
                PatchNumber = 0,
                Producer = "substrait-csharp-tests",
            },
        };

        Plan binaryRoundTrip = Plan.Parser.ParseFrom(expected.ToByteArray());
        Plan jsonRoundTrip = JsonParser.Default.Parse<Plan>(JsonFormatter.Default.Format(expected));

        Assert.AreEqual(expected, binaryRoundTrip);
        Assert.AreEqual(expected, jsonRoundTrip);
    }

    [TestMethod]
    public void TypeGrammarParsesScalarType()
    {
        AntlrInputStream input = new("i32");
        SubstraitTypeLexer lexer = new(input);
        SubstraitTypeParser parser = new(new CommonTokenStream(lexer));

        parser.startRule();

        Assert.AreEqual(0, parser.NumberOfSyntaxErrors);
    }

    [TestMethod]
    public void StandardExtensionsAreEmbedded()
    {
        string[] expected =
        [
            "DefaultExtensions/functions_aggregate_approx.yaml",
            "DefaultExtensions/functions_aggregate_generic.yaml",
            "DefaultExtensions/functions_arithmetic.yaml",
            "DefaultExtensions/functions_boolean.yaml",
            "DefaultExtensions/functions_comparison.yaml",
            "DefaultExtensions/functions_datetime.yaml",
            "DefaultExtensions/functions_logarithmic.yaml",
            "DefaultExtensions/functions_rounding.yaml",
            "DefaultExtensions/functions_string.yaml",
            "DefaultExtensions/type_variations.yaml",
        ];

        string[] actual = typeof(Plan).Assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith("DefaultExtensions/", StringComparison.Ordinal))
            .ToArray();

        CollectionAssert.AreEquivalent(expected, actual);
    }
}
