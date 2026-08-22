// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Extension.Functions;

namespace Substrait.Core.Expression;

/// <summary>
/// An immutable implementation of an enum argument, <see cref="Protobuf.FunctionArgument.Enum"/>.
/// </summary>
public sealed class EnumArgumentValue : IFunctionArg, IEquatable<EnumArgumentValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnumArgumentValue"/> class.
    /// </summary>
    /// <param name="argumentType">The enum argument type.</param>
    /// <param name="option">The selected enum option value.</param>
    public EnumArgumentValue(EnumArgument argumentType, string? option)
    {
        this.ArgumentType = argumentType;
        this.Option = option;
    }

    /// <summary>
    /// Gets the enum argument type.
    /// </summary>
    public EnumArgument ArgumentType { get; }

    /// <summary>
    /// Gets the selected enum option value.
    /// </summary>
    public string? Option { get; }

    /// <inheritdoc/>
    public bool Equals(EnumArgumentValue? other)
    {
        return this.ArgumentType == other?.ArgumentType && string.Equals(this.Option, other.Option);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as EnumArgumentValue);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(this.ArgumentType, this.Option);
    }
}
