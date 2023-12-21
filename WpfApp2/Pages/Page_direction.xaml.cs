using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для Page_direction.xaml
/// </summary>
public partial class Page_direction : Page
{
    public Page_direction()
    {
        InitializeComponent();
        // Установите параметры подключения
        var connectionString = "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

        // Создайте соединение с базой данных
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Заполнение данных из таблицы "Direction"
                var directionSql = "SELECT * FROM public.\"Direction\"";
                var directionCommand = new NpgsqlCommand(directionSql, connection);

                var directionAdapter = new NpgsqlDataAdapter(directionCommand);
                var directionDataTable = new DataTable();
                directionAdapter.Fill(directionDataTable);

                // Установите таблицу данных "Direction" как источник данных для DataGrid dtdirection
                dtdirection.ItemsSource = directionDataTable.DefaultView;

                // Заполнение данных из таблицы "Group"
                var groupSql = "SELECT * FROM public.\"Group\"";
                var groupCommand = new NpgsqlCommand(groupSql, connection);

                var groupAdapter = new NpgsqlDataAdapter(groupCommand);
                var groupDataTable = new DataTable();
                groupAdapter.Fill(groupDataTable);

                // Установите таблицу данных "Group" как источник данных для DataGrid dtgroup
                dtgroup.ItemsSource = groupDataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }
    }

    private void add_direction_Click(object sender, RoutedEventArgs e)
    {
        var DirectionForm = new direction_form();
        DirectionForm.ShowDialog();
    }
}