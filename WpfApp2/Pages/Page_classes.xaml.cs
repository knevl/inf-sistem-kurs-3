using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для Page_classes.xaml
/// </summary>
public partial class Page_classes : Page
{
    public Page_classes()
    {
        InitializeComponent();
        var connectionString = "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

        // Создайте соединение с базой данных
        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                // Заполнение данных из таблицы "Classes"
                var classesSql = "SELECT * FROM public.\"Classes\"";
                var classesCommand = new NpgsqlCommand(classesSql, connection);

                var classesAdapter = new NpgsqlDataAdapter(classesCommand);
                var classesDataTable = new DataTable();
                classesAdapter.Fill(classesDataTable);

                // Установите таблицу данных "Classes" как источник данных для DataGrid dtclasses
                dtclasses.ItemsSource = classesDataTable.DefaultView;

                // Заполнение данных из таблицы "Schedule"
                var scheduleSql = "SELECT * FROM public.\"Schedule\"";
                var scheduleCommand = new NpgsqlCommand(scheduleSql, connection);

                var scheduleAdapter = new NpgsqlDataAdapter(scheduleCommand);
                var scheduleDataTable = new DataTable();
                scheduleAdapter.Fill(scheduleDataTable);

                // Установите таблицу данных "Schedule" как источник данных для DataGrid dtschedule
                dtschedule.ItemsSource = scheduleDataTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
            }
        }
    }
}