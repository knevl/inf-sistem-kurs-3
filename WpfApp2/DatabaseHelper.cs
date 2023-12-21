using System.Data;
using System.Windows;
using Npgsql;
// Добавьте эту строку

public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public DataTable? ExecuteQuery(string sql, NpgsqlParameter[] parameters = null)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var command = new NpgsqlCommand(sql, connection);

                // Добавляем параметры к команде, если они переданы
                if (parameters != null) command.Parameters.AddRange(parameters);

                var adapter = new NpgsqlDataAdapter(command);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
                return null;
            }
        }
    }

    public void ExecuteNonQuery(string sql, NpgsqlParameter[] parameters = null)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    // Добавляем параметры к команде, если они переданы
                    if (parameters != null) command.Parameters.AddRange(parameters);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении запроса: {ex.Message}");
            }
        }
    }
}