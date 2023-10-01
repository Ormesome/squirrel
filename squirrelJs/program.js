const edge = require("edge-js");

/*
 * Map out all the methods we want
 */
const fail = edge.func({
  assemblyFile:
    __dirname + "..squirrelLib\binDebug\net7.0win-x64publishsquirrelLib.dll", // Path to the compiled .NET assembly
  typeName: "squirrelLib.Operations", // Namespace and class name
  methodName: "CountAcorns2", // Method name
});

fail("", function (error, result) {
  if (error) throw error;
  console.log(result);
});
