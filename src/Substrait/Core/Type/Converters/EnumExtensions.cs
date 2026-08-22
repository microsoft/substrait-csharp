// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Tools;
using static Substrait.Core.Type.IType;
using ProtoNullability = Substrait.Protobuf.Type.Types.Nullability;

namespace Substrait.Core.Type.Converters;

/// <summary>
/// Extension methods for type enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>Converts nullability to protobuf.</summary>
    public static ProtoNullability ToProto(this NullableType nullable)
    {
        return EnumUtils.Cast<NullableType, ProtoNullability>(nullable);
    }

    /// <summary>Converts nullability from protobuf.</summary>
    public static NullableType FromProto(this ProtoNullability nullable)
    {
        return EnumUtils.Cast<ProtoNullability, NullableType>(nullable);
    }
}
