// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Tools;

namespace Substrait.Tests.Tools;

[TestClass]
public sealed class ObjectUtilsTests
{
    private static readonly int[] Values = [1, 2, 3];
    private static readonly int[] ReorderedValues = [3, 2, 1];

    [TestMethod]
    public void CombineHashCodesIsDeterministicAndOrderSensitive()
    {
        int hash = Values.CombineHashCodes();

        Assert.AreEqual(hash, Values.CombineHashCodes());
        Assert.AreNotEqual(hash, ReorderedValues.CombineHashCodes());
    }
}
