using Npgsql;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp2.forms;

/// <summary>
///     Логика взаимодействия для group2_form.xaml
/// </summary>
public partial class group2_form : Window
{
    private readonly int groupId;
    private readonly Page_direction parentPage;
    private readonly int directionId;

    public group2_form(Page_direction page)
    {
        InitializeComponent();
        parentPage = page;
        Closed += GroupForm_Closed;
    }

    public group2_form(Page_direction page, int id, string groupName, int directionId)
        : this(page)
    {
        groupId = id;
        g_nameTextBox.Text = groupName;
        g_id_dTextBox.Text = directionId.ToString();
    }

    private void GroupForm_Closed(object sender, EventArgs e)
    {
        if (sender is group2_form groupForm) groupForm.parentPage.RefreshData();
    }

    private void save_group_Click(object sender, RoutedEventArgs e)
    {
        var groupName = g_nameTextBox.Text;
        var directionId = int.Parse(g_id_dTextBox.Text); 

        if (groupId > 0)
            DatabaseHelper.UpdateGroup(groupId, groupName, directionId);
        else
            DatabaseHelper.AddGroup(groupName, directionId);

        Close();
    }

    private void cancel_group_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
            "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes) Close();
    }

    public class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";
         public bool DirectionExists(int directionId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query = "SELECT COUNT(*) FROM public.\"Direction\" WHERE \"ID\" = @DirectionId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DirectionId", directionId);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }
        
        public static void AddGroup(string groupName, int directionId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                var query =
                    "INSERT INTO public.\"Group\"(\"Name\", \"direction\") " +
                    "VALUES (@GroupName, @DirectionId)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupName", groupName);
                    command.Parameters.AddWithValue("@DirectionId", directionId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateGroup(int groupId, string groupName, int directionId)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();


                var query = "UPDATE public.\"Group\" " +
                            "SET \"Name\" = @GroupName, \"direction\" = @DirectionId " +
                            "WHERE \"id\" = @GroupId";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@GroupId", groupId);
                    command.Parameters.AddWithValue("@GroupName", groupName);
                    command.Parameters.AddWithValue("@DirectionId", directionId);

                    command.ExecuteNonQuery();
                }
            }
        }
       
    }
    private new void PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (!char.IsDigit(e.Text, 0))
        {
            e.Handled = true;
        }
    }

    
}