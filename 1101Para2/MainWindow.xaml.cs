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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace _1101Para2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "Host=localhost;Database=itproduction;Username=postgres;Password=student";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT role FROM users WHERE username = @username AND password = @password";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string role = reader["role"].ToString();

                                if (role == "admin")
                                {
                                    MessageBox.Show("Вы вошли как администратор.");
                                    OpenAdminWindow();
                                }
                                else if (role == "user")
                                {
                                    MessageBox.Show("Вы вошли как пользователь.");
                                    OpenUserWindow();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неверный логин или пароль.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void OpenAdminWindow()
        {
            var adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }

        private void OpenUserWindow()
        {
            var userWindow = new UserWindow();
            userWindow.Show();
            this.Close();
        }
    }
}
