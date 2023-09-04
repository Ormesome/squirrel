const edge = require("edge-js");

/*
 * Map out all the methods we want
 */
const fail = edge.func({
  assemblyFile:
    __dirname + "/../squirrel/bin/Debug/net7.0/win-x64/publish/squirrel.dll", // Path to your compiled .NET assembly
  typeName: "squirrel.Operations", // Namespace and class name
  methodName: "fail", // Method name
});

fail("", function (error, result) {
  if (error) throw error;
  console.log(result);
});
