using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp2.forms
{
    /// <summary>
    /// Логика взаимодействия для clasess_form.xaml
    /// </summary>
    public partial class clasess_form : Window
    {
        private readonly Page_classes parentPage;
        private readonly int classesId;
        private static Page_classes page;

        public clasess_form(Page_classes page)
        {
            InitializeComponent();
            parentPage = page;
            Closed += ClassesForm_Closed;
        }

        public clasess_form (Page_classes parentPage, int id, DateTime Date, int group, int teacher, bool done)
            : this(page)
        {
            classesId = id;
            c_dateDatePicker.SelectedDate = Date;
            c_idgroupTextBox.Text = group.ToString();
            c_idteacherTextBox.Text = teacher.ToString();
            doneCheck.IsChecked = done;
        }

        private void ClassesForm_Closed(object sender, EventArgs e)
        {
            if (sender is clasess_form clasessForm && clasessForm != null)
            {
                clasessForm.parentPage?.RefreshData();
            }
        }


        private void save_student_Click(object sender, RoutedEventArgs e)
        {
            var date = c_dateDatePicker.SelectedDate ?? DateTime.Now;
            var group = Convert.ToInt32(c_idgroupTextBox.Text);
            var teacher = Convert.ToInt32(c_idteacherTextBox.Text);
            var done = doneCheck.IsChecked ?? false;  // По умолчанию false, если флажок не установлен

            if (classesId > 0)
            {
                DatabaseHelper.UpdateClasses(classesId, date, group, teacher, done);
            }
            else
            {
                DatabaseHelper.AddClasses(date, group, teacher, done);
            }

            Close();
        }

        private void cancel_student_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
                "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) Close();
        }

        public class DatabaseHelper
        {
            private const string ConnectionString =
                "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";
            
            public static void AddClasses(DateTime date, int group, int teacher, bool done)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    var query =
                        "INSERT INTO public.\"Classes\"(\"Date\", \"group\", \"teacher\", \"done\") " +
                        "VALUES (@Date, @Group, @Teacher, @Done)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Group", group);
                        command.Parameters.AddWithValue("@Teacher", teacher);
                        command.Parameters.AddWithValue("@Done", done);

                        command.ExecuteNonQuery();
                    }
                }
            }

            public static void UpdateClasses(int classesId, DateTime date, int group, int teacher, bool done)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    var query = "UPDATE public.\"Classes\" " +
                                "SET \"Date\" = @Date, \"group\" = @Group, \"teacher\" = @Teacher, \"done\" = @Done " +
                                "WHERE \"id\" = @ClassesId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClassesId", classesId);
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Group", group);
                        command.Parameters.AddWithValue("@Teacher", teacher);
                        command.Parameters.AddWithValue("@Done", done);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
