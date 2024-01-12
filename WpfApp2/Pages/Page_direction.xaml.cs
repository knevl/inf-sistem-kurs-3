using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Npgsql;
using WpfApp2.forms;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для Page_direction.xaml
/// </summary>
public partial class Page_direction : Page
{
    private readonly string connectionString =
        "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

    private readonly DatabaseHelper databaseHelper;
    public Page_direction()
    {
        InitializeComponent();
        databaseHelper = new DatabaseHelper(connectionString);
        LoadData();
    }
    private void LoadData()
    {
        var directionSql = "SELECT * FROM public.\"Direction\"";
        var directionDataTable = databaseHelper.ExecuteQuery(directionSql);
        dtdirection.ItemsSource = directionDataTable?.DefaultView;

        var groupSql = "SELECT * FROM public.\"Group\"";
        var groupDataTable = databaseHelper.ExecuteQuery(groupSql);
        dtgroup.ItemsSource = groupDataTable?.DefaultView;
    }

    private void add_direction_Click(object sender, RoutedEventArgs e)
    {
        var DirectionForm = new direction_form(page: this);
        DirectionForm.ShowDialog();
    }
    public void RefreshData()
    {
        LoadData();
    }

    private void edit_direction_Click(object sender, RoutedEventArgs e)
    {
        if (dtdirection.SelectedItems.Count > 0)
        {
            var selectedRow = dtdirection.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var directionId = Convert.ToInt32(selectedRow["id"]);
                var name = selectedRow["Name"]?.ToString() ?? "";
                var comments = selectedRow["Comments"]?.ToString() ?? "";

                var directionForm = new direction_form(this, directionId, name, comments);

                directionForm.ShowDialog();

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

    private void del_direction_Click(object sender, RoutedEventArgs e)
    {
        if (dtdirection.SelectedItems.Count > 0)
        {
            var selectedRowView = dtdirection.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["ID"];
                var directionId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Direction\" WHERE \"id\" = {directionId}");
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

    private void search_direction_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_direction.Text.Trim();

        var searchSql = string.IsNullOrEmpty(searchTerm)
            ? "SELECT * FROM public.\"Direction\""
            : $"SELECT * FROM public.\"Direction\" WHERE LOWER(\"Name\") LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtdirection.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }

    private void add_group_Click(object sender, RoutedEventArgs e)
    {
        var GroupForm = new group2_form(page: this);
        GroupForm.ShowDialog();
    }

    private void edit_group_Click(object sender, RoutedEventArgs e)
    {
        if (dtgroup.SelectedItems.Count > 0)
        {
            var selectedRow = dtgroup.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var groupId = Convert.ToInt32(selectedRow["id"]);
                var name = selectedRow["Name"]?.ToString() ?? "";
                var direction = Convert.ToInt32(selectedRow["direction"]);
                // Предполагаем, что у вас есть форма редактирования группы с соответствующими полями
                var groupForm = new group2_form(this, groupId, name, direction);

                groupForm.ShowDialog();

                RefreshData(); // Перезагрузка данных после изменений
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

    private void del_group_Click(object sender, RoutedEventArgs e)
    {
        if (dtgroup.SelectedItems.Count > 0)
        {
            var selectedRowView = dtgroup.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["ID"];
                var groupId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Group\" WHERE \"id\" = {groupId}");
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

    private void search_group_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_group.Text.Trim();

        var searchSql = string.IsNullOrEmpty(searchTerm)
            ? "SELECT * FROM public.\"Group\""
            : $"SELECT * FROM public.\"Group\" WHERE LOWER(\"Name\") LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtgroup.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }
}