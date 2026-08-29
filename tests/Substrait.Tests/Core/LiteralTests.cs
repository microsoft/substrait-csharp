// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Type;
using static Substrait.Core.Expression.Literal;
using static Substrait.Core.Type.IType;

namespace Substrait.Tests.Core;

/// <summary>
/// Tests substrait literals.
/// </summary>
[TestClass]
public class LiteralTests
{
    /// <summary>
    /// Gets simple literal equals test cases.
    /// </summary>
    public static IEnumerable<object[]> SimpleLiteralEqualsTestCases
    {
        get
        {
            return new[]
            {
                new object[] { typeof(BoolLiteral), typeof(bool), true, false },
                new object[] { typeof(I8Literal), typeof(int), 123, 456 },
                new object[] { typeof(I16Literal), typeof(int), 123, 456 },
                new object[] { typeof(I32Literal), typeof(int), 123, 456 },
                new object[] { typeof(I64Literal), typeof(long), 12345, 67890 },
                new object[] { typeof(FP32Literal), typeof(int), 123, 456 },
                new object[] { typeof(FP64Literal), typeof(int), 123, 456 },
                new object[] { typeof(DateLiteral), typeof(int), 123, 456 },
                new object[] { typeof(TimeLiteral), typeof(long), 12345, 67890 },
                new object[] { typeof(StrLiteral), typeof(string), "abc", "def" },
                new object[] { typeof(BinaryLiteral), typeof(ByteString), ByteString.FromBase64("abcd"), ByteString.FromBase64("cdef") },
                new object[] { typeof(FixedCharLiteral), typeof(string), "abc", "def" },
                new object[] { typeof(FixedBinaryLiteral), typeof(ByteString), ByteString.FromBase64("abcd"), ByteString.FromBase64("cdef") },
            };
        }
    }

    /// <summary>
    /// Tests null literal equality.
    /// </summary>
    [TestMethod]
    public void TestNullLiteralEquals()
    {
        var v1 = new NullLiteral(TypeFactory.NULLABLE.I64);
        var v2 = new NullLiteral(TypeFactory.NULLABLE.I64);
        var v3 = new NullLiteral(TypeFactory.NULLABLE.I32);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
    }

    /// <summary>
    /// Tests null literal equality.
    /// If the base type is not nullable, the literal type should entail corresponding nullable type.
    /// </summary>
    [TestMethod]
    public void TestNullLiteralEqualsWithNonNullType()
    {
        var v1 = new NullLiteral(TypeFactory.NULLABLE.I64);
        var v2 = new NullLiteral(TypeFactory.REQUIRED.I64);

        Assert.AreEqual(v1, v2);
        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
    }

    /// <summary>
    /// Tests primivive literal equality.
    /// </summary>
    /// <param name="literalType">type of literal to test.</param>
    /// <param name="valueType">type of value to construct literal.</param>
    /// <param name="value1">first value for test.</param>
    /// <param name="value2">second value for test.</param>
    [TestMethod]
    [DynamicData(nameof(SimpleLiteralEqualsTestCases))]
    [DataTestMethod]
    public void TestSimpleLiteralEquals(System.Type literalType, System.Type valueType, object value1, object value2)
    {
        var ctor = literalType.GetConstructor(new[] { valueType, typeof(NullableType) });
        Assert.IsNotNull(ctor);

        var v1 = ctor.Invoke(new object[] { value1, NullableType.Required });
        var v2 = ctor.Invoke(new object[] { value1, NullableType.Required });
        var v3 = ctor.Invoke(new object[] { value2, NullableType.Required });
        var v4 = ctor.Invoke(new object[] { value1, NullableType.Nullable });

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
    }

    /// <summary>
    /// Tests precision timestamp literal equals.
    /// </summary>
    [TestMethod]
    public void TestPrecisionTimestampLiteralEquals()
    {
        var v1 = new PrecisionTimestampLiteral(12345, 5, NullableType.Required);
        var v2 = new PrecisionTimestampLiteral(12345, 5, NullableType.Required);
        var v3 = new PrecisionTimestampLiteral(56789, 5, NullableType.Required);
        var v4 = new PrecisionTimestampLiteral(12345, 5, NullableType.Nullable);
        var v5 = new PrecisionTimestampLiteral(12345, 3, NullableType.Nullable);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);
        Assert.AreNotEqual(v1, v5);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v5.GetHashCode());
    }

    /// <summary>
    /// Tests precision timestamp tz literal equals.
    /// </summary>
    [TestMethod]
    public void TestPrecisionTimestampLiteralTZEquals()
    {
        var v1 = new PrecisionTimestampTZLiteral(12345, 5, NullableType.Required);
        var v2 = new PrecisionTimestampTZLiteral(12345, 5, NullableType.Required);
        var v3 = new PrecisionTimestampTZLiteral(56789, 5, NullableType.Required);
        var v4 = new PrecisionTimestampTZLiteral(12345, 5, NullableType.Nullable);
        var v5 = new PrecisionTimestampTZLiteral(12345, 3, NullableType.Nullable);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);
        Assert.AreNotEqual(v1, v5);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v5.GetHashCode());
    }

    /// <summary>
    /// Tests varchar literal equals.
    /// </summary>
    [TestMethod]
    public void TestVarCharLiteralEquals()
    {
        var v1 = new VarCharLiteral("abcd", 16, NullableType.Required);
        var v2 = new VarCharLiteral("abcd", 16, NullableType.Required);
        var v3 = new VarCharLiteral("defg", 16, NullableType.Required);
        var v4 = new VarCharLiteral("abcd", 16, NullableType.Nullable);
        var v5 = new VarCharLiteral("abcd", 9, NullableType.Nullable);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);
        Assert.AreNotEqual(v1, v5);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v5.GetHashCode());
    }

    /// <summary>
    /// Tests decimal literal equals.
    /// </summary>
    [TestMethod]
    public void TestDecimalLiteralEquals()
    {
        var v1 = new DecimalLiteral(ByteString.FromBase64("abcd"), 10, 3, NullableType.Required);
        var v2 = new DecimalLiteral(ByteString.FromBase64("abcd"), 10, 3, NullableType.Required);
        var v3 = new DecimalLiteral(ByteString.FromBase64("dbca"), 10, 3, NullableType.Required);
        var v4 = new DecimalLiteral(ByteString.FromBase64("abcd"), 10, 3, NullableType.Nullable);
        var v5 = new DecimalLiteral(ByteString.FromBase64("abcd"), 12, 3, NullableType.Required);
        var v6 = new DecimalLiteral(ByteString.FromBase64("abcd"), 10, 4, NullableType.Required);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);
        Assert.AreNotEqual(v1, v5);
        Assert.AreNotEqual(v1, v6);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v5.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v6.GetHashCode());
    }

    /// <summary>
    /// Tests struct literal equals.
    /// </summary>
    [TestMethod]
    public void TestStructLiteralEquals()
    {
        var v1 = new StructLiteral([new I64Literal(1), new StrLiteral("abc")], NullableType.Required);
        var v2 = new StructLiteral([new I64Literal(1), new StrLiteral("abc")], NullableType.Required);
        var v3 = new StructLiteral([new I64Literal(2), new StrLiteral("def")], NullableType.Required);
        var v4 = new StructLiteral([new I64Literal(1), new StrLiteral("abc")], NullableType.Nullable);
        var v5 = new StructLiteral([new I32Literal(1), new StrLiteral("abc")], NullableType.Nullable);

        Assert.AreEqual(v1, v2);
        Assert.AreNotEqual(v1, v3);
        Assert.AreNotEqual(v1, v4);
        Assert.AreNotEqual(v1, v5);

        Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v3.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v4.GetHashCode());
        Assert.AreNotEqual(v1.GetHashCode(), v5.GetHashCode());
    }
}
