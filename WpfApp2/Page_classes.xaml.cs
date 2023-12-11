using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для Page_classes.xaml
    /// </summary>
    public partial class Page_classes : Page
    {
        public Page_classes()
        {
            InitializeComponent();
            string connectionString = "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

            // Создайте соединение с базой данных
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Заполнение данных из таблицы "Classes"
                    string classesSql = "SELECT * FROM public.\"Classes\"";
                    NpgsqlCommand classesCommand = new NpgsqlCommand(classesSql, connection);

                    NpgsqlDataAdapter classesAdapter = new NpgsqlDataAdapter(classesCommand);
                    DataTable classesDataTable = new DataTable();
                    classesAdapter.Fill(classesDataTable);

                    // Установите таблицу данных "Classes" как источник данных для DataGrid dtclasses
                    dtclasses.ItemsSource = classesDataTable.DefaultView;

                    // Заполнение данных из таблицы "Schedule"
                    string scheduleSql = "SELECT * FROM public.\"Schedule\"";
                    NpgsqlCommand scheduleCommand = new NpgsqlCommand(scheduleSql, connection);

                    NpgsqlDataAdapter scheduleAdapter = new NpgsqlDataAdapter(scheduleCommand);
                    DataTable scheduleDataTable = new DataTable();
                    scheduleAdapter.Fill(scheduleDataTable);

                    // Установите таблицу данных "Schedule" как источник данных для DataGrid dtschedule
                    dtschedule.ItemsSource = scheduleDataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }
        }
    }
}
