# Registering regression fixtures

For a new parameterless `ITestX` fixture in `FastExpressionCompiler.UnitTests`,
add `[RunTestX]` beside its declaration:

```csharp
[RunTestX]
public struct MyRegressionTests : ITestX
{
    public void Run(TestRun tests) { /* call the regression methods */ }
}
```

No changes to either runner's `Program.cs` are needed. The shared test source is
compiled for Standard and LightExpression, and both the modern and Framework
runners call a generated registration method for each expression API. A test-only
Roslyn incremental source generator emits `tests.Run(new MyRegressionTests())`
calls from the marked types at build time. Registration uses no runtime type scan
or reflection-based construction.
Use the fixture's existing `#if` guards for API- or framework-specific code.
Each flavor runs marked fixtures once, in ordinal full-type-name order.

Generation is opt-in. Unmarked tests, legacy `Run()` fixtures, issue-test lists,
and the early runtime-accessor contracts retain their existing entry points.
Do not also register a marked fixture manually. A marked fixture must implement
`ITestX` and have a public parameterless constructor (implicit for structs).

Keeping registration in the fixture file lets separate regression PRs add their
own files without editing shared registration lists. Existing PRs that still add
manual registrations need a one-time migration after this change is merged.

The generator targets `netstandard2.0` and is referenced as an analyzer by both
unit-test projects. Its Roslyn dependency is build-only. The SDK compiler runs it
for each target, including `net472`; ordinary C# calls execute on that runtime.
Generated source is available below each unit-test project's
`obj/<Configuration>/<TargetFramework>/generated` directory and is not committed.
The public partial registration method requires a generated implementation, so
an unavailable generator fails compilation instead of silently skipping tests.
