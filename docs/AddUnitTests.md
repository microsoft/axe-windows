<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Unit testing

We use the [MSTest framework](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting?view=mstest-net-1.2.0) to write unit tests.

### Unit test setup
Setting up unit tests for an assembly is trivial. 
The steps are:
1. Use Visual Studio to **add a new Unit Test Project** (available as a new project template listed under Visual C#, then Test)
   1. In Visual Studio, right-click on the solution and add a new project.
   2. Select unit tests from the template list.
   3. Name your project consistently with the existing test projects. If your project is Widgets.Stuff, then your test project should be named *Widgets.Stuff**Tests***.
   4. Finish setting up the project by following instructions in the [Readme.md](../src/Readme.md)

2. **Add a unit test class** to the project. Name the class after the class it will be testing--if the class is named *ExplodingWidget*, then the test class should be named *ExplodingWidget**Tests***
    1. In Visual Studio, right-click on the test project and add a new test class.
    2. Rename the test class from UnitTests to the name that matches your class name. So if your class is SuperList, your unit test class should be SuperListTests.

>Hint: if you right-click on a method that you need to test, you'll get a template that does this for you automatically. The only thing it doesn't do is to name the actual test according to what's described in Standards

For each test, add a method like this:

```C#
[TestMethod]
[Timeout (1000)]
public void DefaultDelay_IsFiveSeconds()
{
    ExplodingWidget widget = new ExplodingWidget();
    Assert.AreEqual(TimeSpan.FromSeconds(5), widget.Delay);
}
```

The required portions in the above example are:
1. `TestMethod` attribute.
2. The test method be `public void`.


When you build, this test will be built and made available for discovery. As long as `widget.Delay` returns a `TimeSpan` object that corresponds to 5 seconds, the test will pass. If the return value is a different type, or if it has a different value, then `Assert.AreEqual` will return `false`, and the test will fail.<br>

If `widget.Delay` throws an exception, then the exception will be caught by the test framework and the test will also fail. In either case, all other tests will continue to execute and the aggregate result will be rolled up into the reports that are included as part of the build loop.

The Assert methods also have versions that include messages for cases where the basic assertion is unclear. For example,<br>

`Assert.IsTrue(dictionary.TryGetValue(key, out string value));` <br>

will tell you that `TryGetValue` failed without telling you the key it was looking for. You can easily improve the readability of the output by adding a description:<br>

`Assert.IsTrue(dictionary.TryGetValue(key, out string value, "Couldn't find key: " + key));`

The output will now tell you that `IsTrue` failed, but also why. This is especially handy in cases where your `Assert` statement appears within a loop

### Working with Microsoft Fakes

**Important**: Do not add any unit tests which use Microsoft Fakes into any project.

Some of the unit tests in Accessibility Insights For Windows use Microsoft Fakes to replace system calls which are otherwise difficult to mock using standard mocking frameworks. The problem is that certain editions of Visual Studio, like Enterprise, support Fakes. And some, like Community, do not. Because Accessibility Insights is open source, we cannot predict which editions of Visual Studio contributors will have access to. Therefore, we have disabled the running of tests which contain Fakes by default. Furthermore, we intend to remove all Fakes from all projects in Axe.Windows. Therefore, no tests requiring Fakes should be added to Axe.Windows.

But while the Fakes still exist, if you have an edition of Visual Studio which supports Fakes and you would like to run the unit tests which require them, add an environment variable to your system named `FAKES_SUPPORTED` and set it equal to `1`. Then, restart Visual Studio.

For unit test bar and standards please visit [Unit test Bar and Standards](UnitTestBarAndStandards.md)
