// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Type;
using static Substrait.Tools.TypeUtils;

namespace Substrait.Tests.Core;

/// <summary>
/// Type related tests.
/// </summary>
[TestClass]
public class TypeTests
{
    /// <summary>
    /// Gets all primitive types from the type factory.
    /// </summary>
    /// <param name="typeFactory">type factory.</param>
    /// <returns>All primitive types.</returns>
    public static IEnumerable<PrimitiveType> GetAllPrimitiveTypes(TypeFactory typeFactory)
    {
        return typeFactory.GetType().GetProperties().Where(x => typeof(PrimitiveType).IsAssignableFrom(x.PropertyType)).Select(x => x.GetValue(typeFactory)).Cast<PrimitiveType>();
    }

    /// <summary>
    /// Tests equality of primitive types.
    /// </summary>
    [TestMethod]
    public void TestPrimitiveTypeEquals()
    {
        var allPrimitiveTypes = GetAllPrimitiveTypes(TypeFactory.REQUIRED).Concat(GetAllPrimitiveTypes(TypeFactory.NULLABLE)).ToImmutableList();

        for (var i = 0; i < allPrimitiveTypes.Count; ++i)
        {
            for (var j = 0; j < allPrimitiveTypes.Count; ++j)
            {
                Assert.AreEqual(allPrimitiveTypes[i].Equals(allPrimitiveTypes[j]), i == j, $"({allPrimitiveTypes[i]},{allPrimitiveTypes[j]})");
                Assert.AreEqual(allPrimitiveTypes[i].GetHashCode() == allPrimitiveTypes[j].GetHashCode(), i == j, $"({allPrimitiveTypes[i]},{allPrimitiveTypes[j]}).GetHashCode()");
            }
        }
    }

    /// <summary>
    /// Tests precision timestamp equals.
    /// </summary>
    [TestMethod]
    public void TestPrecisionTimestampEquals()
    {
        var t1 = TypeFactory.REQUIRED.PrecisionTimestamp(1);
        var t2 = TypeFactory.REQUIRED.PrecisionTimestamp(1);
        var t3 = TypeFactory.REQUIRED.PrecisionTimestamp(2);
        var t4 = TypeFactory.NULLABLE.PrecisionTimestamp(1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests precision timestamp with timezone equals.
    /// </summary>
    [TestMethod]
    public void TestPrecisionTimestampTZEquals()
    {
        var t1 = TypeFactory.REQUIRED.PrecisionTimestampTZ(1);
        var t2 = TypeFactory.REQUIRED.PrecisionTimestampTZ(1);
        var t3 = TypeFactory.REQUIRED.PrecisionTimestampTZ(2);
        var t4 = TypeFactory.NULLABLE.PrecisionTimestampTZ(1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests fixed char equals.
    /// </summary>
    [TestMethod]
    public void TestFixedCharEquals()
    {
        var t1 = TypeFactory.REQUIRED.FixedChar(1);
        var t2 = TypeFactory.REQUIRED.FixedChar(1);
        var t3 = TypeFactory.REQUIRED.FixedChar(2);
        var t4 = TypeFactory.NULLABLE.FixedChar(1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests fixed char equals.
    /// </summary>
    [TestMethod]
    public void TestVarCharEquals()
    {
        var t1 = TypeFactory.REQUIRED.VarChar(1);
        var t2 = TypeFactory.REQUIRED.VarChar(1);
        var t3 = TypeFactory.REQUIRED.VarChar(2);
        var t4 = TypeFactory.NULLABLE.VarChar(1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests fixed binary equals.
    /// </summary>
    [TestMethod]
    public void TestFixedBinaryEquals()
    {
        var t1 = TypeFactory.REQUIRED.FixedBinary(1);
        var t2 = TypeFactory.REQUIRED.FixedBinary(1);
        var t3 = TypeFactory.REQUIRED.FixedBinary(2);
        var t4 = TypeFactory.NULLABLE.FixedBinary(1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests decimal equals.
    /// </summary>
    [TestMethod]
    public void TestDecimalEquals()
    {
        var t1 = TypeFactory.REQUIRED.Decimal(2, 1);
        var t2 = TypeFactory.REQUIRED.Decimal(2, 1);
        var t3 = TypeFactory.REQUIRED.Decimal(2, 2);
        var t4 = TypeFactory.REQUIRED.Decimal(3, 1);
        var t5 = TypeFactory.NULLABLE.Decimal(2, 1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);
        Assert.AreNotEqual(t1, t5);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t5.GetHashCode());
    }

    /// <summary>
    /// Tests struct equals.
    /// </summary>
    [TestMethod]
    public void TestStructEquals()
    {
        var fields1 = new List<IType> { TypeFactory.REQUIRED.I64, TypeFactory.NULLABLE.STR };
        var fields2 = new List<IType> { TypeFactory.NULLABLE.STR, TypeFactory.REQUIRED.I64 };

        var t1 = TypeFactory.REQUIRED.Struct(fields1);
        var t2 = TypeFactory.REQUIRED.Struct(fields1);
        var t3 = TypeFactory.REQUIRED.Struct(fields2);
        var t4 = TypeFactory.NULLABLE.Struct(fields1);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);
        Assert.AreNotEqual(t1, t4);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t4.GetHashCode());
    }

    /// <summary>
    /// Tests nested struct equals.
    /// </summary>
    [TestMethod]
    public void TestNestedStructEquals()
    {
        var fields1 = new List<IType> { TypeFactory.REQUIRED.I64, TypeFactory.NULLABLE.STR };
        var struct1 = TypeFactory.REQUIRED.Struct(fields1);
        var struct11 = TypeFactory.REQUIRED.Struct(fields1);
        var struct2 = TypeFactory.NULLABLE.Struct(fields1);

        var fields2 = new List<IType> { TypeFactory.NULLABLE.STR, struct1, TypeFactory.REQUIRED.I64 };
        var fields3 = new List<IType> { TypeFactory.NULLABLE.STR, struct11, TypeFactory.REQUIRED.I64 };
        var fields4 = new List<IType> { TypeFactory.NULLABLE.STR, struct2, TypeFactory.REQUIRED.I64 };

        var t1 = TypeFactory.REQUIRED.Struct(fields2);
        var t2 = TypeFactory.REQUIRED.Struct(fields3);
        var t3 = TypeFactory.REQUIRED.Struct(fields4);

        Assert.AreEqual(t1, t2);
        Assert.AreNotEqual(t1, t3);

        Assert.AreEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.AreNotEqual(t1.GetHashCode(), t3.GetHashCode());
    }

    /// <summary>
    /// Tests whether inverse nullable throws with unspecified nullable type.
    /// </summary>
    [TestMethod]
    public void TestInverseNullableThrowsWithUnspecified()
    {
        Assert.ThrowsException<NotImplementedException>(() => IType.NullableType.Unspecified.Inverse());
    }
}
