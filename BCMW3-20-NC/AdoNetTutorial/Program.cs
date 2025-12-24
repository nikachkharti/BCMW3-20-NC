using AdoNetTutorial.Helper;
using AdoNetTutorial.Models;
using Microsoft.Data.SqlClient;
using System.Data;

//SqlConnection --- Connects to a SQL Server Database
//SqlCommand --- Executes a command against a SQL Server Database
//SqlDataReader --- Reads a forward-only stream of rows from a SQL Server Database
//SqlTransaction

namespace AdoNetTutorial
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            var result = await GetAllClients();
        }




        private static async Task<int> DeleteClient(int id)
        {
            return await AdoNetHelper.ExecuteStoredProcAsync("dbo.sp_DeleteClient", new
            {
                Id = id
            });
        }


        private static async Task<int> AddNewClient(Client row)
        {
            return await AdoNetHelper.ExecuteStoredProcAsync("dbo.sp_CreateClient", new
            {
                row.ClientCode,
                row.FirstName,
                row.LastName,
                row.PersonalNumber,
                row.BirthDate,
                row.Phone,
                row.Email
            });
        }

        private static async Task<List<Client>> GetAllClients()
        {
            return await AdoNetHelper.ExecuteReaderAsync(
                        "SELECT * FROM dbo.Clients",
                        CommandType.Text,
                        null,
                        r => new Client
                        {
                            Id = r.GetInt32(r.GetOrdinal("Id")),
                            ClientCode = r.GetString(r.GetOrdinal("ClientCode")),
                            FirstName = r.GetString(r.GetOrdinal("FirstName")),
                            LastName = r.GetString(r.GetOrdinal("LastName")),
                            CreatedAt = r.GetDateTime(r.GetOrdinal("CreatedAt"))
                        });
        }
    }
}
