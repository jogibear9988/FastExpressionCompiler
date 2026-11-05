using System;
#if LIGHT_EXPRESSION
namespace FastExpressionCompiler.LightExpression.UnitTests;
#else
namespace FastExpressionCompiler.UnitTests;
#endif

/// <summary>Opt a fixture into compile-time generation of typed test calls.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class RunTestXAttribute : Attribute { }

public static partial class RegisteredTestX
{
    // A missing generator must fail the build, rather than silently omitting tests.
    public static partial void Run(TestRun tests);
}
