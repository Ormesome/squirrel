# Squirrel

SquirrelLib is a .NET 7.0 library that uses the Microsoft.Data.SqlClient package to connect to a SQL Server database. It is intended to be called from a Node.js application using edge-js.

# SquirrelLib

This is the dll project.

There is an included main so that the code can be run as an exe:

```console
dotnet run
```

But the goal is to create a dll to be called by the other application, squirrelApp, and later through edge-js. To create the dll:

```console
dotnet publish
```

## squirrelApp

This is the application that will call the dll. It is a .NET 7.0 console application.

```console
dotnet run
```

## squirrelJs

This is the Node.js application that will call the dll. It uses edge-js to call the dll.

```console
node program.js
```

## squirrelDb

This is the SQL Server database that will be accessed by the dll.

## Issue

When run as a dll the following error is received:

```
System.TypeInitializationException: The type initializer for 'Microsoft.Data.SqlClient.TdsParser' threw an exception.
---> System.TypeInitializationException: The type initializer for 'Microsoft.Data.SqlClient.SNILoadHandle' threw an exception.
---> System.DllNotFoundException: Unable to load shared library 'Microsoft.Data.SqlClient.SNI.dll' or one of its dependencies.
```

Recommended Solution: Ensure "Execute" permissions are granted on the filesystem where native runtime libraries are loaded in the .NET Core process.

https://learn.microsoft.com/en-us/sql/connect/ado-net/sqlclient-troubleshooting-guide?view=sql-server-ver16
