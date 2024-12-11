using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ExerciceMonster.Models;

namespace ExerciceMonster.Views
{
    public partial class SpellsView : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Spell> _spells = new ObservableCollection<Spell>();
        private readonly string _connectionString;
        public event PropertyChangedEventHandler PropertyChanged;

        public SpellsView()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ExerciceMonsterContext"].ConnectionString;
            InitializeComponent();
            DataContext = this;
            LoadSpells();
           

        }

        public ObservableCollection<Spell> Spells
        {
            get => _spells;
            set
            {
                _spells = value;
                OnPropertyChanged();
            }
        }

        private void LoadSpells()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT ID, Name, Damage, Description FROM Spell", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Spells.Add(new Spell
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Damage = reader.GetInt32(2),
                            Description = reader.GetString(3)
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
