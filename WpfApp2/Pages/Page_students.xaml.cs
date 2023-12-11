using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Npgsql;

namespace WpfApp2
{
    public partial class Page_students : Page
    {
        public Page_students()
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

                    // Заполнение данных из таблицы "Students"
                    string studentsSql = "SELECT * FROM public.\"Students\"";
                    NpgsqlCommand studentsCommand = new NpgsqlCommand(studentsSql, connection);

                    NpgsqlDataAdapter studentsAdapter = new NpgsqlDataAdapter(studentsCommand);
                    DataTable studentsDataTable = new DataTable();
                    studentsAdapter.Fill(studentsDataTable);

                    // Установите таблицу данных "Students" как источник данных для DataGrid dtstudents
                    dtstudents.ItemsSource = studentsDataTable.DefaultView;

                    // Заполнение данных из таблицы "Lists"
                    string listsSql = "SELECT * FROM public.\"Lists\"";
                    NpgsqlCommand listsCommand = new NpgsqlCommand(listsSql, connection);

                    NpgsqlDataAdapter listsAdapter = new NpgsqlDataAdapter(listsCommand);
                    DataTable listsDataTable = new DataTable();
                    listsAdapter.Fill(listsDataTable);

                    // Установите таблицу данных "Lists" как источник данных для DataGrid dtlist
                    dtlist.ItemsSource = listsDataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}");
                }
            }
        }
    }
}
