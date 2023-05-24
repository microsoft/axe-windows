<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Debugging tips

### Skip the intermediate build
If you are working on a client that consumes Axe.Windows (for example, Accessibility Insights for Windows), you may encounter situations where the inner dev loop is
- Build Axe.Windows
- Publish the `.nupkg` file
- Consume the `.nupkg` file
- Build the client
- Run the client/evaluate results
- Repeat

This loop will be needed for changes that modify the interface, but since most cases do _not_ modify the interface, a simple script can often streamline the process:
```
xcopy /a "$(AxeWindowsRoot)\src\CI\bin\release\axe.windows\*.dll" "$(YourClientDropFolder)"
```

You'll still need to build your project the first time, then your inner dev loop simplifies to
- Build Axe.Windows
- Run the script
- Run the client/evaluate results
- Repeat

