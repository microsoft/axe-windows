<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Building the code

### 1. Clone the repository
- Clone the repository using one of the following commands
  ``` bash
  git clone https://github.com/Microsoft/axe-windows.git
  ```
  or
  ``` bash
  git clone git@github.com:Microsoft/axe-windows.git
  ```
- Select the created directory
  ``` bash
  cd axe-windows
  ```

### 2. Open the solution in Visual Studio
- Use the `src/AxeWindows.sln` file to open the solution.

### 3. Build and run unit tests

For details about how the code is organized, please visit the [solution overview](./solution.md).

If you need more information on how to set up your development environment, please visit the [development environment setup page](./SetUpDevEnv.md).

#### Building the NuGet package

Because the default dev environment does not include `nuget.exe`, the default behavior of the `CI` project is to skip packaging the files into the `.nupkg` file. Please follow these steps if you need to build a local copy of the NuGet package.

- Install `nuget.exe` from https://www.nuget.org/downloads and ensure that its location is included in your `PATH` environment variable. We currently use `nuget.exe` version 5.X in our build pipelines, but we reserve the right to move to a different version at any point in time.
- Add `CreateAxeWindowsNugetPackage=true` to your environment variables. This variable will be evaluated when the `CI` project is built.

## Testing

We use the unit test framework from Visual Studio. Find more information in our [FAQ section](./FAQ.md).
