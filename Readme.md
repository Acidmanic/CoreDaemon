
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

Check if CoreDaemon Handled the input commands
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

You can specify the default way of creating/removing daemons by using ```Damien.UseInitRc()``` to create service in
 /etc/init.d or using ```UseServiceUnitFile()``` to create service in /etc/systemd/system. This will be a default and can 
 be overridden when using the application (Next section).
 
Logging
=====

You can use any implementation of Microsoft's ```ILogger``` by calling ```Damien.UseLogger(logger)```,
 where logger is your ```ILogger``` instance. 

```c#

     var logger = new ConsoleLogger();

     Damien.Summon().UseLogger(logger);
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

 For install scripts, cicds and etc. You can add 'init-rc' or 'unit' 
 argument to both install and uninstall commands. These arguments will 
 override the default installation method. __init-rc__ will install/uninstall 
 the select-case script in /etc/init.d/service-name, and __unit__ will  
 install/uninstall the service unit file in /etc/systemd/system/servce-name.service.

Limitations
============

 * For now, the library only woks on Linux.
 * The code has been tested on Ubuntu
 * Since CoreDaemon writes on System files, calling your application with daemon commands must be elevated. (use sudo or call by root user)


Regards;
Mani
