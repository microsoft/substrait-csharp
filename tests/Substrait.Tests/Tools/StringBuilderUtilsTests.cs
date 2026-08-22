// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Tools;
using static Substrait.Tools.StringBuilderUtils;

namespace Substrait.Tests.Tools;

[TestClass]
public sealed class StringBuilderUtilsTests
{
    private static readonly char[] Space = [' '];
    private static readonly char[] HelloSuffix = ['l', 'e', 'o'];

    [TestMethod]
    public void AppendTypeNameFormatsBuiltInNestedAndGenericTypes()
    {
        var builder = new StringBuilder();
        Assert.AreEqual("string", builder.AppendTypeName(typeof(string)).ToString());

        builder.Clear();
        Assert.AreEqual("StringBuilderUtilsTests.NestedType", builder.AppendTypeName(typeof(NestedType)).ToString());

        builder.Clear();
        Assert.AreEqual("NestedType", builder.AppendTypeName(typeof(NestedType), qualifyDeclaringTypes: false).ToString());

        builder.Clear();
        Assert.AreEqual(
            "List<Dictionary<int,string>>",
            builder.AppendTypeName(typeof(List<Dictionary<int, string>>), qualifyDeclaringTypes: false).ToString());
    }

    [TestMethod]
    public void TrimEndRemovesOnlyMatchingSuffixCharacters()
    {
        Assert.AreEqual("abcde", new StringBuilder("abcde  ").TrimEnd(Space).ToString());
        Assert.AreEqual("abcdeh", new StringBuilder("abcdehello").TrimEnd(HelloSuffix).ToString());
        Assert.AreEqual("abcde", new StringBuilder("abcde").TrimEnd(Space).ToString());
    }

    [TestMethod]
    public void EndsWithRecognizesSuffixes()
    {
        var builder = new StringBuilder("abcde");

        Assert.IsTrue(builder.EndsWith("e"));
        Assert.IsTrue(builder.EndsWith("de"));
        Assert.IsTrue(builder.EndsWith("cde"));
        Assert.IsTrue(builder.EndsWith("bcde"));
        Assert.IsTrue(builder.EndsWith("abcde"));
        Assert.IsFalse(builder.EndsWith(" abcde"));
        Assert.IsFalse(builder.EndsWith(" "));
    }

    [TestMethod]
    public void IndentAppendsCharactersForEachLevel()
    {
        var builder = new StringBuilder();
        var indent = new IndentChar('!', 2);

        Assert.AreEqual(string.Empty, builder.Indent(indent, 0).ToString());
        Assert.AreEqual("!!!!", builder.Indent(indent, 2).ToString());
    }

    private sealed class NestedType
    {
    }
}
