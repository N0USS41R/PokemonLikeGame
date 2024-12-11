using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExerciceMonster.Utilities;

namespace ExerciceMonster.Views
{
    public partial class SettingsView : UserControl, INotifyPropertyChanged
    {
        private string _connectionString;
        private ObservableCollection<string> _databaseDetails;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsView()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;

            InitializeComponent();
            DataContext = this;

            RefreshDatabaseDetailsCommand = new RelayCommand(RefreshDatabaseDetails);
            SaveConnectionStringCommand = new RelayCommand(SaveConnectionString);

            DatabaseDetails = new ObservableCollection<string>();
            RefreshDatabaseDetails(null);
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> DatabaseDetails
        {
            get => _databaseDetails;
            set
            {
                _databaseDetails = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshDatabaseDetailsCommand { get; private set; }
        public ICommand SaveConnectionStringCommand { get; private set; }

        private void RefreshDatabaseDetails(object parameter)
        {
            DatabaseDetails.Clear();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Fetch table names
                    var schemaTable = connection.GetSchema("Tables");
                    foreach (DataRow row in schemaTable.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        DatabaseDetails.Add($"Table: {tableName}");

                        // Fetch row count for each table
                        using (var command = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", connection))
                        {
                            int rowCount = (int)command.ExecuteScalar();
                            DatabaseDetails.Add($"  - Rows: {rowCount}");
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error loading database details: {ex.Message}", "Error", MessageBoxButton.OK);
            }
        }

        private void SaveConnectionString(object parameter)
        {
            var result = MessageBox.Show(
                "Warning: Changing the connection string may drop the connection if the database is not valid. Continue?",
                "Warning",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                // Revert to the original connection string 
                ConnectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;
                return;
            }

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error testing new connection string: {ex.Message}", "Error", MessageBoxButton.OK);
                return;
            }

            MessageBox.Show("Connection string updated successfully!", "Success", MessageBoxButton.OK);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
