using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Page_report.xaml
    /// </summary>
    public partial class Page_report : Page
    {
        private readonly string connectionString ="Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

        public Page_report()
        {
            InitializeComponent();
        }

        private void report_raspisanie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь к рабочему столу пользователя
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // Полный путь к файлу на рабочем столе
                string filePath = Path.Combine(desktopPath, "Report_Raspisanie.txt");

                using (var connection = new Npgsql.NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Schedule и связанных таблиц Group и Teachers
                    var query =
                        "SELECT s.weekday, s.starttime, s.endtime, g.\"Name\" AS GroupName, " +
                        "t.\"Surname\" || ' ' || t.\"Name\" || ' ' || t.\"Patronym\" AS TeacherName, s.classroom " +
                        "FROM public.\"Schedule\" s " +
                        "INNER JOIN public.\"Group\" g ON s.\"group\" = g.id " +
                        "INNER JOIN public.\"Teachers\" t ON s.teacher = t.\"ID\" " +
                        "ORDER BY CASE " +
                        "WHEN s.weekday = 'Понедельник' THEN 1 " +
                        "WHEN s.weekday = 'Вторник' THEN 2 " +
                        "WHEN s.weekday = 'Среда' THEN 3 " +
                        "WHEN s.weekday = 'Четверг' THEN 4 " +
                        "WHEN s.weekday = 'Пятница' THEN 5 " +
                        "WHEN s.weekday = 'Суббота' THEN 6 " +
                        "WHEN s.weekday = 'Воскресенье' THEN 7 " +
                        "ELSE 8 " +
                        "END, s.starttime";

                    using (var command = new Npgsql.NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            // Запись данных в файл
                            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                            {
                                // Записываем заголовок
                                writer.WriteLine("День недели\tВремя начала\tВремя окончания\tГруппа\tПреподаватель\tКабинет");

                                // Записываем данные
                                while (reader.Read())
                                {
                                    string weekday = reader.GetString(0);
                                    TimeSpan startTime = reader.GetTimeSpan(1);
                                    TimeSpan endTime = reader.GetTimeSpan(2);
                                    string groupName = reader.GetString(3);
                                    string teacherName = reader.GetString(4);
                                    int classroom = reader.GetInt32(5);

                                    // Записываем данные в файл
                                    writer.WriteLine($"{weekday}\t{startTime}\t{endTime}\t{groupName}\t{teacherName}\t{classroom}");
                                }
                            }
                        }
                    }
                }

                MessageBox.Show($"Отчет успешно создан. Путь: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void report_lists_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь к рабочему столу пользователя
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // Полный путь к файлу на рабочем столе
                string filePath = Path.Combine(desktopPath, "Списки групп.txt");

                using (var connection = new Npgsql.NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Students и Group
                    var query =
                        "SELECT s.\"Name\", s.\"Surname\", s.\"Patronym\", g.\"Name\" " +
                        "FROM public.\"Students\" s " +
                        "INNER JOIN public.\"Lists\" l ON s.\"ID\" = l.student " +
                        "INNER JOIN public.\"Group\" g ON l.\"group\" = g.id " +
                        "ORDER BY g.\"Name\", s.\"Surname\"";


                    using (var command = new Npgsql.NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            // Запись данных в файл
                            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                            {
                                string currentGroup = string.Empty;

                                // Записываем данные
                                while (reader.Read())
                                {
                                    string name = reader.GetString(0);
                                    string surname = reader.GetString(1);
                                    string patronym = reader.GetString(2);
                                    string groupName = reader.GetString(3);

                                    // Проверяем изменение группы
                                    if (groupName != currentGroup)
                                    {
                                        // Если группа изменилась, записываем ее в новой строке
                                        writer.WriteLine($"{Environment.NewLine}{groupName}");
                                        currentGroup = groupName;
                                    }

                                    // Записываем ФИО в столбце
                                    writer.WriteLine($"{surname} {name} {patronym}");
                                }
                            }
                        }
                    }
                }

                MessageBox.Show($"Отчет успешно создан. Путь: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void report_clasess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Путь к рабочему столу пользователя
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                // Полный путь к файлу на рабочем столе
                string filePath = Path.Combine(desktopPath, "Проведенные занятия.txt");

                using (var connection = new Npgsql.NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для выборки данных из таблицы Classes и Teachers
                    var query =
                        "SELECT c.id, c.\"Date\", g.\"Name\" AS GroupName, " +
                        "t.\"Surname\" || ' ' || t.\"Name\" || ' ' || t.\"Patronym\" AS TeacherName, c.done " +
                        "FROM public.\"Classes\" c " +
                        "INNER JOIN public.\"Group\" g ON c.\"group\" = g.id " +
                        "INNER JOIN public.\"Teachers\" t ON c.teacher = t.\"ID\" " +
                        "ORDER BY c.done DESC, c.\"Date\"";

                    using (var command = new Npgsql.NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            // Запись данных в файл
                            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                            {
                                int doneCount = 0;
                                int notDoneCount = 0;

                                bool doneHeaderWritten = false;
                                bool notDoneHeaderWritten = false;

                                // Записываем данные
                                while (reader.Read())
                                {
                                    int id = reader.GetInt32(0);
                                    DateTime date = reader.GetDateTime(1);
                                    string groupName = reader.GetString(2);
                                    string teacherName = reader.GetString(3);
                                    bool done = reader.GetBoolean(4);

                                    // Если это первая запись для данного статуса (done или not done), записываем заголовок
                                    if (!done && !notDoneHeaderWritten)
                                    {
                                        writer.WriteLine("\nНе проведено:");
                                        notDoneHeaderWritten = true;
                                    }
                                    else if (done && !doneHeaderWritten)
                                    {
                                        writer.WriteLine("\nПроведено:");
                                        doneHeaderWritten = true;
                                    }

                                    // Записываем данные в файл
                                    writer.WriteLine($"{id}\t{date:dd.MM.yyyy}\t{groupName}\t{teacherName}");

                                    // Проверяем состояние проведения и увеличиваем соответствующий счетчик
                                    if (done)
                                    {
                                        doneCount++;
                                    }
                                    else
                                    {
                                        notDoneCount++;
                                    }
                                }

                                // Выводим итоги
                                writer.WriteLine($"\nИтого проведенных: {doneCount}\n\nИтого не проведенных: {notDoneCount}\n\nИтого: {doneCount + notDoneCount}");
                            }
                        }
                    }
                }

                MessageBox.Show($"Отчет успешно создан. Путь: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
