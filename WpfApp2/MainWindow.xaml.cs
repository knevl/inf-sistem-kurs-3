using Npgsql;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Host=localhost; Database=cdt;Port=5432;Username=postgres;Password=admin;";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_auth_click(object sender, RoutedEventArgs e)
        {
            string login = textbox_login.Text.Trim();
            string pass = passwordbox_pass.Password.Trim();

            if (IsValidInput(login, pass))
            {
                AuthenticateUser(login, pass);
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректные логин и пароль!");
            }
        }

        private void AuthenticateUser(string login, string pass)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM users WHERE username = @username", connection))
                    {
                        command.Parameters.AddWithValue("username", login);

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                // Читаем данные пользователя из базы данных
                                reader.Read();
                                string storedHash = reader["password_hash"] != DBNull.Value ? reader["password_hash"].ToString() : null;

                                // Проверяем, что storedHash не равен null
                                if (storedHash != null)
                                {
                                    // Хешируем предоставленный пароль
                                    string enteredHash = crypt(pass, storedHash);

                                    // Сравниваем хеши
                                    if (enteredHash == storedHash)
                                    {
                                        MessageBox.Show("Пользователь авторизован!");
                                        Window1 window1 = new Window1();
                                        window1.Show();
                                        // Закрываем текущее окно
                                        this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Неверный логин или пароль!");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Неверный логин или пароль!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль!");
                            }
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show($"Ошибка при подключении к базе данных: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла неизвестная ошибка: {ex.Message}");
                }
            }
        }

        private string crypt(string password, string storedHash)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand("SELECT crypt(@password, @storedHash)", connection))
                    {
                        command.Parameters.AddWithValue("password", password);
                        command.Parameters.AddWithValue("storedHash", storedHash);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
                catch (NpgsqlException ex)
                {
                    MessageBox.Show($"Ошибка при выполнении хеширования: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла неизвестная ошибка: {ex.Message}");
                }

                return null;
            }
        }
        private bool IsValidInput(string login, string pass)
        {
            return (login.Length > 3 && login.Length <= 15) && (pass.Length > 3 && pass.Length <= 15);
        }  
    }
}
