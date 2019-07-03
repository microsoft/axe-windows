<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Test requirements
* All new code *must* have unit tests. Note: there may be some situations where it is not possible to add unit tests, but these are infrequent.
* Unit tests should accompany the code in the pull request. This provides implicit documentation of the expected behavior, and ensures that the behavior is being satisfied. In an exceptional circumstance, a PR might be approved without its unit tests, but that is not the normal expectation.
* All unit tests should be passing locally before the PR is created.
* Unit tests must conform to the standards listed below.

## Standards
* The test project is appropriately named (tests for the `Foo.Bar.dll` are in `Foo.BarTests.dll`)
* The test namespace is appropriately named (the top level namespace matches the assembly name, then the hierarchy in the assembly is preserved. So if `Foo.Bar.dll` contains a namespace named `Widget`, the tests will be in the `Foo.BarTests.Widget` namespace)
* The test class is appropriately named (tests for the `ExplodingWidget` class will be in the `ExplodingWidgetUnitTests` class)
* Each test follows the naming conventions documented at [Unit Test Basics](https://msdn.microsoft.com/en-us/library/hh694602.aspx) Summarizing:
  * Name is in the general form `MethodName_TestCase_ExpectedBehavior`
  * If the test is expected to throw an `Exception`, the test should declare this via the `ExpectedException` attribute. If the test needs to check the contents of the `Exception`, the best pattern is to catch the expected `Exception` type, validate it, then re-throw the `Exception`
* Each test uses appropriate `Assert` methods to validate its conditions--use the version with a hint where it seems helpful
* Each test specifies a reasonable `Timeout` value (default to 1 second each). The test output is much more readable if the test times out and fails than if the entire build loop hangs and reaches the 60 minute time constraint
  * One downside to the `Timeout` attribute--because the `Timeout` applies while debugging the unit test, it may need to be commented out to make debugging feasible.
* Mocks decouple the test class from the underlying dependencies (Dependency Injection is a common pattern for this). This is required for new classes, best effort for changes to existing classes
* `Microsoft Fakes` may not be used for any new tests because not all editions of Visual Studio support them.
* If Moq is used, mocks should ideally use `MockBehavior.Strict`--this isn't always possible, but using it makes for a much stronger test
* If Moq is used, each Mock's `VerifyAll` should be called at the end of the test, to ensure that all configured methods were exercised as expected. Where both `MockBehavior.Strict` and `VerifyAll`, the resulting test will exactly pin down the interaction with dependency objects
* Any tests which operate on live applications (whether launched by the test or not) must use the     `[TestCategory(TestCategory.Integration)]` attribute on either the test method, or in the case where the entire class contains such tests, the test class. This makes it easy to exclude such tests when running unit tests on one's local machine. Hint: to ignore these tests, type "-trait:integration" into the search field of the Test Explorer in Visual Studio.

