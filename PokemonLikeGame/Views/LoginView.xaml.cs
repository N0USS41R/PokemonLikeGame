using ExerciceMonster.Utilities; // Make sure you have RelayCommand and HashHelper here
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExerciceMonster.Views
{
    public partial class LoginView : Window, INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _registerUsername;
        private string _registerPassword;
        private string _confirmPassword;
        private readonly string _connectionString;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginView()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;

            InitializeComponent();
            this.DataContext = this; 
            LoginCommand = new RelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string RegisterUsername
        {
            get => _registerUsername;
            set
            {
                _registerUsername = value;
                OnPropertyChanged();
            }
        }

        public string RegisterPassword
        {
            get => _registerPassword;
            set
            {
                _registerPassword = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private bool CanLogin(object parameter)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
        }

        private void Login(object parameter)
        {
            var hashedPassword = HashHelper.HashPassword(Password);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Login WHERE Username = @Username AND PasswordHash = @PasswordHash";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", Username);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    int userCount = (int)command.ExecuteScalar();

                    if (userCount > 0)
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();

                        foreach (Window window in Application.Current.Windows)
                        {
                            if (window is LoginView)
                            {
                                window.Close();
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private bool CanRegister(object parameter)
        {
            return !string.IsNullOrEmpty(RegisterUsername) && !string.IsNullOrEmpty(RegisterPassword) && RegisterPassword == ConfirmPassword;
        }

        private void Register(object parameter)
        {
            var hashedPassword = HashHelper.HashPassword(RegisterPassword);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Login (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", RegisterUsername);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Registration successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = ((PasswordBox)sender).Password;
        }

        private void PasswordBox_PasswordChangedRegister(object sender, RoutedEventArgs e)
        {
            RegisterPassword = ((PasswordBox)sender).Password;
        }

        private void PasswordBox_PasswordChangedConfirm(object sender, RoutedEventArgs e)
        {
            ConfirmPassword = ((PasswordBox)sender).Password;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
