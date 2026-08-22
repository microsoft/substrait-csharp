// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using Substrait.Protobuf;

namespace Substrait.Tools;

/// <summary>
/// Utility methods for reading and writing protobuf plans.
/// </summary>
public static class FileUtils
{
    /// <summary>
    /// Supported plan file formats.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// Protobuf binary format.
        /// </summary>
        Protobuf,

        /// <summary>
        /// Protobuf JSON format.
        /// </summary>
        Json,
    }

    /// <summary>
    /// Reads a plan from a file.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="fileType">The file format.</param>
    /// <returns>The parsed plan.</returns>
    public static Plan FetchPlan(string filePath, FileType fileType)
    {
        return fileType switch
        {
            FileType.Protobuf => FetchProtobufPlan(filePath),
            FileType.Json => FetchJsonPlan(filePath),
            _ => throw new ArgumentException("Invalid file type", nameof(fileType)),
        };
    }

    /// <summary>
    /// Writes a plan to a file, creating its parent directory when needed.
    /// </summary>
    /// <param name="plan">The plan to write.</param>
    /// <param name="filePath">The file path.</param>
    /// <param name="fileType">The file format.</param>
    public static void WritePlan(Plan plan, string filePath, FileType fileType)
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        switch (fileType)
        {
            case FileType.Protobuf:
                WriteProtobufPlan(plan, filePath);
                break;
            case FileType.Json:
                WriteJsonPlan(plan, filePath);
                break;
            default:
                throw new ArgumentException("Invalid file type", nameof(fileType));
        }
    }

    private static Plan FetchProtobufPlan(string filePath)
    {
        using FileStream file = File.OpenRead(filePath);
        return Plan.Parser.ParseFrom(file);
    }

    private static Plan FetchJsonPlan(string filePath)
    {
        using StreamReader reader = new(filePath);
        return JsonParser.Default.Parse<Plan>(reader);
    }

    private static void WriteProtobufPlan(Plan plan, string filePath)
    {
        using FileStream file = File.Create(filePath);
        plan.WriteTo(file);
    }

    private static void WriteJsonPlan(Plan plan, string filePath)
    {
        JsonFormatter formatter = new(JsonFormatter.Settings.Default.WithFormatDefaultValues(true));
        using StreamWriter writer = new(filePath);
        formatter.Format(plan, writer);
    }
}
