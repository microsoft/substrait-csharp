// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using Google.Protobuf.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Extension;
using Substrait.Protobuf;
using Substrait.Tools;

namespace Substrait.Tests.Tools;

[TestClass]
public sealed class SerializationSupportTests
{
    private static readonly string[] ExpectedExtensionUris = ["functions.yaml", "types.yaml"];
    private static readonly int[] ExpectedSingleRepeatedValue = [1];
    private static readonly int[] ExpectedRepeatedValues = [1, 2, 3];

    [TestMethod]
    public void FileUtilsPreservesBinaryAndJsonPlanSemantics()
    {
        Plan expected = new()
        {
            Version = new Substrait.Protobuf.Version
            {
                MajorNumber = 0,
                MinorNumber = 73,
                PatchNumber = 0,
                Producer = "serialization-support-tests",
            },
        };
        string directory = Path.Combine(Path.GetTempPath(), nameof(SerializationSupportTests), Guid.NewGuid().ToString("N"));
        string binaryPath = Path.Combine(directory, "plan.pb");
        string jsonPath = Path.Combine(directory, "plan.json");

        try
        {
            FileUtils.WritePlan(expected, binaryPath, FileUtils.FileType.Protobuf);
            FileUtils.WritePlan(expected, jsonPath, FileUtils.FileType.Json);

            CollectionAssert.AreEqual(expected.ToByteArray(), File.ReadAllBytes(binaryPath));
            Assert.AreEqual(expected, FileUtils.FetchPlan(binaryPath, FileUtils.FileType.Protobuf));
            Assert.AreEqual(expected, FileUtils.FetchPlan(jsonPath, FileUtils.FileType.Json));
        }
        finally
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }
        }
    }

    [TestMethod]
    public void ExtensionsCollectorAssignsStableAnchors()
    {
        ExtensionsCollector.Builder builder = new();

        Assert.AreEqual(0, builder.Collect(ExtensionsCollector.ExtensionType.Function, "functions.yaml", "add"));
        Assert.AreEqual(0, builder.Collect(ExtensionsCollector.ExtensionType.Function, "functions.yaml", "add"));
        Assert.AreEqual(1, builder.Collect(ExtensionsCollector.ExtensionType.Function, "functions.yaml", "subtract"));
        Assert.AreEqual(1, builder.Collect(ExtensionsCollector.ExtensionType.TypeVariation, "types.yaml", "unsigned"));

        ExtensionsCollector collector = builder.Build();
        CollectionAssert.AreEqual(ExpectedExtensionUris, collector.ExtensionUris.ToArray());
        Assert.AreEqual(3, collector.Extensions.Count);
    }

    [TestMethod]
    public void AllocateAndAddRangePreservesExistingValues()
    {
        RepeatedField<int> values = [1];

        values.AllocateAndAddRange(2, [2, 3]);

        CollectionAssert.AreEqual(ExpectedRepeatedValues, values.ToArray());
        Assert.IsTrue(values.Capacity >= 3);
    }

    [TestMethod]
    public void AllocateAndAddRangeRejectsNegativeCount()
    {
        RepeatedField<int> values = [1];

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => values.AllocateAndAddRange(-1, []));
        CollectionAssert.AreEqual(ExpectedSingleRepeatedValue, values.ToArray());
    }

    [TestMethod]
    public void UniqueListUnsupportedMutationsThrowNotSupportedException()
    {
        UniqueList<int> values = [1];

        Assert.ThrowsException<NotSupportedException>(() => ((IList<int>)values)[0] = 2);
        Assert.ThrowsException<NotSupportedException>(() => ((IList<int>)values).Insert(0, 2));
        Assert.ThrowsException<NotSupportedException>(() => ((IList<int>)values).RemoveAt(0));
        Assert.ThrowsException<NotSupportedException>(() => ((IList<int>)values).Remove(1));
    }
}
