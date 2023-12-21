using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace WpfApp2;

public partial class Page_students : Page
{
    private readonly string connectionString =
        "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

    private readonly DatabaseHelper databaseHelper;

    public Page_students()
    {
        InitializeComponent();
        databaseHelper = new DatabaseHelper(connectionString);
        LoadData();
    }

    private void LoadData()
    {
        var studentsSql = "SELECT * FROM public.\"Students\"";
        var studentsDataTable = databaseHelper.ExecuteQuery(studentsSql);

        dtstudents.ItemsSource = studentsDataTable?.DefaultView;

        var listsSql = "SELECT * FROM public.\"Lists\"";
        var listsDataTable = databaseHelper.ExecuteQuery(listsSql);

        dtlist.ItemsSource = listsDataTable?.DefaultView;
    }

    private void add_students_Click(object sender, RoutedEventArgs e)
    {
        var studentForm = new student_form(this);
        studentForm.ShowDialog();
    }

    public void RefreshData()
    {
        LoadData();
    }

    private void del_students_Click(object sender, RoutedEventArgs e)
    {
        if (dtstudents.SelectedItems.Count > 0)
        {
            var selectedRowView = dtstudents.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var selectedRow = selectedRowView.Row;
                var idValue = selectedRow["ID"];
                var studentId = Convert.ToInt32(idValue);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Students\" WHERE \"ID\" = {studentId}");
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

    private void edit_students_Click(object sender, RoutedEventArgs e)
    {
        if (dtstudents.SelectedItems.Count > 0)
        {
            var selectedRow = dtstudents.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var studentId = Convert.ToInt32(selectedRow["ID"]);
                var surname = selectedRow["Surname"]?.ToString() ?? "";
                var name = selectedRow["Name"]?.ToString() ?? "";
                var patronym = selectedRow["Patronym"]?.ToString() ?? "";
                var DoB = Convert.ToDateTime(selectedRow["DoB"]);
                var parent = selectedRow["Parent"]?.ToString() ?? "";
                var comment = selectedRow["Comments"]?.ToString() ?? "";

                var studentForm = new student_form(this, studentId, surname, name, patronym, DoB, parent, comment);

                studentForm.ShowDialog();

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

    private void search_students_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm = searchbox_students.Text.Trim();
        var selectedColumnName = "Surname";

        var searchSql =
            $"SELECT * FROM public.\"Students\" WHERE LOWER(\"{selectedColumnName}\") LIKE LOWER('%{searchTerm}%')";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtstudents.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }

    private void add_stud_droup_Click(object sender, RoutedEventArgs e)
    {
        var groupForm = new group_form(this);
        groupForm.ShowDialog();
    }

    private void edit_stud_droup_Click(object sender, RoutedEventArgs e)
    {
        if (dtlist.SelectedItems.Count > 0)
        {
            var selectedRow = dtlist.SelectedItems[0] as DataRowView;

            if (selectedRow != null)
            {
                var listID = Convert.ToInt32(selectedRow["id"]);
                var studentID = Convert.ToInt32(selectedRow["student"]);

                var groupForm = new group_form(this, listID, studentID);

                groupForm.ShowDialog();

                RefreshData();
            }
            else
            {
                MessageBox.Show("Не удалось получить строку данных для редактирования.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для редактирования.");
        }
    }

    private void del_stud_droup_Click(object sender, RoutedEventArgs e)
    {
        if (dtlist.SelectedItems.Count > 0)
        {
            var selectedRowView = dtlist.SelectedItems[0] as DataRowView;

            if (selectedRowView != null)
            {
                var listID = Convert.ToInt32(selectedRowView["id"]);
                databaseHelper.ExecuteNonQuery($"DELETE FROM public.\"Lists\" WHERE \"id\" = {listID}");
                RefreshData();
            }
            else
            {
                MessageBox.Show("Не удалось получить строку данных для удаления из группы.");
            }
        }
        else
        {
            MessageBox.Show("Выберите запись для удаления из группы.");
        }
    }

    private void search_stud_droup_Click(object sender, RoutedEventArgs e)
    {
        var searchTerm1 = searchbox_stud_droup.Text.Trim();

        // Измененный запрос с условием WHERE, если поле поиска не пустое
        var searchSql = string.IsNullOrEmpty(searchTerm1)
            ? "SELECT * FROM public.\"Lists\""
            : $"SELECT * FROM public.\"Lists\" WHERE \"student\" = {searchTerm1}";

        using (var connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                var searchCommand = new NpgsqlCommand(searchSql, connection);
                var searchAdapter = new NpgsqlDataAdapter(searchCommand);
                var searchDataTable = new DataTable();
                searchAdapter.Fill(searchDataTable);

                dtlist.ItemsSource = searchDataTable?.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении поиска: {ex.Message}");
            }
        }
    }

}