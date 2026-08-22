// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Plan;
using Substrait.Core.Relation;
using Substrait.Core.Type;

namespace Substrait.Tests.Core;

[TestClass]
public sealed class PlanTests
{
    [TestMethod]
    public void CurrentVersionMatchesPinnedSubstraitRevision()
    {
        Assert.AreEqual(0U, Substrait.Core.Plan.Version.Current.MajorNumber);
        Assert.AreEqual(73U, Substrait.Core.Plan.Version.Current.MinorNumber);
        Assert.AreEqual(0U, Substrait.Core.Plan.Version.Current.PatchNumber);
        Assert.AreEqual("d430e521f203aec6a4e06731d4bfd68cdf61f443", Substrait.Core.Plan.Version.Current.GitHash);
    }

    [TestMethod]
    public void EquivalentPlansHaveEqualRootsAndVersions()
    {
        Plan first = CreatePlan();
        Plan equivalent = CreatePlan();

        Assert.AreEqual(first, equivalent);
        Assert.AreEqual(first.GetHashCode(), equivalent.GetHashCode());
    }

    private static Plan CreatePlan()
    {
        NamedStruct schema = new(["value"], TypeFactory.REQUIRED.Struct([TypeFactory.REQUIRED.I64]));
        NamedTableRead read = new(schema, ["orders"], filter: null);
        Plan.Root root = new(read, ["value"]);
        return new Plan([root], Substrait.Core.Plan.Version.Current);
    }
}
