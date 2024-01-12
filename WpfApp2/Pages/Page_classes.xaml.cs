using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using WpfApp2.forms;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для Page_classes.xaml
/// </summary>
public partial class Page_classes : Page
{
    private readonly string connectionString =
        "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

    private readonly DatabaseHelper databaseHelper;
    public Page_classes()
    {
        InitializeComponent();
        databaseHelper = new DatabaseHelper(connectionString);
        LoadData();
    }
    private void LoadData()
    {
        var scheduleSql = "SELECT * FROM public.\"Schedule\"";
        var scheduleDataTable = databaseHelper.ExecuteQuery(scheduleSql);
        dtschedule.ItemsSource = scheduleDataTable?.DefaultView;

        var classesSql = "SELECT * FROM public.\"Classes\"";
        var classesDataTable = databaseHelper.ExecuteQuery(classesSql);
        dtclasses.ItemsSource = classesDataTable?.DefaultView;
    }
    public void RefreshData()
    {
        LoadData();
    }
    private void add_schedule_Click(object sender, RoutedEventArgs e)
    {
        var ScheduleForm = new schedule_form(page: this);
        ScheduleForm.ShowDialog();
    }

    private void edit_schedule_Click(object sender, RoutedEventArgs e)
    {
        if (dtschedule.SelectedItems.Count > 0)
        {
            var selectedRow = dtschedule.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var scheduleId = Convert.ToInt32(selectedRow["id"]);
                var weekday = selectedRow["weekday"]?.ToString() ?? "";
                var starttime = (TimeSpan)selectedRow["starttime"];
                var endtime = (TimeSpan)selectedRow["endtime"];
                var group = Convert.ToInt32(selectedRow["group"]);
                var teacher = Convert.ToInt32(selectedRow["teacher"]);
                var classroom = Convert.ToInt32(selectedRow["classroom"]);

                var scheduleForm = new schedule_form(this, scheduleId, weekday, starttime, endtime, group, teacher, classroom);

                scheduleForm.ShowDialog();

                RefreshData();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для редактирования.");
        }
    }

    private void del_schedule_Click(object sender, RoutedEventArgs e)
    {
        if (dtschedule.SelectedItems.Count > 0)
        {
            var selectedRowView = dtschedule.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["id"];
                var scheduleId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Schedule\" WHERE \"id\" = {scheduleId}");
                RefreshData();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для удаления.");
        }

    }

    private void search_schedule_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_schedule.Text.Trim();
        var selectedColumnName = "weekday";

        var searchSql =
            $"SELECT * FROM public.\"Schedule\" WHERE LOWER(\"{selectedColumnName}\") LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtschedule.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }

    private void add_clasess_Click(object sender, RoutedEventArgs e)
    {
        var classesForm = new clasess_form(page: this);
        classesForm.ShowDialog();
    }

    private void edit_clasess_Click(object sender, RoutedEventArgs e)
    {
        if (dtclasses.SelectedItems.Count > 0)
        {
            var selectedRow = dtclasses.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var classesId = Convert.ToInt32(selectedRow["id"]);
                var date = Convert.ToDateTime(selectedRow["Date"]);
                var group = Convert.ToInt32(selectedRow["group"]);
                var teacher = Convert.ToInt32(selectedRow["teacher"]);
                var done = Convert.ToBoolean(selectedRow["done"]);

                var classesForm = new clasess_form(this, classesId, date, group, teacher, done);

                classesForm.ShowDialog();

                RefreshData();
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для редактирования.");
        }
    }

    private void del_clasess_Click(object sender, RoutedEventArgs e)
    {
        if (dtclasses.SelectedItems.Count > 0)
        {
            var selectedRowView = dtclasses.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["id"];
                var classesId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Classes\" WHERE \"id\" = {classesId}");
                RefreshData();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для удаления.");
        }
    }

    private void search_clasess_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_clasess.Text.Trim();
        var selectedColumnName = "group";

        var searchSql =
            $"SELECT * FROM public.\"Classes\" WHERE CAST(\"{selectedColumnName}\" AS TEXT) LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtclasses.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }
}