# NetCoreUtils

%%{init: {'theme': 'base', 'themeVariables': { 'textColor': 'white'}}}%%
```mermaid
pie
    title Key elements in Product X
    "Calcium" : 42.96
    "Potassium" : 50.05
    "Magnesium" : 10.01
    "Iron" :  5
```

%%{init: {'theme': 'base', 'themeVariables': { 'textColor': 'white'}}}%%

```mermaid
sequenceDiagram
    Alice ->> Bob: Hello Bob, how are you?
    Bob-->>John: How about you John?
    Bob--x Alice: I am good thanks!
    Bob-x John: I am good thanks!
    Note right of John: Bob thinks a long<br/>long time, so long<br/>that the text does<br/>not fit on a row.

    Bob-->Alice: Checking with John...
    Alice->John: Yes... John, how are you?
```

## NuGet

https://www.nuget.org/packages/NetCoreUtils/

## Release Notes

### v1.3.0 (working)

**Enhancement**:

- Add JsonConfigOperator
- Add ProcUtil
- Add SystemUtil
- Update SafeCall.Execute() to use Console.Error.WriteLine()

**Breaking Change**:

- Rename ShellExecutor to TerminalUtil
- Move namespaces Expression and Reflection to Lang

### v1.2.0

**Enhancement**:

- Add ExpressionUtil
- Add NetCoreUtils.Text.Table
- Add NetCoreUtils.Text.Indent

**Breaking Change**:

- Move String, Xml to Text namespace

### v1.1.0.6

- Add MethodCall.Pipeline

### v1.0.0.5

*Breaking Change*:

- Upgrade to .net core 3.1
- Add ReflectUtil

### v0.3.0.4

*Enhancement*:

- Add PipeForwardExt

*Breaking Change*:

- Upgrade to .net core 2.2
- Rename namespace "Misc" to "MethodCall"

### v0.2.0.3

*Enhancement*:

- Add Logger
- Add SafeCaller

*Breaking Change*:

- Rename namespace "Diagnose" to "Diagnosis"
