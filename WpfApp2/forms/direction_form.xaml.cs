using Npgsql;
using System.Windows;

namespace WpfApp2;

/// <summary>
///     Логика взаимодействия для direction_form.xaml
/// </summary>
public partial class direction_form : Window
{
    private readonly Page_direction parentPage;
    private readonly int directionId;

    public direction_form(Page_direction page)
    {
        InitializeComponent();
        parentPage = page;
        Closed += DirectionForm_Closed;
    }

    public direction_form(Page_direction page, int id, string name, string comment)
        : this(page)
    {
        directionId = id;
        d_nameTextBox.Text = name;
        d_commentTextBox.Text = comment;
    }

    private void Button_Click_Save_dir(object sender, RoutedEventArgs e)
    {
        var name = d_nameTextBox.Text;
        var comment = d_commentTextBox.Text;

        if (directionId > 0)
            DatabaseHelper.UpdateDirection(directionId, name, comment);
        else
            DatabaseHelper.AddDirection(name, comment);

        Close();
    }

    private void DirectionForm_Closed(object sender, EventArgs e)
    {
        if (sender is direction_form directionForm) directionForm.parentPage.RefreshData();
    }
    private void Button_Click_cancel_dir(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }
    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";

        public static void AddDirection(string name, string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "INSERT INTO public.\"Direction\"(\"Name\", \"Comments\") " +
                    "VALUES (@Name, @Comment)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateDirection(int directionId, string name, string comment)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "UPDATE public.\"Direction\" " +
                            "SET \"Name\" = @Name, \"Comments\" = @Comment " +
                            "WHERE \"id\" = @DirectionId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DirectionId", directionId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Comment", comment);

                    command.ExecuteNonQuery();
                }
            }
        }
    }

}