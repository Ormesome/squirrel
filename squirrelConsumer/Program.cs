using squirrel;

Operations op = new Operations();

object input = "Secret Squirrel";
object oResult;
string result;

oResult = await op.greet(input);
result = (string)oResult;
Console.WriteLine(result);

oResult = await op.CountAcorns(input);
result = (string)oResult;
Console.WriteLine(result);

oResult = await op.encrypt(input);
result = (string)oResult;
Console.WriteLine(result);
