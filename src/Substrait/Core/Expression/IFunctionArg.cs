// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using Substrait.Core.Extension.Functions;
using Substrait.Core.Type;

namespace Substrait.Core.Expression;

/// <summary>
/// IFuntionArg is the interface an argument of a <see cref="FunctionImpl"/> invocation.
/// Subtypes are <see cref="IExpression"/> and <see cref="IType"/>.
/// </summary>
public interface IFunctionArg
{
}
