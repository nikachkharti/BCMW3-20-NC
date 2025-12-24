using Microsoft.Data.SqlClient;
using System.Data;

namespace AdoNetTutorial.Helper
{
    public static class AdoNetHelper
    {
        private static string _connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=Class;Trusted_Connection=True;TrustServerCertificate=True";
        private static async Task<SqlCommand> CreateCommandAsync(string text, CommandType type, object parameters)
        {
            var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(text, connection) { CommandType = type };

            if (parameters != null)
            {
                foreach (var prop in parameters.GetType().GetProperties())
                {
                    var value = prop.GetValue(parameters) ?? DBNull.Value;
                    command.Parameters.AddWithValue("@" + prop.Name, value);
                }
            }

            await connection.OpenAsync();
            return command;
        }


        public static async Task<int> ExecuteStoredProcAsync<T>(string procName, T parameters) => await ExecuteNonQueryAsync(procName, CommandType.StoredProcedure, parameters);

        public static async Task<int> ExecuteNonQueryAsync(string text, CommandType type, object parameters)
        {
            using var command = await CreateCommandAsync(text, type, parameters);
            return await command.ExecuteNonQueryAsync();
        }

        public static async Task<List<TResult>> ExecuteReaderAsync<TResult>(string text, CommandType type, object parameters, Func<SqlDataReader, TResult> map)
        {
            using var command = await CreateCommandAsync(text, type, parameters);
            using var reader = await command.ExecuteReaderAsync();

            var list = new List<TResult>();
            while (await reader.ReadAsync())
                list.Add(map(reader));

            return list;
        }

    }
}
