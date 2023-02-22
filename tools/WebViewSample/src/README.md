# README

## Overview
This project provides a test app that opens a web page in the [Microsoft.Web.WebView2](https://www.nuget.org/packages/Microsoft.Web.WebView2) control. It is used as a test bed for running Accessibility Insights for Windows on a web application. Thsi is generaly _not_ recommneded, since it is difficult to know how to address any issues that are raised. The recommended option is to scan the HTML using a web-based engine such as Accessibility Insights for Web.

## Updating the project
You can build and run the project from within Visual Studio. If you need to update the binary that we use for tests, you will need to run the following command:

```
dotnet publish -r win-x64 -c release 
```

Then copy `bin\release\net6.0-windows\win-x64\publish\WebViewSample.exe` over the binary that is stored in the repo.

