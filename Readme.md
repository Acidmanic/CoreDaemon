
![icon](CoreDaemon/graphics/damien.png)

About
==============

This package will help you produce binaries capable of installing init.d services of themselves on the target machine. The goal of writing this code, was to ease the deployment of dotnetcore web applications on linux hosts from Cicd pipelines. 



Get the library
===============

This library is available on [NuGet.org](https://www.nuget.org/packages/CoreDaemon/), so you can add library to your dotnet core application via .Net Cli using:

```bash 
	dotnet add package CoreDaemon 

```

Or Visualstudio Package Manager, using 

```bash 
	Install-Package CoreDaemon 

```
Or by directly adding reference to .csproj file:

```xml
	<PackageReference Include="CoreDaemon" />

```

Usage
======

To use the library, You would add the package in your project. Then at the Main Entry (Program.cs -> Main(args[])) add call the library's entry:


```c#

static void Main(string[] args)
{
            Damien.Summon().ExecuteCommands(args);

           // Start your sync (blocking) service here
} 
```

The library will check input commands and install or uninstall a service on running machine according to the input arguments.  

The commands would be

 * daemon install
 * daemon uninstall
 * daemon help

Check if CoreDaemon Handeled the input commands
-------------------------

Most probably you would prefer your service not being started when the binary is called by daemon arguments. For this you can simply check the result of calling the Damien execution:

```c#


static void Main(string[] args)
 {
	if (Damien.Summon().ExecuteCommands(args) == ExecutionResult.NoActionTaken)
	{
	    // Start sync (blocking) service here 
	}

 } 
```

How daemon gets installed/uninstalled
==============


By adding the code mentioned above, to your application entry, when your applicatin gets started by CoreDaemon commands, It will handle the commands
 and will install or uninstall a service (on linux's init.d). CoreDaemon commands are: 

 * daemon install
 * daemon uninstall
 * daemon help

For example if your application name is Example, after it has been built, you would call 

```bash
	sudo ./Example daemon install
```

Limitations
============

 * For now, the library only woks on Linux, and only linux distributions supporting init.d daemons.
 * The code has been tested on Ubuntu
 * Since CoreDaemon writes on System files, calling your application with daemon commands must be elevated. (use sudo or call by root user)


Regards;
Mani
