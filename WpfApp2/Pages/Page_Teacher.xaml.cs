using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace WpfApp2.Pages;

/// <summary>
///     Логика взаимодействия для Page_Teacher.xaml
/// </summary>
public partial class Page_Teacher : Page
{
    private readonly string connectionString =
        "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

    private readonly DatabaseHelper databaseHelper;

    public Page_Teacher()
    {
        InitializeComponent();
        databaseHelper = new DatabaseHelper(connectionString);
        LoadData();
    }

    private void LoadData()
    {
        var teachersSql = "SELECT * FROM public.\"Teachers\"";
        var teachersDataTable = databaseHelper.ExecuteQuery(teachersSql);

        dtteachers.ItemsSource = teachersDataTable?.DefaultView;
    }

    private void add_teacher_Click(object sender, RoutedEventArgs e)
    {
        var TeacherForm = new teacher_form(page: this);
        TeacherForm.ShowDialog();
    }
    public void RefreshData()
    {
        LoadData();
    }
    private void edit_teacher_Click(object sender, RoutedEventArgs e)
    {
        if (dtteachers.SelectedItems.Count > 0)
        {
            var selectedRow = dtteachers.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var teacherId = Convert.ToInt32(selectedRow["ID"]);
                var surname = selectedRow["Surname"]?.ToString() ?? "";
                var name = selectedRow["Name"]?.ToString() ?? "";
                var patronym = selectedRow["Patronym"]?.ToString() ?? "";
                var DoB = Convert.ToDateTime(selectedRow["DoB"]);
                var comment = selectedRow["Comments"]?.ToString() ?? "";

                var teacherForm = new teacher_form(this, teacherId, surname, name, patronym, DoB, comment);

                teacherForm.ShowDialog();

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

    private void del_teacher_Click(object sender, RoutedEventArgs e)
    {
        if (dtteachers.SelectedItems.Count > 0)
        {
            var selectedRowView = dtteachers.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["ID"];
                var teacherId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Teachers\" WHERE \"ID\" = {teacherId}");
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

    private void search_teacher_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_teacher.Text.Trim();

        var searchSql = string.IsNullOrEmpty(searchTerm)
            ? "SELECT * FROM public.\"Teachers\""
            : $"SELECT * FROM public.\"Teachers\" WHERE LOWER(\"Surname\") LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtteachers.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }
}