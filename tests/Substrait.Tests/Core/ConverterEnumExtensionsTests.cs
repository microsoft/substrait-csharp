// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Substrait.Core.Expression.AggregateFunctionInvocation;
using static Substrait.Core.Relation.AbstractJoin;
using static Substrait.Core.Relation.Set;
using ExpressionEnums = Substrait.Core.Expression.Converters.EnumExtensions;
using ProtoAggregationPhase = Substrait.Protobuf.AggregationPhase;
using ProtoHashJoinType = Substrait.Protobuf.HashJoinRel.Types.JoinType;
using ProtoJoinType = Substrait.Protobuf.JoinRel.Types.JoinType;
using ProtoSetOp = Substrait.Protobuf.SetRel.Types.SetOp;
using RelationEnums = Substrait.Core.Relation.Converters.EnumExtensions;

namespace Substrait.Tests.Core;

[TestClass]
public sealed class ConverterEnumExtensionsTests
{
    [TestMethod]
    public void ExpressionEnumsRoundTripByValue()
    {
        ProtoAggregationPhase proto = ExpressionEnums.ToProto(AggregationPhase.IntermediateToResult);

        Assert.AreEqual(ProtoAggregationPhase.IntermediateToResult, proto);
        Assert.AreEqual(AggregationPhase.IntermediateToResult, ExpressionEnums.FromProto(proto));
    }

    [TestMethod]
    public void RelationEnumsRoundTripByValueAndName()
    {
        ProtoJoinType joinProto = RelationEnums.ToProto(JoinType.Left);
        ProtoHashJoinType hashJoinProto = RelationEnums.ToHashJoinProto(JoinType.Left);
        ProtoSetOp setProto = RelationEnums.ToProto(SetOp.UnionDistinct);

        Assert.AreEqual(JoinType.Left, RelationEnums.FromProto(joinProto));
        Assert.AreEqual(JoinType.Left, RelationEnums.FromProto(hashJoinProto));
        Assert.AreEqual(SetOp.UnionDistinct, RelationEnums.FromProto(setProto));
    }

    [TestMethod]
    public void RelationEnumsRejectUnspecifiedValues()
    {
        Assert.ThrowsException<ArgumentException>(() => RelationEnums.FromProto(ProtoJoinType.Unspecified));
        Assert.ThrowsException<ArgumentException>(() => RelationEnums.FromProto(ProtoHashJoinType.Unspecified));
        Assert.ThrowsException<ArgumentException>(() => RelationEnums.FromProto(ProtoSetOp.Unspecified));
    }
}
