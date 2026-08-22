// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Tools;
using static Substrait.Core.Relation.AbstractJoin;
using static Substrait.Core.Relation.Set;
using ProtoHashJoinType = Substrait.Protobuf.HashJoinRel.Types.JoinType;
using ProtoJoinType = Substrait.Protobuf.JoinRel.Types.JoinType;
using ProtoSetOp = Substrait.Protobuf.SetRel.Types.SetOp;
using ProtoSimpleComparisonType = Substrait.Protobuf.ComparisonJoinKey.Types.SimpleComparisonType;

namespace Substrait.Core.Relation.Converters;

/// <summary>
/// Conversion methods for relation enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Converts a join type to protobuf.
    /// </summary>
    /// <param name="op">The join type.</param>
    /// <returns>The protobuf join type.</returns>
    public static ProtoJoinType ToProto(this JoinType op)
    {
        if (op == JoinType.Unspecified)
        {
            throw new ArgumentException($"The JoinType '{op}' is not supported.", nameof(op));
        }

        return EnumUtils.Cast<JoinType, ProtoJoinType>(op);
    }

    /// <summary>
    /// Converts a join type from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf join type.</param>
    /// <returns>The internal join type.</returns>
    public static JoinType FromProto(this ProtoJoinType proto)
    {
        if (proto == ProtoJoinType.Unspecified)
        {
            throw new ArgumentException($"The ProtoJoinType '{proto}' is not supported.", nameof(proto));
        }

        return EnumUtils.Cast<ProtoJoinType, JoinType>(proto);
    }

    /// <summary>
    /// Converts a hash join type from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf hash join type.</param>
    /// <returns>The internal join type.</returns>
    public static JoinType FromProto(this ProtoHashJoinType proto)
    {
        if (proto != ProtoHashJoinType.Unspecified && Enum.TryParse(proto.ToString(), out JoinType joinType))
        {
            return joinType;
        }

        throw new ArgumentException($"The ProtoHashJoinType '{proto}' is not supported.", nameof(proto));
    }

    /// <summary>
    /// Converts a join type to its hash join protobuf representation.
    /// </summary>
    /// <param name="joinType">The join type.</param>
    /// <returns>The protobuf hash join type.</returns>
    public static ProtoHashJoinType ToHashJoinProto(this JoinType joinType)
    {
        if (joinType != JoinType.Unspecified && Enum.TryParse(joinType.ToString(), out ProtoHashJoinType hashJoinType))
        {
            return hashJoinType;
        }

        throw new ArgumentException($"The JoinType '{joinType}' is not supported.", nameof(joinType));
    }

    /// <summary>
    /// Converts a simple comparison type to protobuf.
    /// </summary>
    /// <param name="type">The comparison type.</param>
    /// <returns>The protobuf comparison type.</returns>
    public static ProtoSimpleComparisonType ToProto(this PhysicalJoin.ComparisonJoinKey.SimpleComparisonType type)
    {
        if (type == PhysicalJoin.ComparisonJoinKey.SimpleComparisonType.Unspecified)
        {
            throw new ArgumentException($"The SimpleComparisonType '{type}' is not supported.", nameof(type));
        }

        return EnumUtils.Cast<PhysicalJoin.ComparisonJoinKey.SimpleComparisonType, ProtoSimpleComparisonType>(type);
    }

    /// <summary>
    /// Converts a simple comparison type from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf comparison type.</param>
    /// <returns>The internal comparison type.</returns>
    public static PhysicalJoin.ComparisonJoinKey.SimpleComparisonType FromProto(this ProtoSimpleComparisonType proto)
    {
        if (proto == ProtoSimpleComparisonType.Unspecified)
        {
            throw new ArgumentException($"The ProtoSimpleComparisonType '{proto}' is not supported.", nameof(proto));
        }

        return EnumUtils.Cast<ProtoSimpleComparisonType, PhysicalJoin.ComparisonJoinKey.SimpleComparisonType>(proto);
    }

    /// <summary>
    /// Converts a set operation to protobuf.
    /// </summary>
    /// <param name="op">The set operation.</param>
    /// <returns>The protobuf set operation.</returns>
    public static ProtoSetOp ToProto(this SetOp op)
    {
        if (op == SetOp.Unspecified)
        {
            throw new ArgumentException($"The SetOpType '{op}' is not supported.", nameof(op));
        }

        return EnumUtils.Cast<SetOp, ProtoSetOp>(op);
    }

    /// <summary>
    /// Converts a set operation from protobuf.
    /// </summary>
    /// <param name="proto">The protobuf set operation.</param>
    /// <returns>The internal set operation.</returns>
    public static SetOp FromProto(this ProtoSetOp proto)
    {
        if (proto == ProtoSetOp.Unspecified)
        {
            throw new ArgumentException($"The ProtoSetOp '{proto}' is not supported.", nameof(proto));
        }

        return EnumUtils.Cast<ProtoSetOp, SetOp>(proto);
    }
}
