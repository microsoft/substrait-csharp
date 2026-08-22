// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Substrait.Tools;

/// <summary>
/// StringBuilder utilities for formatting.
/// </summary>
public static class StringBuilderUtils
{
    /// <summary>
    /// Characters of generic type suffixes.
    /// </summary>
    private static readonly char[] GENERICTYPESUFFIXCHARS = new[] { '`', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

    private static readonly Dictionary<Type, string> BUILTINTYPEKEYWORD = new Dictionary<Type, string>
    {
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(sbyte), "sbyte" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(object), "object" },
        { typeof(string), "string" },
    };

    /// <summary>
    /// Appends type name and also spells out the declaring types.
    /// </summary>
    /// <param name="buf">buffer.</param>
    /// <param name="type">type.</param>
    /// <param name="qualifyDeclaringTypes">when true, qualify all declaring types.</param>
    /// <param name="qualifyNamespace">when true, qualify namespace.</param>
    /// <returns>string builder.</returns>
    public static StringBuilder AppendTypeName(this StringBuilder buf, Type type, bool qualifyDeclaringTypes = true, bool qualifyNamespace = false)
    {
        if (qualifyNamespace && !IsBuiltinType(type) && type.Namespace is not null)
        {
            buf.Append(type.Namespace).Append('.');
        }

        if (!qualifyDeclaringTypes)
        {
            return buf.AppendSimpleTypeName(type, qualifyDeclaringTypes: false);
        }

        return AppendDeclaringTypeName(buf, type);
    }

    /// <summary>
    /// Trims ch from the end of the builder.
    /// </summary>
    /// <param name="buf">buffer.</param>
    /// <param name="ch">character to trim.</param>
    /// <returns>string builder.</returns>
    public static StringBuilder TrimEnd(this StringBuilder buf, char[] ch)
    {
        int toDelete = 0;
        for (int i = buf.Length - 1; i >= 0 && Array.IndexOf(ch, buf[i]) >= 0; --i)
        {
            ++toDelete;
        }

        buf.Length -= toDelete;
        return buf;
    }

    /// <summary>
    /// Checks whether the buffer ends with string.
    /// </summary>
    /// <param name="buf">buffer.</param>
    /// <param name="s">string to check.</param>
    /// <returns>true when the buffer ends with the pattern.</returns>
    public static bool EndsWith(this StringBuilder buf, string s)
    {
        if (buf.Length < s.Length)
        {
            return false;
        }

        for (int i = 1; i <= s.Length; ++i)
        {
            if (buf[^i] != s[^i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Append indentation.
    /// </summary>
    /// <param name="buf">builder.</param>
    /// <param name="indentChar">indentation char.</param>
    /// <param name="level">indentation level.</param>
    /// <returns>builder for chaining.</returns>
    public static StringBuilder Indent(this StringBuilder buf, IndentChar indentChar, int level)
    {
        return buf.Append(indentChar.Char, level * indentChar.Count);
    }

    private static bool IsBuiltinType(Type t)
    {
        return t.IsPrimitive || BUILTINTYPEKEYWORD.ContainsKey(t);
    }

    private static StringBuilder AppendSimpleTypeName(this StringBuilder buf, Type type, bool qualifyDeclaringTypes = true)
    {
        if (IsBuiltinType(type))
        {
            buf.Append(BUILTINTYPEKEYWORD[type]);
        }
        else
        {
            buf.Append(type.Name);
        }

        if (!type.IsGenericType)
        {
            return buf;
        }

        buf.TrimEnd(GENERICTYPESUFFIXCHARS);
        buf.Append('<');
        foreach (var genericArg in type.GenericTypeArguments)
        {
            buf.AppendTypeName(genericArg, qualifyDeclaringTypes).Append(',');
        }

        --buf.Length;
        buf.Append('>');
        return buf;
    }

    private static StringBuilder AppendDeclaringTypeName(this StringBuilder buf, Type type)
    {
        if (type.DeclaringType is not null)
        {
            AppendDeclaringTypeName(buf, type.DeclaringType);
            buf.Append('.');
        }

        return buf.AppendSimpleTypeName(type, qualifyDeclaringTypes: true);
    }

    /// <summary>
    /// Indentation Char.
    /// </summary>
    /// <param name="ch">character to use.</param>
    /// <param name="count">number of repeates.</param>
    public readonly struct IndentChar(char ch = ' ', int count = 4)
    {
        /// <summary>
        /// Default value.
        /// </summary>
        public static readonly IndentChar Default = new(' ', 4);

        /// <summary>
        /// Gets the characters to use for indentation.
        /// </summary>
        public char Char { get; } = ch;

        /// <summary>
        /// Gets the number of Char to repeat per indentation.
        /// </summary>
        public int Count { get; } = count;
    }
}
