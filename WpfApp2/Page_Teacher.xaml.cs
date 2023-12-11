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

namespace WpfApp2.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page_Teacher.xaml
    /// </summary>
    public partial class Page_Teacher : Page
    {
        public Page_Teacher()
        {
            InitializeComponent();
            // Установите параметры подключения
            string connectionString = "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

            // Создайте соединение с базой данных
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Заполнение данных из таблицы "Teachers"
                    string teachersSql = "SELECT * FROM public.\"Teachers\"";
                    NpgsqlCommand teachersCommand = new NpgsqlCommand(teachersSql, connection);

                    NpgsqlDataAdapter teachersAdapter = new NpgsqlDataAdapter(teachersCommand);
                    DataTable teachersDataTable = new DataTable();
                    teachersAdapter.Fill(teachersDataTable);

                    // Установите таблицу данных "Teachers" как источник данных для DataGrid dtteachers
                    dtteachers.ItemsSource = teachersDataTable.DefaultView;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }

        }
    }
}
