// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Extension;
using Substrait.Tools;

namespace Substrait.Tests.Tools;

[TestClass]
public sealed class ExtensionUtilsTests
{
    [TestMethod]
    public void LoadDefaultsLoadsEmbeddedExtensions()
    {
        ExtensionsCollection extensions = ExtensionUtils.LoadDefaults();

        Assert.IsTrue(extensions.ScalarFunctionImpls.Count > 0);
        Assert.IsTrue(extensions.AggregateFunctionImpls.Count > 0);
        Assert.IsTrue(extensions.WindowFunctionImpls.Count > 0);
        Assert.IsTrue(extensions.TypeVariationImpls.Count > 0);
    }

    [TestMethod]
    public void FileSystemResolverLoadsExtensionFile()
    {
        const string yaml = "scalar_functions:\n  - name: identity\n    impls:\n      - args:\n          - value: i64\n            name: value\n        return: i64\n";
        string path = Path.GetTempFileName();

        try
        {
            File.WriteAllText(path, yaml, Encoding.UTF8);
            ExtensionsCollection extensions = ExtensionUtils.Load(
                [new ExtensionUtils.ExtensionFile("/test.yaml", path)],
                new ExtensionUtils.FileSystemResolver());

            Assert.AreEqual(1, extensions.ScalarFunctionImpls.Count);
            Assert.AreEqual("identity:i64", extensions.ScalarFunctionImpls[0].Key);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
