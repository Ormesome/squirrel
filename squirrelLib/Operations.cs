using System.Threading.Tasks;
using System;
using System.Text;
using System.Text.Json;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace squirrelLib
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
    private int addProcess(int a, int b)
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
    private string encryptProcess(string plaintext)
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

    private List<List<Dictionary<string, object>>> ExecuteStoredProcedureProcess(string connectionString, string storedProcedureName, object? parameters = null)
    {
      var result = new List<List<Dictionary<string, object>>>();
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        using (SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure })
        {
          if (parameters != null)
          {
            foreach (var property in parameters.GetType().GetProperties())
            {
              var parameter = new SqlParameter($"@{property.Name}", property.GetValue(parameters));
              command.Parameters.Add(parameter);
            }
          }

          connection.Open(); // This is the line that dies
          var reader = command.ExecuteReader();

          do
          {
            var resultSet = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
              var row = new Dictionary<string, object>();
              for (int i = 0; i < reader.FieldCount; i++)
              {
                row[reader.GetName(i)] = reader.GetValue(i);
              }
              resultSet.Add(row);
            }
            result.Add(resultSet);
          } while (reader.NextResult());
        }
      }
      return result;
    }

    public async Task<object> CountAcorns(dynamic input)
    {
      string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=squirrelDb;User Id=sa;Password=<YourStrong!Passw0rd>;Encrypt=True;TrustServerCertificate=True;";
      string storedProcedureName = "dbo.pCountAcorns";
      object parameters = new { squirrelId = "430c8813-2ccd-4e89-997c-5fdeabc80efc"};
      return await Task.Run(() => ExecuteStoredProcedureProcess(connectionString, storedProcedureName, parameters));
    }

    public string CountAcorns2()
    {
      string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=squirrelDb;User Id=sa;Password=<YourStrong!Passw0rd>;Encrypt=True;TrustServerCertificate=True;";
      string storedProcedureName = "dbo.pCountAcorns";
      object parameters = new { squirrelId = "430c8813-2ccd-4e89-997c-5fdeabc80efc"};
      var response = ExecuteStoredProcedureProcess(connectionString, storedProcedureName, parameters);
      var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
      return json;
    }

    static void Main(string[] args)
    {
      var operations = new Operations();

      Console.WriteLine(operations.encryptProcess("Secret Squirel"));

      var result = operations.CountAcorns2();
      var data = JsonDocument.Parse(result)!;
      Console.WriteLine("Acorns collected = {0}",data.RootElement[0][0].GetProperty("acornCount"));
    }
  }
}
