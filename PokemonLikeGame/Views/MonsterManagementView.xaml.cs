using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ExerciceMonster.Models;

namespace ExerciceMonster.Views
{
    public partial class MonsterManagementView : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Monster> _monsters = new ObservableCollection<Monster>();
        private Monster _selectedMonster;
        private readonly string _connectionString;
        public event PropertyChangedEventHandler PropertyChanged;

        public MonsterManagementView()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;

            InitializeComponent();
            DataContext = this;
            LoadMonsters();
         
        }

        public ObservableCollection<Monster> Monsters
        {
            get => _monsters;
            set
            {
                _monsters = value;
                OnPropertyChanged();
            }
        }

        public Monster SelectedMonster
        {
            get => _selectedMonster;
            set
            {
                _selectedMonster = value;
                OnPropertyChanged();
            }
        }

        private void LoadMonsters()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, Name, Health, ImageURL FROM Monster", connection);
                using (var reader = command.ExecuteReader())
                {
                    Monsters.Clear();
                    while (reader.Read())
                    {
                        Monsters.Add(new Monster
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Health = reader.GetInt32(2),
                            ImageURL = reader.IsDBNull(3) ? null : reader.GetString(3)  // Handle potential DBNulls
                        });
                    }
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
