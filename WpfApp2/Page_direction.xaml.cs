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
    /// Логика взаимодействия для Page_direction.xaml
    /// </summary>
    public partial class Page_direction : Page
    {
        public Page_direction()
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

                    // Заполнение данных из таблицы "Direction"
                    string directionSql = "SELECT * FROM public.\"Direction\"";
                    NpgsqlCommand directionCommand = new NpgsqlCommand(directionSql, connection);

                    NpgsqlDataAdapter directionAdapter = new NpgsqlDataAdapter(directionCommand);
                    DataTable directionDataTable = new DataTable();
                    directionAdapter.Fill(directionDataTable);

                    // Установите таблицу данных "Direction" как источник данных для DataGrid dtdirection
                    dtdirection.ItemsSource = directionDataTable.DefaultView;

                    // Заполнение данных из таблицы "Group"
                    string groupSql = "SELECT * FROM public.\"Group\"";
                    NpgsqlCommand groupCommand = new NpgsqlCommand(groupSql, connection);

                    NpgsqlDataAdapter groupAdapter = new NpgsqlDataAdapter(groupCommand);
                    DataTable groupDataTable = new DataTable();
                    groupAdapter.Fill(groupDataTable);

                    // Установите таблицу данных "Group" как источник данных для DataGrid dtgroup
                    dtgroup.ItemsSource = groupDataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }
        }
    }
}
