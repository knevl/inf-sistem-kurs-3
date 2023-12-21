using System.Windows;
using Npgsql;

namespace WpfApp2;

public partial class student_form : Window
{
    private readonly Page_students parentPage;
    private readonly int studentId;

    public student_form(Page_students page)
    {
        InitializeComponent();
        parentPage = page;
        Closed += StudentForm_Closed;
    }

    public student_form(Page_students page, int id, string surname, string name, string patronym, DateTime DoB,
        string parent, string comment)
        : this(page)
    {
        studentId = id;
        s_surnameTextBox.Text = surname;
        s_nameTextBox.Text = name;
        s_patronymTextBox.Text = patronym;
        s_DoBPicker.SelectedDate = DoB;
        s_parentTextBox.Text = parent;
        s_commentTextBox.Text = comment;
    }

    private void StudentForm_Closed(object? sender, EventArgs e)
    {
        if (sender is student_form studentForm) studentForm.parentPage.RefreshData();
    }

    private void Button_Click_Save(object sender, RoutedEventArgs e)
    {
        var surname = s_surnameTextBox.Text;
        var name = s_nameTextBox.Text;
        var patronym = s_patronymTextBox.Text;
        var DoB = s_DoBPicker.SelectedDate ?? DateTime.Now;
        var parent = s_parentTextBox.Text;
        var comment = s_commentTextBox.Text;

        if (studentId > 0)
            DatabaseHelper.UpdateStudent(studentId, surname, name, patronym, DoB, parent, comment);
        else
            DatabaseHelper.AddStudent(surname, name, patronym, DoB, parent, comment);

        Close();
    }

    private void Button_Click_cancel_student(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }

    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";

        public static void AddStudent(string surname, string name, string patronym, DateTime DoB, string parent,
            string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "INSERT INTO public.\"Students\"(\"Surname\", \"Name\", \"Patronym\", \"DoB\", \"Parent\", \"Comments\") " +
                    "VALUES (@Surname, @Name, @Patronym, @DoB, @Parent, @Comment)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Patronym", patronym);
                    command.Parameters.AddWithValue("@DoB", DoB);
                    command.Parameters.AddWithValue("@Parent", parent);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateStudent(int studentId, string surname, string name, string patronym, DateTime DoB,
            string parent, string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "UPDATE public.\"Students\" " +
                            "SET \"Surname\" = @Surname, \"Name\" = @Name, \"Patronym\" = @Patronym, \"DoB\" = @DoB, \"Parent\" = @Parent, \"Comments\" = @Comment " +
                            "WHERE \"ID\" = @StudentId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Patronym", patronym);
                    command.Parameters.AddWithValue("@DoB", DoB);
                    command.Parameters.AddWithValue("@Parent", parent);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}