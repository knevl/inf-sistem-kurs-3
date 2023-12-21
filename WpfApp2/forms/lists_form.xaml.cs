using System.Windows;
using Npgsql;

namespace WpfApp2;

public partial class group_form : Window
{
    private readonly int listID; // Используем новое имя
    private readonly Page_students parentPage;
    private readonly int studentID;

    private readonly bool isEditMode;

    public group_form(Page_students page)
    {
        InitializeComponent();
        parentPage = page;
        Closed += GroupForm_Closed;
    }

    public group_form(Page_students page, int listID, int studentID)
        : this(page)
    {
        this.listID = listID;
        this.studentID = studentID;
        isEditMode = true;
        FillFormWithExistingData();
    }

    private void GroupForm_Closed(object? sender, EventArgs e)
    {
        if (sender is group_form groupForm) groupForm.parentPage.RefreshData();
    }

    private void FillFormWithExistingData()
    {
        try
        {
            var data = DatabaseHelper.GetListData(listID, studentID);
            l_id_sTextBox.Text = data.StudentId.ToString();
            l_id_gTextBox.Text = data.GroupId.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при получении данных для редактирования: {ex.Message}", "Ошибка",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void Button_Click_Save(object sender, RoutedEventArgs e)
    {
        var studentId = int.Parse(l_id_sTextBox.Text);
        var groupId = int.Parse(l_id_gTextBox.Text);

        if (isEditMode)
            DatabaseHelper.UpdateList(listID, studentID, studentId, groupId);
        else
            DatabaseHelper.AddList(studentId, groupId);

        Close();
    }

    private void Button_Click_Cancel(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }

    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";

        public static (int StudentId, int GroupId) GetListData(int listID, int studentId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "SELECT \"student\", \"group\" FROM public.\"Lists\" WHERE \"id\" = @ListID AND \"student\" = @StudentId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ListID", listID);
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            return (reader.GetInt32(0), reader.GetInt32(1));
                        throw new Exception("Запись не найдена.");
                    }
                }
            }
        }

        public static void UpdateList(int oldListID, int oldStudentId, int newStudentId, int newGroupId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                if (!StudentExists(newStudentId) || !GroupExists(newGroupId))
                {
                    MessageBox.Show("Введите корректные ID студента и группы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var query =
                    "UPDATE public.\"Lists\" SET \"student\" = @NewStudentId, \"group\" = @NewGroupId " +
                    "WHERE \"id\" = @OldListID AND \"student\" = @OldStudentId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OldListID", oldListID);
                    command.Parameters.AddWithValue("@OldStudentId", oldStudentId);
                    command.Parameters.AddWithValue("@NewStudentId", newStudentId);
                    command.Parameters.AddWithValue("@NewGroupId", newGroupId);

                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool StudentExists(int studentId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM public.\"Students\" WHERE \"ID\" = @StudentId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);

                    var count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private static bool GroupExists(int groupId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM public.\"Group\" WHERE \"id\" = @GroupId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", groupId);

                    var count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        public static void AddList(int studentId, int groupId)
        {
            if (!StudentExists(studentId) || !GroupExists(groupId))
            {
                MessageBox.Show("Введите корректные ID студента и группы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "INSERT INTO public.\"Lists\"(\"student\", \"group\") " +
                    "VALUES (@StudentId, @GroupId)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@GroupId", groupId);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}