# Disclaimer
This workshop is based on the [.NET 6 Preview 5](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-5/) and will not be update to future version, at least for now. 

# Getting started
To get started with Blazor and MAUI development in .NET 6 Preview 5, [install the .NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0). Next, if you’re on Windows using Visual Studio, we recommend [installing the latest preview of Visual Studio 2019 16.11](http://visualstudio.com/preview). If you’re on macOS, we recommend [installing the latest preview of Visual Studio 2019 for Mac 8.10](https://docs.microsoft.com/visualstudio/mac/install-preview). Next, follow the [.NET MAUI getting started guide](https://docs.microsoft.com/dotnet/maui/get-started/installation).
To verify your development environment, and install any missing components, use the maui-check utility. Install this utility using the following .NET CLI command:
```powershell
dotnet tool install -g redth.net.maui.check
```

Then, run maui-check:
```powershell
maui-check
```
If any tools and SDKs required by .NET MAUI are missing, maui-check will install them.
