using Npgsql;
using NpgsqlTypes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2.forms
{
    public partial class schedule_form : Window
    {
        private readonly Page_classes parentPage;
        private readonly int scheduleId;

        public schedule_form(Page_classes page)
        {
            InitializeComponent();
            parentPage = page;
            Closed += ScheduleForm_Closed;
        }

        public schedule_form(Page_classes page, int id, string weekday, TimeSpan starttime, TimeSpan endtime, int group, int teacher, int classroom)
            : this(page)
        {
            scheduleId = id;
            sch_weekdayComboBox.Text = weekday;
            sch_idgroupTextBox.Text = group.ToString();
            sch_idteacherTextBox.Text = teacher.ToString();
            sch_roomTextBox.Text = classroom.ToString();

            starttime_tp.StartTime = starttime;
            endtime_tp.EndTime = endtime;
        }

        private void ScheduleForm_Closed(object sender, EventArgs e)
        {
            if (sender is schedule_form scheduleForm) scheduleForm.parentPage.RefreshData();
        }

        private void save_sch_Click(object sender, RoutedEventArgs e)
        {
            var weekday = sch_weekdayComboBox.Text;
            var group = Convert.ToInt32(sch_idgroupTextBox.Text);
            var teacher = Convert.ToInt32(sch_idteacherTextBox.Text);
            var classroom = Convert.ToInt32(sch_roomTextBox.Text);
            var starttime = starttime_tp.Value.HasValue ? starttime_tp.Value.Value.TimeOfDay : TimeSpan.Zero;
            var endtime = endtime_tp.Value.HasValue ? endtime_tp.Value.Value.TimeOfDay : TimeSpan.Zero;



            if (scheduleId > 0)
            {
                DatabaseHelper.UpdateSchedule(scheduleId, weekday, starttime, endtime, group, teacher, classroom);
            }
            else
            {
                DatabaseHelper.AddSchedule(weekday, starttime, endtime, group, teacher, classroom);
            }

            Close();
        }

        private void cancel_sch_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите отменить действия, не сохранив результат?",
                "Подтверждение отмены", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) Close();
        }

        public class DatabaseHelper
        {
            private const string ConnectionString =
                "Host=localhost; Database=cdt; Port=5432; Username=postgres; Password=admin;";

            public static void AddSchedule(string weekday, TimeSpan starttime, TimeSpan endtime, int group, int teacher, int classroom)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    var query =
                        "INSERT INTO public.\"Schedule\"(\"weekday\", \"starttime\", \"endtime\", \"group\", \"teacher\", \"classroom\") " +
                        "VALUES (@Weekday, @StartTime, @EndTime, @Group, @Teacher, @Classroom)";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Weekday", weekday);
                        command.Parameters.AddWithValue("@StartTime", NpgsqlDbType.Interval, starttime);
                        command.Parameters.AddWithValue("@EndTime", NpgsqlDbType.Interval, endtime);
                        command.Parameters.AddWithValue("@Group", group);
                        command.Parameters.AddWithValue("@Teacher", teacher);
                        command.Parameters.AddWithValue("@Classroom", classroom);

                        command.ExecuteNonQuery();
                    }
                }
            }

            public static void UpdateSchedule(int scheduleId, string weekday, TimeSpan starttime, TimeSpan endtime, int group, int teacher, int classroom)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    var query = "UPDATE public.\"Schedule\" " +
                                "SET \"weekday\" = @Weekday, \"starttime\" = @StartTime, \"endtime\" = @EndTime, " +
                                "\"group\" = @Group, \"teacher\" = @Teacher, \"classroom\" = @Classroom " +
                                "WHERE \"id\" = @ScheduleId";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ScheduleId", scheduleId);
                        command.Parameters.AddWithValue("@Weekday", weekday);
                        command.Parameters.AddWithValue("@StartTime", NpgsqlDbType.Interval, starttime);
                        command.Parameters.AddWithValue("@EndTime", NpgsqlDbType.Interval, endtime);
                        command.Parameters.AddWithValue("@Group", group);
                        command.Parameters.AddWithValue("@Teacher", teacher);
                        command.Parameters.AddWithValue("@Classroom", classroom);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
