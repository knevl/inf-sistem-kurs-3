using System.Windows;
using WpfApp2.Pages;
using Npgsql;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для teacher_form.xaml
/// </summary>
public partial class teacher_form : Window
{
    private readonly Page_Teacher parentPage;
    private readonly int teacherId;
    public teacher_form(Page_Teacher page)
    {
        InitializeComponent();
        parentPage = page;
        Closed += TeacherForm_Closed;
    }

    public teacher_form(Page_Teacher page, int id, string surname, string name, string patronym, DateTime DoB,
        string comment)
        : this(page)
    {
        teacherId = id;
        t_surnameTextBox.Text = surname;
        t_nameTextBox.Text = name;
        t_patTextBox.Text = patronym;
        t_dobPicker.SelectedDate = DoB;
        t_commentTextBox.Text = comment;
    }
    private void TeacherForm_Closed(object? sender, EventArgs e)
    {
        if (sender is teacher_form teacherForm) teacherForm.parentPage.RefreshData();
    }
    private void Button_Click_cancel_teacher(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }

    private void Button_Click_Save_teacher(object sender, RoutedEventArgs e)
    {
        var surname = t_surnameTextBox.Text;
        var name = t_nameTextBox.Text;
        var patronym = t_patTextBox.Text;
        var DoB = t_dobPicker.SelectedDate ?? DateTime.Now;
        var comment = t_commentTextBox.Text;

        if (teacherId > 0)
            DatabaseHelper.UpdateTeacher(teacherId, surname, name, patronym, DoB, comment);
        else
            DatabaseHelper.AddTeacher(surname, name, patronym, DoB, comment);

        Close();
    }
    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";

        public static void AddTeacher(string surname, string name, string patronym, DateTime DoB, string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "INSERT INTO public.\"Teachers\"(\"Surname\", \"Name\", \"Patronym\", \"DoB\", \"Comments\") " +
                    "VALUES (@Surname, @Name, @Patronym, @DoB, @Comment)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Patronym", patronym);
                    command.Parameters.AddWithValue("@DoB", DoB);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateTeacher(int teacherId, string surname, string name, string patronym, DateTime DoB,
            string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "UPDATE public.\"Teachers\" " +
                            "SET \"Surname\" = @Surname, \"Name\" = @Name, \"Patronym\" = @Patronym, \"DoB\" = @DoB, \"Comments\" = @Comment " +
                            "WHERE \"ID\" = @TeacherId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Patronym", patronym);
                    command.Parameters.AddWithValue("@DoB", DoB);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}