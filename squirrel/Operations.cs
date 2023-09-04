using System.Threading.Tasks;
using System;
using System.Text;
using System.Text.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace squirrel
{
  public class Operations
  {

    /*
    Demonstrate a basic entry point
    */
    public async Task<object> greet(dynamic input)
    {
      return await Task.Run(() => "Hello, World!");
    }

    /*
    Demonstrate parameter parsing and process redirection
    */
    public int addProcess(int a, int b)
    {
      return a + b;
    }

    public async Task<object> add(dynamic input)
    {
      int a = (int)input.a;
      int b = (int)input.b;
      return await Task.Run(() => addProcess(a, b));
    }

    /*
    Demonstrate a working call to a dll
    */
    private static string encryptProcess(string plaintext)
    {
      string salt = "this-is-the-salt-used-to-encrypt-the-plaintext";

      // Convert the plaintext string to a byte array
      byte[] plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plaintext);

      // Derive a new password using the PBKDF2 algorithm and a random salt
      Rfc2898DeriveBytes passwordBytes = new Rfc2898DeriveBytes(salt, 20);

      // Use the password to encrypt the plaintext
      Aes encryptor = Aes.Create();
      encryptor.Key = passwordBytes.GetBytes(32);
      encryptor.IV = passwordBytes.GetBytes(16);
      using (MemoryStream ms = new MemoryStream())
      {
        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
          cs.Write(plaintextBytes, 0, plaintextBytes.Length);
        }
        return Convert.ToBase64String(ms.ToArray());
      }
    }

    public async Task<object> encrypt(dynamic input)
    {
      return await Task.Run(() => encryptProcess(input));
    }

    /*
    Demonstrate a non working call to a dll
    */
    private static int ExecuteStoredProcedure(string connectionString, string storedProcedureName, object parameters = null)
    {
      Console.WriteLine("Begin ExecuteStoredProcedure");
      Microsoft.Data.SqlClient.SqlConnection connection = new SqlConnection(connectionString);
      connection.Open();
      connection.Close();
      connection.Dispose();
      Console.WriteLine("Begin ExecuteStoredProcedure");
      return 1;
    }

    public async Task<object> CountAcorns(dynamic input)
    {
      string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=squirrelDb;User Id=sa;Password=<YourStrong!Passw0rd>;Encrypt=False;TrustServerCertificate=true;";
      string storedProcedureName = "dbo.pCountAcorns";
      object parameters = new { };
      return await Task.Run(() => ExecuteStoredProcedure(connectionString, storedProcedureName, parameters));
    }

    private static string pCountAcornsProcess()
    {
      string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=squirrelDb;User Id=sa;Password=<YourStrong!Passw0rd>;Encrypt=False;TrustServerCertificate=true;";
      string storedProcedureName = "dbo.pCountAcorns";
      object parameters = new { };
      var response = ExecuteStoredProcedure(connectionString, storedProcedureName, parameters);
      var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
      return json;
    }
    
    static void Main(string[] args)
    {
      Console.WriteLine("Begin Program");
      pCountAcornsProcess();
      Console.WriteLine(encryptProcess("Secret Squirel"));
      Console.WriteLine("End Program");
    }
  }
}
