<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Release notes

### 0.3.0

- Update dependencies specified by the NuGet package. This allows NuGet to manage shared dependencies between Axe.Windows and applications which consume it.
- Change `Axe.Windows.Automation.ScanResults.OutputFile` from a `Tuple` to a `struct`. This allows Axe.Windows.Automation to work with .Net runtime versions which do not support `Tuple`.
