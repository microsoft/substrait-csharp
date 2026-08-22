// Copyright (c) Microsoft Corporation
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Immutable;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Substrait.Core.Expression;
using Substrait.Core.Extension.Functions;
using Substrait.Core.Relation;
using Substrait.Core.Type;
using Substrait.Tools.Visitor;
using static Substrait.Core.Expression.Expression;
using static Substrait.Core.Expression.Literal;

namespace Substrait.Tests.Core.Expression;

/// <summary>
/// Tests for expression visitors.
/// </summary>
[TestClass]
public class ExpressionVisitorTests
{
    private readonly ExpressionTopDownDispatcher<StringBuilderContext, VoidOutput> topDownDispatcher;
    private readonly ExpressionBottomUpDispatcher<StringBuilderContext, VoidOutput> bottomUpDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionVisitorTests"/> class.
    /// </summary>
    public ExpressionVisitorTests()
    {
        this.topDownDispatcher = new ExpressionTopDownDispatcher<StringBuilderContext, VoidOutput>(new ExpressionPrinter());
        this.bottomUpDispatcher = new ExpressionBottomUpDispatcher<StringBuilderContext, VoidOutput>(new ExpressionPrinter());
    }

    /// <summary>
    /// Verifies that all sealed classes that implement IExpression have a corresponding Visit method in ExpressionVisitor.
    /// </summary>
    [TestMethod]
    public void TestExpressionVisitorContainsAllSealedClasses()
    {
        // Get all types that implement IExpression
        var iexpressionTypes = Assembly.GetAssembly(typeof(IExpression))!
            .GetTypes()
            .Where(t => typeof(IExpression).IsAssignableFrom(t) && t.IsClass && t.IsSealed && t.Namespace == "Substrait.Core.Expression")
            .ToList();

        // Get all methods in ExpressionVisitor
        var expressionVisitorMethods = typeof(ExpressionVisitor<,>).GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name == "Visit" && m.GetParameters().Length == 2)
            .Select(m => m.GetParameters()[0].ParameterType)
            .ToList();

        // Check if all IExpression types have corresponding Visit methods in ExpressionVisitor
        foreach (var type in iexpressionTypes)
        {
            Assert.IsTrue(expressionVisitorMethods.Contains(type), $"ExpressionVisitor does not contain a Visit method for {type.Name}");
        }
    }

    /// <summary>
    /// Tests the expression visitor with a cast expression.
    /// </summary>
    [TestMethod]
    public void TestCastExpression()
    {
        var castExpr = new Cast(TypeFactory.REQUIRED.I64, new StrLiteral("123"), Cast.FailureBehavior.ThrowException);
        this.CheckTopDownTraversal(castExpr, "Cast|StrLiteral|");
    }

    /// <summary>
    /// Tests the expression visitor with a scalar function invocation.
    /// </summary>
    [TestMethod]
    public void TestScalarFunctionInvocation()
    {
        var scalarFunctionImpl = new ScalarFunctionImpl("urix", "fname1", "fdesc", FunctionImpl.NullabilityMode.Mirror, new[] { new ValueArgument("i64", "vname", "vdesc", true) }, ImmutableDictionary<string, IOption>.Empty, null, null, "i64");
        var scalarFunctionInvocation = new ScalarFunctionInvocation(scalarFunctionImpl.Uri, scalarFunctionImpl.Key, ImmutableList.Create(new I64Literal(1)), TypeFactory.REQUIRED.I64, scalarFunctionImpl);
        this.CheckTopDownTraversal(scalarFunctionInvocation, "ScalarFunctionInvocation|I64Literal|");
    }

    /// <summary>
    /// Tests the expression visitor with an if-then expression.
    /// </summary>
    [TestMethod]
    public void TestIfThenExpression()
    {
        (IExpression, IExpression) ifClause = (new BoolLiteral(true), new I64Literal(1));
        var ifThenExpr = new IfThen(new[] { ifClause }.Cast<(IExpression Condition, IExpression Then)>(), new I64Literal(1234));
        this.CheckTopDownTraversal(ifThenExpr, "IfThen|BoolLiteral|I64Literal|I64Literal|");
    }

    /// <summary>
    /// Tests the expression visitor with a field reference.
    /// </summary>
    [TestMethod]
    public void TestFieldReference()
    {
        var fieldRef = new FieldReference(TypeFactory.REQUIRED.I64, 1, 2);
        this.CheckTopDownTraversal(fieldRef, "FieldReference|");
    }

    /// <summary>
    /// Tests the expression visitor with various literals.
    /// </summary>
    [TestMethod]
    public void TestLiterals()
    {
        var boolLiteral = new BoolLiteral(true);
        this.CheckTopDownTraversal(boolLiteral, "BoolLiteral|");

        var i64Literal = new I64Literal(123);
        this.CheckTopDownTraversal(i64Literal, "I64Literal|");

        var strLiteral = new StrLiteral("test");
        this.CheckTopDownTraversal(strLiteral, "StrLiteral|");
    }

    /// <summary>
    /// Tests the expression visitor with a struct expression.
    /// </summary>
    [TestMethod]
    public void TestStructExpression()
    {
        var structExpr = new Struct(new IExpression[]
        {
            new BoolLiteral(true),
            new I64Literal(123),
            new StrLiteral("test"),
        });

        string topDownExpected = "Struct|BoolLiteral|I64Literal|StrLiteral|";
        string bottomUpExpected = "BoolLiteral|I64Literal|StrLiteral|Struct|";

        this.CheckTopDownTraversal(structExpr, topDownExpected);
        this.CheckBottomUpTraversal(structExpr, bottomUpExpected);
    }

    /// <summary>
    /// Tests the expression visitor with a set predicate subquery expression.
    /// </summary>
    [TestMethod]
    public void TestSetPredicateSubquery()
    {
        var setPredicateExpr = new SetPredicateSubquery(
            new VirtualTableRead(new NamedStruct(["a"], TypeFactory.REQUIRED.Struct([TypeFactory.REQUIRED.I64])), [new Struct([new I64Literal(456)])], filter: null),
            SetPredicateSubquery.PredicateOp.Exists);

        string topDownExpected = "SetPredicateSubquery|";
        string bottomUpExpected = "SetPredicateSubquery|";

        this.CheckTopDownTraversal(setPredicateExpr, topDownExpected);
        this.CheckBottomUpTraversal(setPredicateExpr, bottomUpExpected);
    }

    /// <summary>
    /// Tests the expression visitor with a set comparison subquery expression.
    /// </summary>
    [TestMethod]
    public void TestSetComparisonSubquery()
    {
        var setComparisonExpr = new SetComparisonSubquery(
            new I64Literal(123),
            SetComparisonSubquery.ComparisonOp.Equal,
            SetComparisonSubquery.ReductionOp.All,
            new VirtualTableRead(new NamedStruct(["a"], TypeFactory.REQUIRED.Struct([TypeFactory.REQUIRED.I64])), [new Struct([new I64Literal(456)])], filter: null));

        string topDownExpected = "SetComparisonSubquery|I64Literal|";
        string bottomUpExpected = "I64Literal|SetComparisonSubquery|";

        this.CheckTopDownTraversal(setComparisonExpr, topDownExpected);
        this.CheckBottomUpTraversal(setComparisonExpr, bottomUpExpected);
    }

    private void CheckTopDownTraversal(IExpression expr, string expected)
    {
        StringBuilderContext context = new StringBuilderContext();
        this.topDownDispatcher.Dispatch(expr, context);
        Assert.AreEqual(expected, context.ToString());
    }

    private void CheckBottomUpTraversal(IExpression expr, string expected)
    {
        StringBuilderContext context = new StringBuilderContext();
        this.bottomUpDispatcher.Dispatch(expr, context);
        Assert.AreEqual(expected, context.ToString());
    }

    private sealed class StringBuilderContext : NoOpContext<IExpression, VoidOutput>
    {
        private readonly StringBuilder builder = new StringBuilder();

        public void Append(string value) => this.builder.Append(value).Append('|');

        public override string ToString() => this.builder.ToString();
    }

    private sealed class ExpressionPrinter : DefaultExpressionVisitor<StringBuilderContext, VoidOutput>
    {
        public override VoidOutput Visit(IExpression other, StringBuilderContext context)
        {
            throw new NotSupportedException($"Unable to print expression {other.GetType().Name}");
        }

        protected override VoidOutput DefaultVisit(IExpression expr, StringBuilderContext context)
        {
            context.Append(expr.GetType().Name);
            return VoidOutput.Instance;
        }
    }
}
