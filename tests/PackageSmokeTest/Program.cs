// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Google.Protobuf;
using Substrait.Core.Type;
using Substrait.Protobuf;

Substrait.Protobuf.Type protoType = new()
{
    I64 = new Substrait.Protobuf.Type.Types.I64
    {
        Nullability = Substrait.Protobuf.Type.Types.Nullability.Required,
    },
};

byte[] serialized = protoType.ToByteArray();
Substrait.Protobuf.Type parsed = Substrait.Protobuf.Type.Parser.ParseFrom(serialized);

if (parsed.KindCase != Substrait.Protobuf.Type.KindOneofCase.I64 ||
    TypeFactory.REQUIRED.I64.Nullable != IType.NullableType.Required)
{
    return 1;
}

return 0;
